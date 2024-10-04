using Microsoft.VisualBasic.Devices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
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
        public const string keyupMusic3 = "keyupMusic3";
        public const string keyupMusic3exe = "C:\\Users\\bu\\source\\repos\\keyupMusic3\\bin\\Debug\\net8.0-windows\\keyupMusic3.exe";
        public const string ACPhoenix = "ACPhoenix";
        public const string Dragonest = "DragonestGameLauncher";
        public const string devenv = "devenv";
        public const string WeChat = "WeChat";
        public const string douyin = "douyin";
        public const string douyinexe = "C:\\Program Files (x86)\\ByteDance\\douyin\\x64\\4.4.0\\douyin.exe";
        public const string msedge = "msedge";
        public const string chrome = "chrome";
        public const string Taskmgr = "Taskmgr";
        public const string explorer = "explorer";
        public const string SearchHost = "SearchHost";
        public const string QQMusic = "QQMusic";
        public const string HuyaClient = "HuyaClient";
        public const string QyClient = "QyClient";
        public const string ApplicationFrameHost = "ApplicationFrameHost";
        public const string QQLive = "QQLive";

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
        HuyaClient,
        ApplicationFrameHost,
        QyClient,
        QQLive,
        QQMusic,
        QQMusic,
        QQMusic,
        QQMusic,
        QQMusic,
        };
        public static List<string> list2 = new List<string> { };


        public static SoundPlayer player = new SoundPlayer();
        public static bool hooked = false;
        public static bool stop_listen = false;
        public static bool ACPhoenix_mouse_hook = false;
        public static DateTime special_delete_key_time;

        public static string ProcessName = "";
        public static string ProcessName2
        {
            get
            {
                //ProcessName = yo();
                yo();
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

            //string windowTitle = GetWindowText(hwnd);
            //Console.WriteLine("当前活动窗口名称: " + windowTitle);

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
            Common.ProcessName = ProcessName;
            return ProcessName;
        }
        static string proc_info = "";
        public static string log_process(string key = "")
        {
            IntPtr hwnd = GetForegroundWindow(); // 获取当前活动窗口的句柄
            string a = "";

            string windowTitle = GetWindowText(hwnd);
            //a += ("当前活动窗口名称: " + windowTitle);

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
            var curr_proc_info = ProcessName + " " + windowTitle + " " + fildsadsePath;
            a = key;
            if (proc_info != curr_proc_info) a = key + " " + curr_proc_info;
            proc_info = ProcessName + " " + windowTitle + " " + fildsadsePath;
            log(a);
            //log(DateTime.Now.ToString("") + " " + windowTitle + " " + fildsadsePath + module_nasme + "\n");
            return ProcessName;
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
                catch (Exception e)
                {
                    string msg = e.Message;
                }
                finally
                {
                    string fff = "ffs";
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
        public static void cmd(string cmd, Action action = null)
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
                if (action != null)
                    action();
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
        // 定义 MOUSEEVENTF_WHEEL 标志  
        public const int MOUSEEVENTF_WHEEL = 0x0800;
        public const int SW_RESTORE = 9;


        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //// 声明 mouse_event 函数  
        //[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);


        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(HandleRef hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr WindowHandle);

        public static void mouse_move(int x, int y, int tick = 0)
        {
            //x = x * 3840 / 2560;
            //y = y * 2160 / 1440;
            x += 1;
            y += 1;
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
        public static void mouse_move_center(int tick = 0)
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
        public static void hide_keyupmusic3()
        {
            {
                HideProcess(keyupMusic3);
                var _po = Position;
                press("2467.220", 110);
                if (judge_color(2467, 220, Color.FromArgb(196, 43, 28))) { press("2352,226", 10); }
                press(_po.X + "." + _po.Y, 0);
            }
        }
        public static bool ExsitProcess(string procName)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
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
        public static string AltTabProcess()
        {
            altab(100);
            return yo();
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
        public static void altab(int tick = 0)
        {
            press([Keys.LMenu, Keys.Tab]);
            Thread.Sleep(tick);
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
                {
                    //Thread.Sleep(10);
                    keybd_event((byte)item, 0, 0, 0);
                }
                Array.Reverse(keys);
                foreach (var item in keys)
                {
                    //Thread.Sleep(10);
                    keybd_event((byte)item, 0, 2, 0);
                }
            }
            Thread.Sleep(tick);
        }
        static Point mousePosition;
        public static Point lastPosition;
        public static void press_hold(Keys keys, int tick = 800)
        {
            _press_hold(keys, tick);
        }
        public static void press_middle_bottom()
        {
            press("1333.1439", 0);
        }

            //1 返回原来鼠标位置
            //2
            //3 跳过delete return
            public static void ctrl_shift(bool zh=true)
        {
            var flag = (judge_color(2289, 1411, Color.FromArgb(202, 202, 202)));
            if (zh && !flag)
                press(Keys.LShiftKey,10);
            else if(!zh && flag)
                press(Keys.LShiftKey, 10);
            return;
            if (zh)
                press([Keys.LControlKey, Keys.LShiftKey, Keys.D1]);
            else
                press([Keys.LControlKey, Keys.LShiftKey, Keys.D2]);
            return;

            //不准确
            var is_zh = InputLanguage.CurrentInputLanguage.Culture.Name == "zh-CH";
            foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
            {
                if (zh && lang.Equals(InputLanguage.DefaultInputLanguage))
                {
                    InputLanguage.CurrentInputLanguage = lang; return;
                }
                if (!zh && !lang.Equals(InputLanguage.DefaultInputLanguage))
                {
                    InputLanguage.CurrentInputLanguage = lang; return;
                }
            }

            if (is_zh && zh) return;
            if (!is_zh && !zh) return;
            press([Keys.LControlKey, Keys.LShiftKey]);
        }
        public static void press(string str, int tick = 800)
        {
            if (is_down(Keys.Delete) && (tick % 10) != 3) return;
            //KeyboardHook.stop_next = false;
            bool isLastDigitOne = (tick % 10) == 1;
            if (isLastDigitOne) mousePosition = Cursor.Position;
            var list = str.Split(";");
            list = list.Where(s => s != null && s != "").ToArray();
            if (list.Length == 0) return;
            foreach (var item in list)
            {
                var click = item.IndexOf(',');
                var move = item.IndexOf('.');

                if (item == "LWin")
                {
                    if (ProcessName == "SearchHost")
                        press([Keys.LControlKey, Keys.A, Keys.Back]);
                    else
                        press(Keys.LWin);
                    Thread.Sleep(100);
                    ctrl_shift(false);
                }
                else if (item == "zh")
                {
                    ctrl_shift(true);
                }
                else if (item == "en")
                {
                    ctrl_shift(false);
                }
                else if (item == "_") down_mouse();
                else if (item == "-") up_mouse();
                //else if (click > 0 && item.Substring(0, click + 1).IndexOf(",") > 0)
                //{ }
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
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // 导入user32.dll中的GetForegroundWindow函数
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        // 导入user32.dll中的GetWindowText函数
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        // 获取窗口标题的辅助方法
        public static string GetWindowText(IntPtr hWnd)
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
            //x = x * 3840 / 2560;
            //y = y * 2160 / 1440;
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
            //x = x * 3840 / 2560;
            //y = y * 2160 / 1440;
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
        public static bool try_press2(int x, int y, Color color, Action action = null, int similar = 70)
        {
            //var pos = Position;
            mouse_move(x, y);
            Thread.Sleep(10);
            var asd = get_mouse_postion_color(new Point(x, y));
            var flag = AreColorsSimilar(asd, color, similar);
            if (flag)
            {
                var flag2 = action != null;
                press(x + "," + y, flag2 ? 100 : 101);
                if (flag2) action();
                else lastPosition = Position;
            }
            log("try_press:" + x + "," + y + "," + color.R + "," + color.G + "," + color.B + " " + asd.R + "," + asd.G + "," + asd.B);
            //mouse_move(pos);
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
        public static void dragonest_notity_click(bool repeat = false)
        {
            Bitmap bitmap = new Bitmap(500, 1);
            int startX = 1800;
            int startY = 1397;
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(startX, startY, 0, 0, new System.Drawing.Size(500, 1));
            Rectangle rect = new Rectangle(0, 0, 500, 1);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
            bitmap.UnlockBits(bmpData);
            bool flag = false;
            var ds11a = DateTime.Now.ToString("ssfff");

            for (int i = 0; i < 500; i++)
            {
                int baseIndex = i * 4; // 每个像素4个字节（ARGB）  
                if (rgbValues[baseIndex + 2] == 233 && rgbValues[baseIndex + 1] == 81 && rgbValues[baseIndex] == 81)
                {
                    press($"{startX + i}, {startY}", 0);
                    i = 600;
                    flag = true;
                    break;
                }
            }
            if (flag == false && repeat == false)
            {
                press(Keys.LWin);
                Thread.Sleep(500);
                dragonest_notity_click(true);
            }
        }
        //占内存
        public static void copy_screen()
        {
            Screen secondaryScreen = Screen.PrimaryScreen;
            Bitmap bmpScreenshot = new Bitmap(secondaryScreen.Bounds.Width, secondaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(new Point(0, 0), Point.Empty, secondaryScreen.Bounds.Size);
            gfxScreenshot.Dispose();
            string aaa = "C:\\Users\\bu\\Pictures\\Screenshots\\";
            if (ProcessName == Common.ACPhoenix) aaa += "dd\\";
            bmpScreenshot.Save(aaa + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png", ImageFormat.Png);
        }
        static string mmapName = "Global\\MyMemoryMappedFile";
        static long mmapSize = 1024;
        public static string share_string = "";
        public static string share(string msg = "")
        {
            return "";
            if (string.IsNullOrEmpty(msg))
            {
                return share_string;
            }
            else
            {
                share_string = msg;
                return "";
            }

            if (string.IsNullOrEmpty(msg))
            {
                using (var mmf = MemoryMappedFile.CreateOrOpen(mmapName, mmapSize))
                {
                    using (var accessor = mmf.CreateViewAccessor())
                    {
                        byte[] buffer = new byte[mmapSize];
                        accessor.ReadArray(0, buffer, 0, buffer.Length);
                        string asd = Encoding.UTF8.GetString(buffer).TrimEnd('\0'); // 去除字符串末尾的null字符  
                        if (asd.Length > 0) share_string = asd;
                        return asd;
                    }
                }
            }
            else
            {
                Task.Run(() =>
                {
                    using (var mmf = MemoryMappedFile.CreateOrOpen(mmapName, mmapSize))
                    {
                        using (var accessor = mmf.CreateViewAccessor())
                        {

                            byte[] data = Encoding.UTF8.GetBytes(msg);
                            accessor.WriteArray(0, data, 0, data.Length);
                            Thread.Sleep(5000);
                            return "";
                        }
                    }
                });
                return "";
            }
        }
        public static void copy_secoed_screen(string path = "")
        {
            Screen secondaryScreen = Screen.AllScreens.FirstOrDefault(scr => !scr.Primary);
            int start_x = 2560;
            if (secondaryScreen == null) { return; }
            //if (secondaryScreen == null) { secondaryScreen = Screen.PrimaryScreen; start_x = 0; }
            //Bitmap bmpScreenshot = new Bitmap(secondaryScreen.Bounds.Width, secondaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            Bitmap bmpScreenshot = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(new Point(start_x, 0), Point.Empty, secondaryScreen.Bounds.Size);
            bmpScreenshot.Save("image\\encode\\" + path + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png" + "g", ImageFormat.Png);
            gfxScreenshot.Dispose();
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
        public static void ProcessRun(string str)
        {
            ProcessStartInfo startInfo2 = new ProcessStartInfo(str);
            startInfo2.UseShellExecute = true;
            startInfo2.Verb = "runas";
            Process.Start(startInfo2);
        }
        public static void DaleyRun(Func<bool> action, int alltime, int tick)
        {
            while (alltime > 0)
            {
                Thread.Sleep(tick);
                alltime -= tick;
                var asd = action.Invoke();
                if (asd) break;
            }
        }
        public static bool DaleyRun_stop = false;
        public static void DaleyRun(Func<bool> flag_action, Action action2, int alltime, int tick)
        {
            if (DaleyRun_stop) return;
            DaleyRun_stop = false;
            int i = 0;
            while (alltime >= 0)
            {
                if (i > 6000) break;
                if (DaleyRun_stop) break;
                Thread.Sleep(tick);
                alltime -= tick;
                var asd = flag_action.Invoke();
                if (asd) { action2(); break; }
            }
        }
        public static string DateTimeNow()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string DateTimeNow2()
        {
            return DateTime.Now.ToString("HH:mm:ss fff");
        }
        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);

        public static void Sleep(int tick)
        {
            Thread.Sleep(tick);
        }
    }
}
