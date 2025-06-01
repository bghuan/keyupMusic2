using System.Diagnostics;
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

        public static bool no_sleep = true;
        public Huan()
        {
            if (start_check()) return;
            InitializeComponent();

            try_restart_in_admin();
            release_all_key();
            startListen();

            ACPhoenix = new ACPhoenixClass();
            Devenv = new DevenvClass();
            Douyin = new DouyinClass(this);
            Other = new OtherClass();
            All = new AllClass();
            Super = new SuperClass(this);
            Chrome = new ChromeClass();

            new TcpServer(this);
            new Tick();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int currentProcessId = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcessesByName(Common.keyupMusic2);
            foreach (Process process in processes)
                if (process.Id != currentProcessId && IsAdministrator())
                    process.Kill();

            if (is_init_show && !Debugger.IsAttached)
            {
                SetVisibleCore(false);
                TaskRun(() => { Invoke(() => SetVisibleCore(false)); }, 200);
            }
            //Location = new Point(Screen.PrimaryScreen.Bounds.Width - 310, 100);
            Location = new Point(2255, 37);

            startPoint = new Point(Location.X - 300, Location.Y);
            endPoint = Location;
            bland_title();
            if (!ExistProcess(TwinkleTray)) { ProcessRun(TwinkleTrayexe); }
        }
        protected override void Dispose(bool disposing)
        {
            stopListen();
            if (TcpServer.listener != null && TcpServer.listener.Server.IsBound)
            {
                TcpServer.listener.Stop();
            }

            if (TcpServer.client != null && TcpServer.client.Connected)
            {
                TcpServer.stream?.Close();
                TcpServer.client.Close();
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
                var args = new KeyboardHookEventArgs(KeyboardType.KeyDown, (Keys)key, 0, new Native.keyboardHookStruct());
                super_listen();
                hook_KeyDown(args);
                args.Type = KeyboardType.KeyUp;
                hook_KeyUp(args);
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
            if (temp_visiable) key_sound = false;
            if (!value) player.Stop();
        }
        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}
