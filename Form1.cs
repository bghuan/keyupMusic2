using System.Diagnostics;
using System.Security.Principal;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Core.Impl.Windows;
using static keyupMusic2.Common;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;

namespace keyupMusic2
{
    public partial class Huan : Form
    {
        ACPhoenix aCPhoenix;
        devenv Devenv;
        douyin Douyin;
        Other Aaa;
        All Bbb;
        Super super;
        Chrome chrome;

        public Huan()
        {
            Task.Run(() => { if (!Debugger.IsAttached) copy_secoed_screen("_"); });
            InitializeComponent();

            try_restart_in_admin();
            startListen();

            aCPhoenix = new ACPhoenix();
            Devenv = new devenv();
            Douyin = new douyin(this);
            Aaa = new Other();
            Bbb = new All();
            super = new Super(this);
            chrome = new Chrome();

            //new TcpServer(this);
            //TcpServer.StartServer(13000);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int currentProcessId = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcessesByName(Common.keyupMusic2);
            foreach (Process process in processes)
                if (process.Id != currentProcessId)
                    process.Kill();

            if (is_ctrl() || Position.X == 0 || Position.Y == 0 || Position.Y == 1439)
            {
                Common.FocusProcess(Common.douyin);
                Common.FocusProcess(Common.ACPhoenix);
                SetVisibleCore(false);
                TaskRun(() => { Invoke(() => SetVisibleCore(false)); }, 200);
            }
        }
        public bool keyupMusic2_onlisten = false;
        DateTime super_listen_time = new DateTime();
        static int super_listen_tick = 144 * 14;
        Double timerMove_Tick_tick = super_listen_tick;
        public MouseKeyboardHook _mouseKbdHook;
        static string lastText = "";
        static int last_index = 0;
        Keys[] special_key = new Keys[] { Keys.F22, Keys.RControlKey, Keys.RShiftKey, Keys.RMenu, Keys.RWin, Keys.MediaPreviousTrack };

        private void hook_KeyUp(KeyboardHookEventArgs e)
        {
            if (e.Type == KeyboardEventType.KeyDown) return;
            stop_keys.Remove(e.key);
            if (mouse_downing) { up_mouse(); mouse_downing = false; }
            Invoke2(() =>
            {
                label1.Text = label1.Text.Replace(easy_read(e.key),easy_read(e.key).ToLower());
            }
            );
        }
        public bool judge_handled(KeyboardHookEventArgs e, string ProcessName)
        {
            if (is_alt() && is_down(Keys.Tab)) return false;
            //if (e.key == Keys.D1 && is_down(Keys.LWin)) return true;
            //if (e.key == Keys.D2 && is_down(Keys.LWin)) return true;
            if (e.key == Keys.F3) return true;

            //if (e.key == Keys.F11 && ProcessName == Common.explorer && !is_ctrl()) return true;
            //if (e.key == Keys.F11 && ProcessName == Common.devenv && !is_ctrl()) return true;
            //if (e.key == Keys.F12 && ProcessName == Common.devenv && !is_ctrl()) return true;
            if (e.key == Keys.F11 || e.key == Keys.F12)
            {
                if (!is_ctrl())
                {
                    var list = new List<string>() { Common.devenv, Common.explorer };
                    if (list.Contains(ProcessName)) return true;               }
                if (Not_F10_F11_F12_Delete(true))
                {
                    var list = new List<string>() { Common.msedge, Common.chrome };
                    if (list.Contains(ProcessName)) return true;
                }
            }
            //if (e.key == Keys.VolumeUp) return true;
            //if (e.key == Keys.VolumeDown) return true;
            if (ProcessName == Common.ACPhoenix)
            {
                if (e.key == Keys.Oem3) return true;
                if (e.key == Keys.F11 && !is_ctrl()) return true;
                if (Default.handling)
                {
                    if (e.key == Keys.Space) return true;
                    if (e.key == Keys.E) return true;
                }
            }
            //if (ProcessName == Common.douyin || ProcessName == Common.msedge)
            if (ProcessName == Common.msedge)
            {
                if (Default.handling)
                {
                    //if (e.key == Keys.Right) return true;
                    //if (e.key == Keys.Left) return true;
                    //if (e.key == Keys.PageDown) return true;
                    //if (e.key == Keys.PageUp) return true;
                    //if (e.key == Keys.VolumeDown) return true;
                    //if (e.key == Keys.VolumeUp) return true;
                    //if (e.key == Keys.S) return true;
                    if (e.key == Keys.Home && is_ctrl()) return true;
                    if (e.key == Keys.End && is_ctrl()) return true;
                }
            }
            if (e.key == Keys.MediaPreviousTrack || e.key == Keys.MediaPlayPause)
            {
                if (ProcessName == HuyaClient) return true;
            }
            if (is_down(Keys.F1))
            {
                var number_button = new List<Keys> { Keys.Oemcomma, Keys.OemPeriod, Keys.Oem2, Keys.K, Keys.L, Keys.OemSemicolon, Keys.I, Keys.O, Keys.P, Keys.Space };
                if (number_button.Contains(e.key))
                    return true;
            }
            var flag = chrome.judge_handled(e) || Douyin.judge_handled(e);
            return flag;
        }
        Keys last_handled_key;
        private void hook_KeyDown(KeyboardHookEventArgs e)
        {
            if (e.Type != KeyboardEventType.KeyDown) return;
            if (Common.hooked) return;
            if (is_down(Keys.LWin)) return;
            if (is_alt() && (e.key == Keys.F4 || e.key == Keys.Tab)) { special_key_quick_yo(e); ; return; }
            if (stop_keys.Contains(e.key)) return;

            FreshProcessName();
            if (keyupMusic2_onlisten &&
                (e.key != Keys.Left && e.key != Keys.Right && e.key != Keys.T)) e.Handled = true;
            if (judge_handled(e, ProcessName)) { last_handled_key = e.key; e.Handled = true; }

            Task.Run(() =>
            {
                handle_special_or_normal_key(e);
                print_easy_read();
                //special_key_quick_yo(e);
            });

            if (e.key == Keys.F3 || (e.key == Keys.LControlKey && is_shift())
           || (e.key == Keys.LShiftKey && is_ctrl()))
            {
                if (keyupMusic2_onlisten)
                {
                    Invoke(() => { SetVisibleCore2(last_visiable); });
                    last_visiable = false;
                    keyupMusic2_onlisten = false;
                    return;
                }
                form_move();
                super_listen();
            }
            else
            {
                Task.Run(() =>
                {
                    super.hook_KeyDown_keyupMusic2(e);
                    if (keyupMusic2_onlisten) return;

                    Devenv.hook_KeyDown_ddzzq(e);
                    aCPhoenix.hook_KeyDown_ddzzq(e);
                    Douyin.hook_KeyDown_ddzzq(e);
                    chrome.handlehandle(e);

                    Aaa.hook_KeyDown_ddzzq(e);
                    Bbb.hook_KeyDown_ddzzq(e);

                    Music.hook_KeyDown_keyupMusic2(e);
                });
            }
        }

        private void special_key_quick_yo(KeyboardHookEventArgs e)
        {
            if (e.key == Keys.F4 || e.key == Keys.LMenu || e.key == Keys.Tab)
            {
                Task.Run(() =>
                {
                    //yo();
                    Thread.Sleep(100);
                    FreshProcessName();
                });
            }
        }
        public bool last_visiable = false;
        private void form_move()
        {
            Invoke2(() =>
            {
                if (Opacity == 0) { return; }
                if (!Visible) { SetVisibleCore(true); last_visiable = true; }
                timerMove.Interval = 1;
                timerMove.Tick += timerMove_Tick;
                timerMove.Start();
                Location = startPoint;
                startTime = DateTime.Now;
            });
        }

        private void handle_special_or_normal_key(KeyboardHookEventArgs e)
        {
            if (!stop_keys.Contains(e.key))
            {
                string _ProcessName = "";
                if (special_key.Contains(e.key) || log_always) _ProcessName = log_process(e.key.ToString());
                if (e.key == Keys.F22 && (_ProcessName == "WeChatAppEx" || _ProcessName == "WeChat")) { e.Handled = true; }
                if (e.key == Keys.VolumeDown || e.key == Keys.VolumeUp) { stop_keys.Remove(Keys.VolumeDown); stop_keys.Remove(Keys.VolumeUp); }
                stop_keys.Add(e.key);
            }
        }
        private void print_easy_read()
        {
            var _stop_keys = stop_keys.ToArray();
            Invoke2(() =>
            {
                string asd = string.Join("+", _stop_keys.Select(key => easy_read(key.ToString())));
                if (label1.Text == asd) asd += " " + DateTimeNow2();
                label1.Text = speak_word + "" + asd;
            }
            );
        }

        private static string easy_read(string asd)
        {
            asd = asd.Replace("LMenu", "Alt").Replace("LWin", "Win").Replace("LControlKey", "Ctrl").Replace("LShiftKey", "Shift");
            asd = asd.Replace("Oem3", "~");
            asd = asd.Replace("VolumeUp", "v↑").Replace("VolumeDown", "v↓");
            for (int i = 0; i <= 9; i++) { asd = asd.Replace($"D{i}", i.ToString()); }

            return asd;
        }
        private static string easy_read(Keys asd)
        {
            return easy_read(asd.ToString());
        }

        private void super_listen()
        {
            keyupMusic2_onlisten = true;
            super_listen_time = DateTime.Now.AddMilliseconds(super_listen_tick);
            Invoke2(() =>
            {
                base.BackColor = Color.Blue;
                TaskRun(() =>
                {
                    if (DateTime.Now > super_listen_time)
                        super_listen_clear(Color.White);
                }, super_listen_tick);
            });
        }
        private void super_listen_clear(Color color)
        {
            keyupMusic2_onlisten = false;
            BackColor = color;
        }
        public static string speak_word = "";
        public static int sssssegmentIndex;
        public void handle_word(string text, int segmentIndex, bool show = true)
        {
            if (segmentIndex == sssssegmentIndex) { return; }
            speak_word = text + "_";
            //if (text == "UP") { press(Keys.PageUp, 0); return; }
            press(Keys.PageDown, 0);
            sssssegmentIndex = segmentIndex;
            return;
            if (show) this.Invoke(new MethodInvoker(() => { label1.Text = text; }));
            //if (ProcessName == msedge)

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
            else if (c == "下" || c == "NEXT")
            {
                if (ProcessName == msedge)
                    press(Keys.PageDown, 0);
            }
        }
        public static Dictionary<string, Keys[]> KeyMap = Listen.KeyMap;
        private void label1_Click(object sender, EventArgs e)
        {
            Point cursorPos = Cursor.Position;
            contextMenuStrip1.Show(cursorPos);
        }
        public void startListen()
        {
            label1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            label2.Text = "";
            _mouseKbdHook = new MouseKeyboardHook();
            _mouseKbdHook.KeyboardHookEvent += hook_KeyDown;
            _mouseKbdHook.KeyboardHookEvent += hook_KeyUp;
            _mouseKbdHook.Install();

            startPoint = new Point(Location.X - 300, Location.Y);
            endPoint = Location;
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
        //protected override void SetVisibleCore(bool value)
        //{
        //    base.SetVisibleCore(value);
        //    if (!value) hide_keyupmusic3();
        //}
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (Enum.TryParse(typeof(Keys), e.ClickedItem.Text.Substring(0, 1), out object asd)) ;
            {
                var a = new KeyboardHookEventArgs(KeyboardEventType.KeyDown, (Keys)asd, 0, new Native.keyboardHookStruct());
                super_listen();
                hook_KeyDown(a);
                a.Type = KeyboardEventType.KeyUp;
                hook_KeyUp(a);
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
        private void Huan_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized || this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                SetVisibleCore(false);
            }
        }

        public void SetVisibleCore2(bool a)
        {
            SetVisibleCore(a);
        }
        public void Invoke2(Action action, int tick = 0)
        {
            Task.Run(() => { Thread.Sleep(tick); this.Invoke(action); });
        }

        private Point startPoint = new Point(1510, 100);
        private Point endPoint = new Point(2250, 100);
        private DateTime startTime;
        public static bool log_always;

        private void timerMove_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - startTime;

            if (elapsed.TotalMilliseconds <= timerMove_Tick_tick)
            {
                int currentX = (int)(startPoint.X + (endPoint.X - startPoint.X) * (elapsed.TotalMilliseconds / timerMove_Tick_tick));
                int currentY = startPoint.Y;
                Location = new Point(currentX, currentY);
            }
            else
            {
                Location = endPoint;
                timerMove.Stop();
                if (last_visiable) { SetVisibleCore(false); last_visiable = false; }
            }
        }
        private void try_restart_in_admin()
        {
            if (!Debugger.IsAttached && !is_down(Keys.LControlKey) && !IsAdministrator())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(Application.ExecutablePath);
                startInfo.UseShellExecute = true;
                startInfo.Verb = "runas";
                TaskRun(() => { if (ProcessName2 == Common.keyupMusic2) press(Keys.Enter, 1000); }, 1000);
                Process.Start(startInfo);
                Application.Exit();
                MessageBox.Show("正在获取管理员权限");
            }
            if (!IsAdministrator())
            {
                Text = Text + "(非管理员)";
            }
            if (!ExsitProcess(keyupMusic3))
                ProcessRun(keyupMusic3exe);
        }
        public void Invoke(Action method)
        {
            try { base.Invoke(method); }
            catch (Exception ex)
            {
                log(ex.Message);
            }
        }
    }
}
