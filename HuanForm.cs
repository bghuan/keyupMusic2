using keyupMusic2.fantasy;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
using static keyupMusic2.Rawinput;

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
        //bool is_mouse_hook = !(Position.Y == 1439);
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
        public static Keys super_key = Keys.F3;
        public static Keys super_key2 = Keys.F9;

        public Huan()
        {
            if (start_check()) return;
            _instance = this;
            InitializeComponent();

            try_restart_in_admin();
            release_all_key();
            //Rawinput.RegisterInputDevices(this.Handle);
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
            //Location = new Point(2255, 37);

            //startPoint = new Point(Location.X - 252, Location.Y);
            //endPoint = Location;
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;

            after_load();
        }

        protected override CreateParams CreateParams { get { CreateParams cp = base.CreateParams; cp.ExStyle |= 0x80; return cp; } }
        void after_load()
        {
            bland_title();
            if (!ExistProcess(TwinkleTray) && ScreenPrimary.Width < 3000) { ProcessRun(TwinkleTrayexe); }
            InitializeFromCurrentWallpaper();

            VirtualKeyboardForm virtualKeyboardForm = new VirtualKeyboardForm();
            MoonTime moontimeForm = new MoonTime();

            moontimeForm.Show();
            if (ScreenPrimary.Width < 3000)
            {
                virtualKeyboardForm.Show();
            }

            timerMove.Interval = 3000;
            timerMove.Tick += timerMove_Tick;

            TaskRun(() =>
            {
                form_move();
                FreshProcessName2();
            }, 100);

            if (ConfigValue(ConfigFormShow) == "0")
                //SetVisibleCore(false);
                TaskRun(() => { Invoke(() => SetVisibleCore(false)); }, 110);

            string location = Common.ConfigValue(Common.ConfigLocation);
            if (location.Split(',').Length < 2 || !int.TryParse(location.Split(',')[0], out int x)) x = 500;
            if (location.Split(',').Length < 2 || !int.TryParse(location.Split(',')[1], out int y)) y = 500;
            Location = new Point(x, y);

            if (isctrl())
                Native.AllocConsole();
        }
        public static Dictionary<int, int> mmmm = new Dictionary<int, int>();
        //protected override void WndProc(ref Message m)
        //{
        //    if (!mmmm.ContainsKey(m.Msg))
        //        mmmm[m.Msg] = 0;
        //    else
        //        mmmm[m.Msg]++;
        //    var lParam = m.LParam;
        //    if (m.Msg == WM_INPUT)
        //    {
        //        Task.Run(() => ProcessRawInputAsync(lParam));
        //        return;
        //    }
        //    base.WndProc(ref m);
        //}
        //private async Task ProcessRawInputAsync(IntPtr lParam)
        //{
        //    try
        //    {
        //        var e = Rawinput.ProcessRawInput2(lParam);
        //        if (e == null)
        //            return;
        //        Invoke((Delegate)(() => Console2.WriteLine(e.dwExtraInfo2)));
        //        if ((e.key != 0 && e.key != VolumeDown && e.key != VolumeUp && e.key != MediaPlayPause))
        //            Common.DeviceName = e.device;
        //        else if (e.key == 0)
        //            Common.DeviceName2 = e.device;
        //        if (ProcessName == Common.keyupMusic2 && e.key != 0)
        //            await Task.Run(() => KeyBoardHookProc(e));

        //        //if (e.key == Menu)
        //        //{
        //        //    var ha = handling_keys2.ContainsKey(e.key);
        //        //    if (!ha && e.key == Menu && e.Type == Downn && e.device == acer)
        //        //    {
        //        //        press(Tab);
        //        //    }
        //        //    if (e.Type == Downn) { if (!handling_keys2.ContainsKey(e.key)) handling_keys2[e.key] = DateTime.Now; }
        //        //    else handling_keys2.TryRemove(e.key, out _);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Console2.WriteLine($"Error processing RawInput: {ex.Message}");
        //    }
        //}
    }
}
