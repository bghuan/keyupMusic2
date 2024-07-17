using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using Timer = System.Timers.Timer;
using KeyboardHooksd____;
using System.Text;

namespace keyupMusic2
{
    public partial class Form1 : Form
    {
        static int int1 = 100;
        static int int2 = 100;
        public Form1()
        {
            InitializeComponent();
            startListen();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OnTimedEvent2();
            //this.WindowState = FormWindowState.Minimized;
            //SetVisibleCore(false);
            string asd = File.ReadAllText("point.txt");
            if (asd == "") asd = "0,0";
            int x = int.Parse(asd.Split(',')[0]);
            int y = int.Parse(asd.Split(',')[1]);
            points[0] = new Point(x, y);
        }

        private void hook_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.PageDown))
            {
                FocusProcess("scrcpy");
                mouse_move(points[0].X, points[0].Y);
                mouse_click();
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
        //    aTimer = new Timer(int1); // ���ü�ʱ�����Ϊ 3000 ����  
        //    aTimer.Elapsed += OnTimedEvent22; // ����Elapsed�¼�  
        //    aTimer.AutoReset = true; // ���ü�ʱ�����ظ����ǵ���  
        //    aTimer.Enabled = true; // ������ʱ��  
        //}

        private void OnTimedEvent2()
        {
            aTimer = new Timer(int1); // ���ü�ʱ�����Ϊ 3000 ����  
            aTimer.Elapsed += OnTimedEvent22; // ����Elapsed�¼�  
            aTimer.AutoReset = true; // ���ü�ʱ�����ظ����ǵ���  
            aTimer.Enabled = true; // ������ʱ��  
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
                aTimer = new Timer(int2); // ���ü�ʱ�����Ϊ 3000 ����  
                aTimer.Elapsed += OnTimedEvent; // ����Elapsed�¼�  
                aTimer.Enabled = true; // ������ʱ��  
                aTimer.AutoReset = false; // ���ü�ʱ�����ظ����ǵ���  
            }
            //UpdateUIFromBackgroundThread();

        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // ����user32.dll�е�GetForegroundWindow����
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        // ����user32.dll�е�GetWindowText����
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        // ��ȡ���ڱ���ĸ�������
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
        public static void log(string message)
        {
            //log(GetWindowText(GetForegroundWindow()));
            File.WriteAllText("log.txt", DateTime.Now.ToString("") + "��" + message + "\n");
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

        private void UpdateUIFromBackgroundThread()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(UpdateUIThreadSafe));
            }
            else
            {
                UpdateUIThreadSafe();
            }
        }
        static int i = 0;
        private void UpdateUIThreadSafe()
        {
            //this.WindowState = FormWindowState.Minimized;

            SetVisibleCore((i++) % 5 == 0);
        }
    }
}
