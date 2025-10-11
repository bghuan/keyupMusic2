using Microsoft.Win32;
using System.Collections.Concurrent;
using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;

namespace keyupMusic2
{
    public partial class Huan
    {
        public static ConcurrentDictionary<Keys, DateTime> handling_keys = new();
        public static ConcurrentDictionary<Keys, DateTime> handling_keys2 = new();
        public async void Invoke2(Action action, int tick = 0)
        {
            await Task.Delay(tick);
            Invoke(action);
        }

        public void Invoke(Action method)
        {
            if (IsDisposed) return;
            try { base.Invoke(method); }
            catch (Exception ex) { }
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
            //HomeProcess(chrome);

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
            //if (SuperClass.start_record)
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
            catch (ArgumentException ex) { }
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
            keyupMusic2_onlisten = false;
            BackColor = color;
        }

        public void startListen()
        {
            label1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            label2.Text = "";

            _mouseKbdHook = new KeyboardMouseHook(this.Handle);
            _mouseKbdHook.KeyEvent += KeyBoardHookProc;

            //if (is_mouse_hook)
            {
                var b = new biu();
                _mouseKbdHook.MouseEvent += b.MouseHookProc;
                //Invoke(() => { b.MoveStopClickListen(); }); 
                //_mouseKbdHook.MouseHookEvent += new Douyin_game(this).MouseHookProc;
            }
            //else
            //{
            //    Text = Text + "no mousehook";
            //}
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
            //TimeSpan elapsed = DateTime.Now - startTime;

            //int currentX = (int)(startPoint.X + (endPoint.X - startPoint.X) * (elapsed.TotalMilliseconds / timerMove_Tick_tick));
            //int currentY = startPoint.Y;
            ////Location = new Point(currentX, currentY);

            //if (elapsed.TotalMilliseconds > timerMove_Tick_tick) { timer_stop(); }
            timer_stop();
        }


        public void system_sleep_timer(int tick = 200)
        {
            if (!ready_to_sleep) return;
            ready_to_sleep = false;
            system_sleep_count = 1;
            Invoke(() => { SetVisibleCore(false); });
            //press("500;LWin;1650,1300;1650,1140", tick);
            //press("500;LWin;1650,1300;", tick);
            log("system_sleep");

            if (!IsDesktopFocused())
                press([LWin, D]);
            CloseDesktopWindow();
            //if (GetWindowTitle() == "关闭 Windows")
            press(100, 500, Return);
        }
        //job no release f3 no hide no stop superlisten
        public void timer_stop()
        {
            blobForm.Hide();
            blobForm.changeFlag(false);
            //Location = endPoint;
            timerMove.Stop();
            //if (!temp_visiable) SetVisibleCore(false);
        }
        private void form_move()
        {
            Invoke(() =>
            {
                if (Opacity == 0) { return; }
                var current_hwnd = ProcessName;
                //temp_visiable = Visible;
                //if (!Visible) { SetVisibleCore(true); }
                if (Focused && current_hwnd == "") { altab(); }
                else if (Focused) { FocusProcessSimple(current_hwnd); }
                blobForm.Show();
                blobForm.changeFlag(true);
                //timerMove.Interval = 1000 / 144;
                //// 解除旧的事件绑定，防止叠加
                //timerMove.Tick -= timerMove_Tick;
                //timerMove.Tick += timerMove_Tick;
                timerMove.Start();
                //Location = startPoint;
                startTime = DateTime.Now;
            });
        }

        private void super_listen()
        {
            keyupMusic2_onlisten = true;
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
            Process[] processes = Process.GetProcessesByName(Common.keyupMusic2);
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
                //RestartProcess(TwinkleTray, TwinkleTrayexe);
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
            //SuperClass.get_point_color();
            mouse_move222(x, y + 20, 20);
            //mouse_middle_click(20);
            down_press(LControlKey);
            mouse_click();
            up_press(LControlKey);
            mouse_move(pos);
            //SuperClass.get_point_color();
            //TaskRun(() => { SuperClass.get_point_color(); }, 3000);
        }

        public bool deal_handilngkey(Keys key, bool down)
        {
            if (down)
            {
                if (handling_keys.ContainsKey(key)) return true;
                handling_keys[key] = DateTime.Now;
                //if (key == LMenu) handling_keys[Menu] = DateTime.Now;
                //log("2" + key + handling_keys[key]);
            }
            else
            {
                handling_keys.TryRemove(key, out _);
                //if (key == LMenu) handling_keys.TryRemove(Menu, out _);
            }
            return false;
        }

    }
}
