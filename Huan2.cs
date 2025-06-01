using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public partial class Huan
    {
        public void Invoke2(Action action, int tick = 0)
        {
            Task.Run(() => { Thread.Sleep(tick); this.Invoke(action); });
        }

        public void Invoke(Action method)
        {
            try { base.Invoke(method); }
            catch (Exception ex)
            {
                log("Invoke err: " + ex.Message);
            }
        }

        public void release_all_key(int tick = 1000)
        {
            TaskRun(() =>
            {
                var pressedKeys = release_all_keydown();
                if (pressedKeys.Any())
                    Invoke2(() => { label1.Text = string.Join(", ", pressedKeys); });
            }, tick);
        }
        private void cancel_sleep(KeyboardHookEventArgs e)
        {
            if (!no_sleep && e.key != Keys.VolumeDown && e.key != Keys.VolumeUp && e.key != Keys.MediaStop && e.key != Keys.F22)
            {
                player.Stop();
                Invoke2(() => { label1.Text = "取消睡眠"; });
                no_sleep = true;
            }
        }
        public void system_sleep()
        {
            temp_visiable = false;
            system_sleep_count = 1;
            press(Keys.MediaStop);
            Invoke(() => { SetVisibleCore(true); });
            KeyTime[system_sleep_string] = DateTime.Now;

            if (is_ctrl() || GetWindowText() == UnlockingWindow || ProcessName == LockApp || ProcessName == err)
            {
                play_sound(Keys.D0);
                system_hard_sleep();
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
            if (SuperClass.start_record)
            {
                log_process(e.key.ToString());
            }
        }

        public void Timer_Tick(int tick = 1000)
        {
            if (no_sleep) return;
            no_sleep = true;
            Invoke(() => { SetVisibleCore(false); });
            Task.Run(() => press("500;LWin;1650,1300;1650,1140", tick));
            for (int i = 0; i < 10; i++)
            {
                if (ProcessName2 == StartMenuExperienceHost) { return; }
                play_sound_di(tick);
            }
        }
        private void handle_special_or_normal_key(KeyboardHookEventArgs e)
        {
            lock (hanling_keys)
                if (!hanling_keys.ContainsKey(e.key))
                {
                    if (e.key == Keys.F9) { return; }
                    string _ProcessName = "";
                    if (special_key.Contains(e.key)) _ProcessName = log_process(e.key.ToString());
                    if (e.key == Keys.F22 && (_ProcessName == "WeChatAppEx" || _ProcessName == "WeChat")) { return; }
                    hanling_keys.Add(e.key, ProcessName);
                }
        }
        private void print_easy_read()
        {
            Dictionary<Keys, string> _stop_keys = new Dictionary<Keys, string>();
            try { _stop_keys = hanling_keys?.ToList().ToDictionary(kv => kv.Key, kv => kv.Value); }
            catch (ArgumentException ex) { }
            if (!no_sleep) return;
            Invoke(() =>
            {
                string asd = string.Join(" ", _stop_keys?.Select(key => easy_read(key.Key.ToString())));
                if (label1.Text.ToLower() == asd.ToLower()) asd += " " + DateTimeNow2();
                label1.Text = asd;
            }
            );
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
        private void super_listen_clear(Color color)
        {
            keyupMusic2_onlisten = false;
            BackColor = color;
        }

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
        public void startListen()
        {
            label1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            label2.Text = "";

            _mouseKbdHook = new MouseKeyboardHook();
            _mouseKbdHook.KeyboardHookEvent += hook_KeyDown;
            _mouseKbdHook.KeyboardHookEvent += hook_KeyUp;

            if (is_mouse_hook)
            {
                var b = new biu(this);
                _mouseKbdHook.MouseHookEvent += b.MouseHookProc;
                //Invoke(() => { b.MoveStopClickListen(); }); 
                //_mouseKbdHook.MouseHookEvent += new Douyin_game(this).MouseHookProc;
            }
            else
            {
                Text = Text + "no mousehook";
            }
            _mouseKbdHook.Install();
            play_sound_di();
        }
        public void stopListen()
        {
            if (_mouseKbdHook != null)
            {
                _mouseKbdHook.Uninstall();
                _mouseKbdHook.Dispose();
            }
        }

    }
}
