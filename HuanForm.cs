using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;

namespace keyupMusic2
{
    public partial class Huan : Form
    {
        Blob blobForm = new Blob();
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
        //bool is_init_show = !(Position.Y == 0);
        bool is_mouse_hook = !(Position.Y == 1439);
        //bool is_mouse_hook = !is_shift();
        public static bool keyupMusic2_onlisten = false;
        DateTime super_listen_time = new DateTime();
        static int super_listen_tick = 144 * 14;
        public KeyboardMouseHook _mouseKbdHook;
        Double timerMove_Tick_tick = super_listen_tick;
        Keys[] special_key = { Keys.F22, Keys.RMenu, Keys.RWin };
        private Point startPoint = new Point(1510, 100);
        private Point endPoint = new Point(2250, 100);
        private DateTime startTime;

        public Huan()
        {
            if (start_check()) return;
            _instance = this;
            InitializeComponent();

            try_restart_in_admin();
            release_all_key();
            Rawinput.RegisterInputDevices(this.Handle);
            startListen();

            ACPhoenix = new ACPhoenixClass();
            Devenv = new DevenvClass();
            Douyin = new DouyinClass();
            Other = new OtherClass();
            All = new AllClass();
            Super = new SuperClass();
            Chrome = new ChromeClass();
            Win = new WinClass();

            new Socket();
            new Tick(); ;
            Opencv = new OpencvReceive();
            new KeyFunc2().init();
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
                SuperClass.hook_KeyDown((Keys)key);
            }
        }
        private void notifyIcon1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) SetVisibleCore(!Visible);
            if (e.Button == MouseButtons.Right)
            {
                // 假设“微亮键盘(显示隐藏)”是你要修改的项
                if (VirtualKeyboardForm.Instance != null && MoonTime.Instance != null)
                    foreach (ToolStripItem item in contextMenuStrip1.Items)
                    {
                        if (item is ToolStripMenuItem menuItem && menuItem.Name.Equals("moontimmeshow"))
                            menuItem.Text = $"月亮表({(MoonTime.Instance.Visible ? "已显示" : "已隐藏")})";
                        if (item is ToolStripMenuItem menuItem2 && menuItem2.Name.Equals("keyboardlightshow"))
                            menuItem2.Text = $"微亮键盘({(VirtualKeyboardForm.Instance.Visible ? "已显示" : "已隐藏")})";
                    }
                var mousePos = Control.MousePosition;
                int menuHeight = contextMenuStrip1.GetPreferredSize(contextMenuStrip1.Size).Height;
                contextMenuStrip1.Show(mousePos.X, mousePos.Y - menuHeight);
            }
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
            ConfigValue(ConfigFormShow, value ? "1" : "0");
            //if (!value)
            //    Task.Run(() => { Sleep(200); player.Stop(); });
        }
        public void SetVisibleCore2(bool value)
        {
            Invoke(() =>
            {
                SetVisibleCore(value);
            });
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
            }
            //is_init_show = Debugger.IsAttached ? !is_init_show : is_init_show;
            //Location = new Point(Screen.PrimaryScreen.Bounds.Width - 310, 100);
            Location = new Point(2255, 37);

            //startPoint = new Point(Location.X - 252, Location.Y);
            //endPoint = Location;
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;

            after_load();
        }
        private bool justResumed = false;

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Resume)
            {
                justResumed = true;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }
        void after_load()
        {
            FreshProcessName();
            bland_title();
            if (!ExistProcess(TwinkleTray)) { ProcessRun(TwinkleTrayexe); }
            //if (!Debugger.IsAttached) HideProcess(devenv);
            InitializeFromCurrentWallpaper();

            VirtualKeyboardForm virtualKeyboardForm = new VirtualKeyboardForm();
            virtualKeyboardForm.Show();
            MoonTime moontimeForm = new MoonTime();
            moontimeForm.Show();

            timerMove.Interval = 3000;
            timerMove.Tick += timerMove_Tick;
            form_move();

            if (ConfigValue(ConfigFormShow) == "0")
                //SetVisibleCore(false);
                TaskRun(() => { Invoke(() => SetVisibleCore(false)); }, 100);

            //var sss= ConfigValue(ConfigFormShow);
            if (isctrl())
                Native.AllocConsole();
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Rawinput.WM_INPUT)
            {
                var e = Rawinput.ProcessRawInput2(m.LParam);
                if (e == null) return;
                Common.DeviceName = e.device;
                ////Invoke(() => { label1.Text = DeviceNameFlag + "device"+i++; });
                if (ProcessName == Common.keyupMusic2)
                    KeyBoardHookProc(e);
            }
            //if (m.Msg == 0x0104)
            //{
            //    var e = Rawinput.ProcessRawInput2(m.LParam);
            //}
            //if (m.Msg == 49643)
            //{
            //    var e = Rawinput.ProcessRawInput2(m.LParam);
            //}
            //else
            //{
            //    var e = Rawinput.ProcessRawInput2(m.LParam);
            //}
            base.WndProc(ref m);
        }
    }
}
