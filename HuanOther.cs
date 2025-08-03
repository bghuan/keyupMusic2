using System.Collections.Concurrent;
using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;

namespace keyupMusic2
{
    public partial class Huan
    {
        public static ConcurrentDictionary<Keys, DateTime> handling_keys = new();
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
                Invoke2(() => label1.Text = label1.Text.Replace(easy_read2(e.key), easy_read2(e.key).ToLower()));
                return;
            }

            try
            {
                var _stop_keys = new Dictionary<Keys, DateTime>(handling_keys);
                Invoke(() =>
                    {
                        string asd = string.Join(" ", _stop_keys?.Select(key => easy_read(key.Key.ToString())));
                        if (label1.Text.ToLower() == asd.ToLower()) asd += " " + DateTimeNow2();
                        label1.Text = asd;
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

            _mouseKbdHook = new KeyboardMouseHook();
            _mouseKbdHook.KeyEvent += KeyBoardHookProc;

            if (is_mouse_hook)
            {
                var b = new biu();
                _mouseKbdHook.MouseEvent += b.MouseHookProc;
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
            press(100, 500, Up, Return);
        }

    }
}
