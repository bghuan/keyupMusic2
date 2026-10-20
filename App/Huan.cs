using Microsoft.Win32;
using NAudio.Gui;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection.Emit;
using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace keyupMusic4
{
    public partial class Huan : Form
    {
        private static Huan _instance;
        public static Huan Instance => _instance;

        public bool judge_handled(KeyboardMouseHook.KeyEventArgs e)
        {
            if (e.key == Keys.LWin) return false;

            if (e.key == Keys.BrowserHome) return true;
            if (e.key == Keys.Apps) return true;

            if (e.key == Keys.F1 && !isctrl()) return true;
            if (e.key == Keys.F2 && !ProcessTitle.Contains("tudio") && IsFullScreen() && ProcessName != cs2) return true;
            if (e.key == Keys.F3) return true;
            if (e.key == Keys.F9) return true;
            if (keyupMusic4_onlisten) { e.Handled = true; }

            if (e.key == Keys.OemPeriod && is_down(Keys.RControlKey)) return true;
            if (e.key == Keys.Escape && is_playing) return true;
            if (e.key == Keys.Escape && is_douyin()) return true;
            if ((e.key == Keys.Left || e.key == Down || e.key == Keys.Right) && (Position.Y == 0)) return true;
            if (e.key == Keys.MediaPreviousTrack || e.key == Keys.MediaNextTrack)
                if (biu.list_go_back.Contains(ProcessName)) return true;
            if ((e.key == Keys.Right || e.key == Keys.Left) && is_ctrl())
                if (ProcessName == vlc || ProcessName == msedge) return true;
            if (is_down(Keys.F1)) if (number_button.Contains(e.key)) return true;

            if (ProcessName == Common.VSCode) if (e.key == Keys.Q && isctrl()) return true;

            if (KeyFunc.judge(e)) return true;
            return HookRegistry.JudgeChain(e);
        }

        private void KeyBoardHookProc(KeyboardMouseHook.KeyEventArgs e)
        {
            if (e.key != F1) FreshProcessName2();
            if (!(e.key == Keys.F3) && ProcessName == Common.mhtab) return;
            var handleddd = judge_handled(e);
            if (handleddd) { e.Handled = true; VirKeyState(e); }
            if (!handleddd) Native.ReleaseCapture();
            var ha = deal_handilngkey(e.key, e.Type == KeyType.Down);

            if (!ha && (!is_steam_game() || (e.key == Tab && e.Type == KeyType.Up)))
                VirtualKeyboardForm.Instance?.TriggerKey(e.key, e.Type == KeyType.Up);

            Task.Run(() =>
            {
                if (!ha)
                    print_easy_read(e);

                // KeyFunc → 按键映射 → KeyUp → 抬起终止 → 头部模块（Coocaa/AirKeyboard/Super）
                if (HookRegistry.KeyHookChainHead(e, ha)) return;

                // job f3 f9 to var special key and customs
                if (e.key == Keys.F3 || e.key == Keys.F9)
                {
                    if (e.key == F3 && isctrl()) { start_catch(start_check_str); return; }
                    if (e.key == F9) play_sound_di();
                    super_listen();
                    form_move(); return;
                }

                // 尾部模块：Devenv → Douyin → Chrome → Other → All → Win → Music
                HookRegistry.KeyHookChainTail(e);
            });
        }
        public void start_catch(string msg)
        {
            play_sound_di2();
            if (msg.Contains(start_check_str))
            {
                start_catch_time = DateTime.Now;
                string[] list_f1 = [clashverge,];
                if (ProcessName == Common.mhtab && GetPointName() == Common.mhtab)
                {
                    LossScale();
                }
                else if (Position.X == 0 && Position.Y == 0)
                {
                    if (ProcessName == devenv)
                    {
                        press([LControlKey, LShiftKey, B]);
                    }
                    else
                        ProcessRun(clashvergeexe);
                }
                else if (Position.X == screenWidth1 && Position.Y == screenHeight1)
                {
                    {
                        press(MediaStop); Native.LockWorkStation();
                    }
                }
                else if (Position.X == 0)
                {

                    {
                        string executablePath = Process.GetCurrentProcess().MainModule.FileName;
                        Process.Start(executablePath);
                        Environment.Exit(0);
                    }
                }
                else if (Position.Y == 0)
                {
                    LossScale();
                }
                else if (Position.X == screenWidth1)
                {
                    if (ProcessName != explorer)
                        SetTransparency();
                }
                else if (Position.Y == screenHeight1)
                {
                    changeClash();
                }
                else
                {
                    huan.Invoke(() =>
                    {
                        if (!huan.Visible) huan.Show();
                        else huan.Hide();
                        if (MoonTime.Instance != null)
                            MoonTime.Instance.TopMost = true;
                    });
                }
            }
            else if (msg.Contains(start_check_str2))
            {
                system_hard_sleep();
            }
        }









        Blob blobForm = new Blob();
        internal ACPhoenixClass ACPhoenix;
        internal DevenvClass Devenv;
        internal DouyinClass Douyin;
        internal OtherClass Other;
        internal AllClass All;
        internal SuperClass Super;
        internal ChromeClass Chrome;
        internal WinClass Win;
        public OpencvReceive Opencv;
        public LongPressClass LongPress;
        public static bool keyupMusic4_onlisten = false;
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
            startListen();

            ACPhoenix = new ACPhoenixClass();
            Devenv = new DevenvClass();
            Douyin = new DouyinClass();
            Other = new OtherClass();
            All = new AllClass();
            Super = new SuperClass();
            Chrome = new ChromeClass();
            Win = new WinClass();

            HookRegistry.Init(this);

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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Debugger.IsAttached)
            {
                int currentProcessId = Process.GetCurrentProcess().Id;
                Process[] processes = Process.GetProcessesByName(Common.AppName);
                foreach (Process process in processes)
                    if (process.Id != currentProcessId && IsAdministrator())
                        process.Kill();
            }

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
                FreshProcessName2();
            }, 100);

            if (ConfigValue(ConfigFormShow) == "0")
                TaskRun(() => { Invoke(() => SetVisibleCore(false)); }, 110);

            string location = Common.ConfigValue(Common.ConfigLocation);
            if (location.Split(',').Length < 2 || !int.TryParse(location.Split(',')[0], out int x)) x = 500;
            if (location.Split(',').Length < 2 || !int.TryParse(location.Split(',')[1], out int y)) y = 500;
            Location = new Point(x, y);

            if (isctrl())
                Native.AllocConsole();
        }
        public static Dictionary<int, int> mmmm = new Dictionary<int, int>();
        //    try










        public static ConcurrentDictionary<Keys, DateTime> handling_keys = new();
        public static ConcurrentDictionary<Keys, DateTime> handling_keys2 = new();
        public async void Invoke2(Action action, int tick = 0)
        {
            await Task.Delay(tick);
            Invoke(action);
        }

        public new void Invoke(Action method)
        {
            if (IsDisposed) return;
            try { base.Invoke(method); }
            catch (Exception) { }
        }
        public void release_all_key(int tick = 1000)
        {
            var pressedKeys = release_all_keydown();
            if (pressedKeys.Any())
                Invoke2(() => { label1.Text = string.Join(", ", pressedKeys); });
        }
        public void system_sleep(bool force = false)
        {
            press(Keys.MediaStop);

            ready_to_sleep = true;
            Invoke(() => { SetVisibleCore(true); });
            int sleep = 0;

            if (!force)
            {
                play_sound(Keys.D0);
                sleep = player_time;
            }
            Task.Run(() =>
            {
                Invoke(() => { label1.Text = "系统即将进入睡眠状态"; });
                Sleep(sleep);
                system_sleep_timer();
            });
        }

        private void start_record(KeyboardMouseHook.KeyEventArgs e)
        {
            {
                log(e.key.ToString());
            }
        }
        private void handle_special_or_normal_key(KeyboardMouseHook.KeyEventArgs e)
        {
            string _ProcessName = "";
            if (special_key.Contains(e.key)) _ProcessName = process_and_log(e.key.ToString());
            if (e.key == Keys.F22 && (_ProcessName == "WeChatAppEx" || _ProcessName == "WeChat")) { return; }
            handling_keys[e.key] = DateTime.Now;
        }
        private void print_easy_read(KeyboardMouseHook.KeyEventArgs e)
        {
            if (e.Type == KeyType.Up)
            {
                Invoke2(() => label1.Text = /*Common.DeviceName + */label1.Text.Replace(easy_read2(e.key), easy_read2(e.key).ToLower()));
                return;
            }

            try
            {
                var _handling_keys = new Dictionary<Keys, DateTime>(handling_keys);
                Invoke(() =>
                {
                    string asd = string.Join(" ", _handling_keys?.Select(key => easy_read(key.Key.ToString())));
                    if (label1.Text.ToLower() == asd.ToLower()) asd += " " + DateTimeNow2();
                    label1.Text = /*Common.DeviceName + */asd;
                }
                );
            }
            catch (ArgumentException) { }
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
        private static string easy_read2(Keys asd)
        {
            return easy_read(asd.ToString());
        }

        private void super_listen_clear(Color color)
        {
            keyupMusic4_onlisten = false;
            BackColor = color;
        }

        public void startListen()
        {
            label1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            label2.Text = "";

            _mouseKbdHook = new KeyboardMouseHook(this.Handle);
            _mouseKbdHook.KeyEvent += KeyBoardHookProc;

            {
                var b = new biu();
                _mouseKbdHook.MouseEvent += b.MouseHookProc;
            }
            _mouseKbdHook.Install();
            play_sound_di();
        }
        public void stopListen()
        {
            if (_mouseKbdHook != null)
            {
                _mouseKbdHook.Uninstall();
            }
        }
        private void try_restart_in_admin()
        {
            if (!Debugger.IsAttached && !IsAdministrator())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(Application.ExecutablePath);
                startInfo.UseShellExecute = true;
                startInfo.Verb = "runas";
                Process.Start(startInfo);
                Environment.Exit(0);
            }
        }
        private void timerMove_Tick(object sender, EventArgs e)
        {


            timer_stop();
        }


        public void system_sleep_timer(int tick = 200)
        {
            if (!ready_to_sleep) return;
            ready_to_sleep = false;
            system_sleep_count = 1;
            Invoke(() => { SetVisibleCore(false); });
            log("system_sleep");

            CloseDesktopWindow();
            press(100, 500, Return);
        }
        //job no release f3 no hide no stop superlisten
        public void timer_stop()
        {
            blobForm.Hide();
            blobForm.changeFlag(false);
            timerMove.Stop();
        }
        private void form_move()
        {
            Invoke(() =>
            {
                if (Opacity == 0) { return; }
                var current_hwnd = ProcessName;
                if (Focused && current_hwnd == "") { altab(); }
                else if (Focused) { FocusProcessSimple(current_hwnd); }
                blobForm.Show();
                blobForm.changeFlag(true);
                //// 解除旧的事件绑定，防止叠加
                timerMove.Start();
                startTime = DateTime.Now;
            });
        }

        private void super_listen()
        {
            keyupMusic4_onlisten = true;
            super_listen_time = DateTime.Now.AddMilliseconds(super_listen_tick);
            Invoke(() =>
            {
                base.BackColor = Color.Blue;
                TaskRun(() =>
                {
                    if (DateTime.Now > super_listen_time)
                        super_listen_clear(Color.White);
                }, super_listen_tick);
            });
        }
        public static bool start_check()
        {
            if (Debugger.IsAttached) return false;
            int currentProcessId = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcessesByName(Common.AppName);
            foreach (Process process in processes)
            {
                if (process.Id != currentProcessId)
                {
                    Socket.socket_run = false;
                    Socket.socket_write(start_check_str);
                    Environment.Exit(0);
                    return true;
                }
            }
            return false;
        }
        public static string start_check_str = "starting";
        public static string start_check_str2 = "sleep";
        public static string huan_invoke = "huan_invoke";
        public bool temp_visiable = false;

        // 此时为唤醒后登录 
        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                MoonTime.Instance?.SetInitAngle();
                if (MoonTime.Instance != null && MoonTime.Instance.Visible)
                    MoonTime.vkMenuItem_DoubleClick(null, null);
                VirtualKeyboardForm.Instance?.SetInitClean();
                system_sleep_count = 0;
                DaleyRun_stop = true;
                player.Stop();
                CleanMouseState();
                ready_to_sleep = false;
                play_sound_bongocat(D4);
                TaskRun(() => play_sound_bongocat(D5), 200);
                TaskRun(() => play_sound_bongocat(D6), 300);
                log("唤醒解锁");
            }
        }

        public static Dictionary<string, Action> refAction = new Dictionary<string, Action>() {
            { "chrome_click_r_up",(() => {
                MessageBox.Show("右键点击 Chrome");
                if (!LongPressClass.long_press_rbutton && ExistProcess(Common.PowerToysCropAndLock, true))
                {
                    if (judge_color(1840, 51, Color.FromArgb(162, 37, 45)))
                        press(Keys.F, 51);
                    quick_max_chrome();
                }})},
        };


        public static string start_reflection = "reflection";
        public void reflection_catch(string msg)
        {
            msg = msg.Replace(start_reflection, "");
            Common.ExecuteCommand(msg);
            TaskRun(() => { VirtualKeyboardForm.Instance?.TriggerKey(LControlKey, true); }, 1000);
        }
        public static string start_next = "nextlocation";
        public void next_catch(string msg)
        {
            msg = msg.Replace(start_next, "");
            int x = int.Parse(msg.Split(",")[0]);
            int y = int.Parse(msg.Split(",")[1]);
            if (!IsFullScreen()) y += 100;
            if (!WaitForKeysReleased(1000, is_lbutton)) return;

            Native.GetCursorPos(out var pos);
            mouse_move222(x, y + 20, 20);
            down_press(LControlKey);
            mouse_click();
            up_press(LControlKey);
            mouse_move(pos);
        }

        public bool deal_handilngkey(Keys key, bool down)
        {
            if (down)
            {
                if (handling_keys.ContainsKey(key)) return true;
                handling_keys[key] = DateTime.Now;
            }
            else
            {
                handling_keys.TryRemove(key, out _);
            }
            return false;
        }

    }
}
