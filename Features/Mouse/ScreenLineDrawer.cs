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
using System.Threading.Tasks;
using static keyupMusic4.Common;
using static keyupMusic4.Simulate;

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

        // ---- 右键接管状态机（账本式，保证“吞掉的按下一定有对应的释放”） ----
        // 只有本进程真正吞掉了右键 DOWN（_captured=true）才需要结算；系统与目标程序从未
        // 收到这个 DOWN，因此任何异常/退出路径都不会把系统留在“按下”状态。
        private bool _captured;      // 已吞下右键 DOWN，等待 UP 结算
        private bool _gesture;       // 已越过起始阈值，画线手势成立

        /// <summary>
        /// 是否接管右键：true=吞掉右键按下/抬起（画线时不弹右键菜单、不拖拽、不选中）；
        /// false=只观察不拦截（右键菜单与拖拽保持原样，画线同时显示）。
        /// 配置项 Mouse.rightGestureCapture，默认 1。
        /// </summary>
        public bool CaptureRight { get; set; } = true;

        /// <summary>
        /// 不接管右键的进程名单（逗号分隔，进程名不含 .exe），用于游戏“按住右键瞄准”
        /// 或需要右键拖拽的程序。配置项 Mouse.rightGestureExclude，默认空。
        /// </summary>
        public string ExcludeProcesses { get; set; } = "";

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

        /// <summary>创建透明置顶画布（幂等），并刷新接管配置。</summary>
        public void Start()
        {
            CaptureRight = LoadCfgInt("Mouse.rightGestureCapture", 1) == 1;
            ExcludeProcesses = LoadCfgStr("Mouse.rightGestureExclude", "");
            if (!_canvas.Started) _canvas.Start();
        }

        /// <summary>销毁画布（幂等）。卸载/停止时无条件结算捕获账本，绝不残留按下态。</summary>
        public void Stop()
        {
            ReleaseCapture();
            if (_canvas.Started) _canvas.Stop();
        }

        /// <summary>无条件释放捕获状态（Stop/Pause/钩子卸载/异常兜底调用）。</summary>
        public void ReleaseCapture()
        {
            ResetCapture();
            TryClearCanvas();
        }

        /// <summary>键盘鼠标钩子分发入口（注册到 KeyboardMouseHook.MouseEvent）。</summary>
        public void MouseHookProc(KeyboardMouseHook.MouseEventArgs e)
        {
            var p = new Point(e.X, e.Y);
            try
            {
                switch (e.Msg)
                {
                    case MouseMsg.click_r: OnRightDown(p, e); break;
                    case MouseMsg.move: OnMouseMove(p, e); break;
                    case MouseMsg.click_r_up: OnRightUp(p, e); break;
                }
            }
            catch
            {
                // fail-open：任何异常一律放行并复位账本，绝不让系统或目标程序停在按下态
                e.Handled = false;
                ResetCapture();
                TryClearCanvas();
            }
        }

        // ---- 事件处理（都在钩子线程上串行触发，无需加锁） ----

        private void OnRightDown(Point p, KeyboardMouseHook.MouseEventArgs e)
        {
            // 新的 DOWN 到来若仍处于捕获态：说明上一个 UP 丢失（极少见）→ 先复位再接管新的，
            // 状态机因此不可能永久卡死。这里不补发 UP：被吞的 DOWN 从未放行，系统本就不认为键已按下。
            if (_captured) ResetCapture();

            _drawing = false;
            _gesture = false;
            _downPoint = p;
            _points.Clear();
            _points.Add(p);

            if (!CaptureRight || IsExcludedProcess()) return; // 不接管：保持旧“只观察”行为

            // 接管：吞掉右键按下。目标程序收不到 DOWN，既不会弹菜单、也不会进入拖拽/选中态。
            e.Handled = true;
            _captured = true;
        }

        private void OnMouseMove(Point p, KeyboardMouseHook.MouseEventArgs e)
        {
            // ---- 接管态：只画线，move 一律放行 ----
            // 目标程序因为没收到 DOWN，不会把这次移动识别为拖拽/选中，放行没有副作用；
            // 放行还能保证系统光标照常移动、其它基于 move 的功能不受影响。
            if (_captured)
            {
                if (!_gesture)
                {
                    if (Distance(_downPoint, p) < MinMoveToStart) return; // 未过阈值：不算手势
                    _gesture = true; // 手势成立（首段从按下点画起，视觉连续）
                }
                else
                {
                    var lastG = _points[_points.Count - 1];
                    if (lastG == p) return;
                }

                _points.Add(p);
                Redraw();
                return;
            }

            // ---- 观察态（CaptureRight=false 或排除进程）：旧逻辑，不拦截 ----
            if (!_drawing) return;
            if (_points.Count == 1 && Distance(_downPoint, p) < MinMoveToStart) return;

            var last = _points[_points.Count - 1];
            if (last == p) return; // 同一点跳过

            _points.Add(p);
            Redraw();
        }

        private void OnRightUp(Point p, KeyboardMouseHook.MouseEventArgs e)
        {
            // ---- 观察态：事件放行，沿用旧行为 ----
            if (!_captured)
            {
                if (!_drawing) return;
                _drawing = false;
                AnalyzeAndShow();
                return;
            }

            // ---- 接管态：吞掉抬起，目标程序不会收到“无 DOWN 配对的孤立 UP” ----
            e.Handled = true;

            bool gesture = _gesture;

            // 把手势最后一个点补上松开时的坐标，让路径完整
            if (gesture && _points.Count > 0 && _points[_points.Count - 1] != p)
                _points.Add(p);

            if (gesture)
            {
                // 手势成立：不补发点击（右键菜单不弹出），只做方向/闭环分析并清画布
                AnalyzeAndShow();
            }
            else
            {
                // 未构成手势 = 用户只是普通右键点击 → 补发一次完整 DOWN+UP，还原原右键点击/菜单
                ReplayRightClick();
                TryClearCanvas();
            }

            ResetCapture();
        }

        /// <summary>
        /// 安全补发一次完整右键点击：DOWN/UP 成对，try/finally 保证 UP 必发，
        /// 即使补发过程中出现异常也不会把系统留在按下状态。
        /// 补发事件带 isVir 标记（mouse_event 内部自动写入），钩子回调会过滤，不会自触发。
        /// </summary>
        private void ReplayRightClick()
        {
            Task.Run(() =>
            {
                try
                {
                    mouse_event(Native.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                    Thread.Sleep(15);
                }
                finally
                {
                    mouse_event(Native.MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                }
            });
        }

        /// <summary>无条件复位捕获账本（Stop/Dispose/异常/新 DOWN / 钩子卸载时调用）。</summary>
        private void ResetCapture()
        {
            _captured = false;
            _gesture = false;
            _drawing = false;
            _points.Clear();
        }

        /// <summary>手势分析（清空点集前）+ 状态栏提示 + 清画布。</summary>
        private void AnalyzeAndShow()
        {
            string gestureText = "";
            try
            {
                var dir = LineGestureAnalyzer.GetDirection(_points);
                bool circle = LineGestureAnalyzer.IsClosedCircle(_points);
                if (circle) gestureText = "手势：闭环圆圈";
                else if (dir != LineGestureAnalyzer.Direction.None)
                    gestureText = "手势：" + LineGestureAnalyzer.DirectionText(dir);
            }
            catch { }

            _points.Clear();

            Common.LabelTick = 1;
            Common.LabelTicking = true;
            if (gestureText != "")
                huan?.Invoke2(() => huan.label1.Text = gestureText);

            TryClearCanvas();
        }

        /// <summary>整条路径重绘（原有实现，抽出来给观察态与接管态共用）。</summary>
        private void Redraw()
        {
            try
            {
                if (_points.Count == 0 || !_canvas.Started) return;
                var g = _canvas.BeginDraw();
                g.Clear(Color.Transparent);
                if (_points.Count >= 2)
                {
                    using (var path = new GraphicsPath())
                    {
                        path.AddLines(_points.ToArray());
                        //g.DrawPath(_borderPen, path);
                        g.DrawPath(_linePen, path);
                    }
                }
                g.Flush();
                _canvas.EndDraw();
                _canvas.Present();
            }
            catch { }
        }

        /// <summary>清空画布（失败不影响状态机）。</summary>
        private void TryClearCanvas()
        {
            try
            {
                if (!_canvas.Started) return;
                var g = _canvas.BeginDraw();
                g.Clear(Color.Transparent);
                g.Flush();
                _canvas.EndDraw();
                _canvas.Present();
            }
            catch { }
        }

        /// <summary>当前进程是否在排除名单内（这些程序不接管右键，保留右键按住/拖拽语义）。</summary>
        private bool IsExcludedProcess()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ExcludeProcesses)) return false;
                var pn = Common.ProcessName;
                if (string.IsNullOrEmpty(pn)) return false;
                foreach (var s in ExcludeProcesses.Split(','))
                {
                    if (s.Length == 0) continue;
                    if (pn.Equals(s.Trim(), StringComparison.OrdinalIgnoreCase)) return true;
                }
            }
            catch { }
            return false;
        }

        /// <summary>读整型配置（本类内用，避免依赖 Common 的 private 辅助方法）。</summary>
        private static int LoadCfgInt(string key, int def)
        {
            try
            {
                var v = GetConfigValue(key);
                if (string.IsNullOrWhiteSpace(v)) return def;
                return int.TryParse(v.Trim(), out var r) ? r : def;
            }
            catch { return def; }
        }

        /// <summary>读字符串配置。</summary>
        private static string LoadCfgStr(string key, string def)
        {
            try
            {
                var v = GetConfigValue(key);
                return string.IsNullOrWhiteSpace(v) ? def : v.Trim();
            }
            catch { return def; }
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
