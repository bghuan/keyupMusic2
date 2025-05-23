using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public partial class Huan : Form
    {
        ACPhoenix aCPhoenix;
        devenv Devenv;
        douyin Douyin;
        Other Otherr;
        All Alll;
        Super super;
        Chrome chrome;
        bool not_init_show = (is_ctrl() && !is_shift()) || Position.Y == 0;
        bool not_mouse_hook = !((is_ctrl() && !is_shift()) || Position.Y == 1439);
        bool not_not_sleep = Position.X == screenWidth1 && Position.Y == screenHeight1;

        public Huan()
        {
            InitializeComponent();

            play_sound_di();
            try_restart_in_admin();
            releas_self_restart_keyup_lost();
            startListen();

            aCPhoenix = new ACPhoenix();
            Devenv = new devenv();
            Douyin = new douyin(this);
            Otherr = new Other();
            Alll = new All();
            super = new Super(this);
            chrome = new Chrome();

            new TcpServer(this);
        }

        public static bool keyupMusic2_onlisten = false;
        DateTime super_listen_time = new DateTime();
        static int super_listen_tick = 144 * 14;
        Double timerMove_Tick_tick = super_listen_tick;
        public MouseKeyboardHook _mouseKbdHook;
        Keys[] special_key = { Keys.F22, Keys.RMenu, Keys.RWin };

        public void hook_KeyUp(KeyboardHookEventArgs e)
        {
            if (e.Type == KeyboardEventType.KeyDown) return;
            lock (stop_keys)
            {
                if (stop_keys.Remove(e.key) == false)
                { string ddfd = "dsad"; }
                if (mouse_downing) { up_mouse(); mouse_downing = false; }
                if (!no_sleep) return;
                Invoke2(() =>
                {
                    label1.Text = label1.Text.Replace(easy_read(e.key), easy_read(e.key).ToLower());
                    //if (stop_keys.Count == 0) Invoke2(() => { label1.Text = ""; }, 1000);
                    keyupMusic2.KeyUp.yo(e);
                });
                //log(e.key + "-" + asdsads + "-up" + string.Join(" ", stop_keys));
            }
            //if (ProcessName == Common.chrome && e.key == Keys.Z) SS().KeyUp(Keys.X);
        }
        public bool judge_handled(KeyboardHookEventArgs e, string ProcessName)
        {
            if (is_alt() && is_down(Keys.Tab)) return false;
            if (e.key == Keys.F3) return true;
            if (e.key == Keys.F9) return true;

            //if (e.key == Keys.F10) return true;
            //if (e.key == Keys.F11) return true;
            //if (e.key == Keys.F12) return true;

            if (e.key == Keys.F2 && ProcessName == Common.chrome)
                return true;

            if (ProcessName == Common.devenv)
            {
                if (e.key == Keys.F && is_shift() && is_alt())
                    return true;
            }
            if (e.key == Keys.F10 || e.key == Keys.F11 || e.key == Keys.F12)
            {
                if (!is_down(Keys.Delete))
                    return true;
            }
            if (ProcessName == Common.msedge && !is_douyin())
            {
                if (e.key == Keys.Home) return true;
                if (e.key == Keys.End) return true;
                //if ((e.key == Keys.PageDown || e.key == Keys.PageUp) && e.X > screenWidth) return true;
                if (e.key == Keys.VolumeUp || e.key == Keys.VolumeDown)
                    if (e.X == screenWidth1 || e.Y == screenHeight1)
                        return true;
            }
            if (e.key == Keys.MediaPreviousTrack || e.key == Keys.MediaNextTrack)
            {
                if (ProcessName == steam) return true;
                if (ProcessName == cs2) return true;
                if (ProcessName == Glass2) return true;
                if (ProcessName == PowerToysCropAndLock) return true;
                if (ProcessName == vlc) return true;
            }
            if ((e.key == Keys.Right || e.key == Keys.Left) && is_ctrl())
            {
                if (ProcessName == vlc) return true;
                if (ProcessName == msedge) return true;
            }
            if (is_down(Keys.F1))
            {
                var number_button = new[] { Keys.Oemcomma, Keys.OemPeriod, Keys.Oem2, Keys.K, Keys.L, Keys.OemSemicolon, Keys.I, Keys.O, Keys.P, Keys.Space };
                if (number_button.Contains(e.key))
                    return true;
            }
            var flag = chrome.judge_handled(e) || Douyin.judge_handled(e) || Super.judge_handled(e);
            return flag;
        }
        Keys last_handled_key;
        DateTime VolumeDown_time = DateTime.MinValue;
        private void hook_KeyDown(KeyboardHookEventArgs e)
        {
            if (e.Type != KeyboardEventType.KeyDown) return;
            if (e.key == Keys.F3) { e.Handled = true; }
            if (keyupMusic2_onlisten) { e.Handled = true; }
            if (is_alt() && (e.key == Keys.F4 || e.key == Keys.Tab)) { return; }
            if (stop_keys.ContainsKey(e.key)) return;
            //if (stop_keys.Count >= 8) { Dispose(); return; }

            FreshProcessName();
            if (keyupMusic2_onlisten &&
                    !(e.key == Keys.Left
                    //|| e.key == Keys.Right
                    || e.key == Keys.T
                    || e.key == Keys.F5))
            {
                /*super_listen_clear(Color.White); */
                e.Handled = true;
            }
            if (judge_handled(e, ProcessName)) { last_handled_key = e.key; e.Handled = true; }

            handle_special_or_normal_key(e);
            Task.Run(() =>
            {
                print_easy_read();
                //quick_volume_zero();
                //start_record(e);
                //special_key_quick_yo(e);
            });
            if (e.key == Keys.F3 || e.key == Keys.F9)
            {
                play_sound_di();
                if (e.key == Keys.F9 && (keyupMusic2_onlisten || !no_sleep))
                {
                    system_sleep();
                    return;
                }
                if (keyupMusic2_onlisten)
                {
                    var aa = temp_visiable;
                    temp_visiable = false;
                    keyupMusic2_onlisten = false;
                    Invoke(() => { SetVisibleCore(aa); });
                    return;
                }
                if (is_ctrl())
                {
                    LossScale();
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

                    Otherr.hook_KeyDown(e);
                    Alll.hook_KeyDown_ddzzq(e);

                    Music.hook_KeyDown_keyupMusic2(e);
                    if (!no_sleep && e.key != Keys.VolumeDown && e.key != Keys.VolumeUp && e.key != Keys.MediaStop && e.key != Keys.F22)
                    {
                        player.Stop();
                        Invoke2(() => { label1.Text = "取消睡眠"; });
                        no_sleep = true;
                    }
                });
            }
        }

        private void system_sleep()
        {
            temp_visiable = false;
            press(Keys.MediaStop);
            Invoke(() => { SetVisibleCore(true); });
            KeyTime[system_sleep_string] = DateTime.Now;

            if (is_ctrl() || GetWindowText() == UnlockingWindow || ProcessName == LockApp || ProcessName == err)
            {
                play_sound(Keys.D0);
                Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,1");
                return;
            }

            if (!no_sleep) { Task.Run(() => Timer_Tick(200)); return; }
            TaskRun(() => { Timer_Tick(); }, 70000);
            Task.Run(() =>
            {
                play_sound(Keys.D0);
                Invoke(() => { label1.Text = "系统即将进入睡眠状态"; });
            });
            no_sleep = false;
        }

        private void start_record(KeyboardHookEventArgs e)
        {
            if (Super.start_record)
            {
                log_process(e.key.ToString());
            }
        }

        private static bool no_sleep = true;
        private void Timer_Tick(int tick = 1000)
        {
            // 执行系统睡眠命令
            //Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,1");
            if (no_sleep) return;
            no_sleep = true;
            Invoke(() => { SetVisibleCore(false); });
            //Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,1");
            Task.Run(
                () =>
            press("500;LWin;1650,1300;1650,1140", tick));
            for (int i = 0; i < 10; i++)
            {
                if (ProcessName2 == StartMenuExperienceHost) { return; }
                //log_process("F9");
                play_sound_di(tick);
            }
        }
        private void handle_special_or_normal_key(KeyboardHookEventArgs e)
        {
            lock (stop_keys)
                if (!stop_keys.ContainsKey(e.key))
                {
                    if (e.key == Keys.F9) { return; }
                    string _ProcessName = "";
                    if (special_key.Contains(e.key) || log_always) _ProcessName = log_process(e.key.ToString());
                    //if (e.key == Keys.F22 && (_ProcessName == "WeChatAppEx" || _ProcessName == "WeChat")) { e.Handled = true; }
                    if (e.key == Keys.F22 && (_ProcessName == "WeChatAppEx" || _ProcessName == "WeChat")) { return; }
                    stop_keys.Add(e.key, ProcessName);
                }
        }
        private void print_easy_read()
        {
            var _stop_keys = stop_keys?.ToArray();
            if (!no_sleep) return;
            Invoke(() =>
            {
                string asd = string.Join(" ", _stop_keys?.Select(key => easy_read(key.Key.ToString())));
                if (label1.Text.ToLower() == asd.ToLower()) asd += " " + DateTimeNow2();
                //label1.Text = Listen.speak_word + "" + asd;
                label1.Text = asd;
            }
            );
        }
        private void quick_volume_zero()
        {
            var stop_keysCopy = new Dictionary<Keys, string>(stop_keys);
            if (stop_keysCopy.Count(key => key.Key != Keys.VolumeDown) >= 7 && VolumeDown_time.AddSeconds(3) < DateTime.Now)
            {
                VolumeDown_time = DateTime.Now;
                press(Keys.VolumeDown, 50, 0);
            }
        }

        private static string easy_read(string asd)
        {
            asd = asd.Replace("LMenu", "Alt").Replace("LWin", "Win").Replace("LControlKey", "Ctrl").Replace("LShiftKey", "Shift");
            asd = asd.Replace("Oem3", "~");
            asd = asd.Replace("VolumeUp", "v↑").Replace("VolumeDown", "v↓");
            asd = asd.Replace("Next", "PageDown");
            for (int i = 0; i <= 9; i++) { asd = asd.Replace($"D{i}", i.ToString()); }

            return asd;
        }
        private static string easy_read(Keys asd)
        {
            return easy_read(asd.ToString());
        }

        public bool temp_visiable = false;
        private void form_move()
        {
            Invoke2(() =>
            {
                if (Opacity == 0) { return; }
                if (!Visible) { temp_visiable = true; SetVisibleCore(true); }
                timerMove.Interval = 1;
                timerMove.Tick += timerMove_Tick;
                timerMove.Start();
                Location = startPoint;
                startTime = DateTime.Now;
            });
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
                if (temp_visiable) { temp_visiable = false; SetVisibleCore(false); }
                temp_visiable = false;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
        }
        public void startListen()
        {
            label1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            label2.Text = "";
            _mouseKbdHook = new MouseKeyboardHook();
            _mouseKbdHook.KeyboardHookEvent += hook_KeyDown;
            _mouseKbdHook.KeyboardHookEvent += hook_KeyUp;
            if (not_mouse_hook)
            {
                var b = new biu(this);
                _mouseKbdHook.MouseHookEvent += b.MouseHookProc;
                //Invoke(() => { b.MoveStopClickListen(); }); 
                //b.MoveStopClickListen();
                //_mouseKbdHook.MouseHookEvent += new Douyin_game(this).MouseHookProc;
            }
            _mouseKbdHook.Install();

            new Tick();
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
            // 停止 TcpListener
            if (TcpServer.listener != null && TcpServer.listener.Server.IsBound)
            {
                TcpServer.listener.Stop();
            }

            // 关闭 TcpClient 和相关的流
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
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(value);
            key_sound = value;
            if (temp_visiable) key_sound = false;
            if (!value) player.Stop();
        }
        public void SetVisibleCore2(bool a)
        {
            SetVisibleCore(a);
        }
        public void Invoke2(Action action, int tick = 0)
        {
            Task.Run(() => { Thread.Sleep(tick); this.Invoke(action); });
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
        }

        public void Invoke(Action method)
        {
            try { base.Invoke(method); }
            catch (Exception ex)
            {
                log("Invoke err: " + ex.Message);
            }
        }

        private void releas_self_restart_keyup_lost()
        {
            release_all_keydown();
            //TaskRun(() =>
            //{
            //    if (is_down(Keys.RControlKey))
            //        SSSS.KeyUp(Keys.RControlKey);
            //    if (is_down(Keys.RShiftKey))
            //        SSSS.KeyUp(Keys.RShiftKey);
            //    if (is_down(Keys.F5))
            //        SSSS.KeyUp(Keys.F5);
            //}, 1000);
        }
        public void release_all_keydown(int tick = 1000)
        {
            TaskRun(() =>
            {
                var pressedKeys = GetPressedKeys();
                if (pressedKeys.Any())
                    Invoke2(() => { label1.Text = "relese: " + string.Join(", ", pressedKeys); });
                foreach (var key in pressedKeys)
                {
                    SSSS.KeyUp(key);
                }
                stop_keys = new Dictionary<Keys, string>();
            }, tick);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int currentProcessId = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcessesByName(Common.keyupMusic2);
            foreach (Process process in processes)
                if (process.Id != currentProcessId)
                    process.Kill();

            if (not_init_show)
            {
                SetVisibleCore(false);
                TaskRun(() => { Invoke(() => SetVisibleCore(false)); }, 200);
            }
            if (not_not_sleep)
            {
                TaskRun(() => { press(Keys.F9); press(Keys.F9); }, 1000);
            }
            Common.FocusProcess(Common.ACPhoenix);
            Common.FocusProcess(Common.Glass3);
            Common.FocusProcess(Common.Kingdom);
            Common.FocusProcess(Common.Kingdom5);
            Location = new Point(Screen.PrimaryScreen.Bounds.Width - 310, 100);

            startPoint = new Point(Location.X - 300, Location.Y);
            endPoint = Location;
            SetWindowTitle(Common.devenv, "");
            SetWindowTitle(Common.chrome, "");
            SetWindowTitle(Common.PowerToysCropAndLock, "");
            //SetWindowTitle(Common.msedge, "");
        }
    }
}
