using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WGestures.Common.OsSpecific.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Win32.User32;
using Point = System.Drawing.Point;

namespace keyupMusic2
{
    public class Common
    {
        public const string keyupMusic2 = "keyupMusic2";
        public const string ACPhoenix = "ACPhoenix";
        public const string DragonestGameLauncher = "DragonestGameLauncher";
        public const string devenv = "devenv";
        public const string WeChat = "WeChat";
        public const string douyin = "douyin";

        public static bool hooked = false;
        public static bool ACPhoenix_mouse_hook = false;

        private static readonly object _lockObject = new object();
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

        static int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        static int screenHeight = Screen.PrimaryScreen.Bounds.Height;

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
            Thread.Sleep(tick);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
        }
        public static void mouse_click(int tick = 0)
        {
            Console.WriteLine(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString("#000") + " mouse_click");
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(tick);
        }
        public static void mouse_down(int tick = 0)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        }
        public static void mouse_up(int tick = 0)
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
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
            press([num], tick);
            return;
        }
        static Point mousePosition;
        public static void press(string str, int tick = 800)
        {
            bool isLastDigitOne = (tick & 1) == 1;
            if (isLastDigitOne) mousePosition = Cursor.Position;
            //C.log($"Mouse Position: X={mousePosition.X}, Y={mousePosition.Y}");
            var list = str.Split(";");
            list = list.Where(s => s != null && s != "").ToArray();
            if (list.Length == 0) return;
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item)) continue;
                if (item.IndexOf(',') >= 0)
                {
                    mouse_move(Int32.Parse(item.Split(",")[0]), Int32.Parse(item.Split(",")[1]));
                    Thread.Sleep(10);
                    mouse_click();
                }
                else if ((int.TryParse(item, out int number)))
                {
                    Thread.Sleep(number);
                }
                else
                {
                    if (Enum.TryParse(typeof(Keys), item, out object asd))
                    {
                        press((Keys)asd);
                    }
                    else if (item.Length > 1)
                    {
                        press(item.Substring(0, 1), 1);
                        if (item.Length > 1)
                            press(item.Substring(1, item.Length - 1), 1);
                    }
                }
                if (!ReferenceEquals(item, list.Last()))
                    Thread.Sleep(tick);
            }
            if (isLastDigitOne && Cursor.Position.X < 3000)
                mouse_move(mousePosition.X, mousePosition.Y, 100);
        }
        public static void _press(Keys keys)
        {
            keybd_event((byte)keys, 0, 0, 0);
            keybd_event((byte)keys, 0, 2, 0);
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
    }
}
