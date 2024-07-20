using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using Timer = System.Timers.Timer;
using KeyboardHooksd____;
using System.Text;
using PortAudioSharp;
using Pinyin4net.Format;
using Pinyin4net;
using System;
using System.Windows.Forms;

namespace keyupMusic2
{
    public partial class Huan : Form
    {
        static int last_index;
        static Dictionary<char, Keys> charToKeyMap = new Dictionary<char, Keys>
        {
            { 'a',Keys.A},
 {'b',Keys.B},
 {'c',Keys.C},
 {'d',Keys.D},
 {'e',Keys.E},
 {'f',Keys.F},
 {'g',Keys.G},
 {'h',Keys.H},
 {'i',Keys.I},
 {'j',Keys.J},
 {'k',Keys.K},
 {'l',Keys.L},
 {'m',Keys.M},
 {'n',Keys.N},
 {'o',Keys.O},
 {'p',Keys.P},
 {'q',Keys.Q},
 {'r',Keys.R},
 {'s',Keys.S},
 {'t',Keys.T},
 {'u',Keys.U},
 {'v',Keys.V},
 {'w',Keys.W},
 {'x',Keys.X},
 {'y',Keys.Y},
 {'z',Keys.Z},
        };

        static string ConvertChineseToPinyin(string chineseText)
        {
            var hanyuPinyinOutputFormat = new HanyuPinyinOutputFormat
            {
                ToneType = HanyuPinyinToneType.WITHOUT_TONE,// 是否带声调   // 可以选择其他音调风格，如TONE2, NORMAL等  
                CaseType = HanyuPinyinCaseType.LOWERCASE, // 拼音的大小写  
                //VCharType = HanyuPinyinVCharType.WITH_U_UNICODE, 
            };

            var pinyinBuilder = new StringBuilder();
            foreach (var c in chineseText)
            {
                //if (char.IsLetterOrDigit(c)) // 如果已经是字母或数字，则直接添加  
                //{
                //    pinyinBuilder.Append(c);
                //}
                //else
                try
                {
                    var asd = PinyinHelper.ToHanyuPinyinStringArray(c, hanyuPinyinOutputFormat);
                    if (asd != null && asd.Length > 0)
                        pinyinBuilder.Append(asd[0]);
                }
                catch (Exception sad)
                {
                    pinyinBuilder.Append(c); // 这里选择添加原字符
                }
            }

            return pinyinBuilder.ToString();
        }

        private void hook_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.PageDown))
            {
                FocusProcess("scrcpy");
                mouse_move(points[0].X, points[0].Y);
                mouse_click();
            }
            else if (e.KeyCode.Equals(Keys.PageUp))
            {
                Point screenPoint = Cursor.Position;
                points[0] = screenPoint;
                File.WriteAllText("point.txt", points[0].X + "," + points[0].Y);
            }
            else return;
        }


        //public static void Record()
        //{
        //    aTimer = new Timer(int1); // 设置计时器间隔为 3000 毫秒  
        //    aTimer.Elapsed += OnTimedEvent22; // 订阅Elapsed事件  
        //    aTimer.AutoReset = true; // 设置计时器是重复还是单次  
        //    aTimer.Enabled = true; // 启动计时器  
        //}

        private void OnTimedEvent2()
        {
            aTimer = new Timer(int1); // 设置计时器间隔为 3000 毫秒  
            aTimer.Elapsed += OnTimedEvent22; // 订阅Elapsed事件  
            aTimer.AutoReset = true; // 设置计时器是重复还是单次  
            aTimer.Enabled = true; // 启动计时器  
        }
        static bool is_changeing = false;
        private void OnTimedEvent22(Object? source, ElapsedEventArgs e)
        {
            string current = GetWindowText(GetForegroundWindow());
            log(is_changeing + "" + current + "");
            if (is_changeing) { }
            else if (current == null) { }
            else if (current.IndexOf(Process.GetCurrentProcess().ProcessName) == 0) { }
            else if (GetWindowText(GetForegroundWindow()) == "ACPhoenix")
            {
                is_changeing = true;
                aTimer = new Timer(int2); // 设置计时器间隔为 3000 毫秒  
                aTimer.Elapsed += OnTimedEvent; // 订阅Elapsed事件  
                aTimer.Enabled = true; // 启动计时器  
                aTimer.AutoReset = false; // 设置计时器是重复还是单次  
            }
            //UpdateUIFromBackgroundThread();

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
        private static void OnTimedEvent(Object? source, ElapsedEventArgs e)
        {
            FocusProcess(Process.GetCurrentProcess().ProcessName);
            is_changeing = false;
        }
        Point[] points = new Point[10];

        public void startListen()
        {
            myKeyEventHandeler_down = new KeyEventHandler(hook_KeyDown);
            k_hook.KeyDownEvent += myKeyEventHandeler_down;
            k_hook.Start();
        }
        static int int1 = 100;
        static int int2 = 100;
        public void stopListen()
        {
            if (myKeyEventHandeler_down != null)
            {
                k_hook.KeyDownEvent -= myKeyEventHandeler_down;
                k_hook.Stop();
            }
        }
        protected override void Dispose(bool disposing)
        {
            stopListen();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        public static void mouse_move(int x, int y)
        {
            Console.WriteLine(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString("#000") + " mouse_move" + x + "," + y);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
        }
        public static void mouse_click()
        {
            Console.WriteLine(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString("#000") + " mouse_click");
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        public static void mouse_click2()
        {
            Console.WriteLine(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString("#000") + " mouse_click");
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }
        static int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        static int screenHeight = Screen.PrimaryScreen.Bounds.Height;

        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        const int MOUSEEVENTF_MOVE = 0x0001;
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        private static void FocusProcess(string procName)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = IntPtr.Zero;
                hWnd = objProcesses[0].MainWindowHandle;
                ShowWindowAsync(new HandleRef(null, hWnd), SW_RESTORE);
                SetForegroundWindow(objProcesses[0].MainWindowHandle);
            }
        }
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(HandleRef hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr WindowHandle);
        public const int SW_RESTORE = 9;
        private static Timer aTimer = new Timer(100);
        KeyEventHandler myKeyEventHandeler_down;
        KeyboardHook k_hook = new KeyboardHook();

        private void load_point()
        {
            string point = File.ReadAllText("point.txt");
            if (point == "") point = "0,0";
            int x = int.Parse(point.Split(',')[0]);
            int y = int.Parse(point.Split(',')[1]);
            points[0] = new Point(x, y);
        }

        public static void press(Keys num, int tick = 0)
        {
            press([num], tick);
            return;
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
