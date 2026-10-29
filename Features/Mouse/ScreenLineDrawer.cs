// ============================================================================
// ScreenLineDrawer.cs
// ----------------------------------------------------------------------------
// 从 WGestures 项目中提炼的「按住鼠标右键移动 → 在屏幕上画线」实现，
// 已集成进 keyupMusic4：
//   - 复用现有 KeyboardMouseHook.MouseEvent（不新增 WH_MOUSE_LL 钩子，避免双钩子 CPU 开销）
//   - 只“观察”鼠标事件不拦截：右键菜单等原行为不受影响
//   - 按住右键并移动 >= MinMoveToStart 像素即开始画线；松开右键清除
// ============================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using static keyupMusic4.Common;

namespace keyupMusic4
{
    /// <summary>右键按住移动画线（单例，由 Huan.startListen 注册到 MouseEvent）。</summary>
    public class ScreenLineDrawer : IDisposable
    {
        private readonly OverlayCanvas _canvas = new OverlayCanvas();
        private readonly List<Point> _points = new List<Point>(1024);

        private Pen _linePen;
        private Pen _borderPen;
        private Point _downPoint;
        private bool _drawing;

        private static ScreenLineDrawer _instance;
        private static readonly object _lock = new object();

        /// <summary>全局单例（懒加载）。</summary>
        public static ScreenLineDrawer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new ScreenLineDrawer();
                    }
                }
                return _instance;
            }
        }

        /// <summary>线条颜色（WGestures 默认的绿色轨迹）。</summary>
        public Color LineColor { get; set; }

        /// <summary>线条宽度（像素）。</summary>
        public float LineWidth { get; set; }

        /// <summary>按下后至少移动多少像素才开始画线（对应 WGestures 的 InitialValidMove）。</summary>
        public int MinMoveToStart { get; set; }

        public ScreenLineDrawer()
        {
            LineColor = Color.FromArgb(255, 50, 200, 100);
            LineWidth = 1.5f;
            MinMoveToStart = 4;

            _linePen = new Pen(LineColor, LineWidth)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };
            // 白色描边，对应 WGestures 的 _borderPen，增强对比度
            _borderPen = new Pen(Color.White, LineWidth + 2.5f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };
        }

        /// <summary>创建透明置顶画布（幂等）。</summary>
        public void Start()
        {
            if (!_canvas.Started) _canvas.Start();
        }

        /// <summary>销毁画布（幂等）。</summary>
        public void Stop()
        {
            if (_canvas.Started) _canvas.Stop();
        }

        /// <summary>键盘鼠标钩子分发入口（注册到 KeyboardMouseHook.MouseEvent）。</summary>
        public void MouseHookProc(KeyboardMouseHook.MouseEventArgs e)
        {
            var p = new Point(e.X, e.Y);
            switch (e.Msg)
            {
                case MouseMsg.click_r: OnRightDown(p); break;
                case MouseMsg.move: OnMouseMove(p); break;
                case MouseMsg.click_r_up: OnRightUp(p); break;
            }
        }

        // ---- 事件处理（都在钩子线程上串行触发，无需加锁） ----

        private void OnRightDown(Point p)
        {
            _drawing = true;
            _downPoint = p;
            _points.Clear();
            _points.Add(p);
        }

        private void OnMouseMove(Point p)
        {
            if (!_drawing) return;

            // 未越过起始阈值前不画（避免轻微抖动就出线）
            if (_points.Count == 1 && Distance(_downPoint, p) < MinMoveToStart) return;

            var last = _points[_points.Count - 1];
            if (last == p) return; // 同一点跳过

            _points.Add(p);

            // 简化版：每次重绘整条路径（原版为性能做了 dirty-rect 局部刷新，本项目可忽略）
            var g = _canvas.BeginDraw();
            g.Clear(Color.Transparent);
            if (_points.Count >= 2)
            {
                using (var path = new GraphicsPath())
                {
                    path.AddLines(_points.ToArray());
                    g.DrawPath(_borderPen, path);
                    g.DrawPath(_linePen, path);
                }
            }
            g.Flush();
            _canvas.EndDraw();
            _canvas.Present();
        }

        private void OnRightUp(Point p)
        {
            _drawing = false;
            _points.Clear();

            // 清空画布
            var g = _canvas.BeginDraw();
            g.Clear(Color.Transparent);
            g.Flush();
            _canvas.EndDraw();
            _canvas.Present();
        }

        private static float Distance(Point a, Point b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public void Dispose()
        {
            _linePen.Dispose();
            _borderPen.Dispose();
            Stop();
        }
    }

    // ==========================================================================
    // 全屏透明置顶画布（对应 WGestures 的 CanvasWindow + DiBitmap）
    //    - 分层窗口 WS_EX_LAYERED：可逐像素 alpha，实现透明画线。
    //    - WS_EX_TRANSPARENT：鼠标事件穿透（画布不挡点击）。
    //    - WS_EX_NOACTIVATE：不抢焦点。
    //    - 窗口在独立线程创建并跑消息泵（与 WGestures 的 CanvasWindow 线程一致）。
    //    - 绘制用 32bppArgb DIB + GDI+，再经 UpdateLayeredWindow 推到屏幕。
    // ==========================================================================
    internal class OverlayCanvas : IDisposable
    {
        private LineNative.WndProc _wndProc; // 保活
        private IntPtr _hwnd;
        private IntPtr _memDc;
        private IntPtr _hBitmap;
        private Graphics _graphics;
        private Rectangle _virtualScreen;
        private Thread _thread;
        private AutoResetEvent _ready = new AutoResetEvent(false);
        private Exception _initError;

        public bool Started { get; private set; }

        public void Start()
        {
            if (Started) return;
            Started = true;

            _thread = new Thread(() =>
            {
                try
                {
                    // 多显示器：取整个虚拟屏幕范围
                    _virtualScreen = new Rectangle(
                        LineNative.GetSystemMetrics(LineNative.SM_XVIRTUALSCREEN),
                        LineNative.GetSystemMetrics(LineNative.SM_YVIRTUALSCREEN),
                        LineNative.GetSystemMetrics(LineNative.SM_CXVIRTUALSCREEN),
                        LineNative.GetSystemMetrics(LineNative.SM_CYVIRTUALSCREEN));

                    _wndProc = WndProc;
                    var wc = new LineNative.WNDCLASSEX
                    {
                        cbSize = (uint)Marshal.SizeOf(typeof(LineNative.WNDCLASSEX)),
                        style = 0,
                        lpfnWndProc = _wndProc,
                        hInstance = LineNative.GetModuleHandle(null),
                        lpszClassName = "ScreenLineOverlay"
                    };
                    LineNative.RegisterClassEx(ref wc);

                    _hwnd = LineNative.CreateWindowEx(
                        LineNative.WS_EX_LAYERED | LineNative.WS_EX_TOPMOST | LineNative.WS_EX_TOOLWINDOW
                        | LineNative.WS_EX_TRANSPARENT | LineNative.WS_EX_NOACTIVATE,
                        wc.lpszClassName, null, LineNative.WS_POPUP,
                        _virtualScreen.X, _virtualScreen.Y, _virtualScreen.Width, _virtualScreen.Height,
                        IntPtr.Zero, IntPtr.Zero, wc.hInstance, IntPtr.Zero);
                    if (_hwnd == IntPtr.Zero)
                        throw new InvalidOperationException("创建画布窗口失败: " + Marshal.GetLastWin32Error());

                    // 32bppArgb DIB 缓冲区（对应 DiBitmap）
                    _memDc = LineNative.CreateCompatibleDC(IntPtr.Zero);
                    var bmi = new LineNative.BITMAPINFO
                    {
                        bmiHeader = new LineNative.BITMAPINFOHEADER
                        {
                            biSize = (uint)Marshal.SizeOf(typeof(LineNative.BITMAPINFOHEADER)),
                            biWidth = _virtualScreen.Width,
                            biHeight = _virtualScreen.Height,
                            biPlanes = 1,
                            biBitCount = 32,
                            biCompression = 0
                        }
                    };
                    IntPtr bits;
                    _hBitmap = LineNative.CreateDIBSection(_memDc, ref bmi, LineNative.DIB_RGB_COLORS, out bits, IntPtr.Zero, 0);
                    if (_hBitmap == IntPtr.Zero)
                        throw new InvalidOperationException("创建 DIB 失败: " + Marshal.GetLastWin32Error());

                    LineNative.SelectObject(_memDc, _hBitmap);
                    _graphics = Graphics.FromHdc(_memDc);
                    _graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    _graphics.CompositingMode = CompositingMode.SourceCopy;

                    LineNative.ShowWindow(_hwnd, LineNative.SW_SHOWNOACTIVATE);
                    _ready.Set();

                    LineNative.MSG msg;
                    while (LineNative.GetMessage(out msg, IntPtr.Zero, 0, 0) > 0)
                    {
                        LineNative.TranslateMessage(ref msg);
                        LineNative.DispatchMessage(ref msg);
                    }
                }
                catch (Exception e)
                {
                    _initError = e;
                    _ready.Set();
                }
            })
            {
                IsBackground = true,
                Name = "OverlayCanvas"
            };
            _thread.Start();

            _ready.WaitOne();
            if (_initError != null) throw _initError;
        }

        public Graphics BeginDraw()
        {
            LineNative.SelectObject(_memDc, _hBitmap);
            return _graphics;
        }

        public void EndDraw()
        {
            // GDI+ 绘制完成后重新选入位图，确保 UpdateLayeredWindow 读取最新内容
            LineNative.SelectObject(_memDc, _hBitmap);
        }

        /// <summary>把缓冲区整屏推送到分层窗口（对应 CanvasWindow.SetDiBitmap -> UpdateLayeredWindowIndirect）。</summary>
        public void Present()
        {
            var screenDc = LineNative.GetDC(IntPtr.Zero);
            try
            {
                LineNative.SelectObject(_memDc, _hBitmap);

                var dst = new LineNative.POINT { x = _virtualScreen.X, y = _virtualScreen.Y };
                var size = new LineNative.SIZE { cx = _virtualScreen.Width, cy = _virtualScreen.Height };
                var src = new LineNative.POINT { x = 0, y = 0 };
                var blend = new LineNative.BLENDFUNCTION
                {
                    BlendOp = LineNative.AC_SRC_OVER,
                    BlendFlags = 0,
                    SourceConstantAlpha = 255,
                    AlphaFormat = LineNative.AC_SRC_ALPHA
                };
                LineNative.UpdateLayeredWindow(_hwnd, screenDc, ref dst, ref size, _memDc, ref src, 0, ref blend, LineNative.ULW_ALPHA);
            }
            finally
            {
                LineNative.ReleaseDC(IntPtr.Zero, screenDc);
            }
        }

        private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case LineNative.WM_CLOSE:
                    LineNative.DestroyWindow(hWnd);
                    return IntPtr.Zero;
                case LineNative.WM_DESTROY:
                    LineNative.PostQuitMessage(0);
                    return IntPtr.Zero;
                default:
                    return LineNative.DefWindowProc(hWnd, msg, wParam, lParam);
            }
        }

        public void Stop()
        {
            if (!Started) return;
            if (_hwnd != IntPtr.Zero)
            {
                LineNative.PostMessage(_hwnd, LineNative.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _hwnd = IntPtr.Zero;
            }
            if (_thread != null && _thread.IsAlive) _thread.Join(2000);
            _thread = null;
            Started = false;
        }

        public void Dispose()
        {
            Stop();
            if (_graphics != null) { _graphics.Dispose(); _graphics = null; }
            if (_memDc != IntPtr.Zero) { LineNative.DeleteDC(_memDc); _memDc = IntPtr.Zero; }
            if (_hBitmap != IntPtr.Zero) { LineNative.DeleteObject(_hBitmap); _hBitmap = IntPtr.Zero; }
            if (_ready != null) { _ready.Dispose(); _ready = null; }
        }
    }

    // ==========================================================================
    // Win32 P/Invoke 声明（改名 LineNative，避免与 Hooks\Native.cs 冲突）
    // ==========================================================================
    internal static class LineNative
    {
        internal delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        internal const uint WM_RBUTTONDOWN = 0x0204;
        internal const uint WM_RBUTTONUP = 0x0205;
        internal const uint WM_MOUSEMOVE = 0x0200;
        internal const uint WM_CLOSE = 0x0010;
        internal const uint WM_DESTROY = 0x0002;
        internal const uint WM_QUIT = 0x0012;

        internal const int WS_POPUP = unchecked((int)0x80000000);
        internal const int WS_EX_LAYERED = 0x00080000;
        internal const int WS_EX_TOPMOST = 0x00000008;
        internal const int WS_EX_TOOLWINDOW = 0x00000080;
        internal const int WS_EX_TRANSPARENT = 0x00000020;
        internal const int WS_EX_NOACTIVATE = 0x08000000;

        internal const int SW_SHOWNOACTIVATE = 4;

        internal const int SM_XVIRTUALSCREEN = 76;
        internal const int SM_YVIRTUALSCREEN = 77;
        internal const int SM_CXVIRTUALSCREEN = 78;
        internal const int SM_CYVIRTUALSCREEN = 79;

        internal const uint ULW_ALPHA = 0x00000002;
        internal const byte AC_SRC_OVER = 0;
        internal const byte AC_SRC_ALPHA = 1;
        internal const uint DIB_RGB_COLORS = 0;

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT { public int x; public int y; }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SIZE { public int cx; public int cy; }

        [StructLayout(LayoutKind.Sequential)]
        internal struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct WNDCLASSEX
        {
            public uint cbSize;
            public uint style;
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPTStr)] public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPTStr)] public string lpszClassName;
            public IntPtr hIconSm;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct BITMAPINFO { public BITMAPINFOHEADER bmiHeader; public uint bmiColors; }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern ushort RegisterClassEx(ref WNDCLASSEX lpWndClass);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName,
            int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool PostQuitMessage(int nExitCode);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        internal static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        internal static extern bool TranslateMessage(ref MSG lpMsg);

        [DllImport("user32.dll")]
        internal static extern IntPtr DispatchMessage(ref MSG lpMsg);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        internal static extern int GetSystemMetrics(int nIndex);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint usage, out IntPtr ppvBits, IntPtr hSection, uint offset);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        internal static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hgdiobj);

        [DllImport("user32.dll")]
        internal static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize,
            IntPtr hdcSrc, ref POINT pptSrc, uint crKey, ref BLENDFUNCTION pblend, uint dwFlags);
    }
}
