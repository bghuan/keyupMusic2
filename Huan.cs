using System;
using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public partial class Huan
    {
        public bool judge_handled(KeyboardHookEventArgs e)
        {
            if (is_alt() && is_down(Keys.Tab)) return false;
            if (e.key == Keys.F3) return true;
            if (e.key == Keys.F9) return true;
            if (keyupMusic2_onlisten) { e.Handled = true; }

            if (replace.Any(t => ProcessName == t.process && e.key == t.defore))
                return true;
            if (e.key == Keys.F2 && ProcessName == Common.chrome)
                return true;
            if (e.key == Keys.F10 || e.key == Keys.F11 || e.key == Keys.F12)
            {
                if (!is_down(Keys.Delete))
                    return true;
            }
            if (e.key == Keys.OemPeriod)
            {
                if (is_down(Keys.RControlKey))
                    return true;
            }
            if (e.key == Keys.MediaPreviousTrack || e.key == Keys.MediaNextTrack)
            {
                List<string> list = new() { steam, cs2, Glass2, PowerToysCropAndLock, vlc };
                if (list.Contains(ProcessName)) return true;
                if (list_go_back.Contains(ProcessName)) return true;
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
            var flag = Chrome.judge_handled(e) || Douyin.judge_handled(e) || WinClass.judge_handled(e);
            return flag;
        }

        private void hook_KeyDown(KeyboardHookEventArgs e)
        {
            if (e.Type != KeyboardType.KeyDown) return;
            if (is_alt() && (e.key == Keys.F4 || e.key == Keys.Tab)) { return; }

            FreshProcessName();
            if (judge_handled(e)) { e.Handled = true; VirKeyState(e.key); }
            if (quick_replace_key(e)) return;

            if (handling_keys.ContainsKey(e.key)) return;
            handling_keys[e.key] = ProcessName;

            print_easy_read();
            //start_record(e);
            Log.logcache(e.key.ToString());

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
                    ACPhoenix.hook_KeyDown_ddzzq(e);
                    Douyin.hook_KeyDown_ddzzq(e);
                    Chrome.handlehandle(e);

                    Other.hook_KeyDown(e);
                    All.hook_KeyDown_ddzzq(e);
                    Win.hook_KeyDown_ddzzq(e);

                    Music.hook_KeyDown_keyupMusic2(e);
                });
            }
        }

        public void hook_KeyUp(KeyboardHookEventArgs e)
        {
            if (e.Type == KeyboardType.KeyDown) return;
            if (judge_handled(e)) { e.Handled = true; VirKeyState(e.key, true); }
            handling_keys.Remove(e.key);

            Invoke2(() => label1.Text = label1.Text.Replace(easy_read2(e.key), easy_read2(e.key).ToLower()));

            quick_replace_key(e, true);
            keyupMusic2.KeyUp.yo(e);
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
                    TcpServer.socket_run = false;
                    TcpServer.socket_write(start_check_str);
                    TcpServer.close();
                    Environment.Exit(0);
                    return true;
                }
            }
            return false;
        }
        public static string start_check_str = "starting";
        public static string start_check_str2 = "sleep";
        public bool temp_visiable = false;

        public void start_catch(string msg)
        {
            play_sound_di2();
            if (msg.Contains(start_check_str))
            {
                string[] list_f1 = [StartMenuExperienceHost, SearchHost, clashverge,];
                string[] list_nothing = [devenv, Common.keyupMusic2, explorer, cs2];
                if (Position.Y == 0 )
                {
                    Invoke(() => { SetVisibleCore(!Visible); });
                }
                if (Position.X == screen2Width)
                {
                    press([Keys.LControlKey, Keys.F1]);
                }
                else if (Position.X == screenWidth1 && Position.Y == screenHeight1)
                {
                    system_sleep(true);
                }
                else if (Position.X == screenWidth1)
                {
                    string executablePath = Process.GetCurrentProcess().MainModule.FileName;
                    Process.Start(executablePath);
                    Environment.Exit(0);
                }
                else if (list_f1.Contains(ProcessName))
                {
                    press([Keys.LControlKey, Keys.F1]);
                    if (iswinopen)
                        press(LWin);
                }
                else if (list_nothing.Contains(ProcessName))
                {
                    return;
                }
                else
                {
                    LossScale();
                }
            }
            else if (msg.Contains(start_check_str2))
            {
                system_hard_sleep();
            }
        }

    }
}
