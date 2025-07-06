using Microsoft.Win32;
using System;
using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public partial class Huan
    {
        private static Huan _instance;
        public static Huan Instance => _instance;

        public bool judge_handled(KeyboardHookEventArgs e)
        {
            if (is_alt() && is_down(Keys.Tab)) return false;
            if (e.key == Keys.F3) return true;
            if (e.key == Keys.F9) return true;
            if (keyupMusic2_onlisten) { e.Handled = true; }

            if (replace.Any(t => e.key == t.defore && (string.IsNullOrEmpty(t.process) || ProcessName == t.process)))
                return true;
            if (e.key == Keys.F10 || e.key == Keys.F11 || e.key == Keys.F12)
                if (!is_down(Keys.Delete))
                    return true;
            if (e.key == Keys.OemPeriod)
                if (is_down(Keys.RControlKey))
                    return true;
            if (e.key == Keys.Escape)
                if (is_playing)
                    return true;
            if (e.key == Keys.MediaPreviousTrack || e.key == Keys.MediaNextTrack)
            {
                if (biu.list_go_back.Contains(ProcessName)) return true;
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
            if (ProcessName == Common.devenv)
            {
                if (e.key == Keys.F && is_shift() && is_alt())
                    return true;
            }
            if (ProcessName == Common.VSCode)
            {
                if (e.key == Keys.Q && isctrl())
                    return true;
            }
            if (ProcessName == Common.msedge && !is_douyin())
            {
                if (e.key == Keys.VolumeUp || e.key == Keys.VolumeDown)
                    if (e.X == screenWidth1 || e.Y == screenHeight1)
                        return true;
            }
            var flag = Chrome.judge_handled(e) || Douyin.judge_handled(e) || WinClass.judge_handled(e);
            return flag;
        }

        private void KeyBoardHookProc(KeyboardHookEventArgs e)
        {
            //if (e.Type != KeyboardType.KeyDown) return;
            FreshProcessName();
            if (!is_steam_game())
                VirtualKeyboardForm.Instance?.TriggerKey(e.key, e.Type == KeyboardType.KeyUp);
            if (judge_handled(e)) { e.Handled = true; VirKeyState(e); }
            if (quick_replace_key(e)) return;
            if (deal_handilngkey(e)) return;
            print_easy_read(e);

            //Log.logcache(e.key.ToString());
            //start_record(e);

            if (e.Type == KeyboardType.KeyUp)
            {
                keyupMusic2.KeyUp.yo(e);
                return;
            }
            if (e.key == Keys.F3 || e.key == Keys.F9)
            {
                F39(e);
            }
            else
            {
                Task.Run(() =>
                {
                    Super.hook_KeyDown_keyupMusic2(e);
                    if (keyupMusic2_onlisten) return;

                    Devenv.hook_KeyDown_ddzzq(e);
                    Douyin.hook_KeyDown_ddzzq(e);
                    Chrome.handlehandle(e);

                    Other.hook_KeyDown(e);
                    All.hook_KeyDown_ddzzq(e);
                    Win.hook_KeyDown_ddzzq(e);

                    Music.hook_KeyDown_keyupMusic2(e);
                });
            }
        }

        private bool deal_handilngkey(KeyboardHookEventArgs e)
        {
            if (e.Type == KeyboardType.KeyDown)
            {
                if (handling_keys.ContainsKey(e.key)) return true;
                handling_keys[e.key] = DateTime.Now;
            }
            else
                handling_keys.TryRemove(e.key, out _);
            return false;
        }

        private void F39(KeyboardHookEventArgs e)
        {
            play_sound_di();
            if (e.key == Keys.F9 && keyupMusic2_onlisten)
            {
                system_sleep();
            }
            else if (keyupMusic2_onlisten)
            {
                keyupMusic2_onlisten = false;
                Invoke(() =>
                {
                    temp_visiable = !temp_visiable;
                    SetVisibleCore(temp_visiable);
                });
            }
            else
            {
                form_move();
                super_listen();
            }
        }

        public void timer_stop()
        {
            Location = endPoint;
            timerMove.Stop();
            if (!temp_visiable) SetVisibleCore(false);
        }
        private void form_move()
        {
            Invoke2(() =>
            {
                if (Opacity == 0) { return; }
                var current_hwnd = ProcessName;
                temp_visiable = Visible;
                if (!Visible) { SetVisibleCore(true); }
                if (Focused) { FocusProcessSimple(current_hwnd); }
                timerMove.Interval = 1000 / 144;
                timerMove.Tick += timerMove_Tick;
                timerMove.Start();
                Location = startPoint;
                startTime = DateTime.Now;
            });
        }
        private void timerMove_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - startTime;

            int currentX = (int)(startPoint.X + (endPoint.X - startPoint.X) * (elapsed.TotalMilliseconds / timerMove_Tick_tick));
            int currentY = startPoint.Y;
            Location = new Point(currentX, currentY);

            if (elapsed.TotalMilliseconds > timerMove_Tick_tick) { timer_stop(); }
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
                justResumed = false;
                MoonTime.Instance?.SetInitAngle();
                VirtualKeyboardForm.Instance?.SetInitClean();
                system_sleep_count = 0;
                DaleyRun_stop = true;
                player.Stop();
                CleanMouseState();
                ready_to_sleep = false;
            }
        }
        public void start_catch(string msg)
        {
            play_sound_di2();
            if (msg.Contains(start_check_str))
            {
                string[] list_f1 = [StartMenuExperienceHost, SearchHost, clashverge,];
                string[] list_nothing = [devenv, Common.keyupMusic2, explorer, cs2];
                if (Position.X == 0 && Position.Y == screenHeight1)
                    AllClass.run_vis();
                else if (Position.Y == 0)
                    Invoke(() => { SetVisibleCore(!Visible); });
                else if (Position.X == screenWidth1 && Position.Y == screenHeight1)
                    system_sleep(true);
                else if (Position.X == 0 || ProcessName == devenv)
                {
                    string executablePath = Process.GetCurrentProcess().MainModule.FileName;
                    Process.Start(executablePath);
                    Environment.Exit(0);
                }
                else if (list_f1.Contains(ProcessName))
                    changeClash();
                else if (list_nothing.Contains(ProcessName)) { }
                else if (IsDesktopFocused()) { }
                else if (ProcessName == Honeyview)
                    press_raw(OemPeriod);
                else
                    LossScale();
            }
            else if (msg.Contains(start_check_str2))
            {
                system_hard_sleep();
            }
        }

    }
}
