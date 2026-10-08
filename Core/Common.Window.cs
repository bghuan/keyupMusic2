using Microsoft.Win32;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using static keyupMusic4.Native;
using Point = System.Drawing.Point;
using RECT = keyupMusic4.Native.RECT;
namespace keyupMusic4
{
    public partial class Common
    {
        public static Point ProcessLocation(string pro)
        {
            IntPtr targetWindowHandle = GetProcessID(pro);

            if (targetWindowHandle != IntPtr.Zero)
            {
                RECT rect;
                GetWindowRect(targetWindowHandle, out rect);
                return new Point(rect.Left, rect.Top);
            }
            return new Point(0, 0);
        }
        public static int WindowWidth(IntPtr hWnd = 0)
        {
            if (hWnd == 0)
                hWnd = Native.GetForegroundWindow();
            Native.RECT windowRect;
            if (!Native.GetWindowRect(hWnd, out windowRect))
            {
                return 0;
            }
            return windowRect.Right - windowRect.Left;
        }
        public static bool IsFullScreen(IntPtr hWnd = 0)
        {
            if (ProcessName == explorer) return false;
            if (hWnd == 0)
                hWnd = Native.GetForegroundWindow();
            Native.RECT windowRect;
            if (!Native.GetWindowRect(hWnd, out windowRect))
            {
                return false;
            }
            if (windowRect.Left < 0)
            {
                return windowRect.Left == screen2Width1 &&
                   windowRect.Top == 0 &&
                   windowRect.Right == 0 &&
                   windowRect.Bottom >= screen2Height;
            }
            if (windowRect.Left > Common.screenWidth)
            {
                return windowRect.Left == screen2X &&
                   windowRect.Top == 0 &&
                   windowRect.Right >= screen2Width1 &&
                   windowRect.Bottom >= screen2Height1;
            }
            //Thread.Sleep(1000); 
            // Check if the window covers the entire screen  
            return windowRect.Left == 0 &&
                   windowRect.Top == 0 &&
                   windowRect.Right >= screenWidth1 &&
                   windowRect.Bottom >= screenHeight1;
        }
        public static List<string> FullVedioTitles = new List<string>() { "nhub", "bilibili", "多多自走棋 梦境", "vip:88", "热门视频", "mster", "抖音", };
        public static bool IsFullVedio() => (ProcessName == msedge || ProcessName == chrome) && FullVedioTitles.Where(e => ProcessTitle.Contains(e)).Count() > 0 && IsFullScreen() && !judge_color(890, 38, Color.FromArgb(27, 33, 26), 1) && !judge_color(1336, 13, Color.FromArgb(0, 0, 0), 1);

        public static void quick_max_chrome(Point point = new Point())
        {
            if (!ExistProcess(PowerToysCropAndLock, true)) return;
            if (point == new Point()) point = Position;
            if (ProcessName == chrome)
            {
                if (GetPointName() != chrome) return;
                press(Keys.F11);
                CenterWindowOnScreen(chrome, true);
                //FocusProcess2(PowerToysCropAndLock);
                altabtab();
                if (ExistProcess(cs2)) { Sleep(10); mouse_click(); }
            }
            else
            {
                //放大
                //HideProcess(PowerToysCropAndLock);
                if (ProcessName == cs2) point = PositionMiddle;
                var pp = new Point(point.X - 450, point.Y - 450);
                MoveProcessWindow(chrome, pp);
                IntPtr targetWindowHandle = GetProcessID(chrome);
                FocusProcess(targetWindowHandle);
                //Sleep(1000);
                mouse_click2(2);
                CenterWindowOnScreen(chrome, true);
                press(Keys.F11);
                if (ExistProcess(cs2)) { press_middle_bottom(); }
            }
        }
        public static uint isVir = 3;
        public const uint isVirConst = 3;

        public static bool is_no_title(string targetWindowTitle)
        {
            IntPtr targetWindowHandle = GetProcessID(targetWindowTitle);

            if (targetWindowHandle != IntPtr.Zero)
            {
                int currentStyle = GetWindowLong(targetWindowHandle, GWL_STYLE);
                bool hasCaption = (currentStyle & WS_CAPTION) != 0;

                if (!hasCaption)
                    return true;
                return false;
            }
            return false;
        }
        public static int PowerToysCropAndLock_Height = 0;
        public static int PowerToysCropAndLock_Wight = 0;
        public static double PowerToysCropAndLock_delta = 1;
        public static bool hideProcessTitle(string targetWindowTitle)
        {
            IntPtr targetWindowHandle = GetProcessID(targetWindowTitle);

            if (targetWindowHandle != IntPtr.Zero)
            {
                // 获取当前窗口样式
                int currentStyle = GetWindowLong(targetWindowHandle, GWL_STYLE);
                bool hasCaption = (currentStyle & WS_CAPTION) != 0;

                if (!hasCaption)
                    return false;
                // 移除标题栏、边框和调整大小边框的样式
                int newStyle = currentStyle & ~(WS_CAPTION | WS_BORDER | WS_THICKFRAME);

                // 设置新的窗口样式
                SetWindowLong(targetWindowHandle, GWL_STYLE, newStyle);

                // 获取当前窗口的位置和大小
                RECT rect;
                GetWindowRect(targetWindowHandle, out rect);

                //int newWidth = rect.Right - rect.Left - 22;
                //int newWidth = rect.Right - rect.Left - 26;
                //int newHeight = rect.Bottom - rect.Top - 55;
                //PowerToysCropAndLock_Height = newHeight;
                //PowerToysCropAndLock_Wight = newWidth;

                // 动态计算调整值（根据DPI和窗口类型自适应）
                var dpiScale = GetWindowScalingFactor(targetWindowHandle);
                int borderAdjust = (int)Math.Ceiling(2 * dpiScale); // 基础边框调整
                int captionAdjust = (int)Math.Ceiling(30 * dpiScale); // 标题栏高度调整

                // 计算新尺寸（核心优化：更精确的尺寸补偿）
                int newWidth = rect.Right - rect.Left - borderAdjust - 18;
                int newHeight = rect.Bottom - rect.Top - (captionAdjust + borderAdjust / 2) - 8;

                var top = rect.Top;
                if (targetWindowTitle == Common.chrome)
                {
                    top += 80;
                    newHeight -= 80;
                }

                // 更新窗口布局，使修改生效，并调整窗口大小
                SetWindowPos(targetWindowHandle, IntPtr.Zero, rect.Left, top, newWidth, newHeight, SWP_FRAMECHANGED | SWP_NOMOVE);
                Console2.WriteLine("标题栏和边框已隐藏");
                return true;
            }
            else
            {
                Console2.WriteLine("未找到目标窗口");
            }
            return false;
        }
        public static void change_process_size(double delta)
        {
            IntPtr targetWindowHandle = GetProcessID(ProcessName);
            if (targetWindowHandle != IntPtr.Zero)
            {
                // 获取当前窗口的矩形
                RECT rect;
                Native.GetWindowRect(targetWindowHandle, out rect);

                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;
                if (PowerToysCropAndLock_Wight != 0)
                {
                    width = PowerToysCropAndLock_Wight;
                    height = PowerToysCropAndLock_Height;
                }
                else
                {
                    PowerToysCropAndLock_Wight = width;
                    PowerToysCropAndLock_Height = height;
                }

                double scaleFactor = 1.1; // 缩放比例，可根据需要调整

                double aspectRatio = (double)width / height; // 计算原始宽高比

                int newWidth, newHeight;

                newWidth = (int)(width / delta);
                newHeight = (int)(newWidth / delta);

                // 计算新的左上角位置，以左下角为基准
                int newLeft = rect.Left;
                int newTop = rect.Bottom - newHeight;

                // 移动窗口并调整大小
                Native.MoveWindow(targetWindowHandle, newLeft, newTop, newWidth, newHeight, true);
            }
        }
        public static void CenterWindowOnScreen(string targetWindowTitle, bool right = false)
        {
            // 查找目标窗口的句柄
            IntPtr targetWindowHandle = GetProcessID(targetWindowTitle);
            if (targetWindowHandle == IntPtr.Zero)
            {
                Console2.WriteLine($"未找到标题为 '{targetWindowTitle}' 的窗口。");
                return;
            }

            // 获取目标窗口的矩形信息
            RECT windowRect;
            if (!GetWindowRect(targetWindowHandle, out windowRect))
            {
                Console2.WriteLine("无法获取窗口的矩形信息。");
                return;
            }

            // 获取屏幕的矩形信息
            RECT screenRect;
            GetWindowRect(Native.GetDesktopWindow(), out screenRect);

            // 计算窗口的宽度和高度
            int windowWidth = windowRect.Right - windowRect.Left;
            int windowHeight = windowRect.Bottom - windowRect.Top;

            // 计算窗口在屏幕中间的位置
            int newX = (screenWidth - windowWidth) + 12;
            int newY = (screenHeight - windowHeight) / 2 + 1;
            if (right) newX = screenWidth1 - 7;

            if (windowRect.Left == 2515 && windowRect.Top == 304)
            {
                ShowWindow((targetWindowHandle), SW.SW_SHOWMAXIMIZED);
                return;
            }

            if (targetWindowTitle == chrome)
            {
                windowWidth = 1301;
                windowHeight = 817;
            }
            if (targetWindowTitle == chrome && screenHeight < 1200)
            {
                windowWidth = 1299;
                windowHeight = 815;
            }
            if (targetWindowTitle == wemeetapp)
            {
                windowWidth = 1927;
                windowHeight = 1132;
            }
            //if (targetWindowTitle == chrome)
            //{
            //    windowWidth = 1937;
            //    windowHeight = 1175;
            //}

            // 移动窗口到屏幕中间
            if (!MoveWindow(targetWindowHandle, newX, newY, windowWidth, windowHeight, true))
            {
                Console2.WriteLine("无法移动窗口到指定位置。");
            }
        }
        public static void CenterWindowOnScreen2(string targetWindowTitle, bool right = false)
        {
            // 查找目标窗口的句柄
            IntPtr targetWindowHandle = GetProcessID(targetWindowTitle);
            if (targetWindowHandle == IntPtr.Zero)
            {
                Console2.WriteLine($"未找到标题为 '{targetWindowTitle}' 的窗口。");
                return;
            }
            //// 获取当前窗口样式
            //int currentStyle = GetWindowLong(targetWindowHandle, GWL_STYLE);
            //bool hasCaption = (currentStyle & WS_CAPTION) != 0;
            //if (hasCaption)
            //{
            //    int newStyle = currentStyle & ~(WS_CAPTION | WS_BORDER | WS_THICKFRAME);
            //    SetWindowLong(targetWindowHandle, GWL_STYLE, newStyle);
            //}

            // 获取目标窗口的矩形信息
            RECT windowRect;
            if (!GetWindowRect(targetWindowHandle, out windowRect))
            {
                Console2.WriteLine("无法获取窗口的矩形信息。");
                return;
            }


            double windowWidth = 2904;
            double windowHeight = 1762;

            // 更新窗口布局，使修改生效，并调整窗口大小
            SetWindowPos(targetWindowHandle, IntPtr.Zero, 0, 0, (int)windowWidth, (int)windowHeight, SWP_FRAMECHANGED | SWP_NOMOVE);
            //// 移动窗口到屏幕中间
            //if (!MoveWindow(targetWindowHandle, newX, newY, windowWidth, windowHeight, true))
            //{
            //    Console.WriteLine("无法移动窗口到指定位置。");
            //}
        }
        public static void MoveProcessWindow(IntPtr targetWindowHandle, Point point)
        {
            RECT windowRect;
            GetWindowRect(targetWindowHandle, out windowRect);

            int windowWidth = windowRect.Right - windowRect.Left;
            int windowHeight = windowRect.Bottom - windowRect.Top;

            MoveWindow(targetWindowHandle, point.X, point.Y, windowWidth, windowHeight, true);
        }
        public static void MoveProcessWindow(string targetWindowTitle, Point point)
        {
            IntPtr targetWindowHandle = GetProcessID(targetWindowTitle);
            RECT windowRect;
            GetWindowRect(targetWindowHandle, out windowRect);

            int windowWidth = windowRect.Right - windowRect.Left;
            int windowHeight = windowRect.Bottom - windowRect.Top;

            MoveWindow(targetWindowHandle, point.X, point.Y, windowWidth, windowHeight, true);
            // 置顶：hWndInsertAfter=HWND_TOPMOST，忽略大小和位置变化（只修改置顶属性）
            SetWindowPos(
                targetWindowHandle,
                HWND_TOPMOST,
                0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW
            );
        }
        public static void MoveProcessWindow2(string targetWindowTitle, Keys key = Keys.None)
        {
            IntPtr targetWindowHandle = GetProcessID(targetWindowTitle);
            RECT windowRect;
            GetWindowRect(targetWindowHandle, out windowRect);

            int windowWidth = windowRect.Right - windowRect.Left;
            int windowHeight = windowRect.Bottom - windowRect.Top;
            int x = 0;
            if (Position.X == screenWidth1)
                x = screenWidth - windowWidth;
            int y = screenHeight - windowHeight;
            if (key != None)
            {
                x = windowRect.Left;
                y = windowRect.Top;
                if (key == Left) x = windowRect.Left - 1;
                if (key == Right) x = windowRect.Left + 1;
                if (key == Up) y = windowRect.Top - 1;
                if (key == Down) y = windowRect.Top + 1;
            }

            MoveWindow(targetWindowHandle, x, y, windowWidth, windowHeight, true);
        }
        // 系统指标常量
        private const int SM_CYCAPTION = 4;
        public static bool IsClickOnTitleBar(string ProcessName, Point clickPosition)
        {
            // 查找目标窗口的句柄
            IntPtr windowHandle = GetProcessID(ProcessName);
            // 获取窗口的矩形信息
            RECT windowRect;
            if (!GetWindowRect(windowHandle, out windowRect))
            {
                return false;
            }

            // 获取标题栏的高度
            int captionHeight = GetSystemMetrics(SM_CYCAPTION);

            // 将鼠标点击的屏幕坐标转换为窗口的客户区坐标
            Point clientPoint = clickPosition;
            ScreenToClient(windowHandle, ref clientPoint);

            // 判断点击位置是否在标题栏内
            return clientPoint.Y >= 0 && clientPoint.Y < captionHeight;
        }

        public static void SendMessageToWindow(string procName, string message)
        {
            int currentProcessId = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcessesByName(procName);
            foreach (Process process in processes)
            {
                if (process.Id != currentProcessId && !process.MainWindowHandle.Equals(IntPtr.Zero))
                {
                    // 获取主窗口句柄
                    IntPtr hWnd = process.MainWindowHandle;

                    // 发送消息
                    IntPtr lParam = Marshal.StringToHGlobalUni(message);
                    PostMessage(hWnd, CUSTOM_MESSAGE, IntPtr.Zero, lParam);
                    Marshal.FreeHGlobal(lParam);
                }
            }
        }
        //public static List<ReplaceKey> replace = ReplaceKey.replace;
        public static bool lock_err
        {
            get
            {
                var aaa =
                 ProcessName == err || ProcessName == LockApp || GetWindowTitle() == UnlockingWindow;
                if (aaa)
                {
                    string _ProcessName = ProcessName;
                    if (_ProcessName == "err")
                    {
                        uint processId = 0;
                        Native.GetWindowThreadProcessId(Native.GetForegroundWindow(), out processId);
                        _ProcessName = "err,processId:" + processId;
                    }
                    Log.log("GetWindowTitle():" + GetWindowTitle() + ",ProcessName:" + ProcessName);
                    if (ProcessName == err)
                        play_sound_di2();
                }
                return aaa;
            }
        }
        // 获取当前窗口的缩放比例
        public static double GetWindowScalingFactor(Form form)
        {
            uint dpi = GetDpiForWindow(form.Handle);
            return dpi / 96.0;
        }
        // 获取当前窗口的缩放比例
        public static double GetWindowScalingFactor(IntPtr form)
        {
            uint dpi = GetDpiForWindow(form);
            return dpi / 96.0;
        }
        public static int LabelTick { get => AppState.LabelTick; set => AppState.LabelTick = value; }
        public static bool LabelTicking { get => AppState.LabelTicking; set => AppState.LabelTicking = value; }
        public static void Show(string msg, int tick = 0)
        {
            Common.LabelTick = tick;
            Common.LabelTicking = true;
            huan.Invoke(() => { huan.label1.Text = msg; });
            //Socket.socket_write(Huan.huan_invoke + ProcessName + " " + msg);
        }

        //顺便进入睡眠
        public static void CloseDesktopWindow()
        {
            //if (ProcessName == explorer)
            //    press([LMenu, F4]);
            //return;
            IntPtr desktopHwnd = FindWindow("Progman", null);
            if (desktopHwnd != IntPtr.Zero)
            {
                PostMessage(desktopHwnd, (uint)WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }// 判断是否有音乐播放软件在运行

        public static bool SetWindowTransparency(string proname, byte alpha)
        {
            // 1. 获取目标窗口句柄（根据窗口标题）
            IntPtr hWnd = GetProcessID(proname);
            if (hWnd == IntPtr.Zero)
                return false;

            // 2. 获取当前扩展样式
            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);

            // 3. 设置分层样式
            exStyle |= WS_EX_LAYERED;

            // 4. 根据透明度决定是否添加/移除穿透样式
            if (alpha < 255)
                exStyle |= WS_EX_TRANSPARENT; // 添加穿透
            else
                exStyle &= ~WS_EX_TRANSPARENT; // 移除穿透

            SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);

            // 5. 设置透明度
            return SetLayeredWindowAttributes(hWnd, 0, alpha, LWA_ALPHA);
        }

        public static void SetTransparency(int num = 0)
        {
            is_tran_alpha = (byte)Math.Clamp(is_tran_alpha + num, 0, 255);
            if (num == 0) is_tran_alpha = 127;

            byte alpha = (byte)is_tran_alpha; // 半透明效果

            if (num == 0 && is_tran_powertoy2)
                alpha = 255;
            is_tran_powertoy2 = !is_tran_powertoy2;

            IntPtr hwnd = Native.GetForegroundWindow();
            int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            exStyle |= WS_EX_LAYERED;
            if (alpha < 255)
            {
                exStyle |= WS_EX_TRANSPARENT; // 添加穿透
                SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE); // 置顶
                is_tran_process = ProcessName;
                //altab();
            }
            else
            {
                exStyle &= ~WS_EX_TRANSPARENT; // 移除穿透
                SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE); // 取消置顶
                is_tran_process = "";
            }
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);
            SetLayeredWindowAttributes(hwnd, 0, alpha, LWA_ALPHA);
        }

        public static bool IsWindowTopMost(IntPtr hWnd)
        {
            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            return (exStyle & WS_EX_TOPMOST) == WS_EX_TOPMOST;
        }
        /// <summary>
        /// 切换置顶状态（Toggle）
        /// </summary>
        public static void ToggleTopMost()
        {
            IntPtr hWnd = Native.GetForegroundWindow();
            if (IsWindowTopMost(hWnd))
            {
                // 取消置顶
                SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
                Console.WriteLine("已取消置顶");
            }
            else
            {
                // 设置置顶
                SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
                Console.WriteLine("已设置置顶");
            }
        }

    }
}
