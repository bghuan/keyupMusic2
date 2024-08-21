using System.Diagnostics;
using System.Drawing.Imaging;
using System.Media;
using System.Security.Principal;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Core.Impl.Windows;
using static keyupMusic2.Common;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;
using Point = System.Drawing.Point;
using Timer = System.Timers.Timer;


namespace keyupMusic2
{
    public partial class Huan : Form
    {
        ACPhoenix aCPhoenix;
        devenv Devenv;
        douyin Douyin;
        aaa Aaa;
        public Huan()
        {
            InitializeComponent();

            startListen();
            aCPhoenix = new ACPhoenix();
            Devenv = new devenv();
            Douyin = new douyin();
            Aaa = new aaa();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Common.FocusProcess(Common.ACPhoenix);

            int currentProcessId = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcessesByName(Common.keyupMusic2);
            if (Debugger.IsAttached)
            {
                foreach (Process process in processes)
                    if (process.Id != currentProcessId)
                        process.Kill();
            }
            else if (processes.Length > 1)
            {
                //mouse_move(2336, 150);
                //mouse_click2();
                foreach (Process process in processes)
                    if (process.Id != currentProcessId)
                        process.Kill();
                notifyIcon1.Visible = true;
                //Point cursorPos = Cursor.Position;
                //contextMenuStrip1.Show(cursorPos);
                //Dispose();
            }
            if (!Common.FocusProcess(Common.ACPhoenix))
                Activate();

            notifyIcon1.Visible = true;
        }
        public void Invoke2(Action action, int tick = 0)
        {
            Task.Run(() =>
            {
                Thread.Sleep(tick);
                this.Invoke(action);
            });
        }
        public bool keyupMusic2_onlisten = false;
        private void hook_KeyUp(KeyboardHookEventArgs e)
        {
            if (e.Type == KeyboardEventType.KeyDown) return;
            stop_keys.Remove(e.key);
            if (mouse_downing) mouse_up();
        }
        public bool judge_handled(KeyboardHookEventArgs e, string ProcessName)
        {
            if (e.key == Keys.F12 && ProcessName == Common.devenv && !is_ctrl()) return true;
            if (e.key == Keys.Oem3 && ProcessName == Common.ACPhoenix) return true;
            return false;
        }
        private void hook_KeyDown(KeyboardHookEventArgs e)
        {
            if (e.Type != KeyboardEventType.KeyDown) return;
            if (ProcessName2 == Common.keyupMusic2 && e.key == Keys.F1) Common.hooked = !Common.hooked;
            if (Common.hooked) return;
            if (keyupMusic2_onlisten) e.Handled = true;
            if (judge_handled(e, ProcessName)) e.Handled = true;

            Invoke2(() => label1.Text = e.key.ToString());
            stop_keys.Add(e.key);

            if (ProcessName == Common.keyupMusic2)
            {
                super_listen();
            }
            if (e.key == Keys.F3 || (e.key == Keys.LControlKey && is_shift())
                 || (e.key == Keys.LShiftKey && is_ctrl()))
            {
                e.Handled = true;
                super_listen();
            }
            else
            {
                Task.Run(() =>
                {
                    hook_KeyDown_keyupMusic2(e);
                    Aaa.hook_KeyDown_ddzzq(e);
                    Devenv.hook_KeyDown_ddzzq(e);
                    aCPhoenix.hook_KeyDown_ddzzq(e);
                    Douyin.hook_KeyDown_ddzzq(e);

                    Invoke2(() => super_listen_clear());
                });
            }
        }

        private void super_listen()
        {
            keyupMusic2_onlisten = true;
            Invoke2(() =>
            {
                base.BackColor = Color.Blue;
                TaskRun(() =>
                {
                    super_listen_clear();
                }, super_listen_tick);
            });
        }
        int super_listen_tick = 2000;

        private void super_listen_clear()
        {
            keyupMusic2_onlisten = false;
            BackColor = Color.White;
        }

        private MouseKeyboardHook _mouseKbdHook;
        public void Sleep(int tick)
        {
            Thread.Sleep(tick);
        }
        public const int SW_RESTORE = 9;
        int SIMULATED_EVENT_TAG = 19900620;
        bool start_record = false;
        string commnd_record = "";

        Keys[] keys = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
        bool key_sound = true;
        SoundPlayer player = new SoundPlayer();

        static string lastText = "";
        static int last_index = 0;
        public void handle_word2(string text, int segmentIndex, bool show = true)
        {
            Thread.Sleep(2000);
            this.Invoke(new MethodInvoker(() => { label1.Text = "text"; }));
        }
        public void handle_word(string text, int segmentIndex, bool show = true)
        {
            if (show) this.Invoke(new MethodInvoker(() => { label1.Text = text; }));
            string text_backup = text;

            string a = "", b = "", b1 = "", b2 = "", b3 = "", b4 = "", c = "";

            a = lastText;
            if (!string.IsNullOrEmpty(a))
                b = text.Replace(a, "");
            else
                b = text;
            if (b.Length >= 1) b1 = b.Substring(0, 1);
            if (b.Length >= 2) b2 = b.Substring(0, 2);
            if (b.Length >= 3) b3 = b.Substring(0, 3);
            if (b.Length >= 4) b4 = b.Substring(0, 4);
            c = text;
            //log($"{a}    {b}    {c}");

            lastText = text;

            if (KeyMap.TryGetValue(b, out Keys[] keys))
            {
                press(keys, 100);
            }
            else if (KeyMap.TryGetValue(b1, out Keys[] keysb1))
            {
                press(keysb1, 100);
            }
            else if (KeyMap.TryGetValue(b2, out Keys[] keysb2))
            {
                press(keysb2, 100);
            }
            else if (KeyMap.TryGetValue(b3, out Keys[] keysb3))
            {
                press(keysb3, 100);
            }
            else if (KeyMap.TryGetValue(b4, out Keys[] keysb4))
            {
                press(keysb4, 100);
            }
            else if (c.Length > 2 && c.IndexOf("打开") >= 0 && !string.IsNullOrEmpty(b))
            {
                Invoke(() => Clipboard.SetText(b1));
                press([Keys.ControlKey, Keys.V]);

                //press(Keys.Enter);
            }
            else if (c.Length > 2 && c.IndexOf("输入") >= 0 && !string.IsNullOrEmpty(b))
            {
                Invoke(() => Clipboard.SetText(b1));
                press([Keys.ControlKey, Keys.V]);
            }
            else if (KeyMap.TryGetValue(c, out Keys[] keys3))
            {
                press(keys3, 100);
            }
            else if (c == "显示")
            {
                Common.FocusProcess(Process.GetCurrentProcess().ProcessName);
                Invoke(() => SetVisibleCore(true));
            }
            else if (c == "连接")
            {
                press("LWin;OPEN;Enter;500;1056, 411;1563, 191", 101);
            }
            else if (c == "隐藏")
            {
                Invoke(() => SetVisibleCore(false));
            }
            else if (c == "边框")
            {
                Invoke(() => FormBorderStyle = FormBorderStyle == FormBorderStyle.None ? FormBorderStyle.Sizable : FormBorderStyle.None);
            }
        }
        public static Dictionary<string, Keys[]> KeyMap = Listen.KeyMap;
        private void label1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText((sender as Label).Text);
        }

        Point[] points = new Point[10];

        public static Timer aTimer = new Timer(100);

        public void startListen()
        {
            _mouseKbdHook = new MouseKeyboardHook();
            _mouseKbdHook.KeyboardHookEvent += hook_KeyDown;
            _mouseKbdHook.KeyboardHookEvent += hook_KeyUp;
            _mouseKbdHook.Install();
        }
        public void stopListen()
        {
            if (_mouseKbdHook != null)
            {
                _mouseKbdHook.Uninstall();
                _mouseKbdHook.Dispose();
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
            if (Enum.TryParse(typeof(Keys), e.ClickedItem.Text, out object asd)) ;
            {
                var a = new KeyboardHookEventArgs(KeyboardEventType.KeyDown, (Keys)asd, 0, new Native.keyboardHookStruct());
                super_listen();
                hook_KeyDown(a);
            }
            if (e.ClickedItem.Text == "L")
            {
                Dispose();
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetVisibleCore(!Visible);
            }
        }
        private void MouseHookProc(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (e.X == 0 && e.Y == 1439)
                HideProcess("chrome");
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            {
                if (start_record) commnd_record += e.X + "," + e.Y + ";";
            }
            if (e.Msg == MouseMsg.WM_LBUTTONUP && e.Y > 1300)
            {
                if (ProcessName == "msedge")
                    press(Keys.PageDown);
                if (ProcessName == "douyin")
                    press(Keys.Down);
            }
        }
        private void hook_KeyDown_keyupMusic2(KeyboardHookEventArgs e)
        {
            //if (ProcessName != Common.keyupMusic2) return;
            if (!keyupMusic2_onlisten) return;
            Common.hooked = true;
            string label_backup = label1.Text;
            bool catched = false;

            Invoke2(() => { keyupMusic2_onlisten = false; BackColor = Color.White; label1.Text = e.key.ToString(); }, 10);

            switch (e.key)
            {
                case Keys.Q:
                    handle_word("连接", 0, false);
                    break;
                case Keys.W:
                    Listen.is_listen = !Listen.is_listen;
                    Invoke(() => SetVisibleCore(Listen.is_listen));
                    Listen.aaaEvent += handle_word;
                    if (Listen.is_listen) Task.Run(() => Listen.listen_word(new string[] { }, (string asd, int a) => { }));
                    break;
                case Keys.E:
                    winBinWallpaper.changeImg();
                    break;
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    if (key_sound && keys.Contains(e.key))
                    {
                        string wav = "wav\\" + e.key.ToString().Replace("D", "") + ".wav";
                        if (!File.Exists(wav)) return;

                        player = new SoundPlayer(wav);
                        player.Play();
                    }
                    break;
                case Keys.R:
                    if (key_sound) player.Stop();
                    key_sound = !key_sound;
                    break;
                case Keys.T:
                    start_record = !start_record;
                    if (start_record)
                    {
                        _mouseKbdHook = new MouseKeyboardHook();
                        _mouseKbdHook.MouseHookEvent += MouseHookProc;
                        _mouseKbdHook.Install();
                    }
                    else
                    {
                        Common.log(commnd_record);
                        if (!string.IsNullOrEmpty(commnd_record))
                            Invoke(() => Clipboard.SetText(commnd_record));
                        commnd_record = "";
                        _mouseKbdHook.Uninstall();
                    }
                    break;
                case Keys.Y:
                    Common.cmd($"/c start ms-settings:taskbar");
                    press("200;978,1042;907,1227;2500,32;", 801);
                    break;
                case Keys.U:
                    Common.cmd($"/c start ms-settings:personalization");
                    press("200;1056,588;2118,530;2031,585;2516,8;", 801);
                    break;
                case Keys.I:
                    Dispose();
                    break;
                case Keys.O:
                    press(Keys.M);
                    break;
                case Keys.P:
                    Screen secondaryScreen = Screen.AllScreens.FirstOrDefault(scr => !scr.Primary);
                    if (secondaryScreen != null)
                    {
                        Bitmap bmpScreenshot = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
                        Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                        gfxScreenshot.CopyFromScreen(new Point(2560, 0), Point.Empty, secondaryScreen.Bounds.Size);
                        gfxScreenshot.Dispose();
                        bmpScreenshot.Save("image\\encode\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png" + "g", ImageFormat.Png);
                    }
                    break;
                case Keys.A:
                    //ddzzq
                    if (ProcessName == Common.ACPhoenix) { press([Keys.LMenu, Keys.F4], 200); break; }
                    if (Common.FocusProcess(Common.ACPhoenix)) break;
                    if (!Common.FocusProcess(Common.Dragonest))
                    {
                        press("10;LWin;500;1076,521", 101);
                        var asd = 10000;
                        while (asd > 0)
                        {
                            int tick = 500;
                            //(1797,55,Color [A=255, R=18, G=23, B=33])
                            if (judge_color(1797, 55, Color.FromArgb(18, 23, 33)))
                            {
                                press("2323, 30");
                                break;
                            }
                            Thread.Sleep(tick);
                            asd -= tick;
                        }
                    }
                    //if (ProcessName2 != Common.Dragonest) break;
                    if (!judge_color(2223, 1325, Color.FromArgb(22, 155, 222))) { click_dragonest_notity(); }
                    press("600;2280,1314;LWin;", 101);
                    Task.Run(() =>
                    {
                        Sleep(3500);
                        //Common.FocusProcess(Common.ACPhoenix);
                        press([Keys.LMenu, Keys.Tab], 200);
                        press("2525,40");
                        mouse_move3();
                    });
                    break;
                case Keys.D:
                    break;
                case Keys.F:
                    Common.FocusProcess(Common.WeChat);
                    Thread.Sleep(100);
                    if (ProcessName2 == Common.WeChat) break;
                    press("LWin;WEIXIN;Enter;", 100);
                    break;
                case Keys.G:
                    Point mousePosition = Cursor.Position;
                    using (Bitmap bitmap = new Bitmap(1, 1))
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.CopyFromScreen(mousePosition.X, mousePosition.Y, 0, 0, new Size(1, 1));
                            string asd = $"({ProcessName2},{mousePosition.X},{mousePosition.Y},{bitmap.GetPixel(0, 0).ToString()})";
                            log(asd);
                            Invoke(() => Clipboard.SetText(asd));
                        }
                    }
                    break;
                case Keys.H:
                    press("LWin;VIS;Apps;Enter;", 100);
                    TaskRun(() => { press("Tab;Down;Enter;", 100); }, 1500);
                    break;
                case Keys.F2:
                    Invoke(() => Opacity = Opacity == 0 ? 1 : 0);
                    break;
                case Keys.Up:
                    Invoke(() => Opacity = Opacity >= 1 ? 1 : Opacity + 0.1);
                    break;
                case Keys.Down:
                    Invoke(() => Opacity = Opacity <= 0 ? 0 : Opacity - 0.1);
                    break;
                case Keys.J:
                    if (!is_ctrl())
                        if (Common.FocusProcess(Common.chrome)) break;
                    press("LWin;CHR;Enter;", 100);
                    break;
                case Keys.K:
                    //var asd = PixelColorChecker.GetPixelColor(Position.X, Position.Y);
                    //log(asd.ToString());
                    // 1800 2300 1900  230 80 80  R=233, G=81, B=81
                    click_dragonest_notity();
                    break;
                case Keys.L:
                    int x = 2560;
                    int y = 1900;
                    while (x > 1000)
                    {
                        x--;
                    }
                    break;
                case Keys.Z:
                    press("100;LWin;KK;Enter;", 110);
                    break;
                case Keys.Escape:
                    if (is_ctrl() && is_shift())
                        Process.Start(new ProcessStartInfo("taskmgr.exe"));
                    break;
                default:
                    catched = true;
                    break;
            }
            if (catched)
            {
                Invoke((() => { label1.Text = label_backup; }));
                //KeyboardHook.stop_next = true;
            }
            Common.hooked = false;
        }

        private void Huan_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                SetVisibleCore(false);
            }
        }
    }
}
