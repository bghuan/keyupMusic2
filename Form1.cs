using System.Diagnostics;
using System.Runtime.InteropServices;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public partial class Huan : Form
    {
        ACPhoenixClass ACPhoenix;
        DevenvClass Devenv;
        DouyinClass Douyin;
        OtherClass Other;
        AllClass All;
        SuperClass Super;
        ChromeClass Chrome;
        WinClass Win;
        public OpencvReceive Opencv;
        public LongPressClass LongPress;
        bool is_init_show = !(Position.Y == 0);
        bool is_mouse_hook = !(Position.Y == 1439);
        public static bool keyupMusic2_onlisten = false;
        DateTime super_listen_time = new DateTime();
        static int super_listen_tick = 144 * 14;
        public MouseKeyboardHook _mouseKbdHook;
        Double timerMove_Tick_tick = super_listen_tick;
        Keys[] special_key = { Keys.F22, Keys.RMenu, Keys.RWin };
        private Point startPoint = new Point(1510, 100);
        private Point endPoint = new Point(2250, 100);
        private DateTime startTime;

        public Huan()
        {
            if (start_check()) return;
            InitializeComponent(); 
            //FormBorderStyle = FormBorderStyle.None;
            //this.BackColor = Color.FromArgb(255, 1, 1, 1);  // 几乎黑色，但不完全是
            //this.TransparencyKey = this.BackColor;
            //label1= new Label
            //{
            //    Text = "这是黑色文本",
            //    Font = new Font("Arial", 24, FontStyle.Bold),
            //    ForeColor = Color.Black,                    // 纯黑色文本
            //    BackColor = Color.Transparent,              // 标签背景透明
            //    Size = new Size(300, 100),
            //    Location = new Point(50, 50),
            //    TextAlign = ContentAlignment.MiddleCenter
            //};        // 标签背景透明

            try_restart_in_admin();
            //release_all_key();
            startListen();

            ACPhoenix = new ACPhoenixClass();
            Devenv = new DevenvClass();
            Douyin = new DouyinClass(this);
            Other = new OtherClass();
            All = new AllClass();
            Super = new SuperClass(this);
            Chrome = new ChromeClass();
            LongPress = new LongPressClass(this);
            Win = new WinClass(this);

            new Socket(this);
            new Tick(this); ;
            Opencv = new OpencvReceive(this);
        }
        protected override void Dispose(bool disposing)
        {
            stopListen();
            if (Socket.listener != null && Socket.listener.Server.IsBound)
            {
                Socket.listener.Stop();
            }

            if (Socket.client != null && Socket.client.Connected)
            {
                Socket.stream?.Close();
                Socket.client.Close();
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Huan_ResizeEnd(object sender, EventArgs e)
        {
            SetVisibleCore(false);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Dispose();
        }
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (Enum.TryParse(typeof(Keys), e.ClickedItem.Text.Substring(0, 1), out object key))
            {
                //var args = new KeyboardHookEventArgs(KeyboardType.KeyDown, (Keys)key, 0, new Native.keyboardHookStruct());
                //super_listen();
                //KeyBoardHookProc(args);
                //args.Type = KeyboardType.KeyUp;
                //KeyBoardHookProc(args);
                //press((Keys)key);

                SuperClass.hook_KeyDown((Keys)key);
            }
        }
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) SetVisibleCore(!Visible);
        }
        private void Huan_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                SetVisibleCore(false);
            }
        }
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(value);
            key_sound = value;
            //if (!value)
            //    Task.Run(() => { Sleep(200); player.Stop(); });
        }
        private void label1_Click(object sender, EventArgs e)
        {
            // 创建并显示WebView2窗口
            //MoonTime webViewWindow = new MoonTime();
            //webViewWindow.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Debugger.IsAttached)
            {
                int currentProcessId = Process.GetCurrentProcess().Id;
                Process[] processes = Process.GetProcessesByName(Common.keyupMusic2);
                foreach (Process process in processes)
                    if (process.Id != currentProcessId && IsAdministrator())
                        process.Kill();
                //mouse_move(screenWidth2,screenHeight2);
            }
            is_init_show = Debugger.IsAttached ? !is_init_show : is_init_show;
            if (is_init_show)
            {
                //SetVisibleCore(false);
                TaskRun(() => { Invoke(() => SetVisibleCore(false)); }, 100);
            }
            //Location = new Point(Screen.PrimaryScreen.Bounds.Width - 310, 100);
            Location = new Point(2255, 37);

            startPoint = new Point(Location.X - 252, Location.Y);
            endPoint = Location;
            after_load();
        }
        void after_load()
        {
            bland_title();
            if (!ExistProcess(TwinkleTray)) { ProcessRun(TwinkleTrayexe); }

            Common.FocusProcess(Common.Glass2);
            Common.FocusProcess(Common.Glass3);

            //if (!Debugger.IsAttached) return;
            // 创建并显示WebView2窗口
            MoonTime webViewWindow = new MoonTime();
            webViewWindow.Show();

        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // 添加 WS_EX_TOOLWINDOW 样式
                cp.ExStyle |= 0x80;  // WS_EX_TOOLWINDOW
                return cp;
            }
        }
    }
}
