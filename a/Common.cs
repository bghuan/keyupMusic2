using Microsoft.VisualBasic.Devices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Core.Impl.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using static Win32.User32;
using Point = System.Drawing.Point;

namespace keyupMusic2
{
    public class Common
    {
        public const string keyupMusic2 = "keyupMusic2";
        public const string ACPhoenix = "ACPhoenix";
        public const string Dragonest = "DragonestGameLauncher";
        public const string devenv = "devenv";
        public const string WeChat = "WeChat";
        public const string douyin = "douyin";
        public const string msedge = "msedge";
        public const string chrome = "chrome";
        public const string Taskmgr = "Taskmgr";
        public const string explorer = "explorer";
        public const string SearchHost = "SearchHost";
        public const string QQMusic = "QQMusic";

        public static string[] list = {
        keyupMusic2,
        ACPhoenix,
        Dragonest,
        devenv,
        WeChat,
        douyin,
        msedge,
        chrome,
        Taskmgr,
        explorer,
        SearchHost,
        QQMusic,
        QQMusic,
        QQMusic,
        QQMusic,
        QQMusic,
        QQMusic,
        };

        public static bool hooked = false;
        public static bool ACPhoenix_mouse_hook = false;

        public static string ProcessName = "";
        public static string ProcessName2
        {
            get
            {
                ProcessName = yo();
                return ProcessName;
            }
            set
            {
                ProcessName = "";
            }
        }
        public static Point Position
        {
            get
            {
                return Cursor.Position;
            }
        }
        public static string yo()
        {
            IntPtr hwnd = GetForegroundWindow(); // 获取当前活动窗口的句柄

            string windowTitle = GetWindowText(hwnd);
            Console.WriteLine("当前活动窗口名称: " + windowTitle);

            var filePath = "a.txt";
            var fildsadsePath = "err";
            var module_name = "err";
            var ProcessName = "err";

            try
            {
                uint processId;
                GetWindowThreadProcessId(hwnd, out processId);
                using (Process process = Process.GetProcessById((int)processId))
                {
                    fildsadsePath = process.MainModule.FileName;
                    module_name = process.MainModule.ModuleName;
                    ProcessName = process.ProcessName;
                }
            }
            catch (System.Exception ex)
            {
                fildsadsePath = ex.Message;
            }

            //log(DateTime.Now.ToString("") + " " + windowTitle + " " + fildsadsePath + module_nasme + "\n");
            return ProcessName;
        }
        public static void log_process(string key = "")
        {
            IntPtr hwnd = GetForegroundWindow(); // 获取当前活动窗口的句柄
            string a = "";

            string windowTitle = GetWindowText(hwnd);
            a += ("当前活动窗口名称: " + windowTitle);

            var filePath = "a.txt";
            var fildsadsePath = "err";
            var module_name = "err";
            var ProcessName = "err";

            try
            {
                uint processId;
                GetWindowThreadProcessId(hwnd, out processId);
                using (Process process = Process.GetProcessById((int)processId))
                {
                    fildsadsePath = process.MainModule.FileName;
                    module_name = process.MainModule.ModuleName;
                    ProcessName = process.ProcessName;
                }
            }
            catch (System.Exception ex)
            {
                fildsadsePath = ex.Message;
            }
            a += key + " " + fildsadsePath + " " + module_name + " " + ProcessName + " " + fildsadsePath;
            log(a);
            //log(DateTime.Now.ToString("") + " " + windowTitle + " " + fildsadsePath + module_nasme + "\n");
            //return ProcessName;
        }

        private static readonly object _lockObject = new object();
        private static readonly object _lockObject2 = new object();
        public static void log(string message)
        {
            lock (_lockObject)
            {
                try
                {
                    File.AppendAllText("log.txt", "\r" + DateTime.Now.ToString("") + " " + message);
                }
                catch (Exception)
                {
                }
            }
        }

        public static bool is_down(Keys key)
        {
            return Native.GetAsyncKeyState(key) < 0;
        }

        public static bool is_ctrl()
        {
            return Native.GetAsyncKeyState(Keys.ControlKey) < 0;
        }
        public static bool is_alt()
        {
            return Native.GetAsyncKeyState(Keys.LMenu) < 0;
        }

        public static bool is_shift()
        {
            return Native.GetAsyncKeyState(Keys.ShiftKey) < 0;
        }
        public static void cmd(string cmd)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = cmd,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };

            using (Process process = Process.Start(startInfo))
            {
            }
        }

        public static int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        public static int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        public static int screenWidth2 = Screen.PrimaryScreen.Bounds.Width / 2;
        public static int screenHeight2 = Screen.PrimaryScreen.Bounds.Height / 2;

        const int MOUSEEVENTF_MOVE = 0x0001;
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        public const int SW_RESTORE = 9;


        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(HandleRef hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr WindowHandle);

        public static void mouse_move(int x, int y, int tick = 0)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
            Thread.Sleep(tick);
        }
        public static void mouse_move(int x, int y, int x2, int y2)
        {
            mouse_move(x, y);
            down_mouse();

            int times = 20;
            int all_times = 20;
            for (int i = 1; i < times + 1; i++)
            {
                int xx = 1;
                int yy = 1;
                if (x == x2) xx = 0;
                else xx = (x2 - x) / times * i;
                if (y == y2) yy = 0;
                else yy = (y2 - y) / times * i;

                mouse_move(x + xx, y + yy, all_times / times);
            }

            mouse_move(x2, y2);
            up_mouse();
        }
        public static void mouse_move3(int tick = 0)
        {
            int x = screenWidth / 2;
            int y = screenHeight / 2;
            Thread.Sleep(tick);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
        }
        public static void mouse_move(Point point, int tick = 0)
        {
            Thread.Sleep(tick);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, point.X * 65536 / screenWidth, point.Y * 65536 / screenHeight, 0, 0);
        }
        public static void mouse_move2(int x, int y, int tick = 0)
        {
            x += Cursor.Position.X;
            y += Cursor.Position.Y;
            Thread.Sleep(tick);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
        }
        public static void mouse_click(int tick = 10)
        {
            if (tick > 0)
            {
                down_mouse(tick);
                up_mouse(tick);
                return;
            }
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        public static void mouse_click(int x, int y, int tick = 10)
        {
            mouse_move(x, y);
            if (tick > 0)
            {
                down_mouse(tick);
                up_mouse(tick);
                return;
            }
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        public static void down_mouse(int tick = 0)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(tick);
        }
        public static void up_mouse(int tick = 0)
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(tick);
        }
        public static void mouse_click2()
        {
            Console.WriteLine(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString("#000") + " mouse_click");
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }
        public static bool FocusProcess(string procName)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = IntPtr.Zero;
                hWnd = objProcesses[0].MainWindowHandle;
                ShowWindow((hWnd), SW.SW_RESTORE);
                if (procName != Dragonest && procName != chrome && procName != devenv)
                    ShowWindowAsync(new HandleRef(null, hWnd), SW_RESTORE);
                //ShowWindow((hWnd), SW.SW_SHOWMAXIMIZED);
                //ShowWindow((hWnd), SW.SW_SHOW);
                //ShowWindow((hWnd), SW.SW_SHOWNA);
                SetForegroundWindow(objProcesses[0].MainWindowHandle);
                return true;
            }
            return false;
        }

        public static void HideProcess(string procName)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = objProcesses[0].MainWindowHandle;
                ShowWindow(hWnd, SW.SW_MINIMIZE);
            }
        }
        private void load_point()
        {
            string point = File.ReadAllText("point.txt");
            if (point == "") point = "0,0";
            int x = int.Parse(point.Split(',')[0]);
            int y = int.Parse(point.Split(',')[1]);
            //points[0] = new Point(x, y);
        }
        public static void press(Keys num, int tick = 0)
        {
            if (is_down(Keys.Delete)) return;
            lock (_lockObject2)
            {
                //bool flag = tick > 0 && tick % 10 == 2;
                //if (flag) MouseKeyboardHook.handling = true;
                press([num], tick);
                //if (flag) MouseKeyboardHook.handling = false;
            }
        }
        public static void close()
        {
            press([Keys.LMenu, Keys.F4]);
        }
        public static void altab()
        {
            press([Keys.LMenu, Keys.Tab]);
        }
        public static void press(Keys[] keys, int tick = 10)
        {
            if (keys == null || keys.Length == 0 || keys.Length > 100)
                return;
            if (keys.Length == 1)
            {
                _press(keys[0]);
            }
            else if (keys.Length > 1 && keys[0] == keys[1])
            {
                foreach (var key in keys)
                {
                    _press(key);
                };
            }
            else
            {
                foreach (var item in keys)
                    keybd_event((byte)item, 0, 0, 0);
                foreach (var item in keys)
                    keybd_event((byte)item, 0, 2, 0);
            }
            Thread.Sleep(tick);
        }
        static Point mousePosition;
        public static Point lastPosition;
        public static void press_hold(Keys keys, int tick = 800)
        {
            _press_hold(keys, tick);
        }
        public static void press(string str, int tick = 800)
        {
            if (is_down(Keys.Delete)) return;
            //KeyboardHook.stop_next = false;
            bool isLastDigitOne = (tick & 1) == 1;
            if (isLastDigitOne) mousePosition = Cursor.Position;
            var list = str.Split(";");
            list = list.Where(s => s != null && s != "").ToArray();
            if (list.Length == 0) return;
            foreach (var item in list)
            {
                var click = item.IndexOf(',');
                var move = item.IndexOf('.');

                if (item == "LWin" && ProcessName != "SearchHost")
                {
                    press(Keys.LWin);
                }
                else if (item == "_") down_mouse();
                else if (item == "-") up_mouse();
                else if (click > 0 || move > 0)
                {
                    var x = int.Parse(item.Substring(0, click + move + 1));
                    var y = int.Parse(item.Substring(click + move + 1 + 1));
                    mouse_move(x, y, 10);
                    if (click > 0) mouse_click(30);
                }
                else if ((int.TryParse(item, out int number)))
                {
                    Thread.Sleep(number);
                }
                else if (Enum.TryParse(typeof(Keys), item, out object asd))
                {
                    press((Keys)asd);
                }
                else if (item.Length > 1)
                {
                    press(item.Substring(0, 1), 10);
                    press(item.Substring(1), 10);
                }
                if (!ReferenceEquals(item, list.Last()) || list.Length == 1)
                    Thread.Sleep(tick);
            }
            if (isLastDigitOne && mousePosition.X < 2560)
                mouse_move(mousePosition.X, mousePosition.Y, 100);
        }
        public static void _press(Keys keys)
        {
            keybd_event((byte)keys, 0, 0, 0);
            keybd_event((byte)keys, 0, 2, 0);
        }
        public static void press_dump(Keys keys, int tick = 500)
        {
            keybd_event((byte)keys, 0, 0, 0);
            Thread.Sleep(tick);
            keybd_event((byte)keys, 0, 2, 0);
        }
        public static void _press_hold(Keys keys, int tick)
        {
            keybd_event((byte)keys, 0, 0, 0);
            Thread.Sleep(tick);
            keybd_event((byte)keys, 0, 2, 0);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // 导入user32.dll中的GetForegroundWindow函数
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        // 导入user32.dll中的GetWindowText函数
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        // 获取窗口标题的辅助方法
        private static string GetWindowText(IntPtr hWnd)
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            if (GetWindowText(hWnd, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        public static void TaskRun(Action action, int tick)
        {
            Task.Run(() =>
            {
                Thread.Sleep(tick);
                action();
            });
        }
        public static Color get_mouse_postion_color(Point point)
        {
            using (Bitmap bitmap = new Bitmap(1, 1))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(point.X, point.Y, 0, 0, new System.Drawing.Size(1, 1));
                    return bitmap.GetPixel(0, 0);
                }
            }
        }
        public static bool judge_color(int x, int y, Color color, Action action = null, int similar = 50)
        {
            var asd = get_mouse_postion_color(new Point(x, y));
            var flag = AreColorsSimilar(asd, color, similar);
            if (flag && action != null) action();
            return flag;
        }
        public static bool try_press(Color color, Action action = null)
        {
            return try_press(Position.X, Position.Y, color, action);
        }
        public static bool try_press(int x, int y, Color color, Action action = null)
        {
            var asd = get_mouse_postion_color(new Point(x, y));
            var flag = AreColorsSimilar(asd, color);
            if (flag)
            {
                var flag2 = action != null;
                press(x + "," + y, flag2 ? 100 : 101);
                if (flag2) action();
                else lastPosition = Position;
            }
            return flag;
        }
        public static bool AreColorsSimilar(Color color1, Color color2, int threshold = 50)
        {
            if (color1 == color2) return true;
            int rDiff = Math.Abs(color1.R - color2.R);
            int gDiff = Math.Abs(color1.G - color2.G);
            int bDiff = Math.Abs(color1.B - color2.B);

            return (rDiff + gDiff + bDiff) <= threshold;
        }
        public static void dragonest_notity_click()
        {
            using (Bitmap bitmap = new Bitmap(500, 1))
            {
                int startX = 1800;
                int startY = 1397;
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(startX, startY, 0, 0, new System.Drawing.Size(500, 1));
                }
                Rectangle rect = new Rectangle(0, 0, 500, 1);
                BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
                byte[] rgbValues = new byte[bytes];

                Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
                bitmap.UnlockBits(bmpData);

                for (int i = 0; i < 500; i++)
                {
                    int baseIndex = i * 4; // 每个像素4个字节（ARGB）  
                    if (rgbValues[baseIndex + 2] == 233 && rgbValues[baseIndex + 1] == 81 && rgbValues[baseIndex] == 81)
                    {
                        press($"{startX + i}, {startY}");
                        i = 600;
                        break;
                    }
                    //Color color = Color.FromArgb(rgbValues[baseIndex + 3], rgbValues[baseIndex + 2], rgbValues[baseIndex + 1], rgbValues[baseIndex]);
                    //if (color == Color.FromArgb(233, 81, 81))
                    //{
                    //    press($"{startX + i}, {startY}");
                    //    break;
                    //}
                }

                //while (sdasd < 500)
                //{
                //    log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                //    var aaa = bitmap.GetPixel(sdasd, 0);
                //    string xxxx = (startX + sdasd).ToString();
                //    string yyyy = (startY).ToString();
                //    sdasd++;
                //    if (aaa.R == 233 && aaa.G == 81 && aaa.B == 81)
                //    {
                //        //mouse_move(startX + sdasd, startY);
                //        press($"{startX + sdasd}, {startY}");
                //        break;
                //        //log($"111111111111({xxxx},{yyyy},{aaa.ToString()})");
                //    }
                //}
            }
        }
        //占内存
        public static void copy_secoed_screen()
        {
            Screen secondaryScreen = Screen.AllScreens.FirstOrDefault(scr => !scr.Primary);
            if (secondaryScreen == null) secondaryScreen = Screen.PrimaryScreen;
            Bitmap bmpScreenshot = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(new Point(2560, 0), Point.Empty, secondaryScreen.Bounds.Size);
            gfxScreenshot.Dispose();
            bmpScreenshot.Save("image\\encode\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png" + "g", ImageFormat.Png);
        }
        public static void copy_ddzzq_screen()
        {
            Screen secondaryScreen = Screen.PrimaryScreen;
            if (secondaryScreen != null)
            {
                Bitmap bmpScreenshot = new Bitmap(2560, 1440, PixelFormat.Format32bppArgb);
                Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.CopyFromScreen(new Point(0, 0), Point.Empty, secondaryScreen.Bounds.Size);
                gfxScreenshot.Dispose();
                bmpScreenshot.Save("C:\\Users\\bu\\Pictures\\Screenshots\\dd\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png", ImageFormat.Png);
            }
        }
    }
}
