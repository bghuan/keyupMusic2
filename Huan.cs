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

            for (int i = 0; i < replace.Count; i++)
            {
                if (ProcessName == replace[i].process && e.key == replace[i].defore)
                    return true;
            }
            //if (e.key == Keys.F1 && !isctrl())
            //{
            //    var list = new List<string> { Common.chrome, msedge, Common.devenv };
            //    if (list.Contains(ProcessName)) return true;
            //}
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
                var list = new List<string> { steam, cs2, Glass2, PowerToysCropAndLock, vlc };
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
            if (ProcessName == Common.msedge && !is_douyin())
            {
                if (e.key == Keys.Home) return true;
                if (e.key == Keys.End) return true;
                if (e.key == Keys.VolumeUp || e.key == Keys.VolumeDown)
                    if (e.X == screenWidth1 || e.Y == screenHeight1)
                        return true;
            }
            if (ProcessName == Common.ElecHead)
            {
                if (e.key == Keys.Left) return true;
                if (e.key == Keys.Right) return true;
            }
            var flag = Chrome.judge_handled(e) || Douyin.judge_handled(e) || SuperClass.judge_handled(e);
            return flag;
        }
        private void hook_KeyDown(KeyboardHookEventArgs e)
        {
            if (e.Type != KeyboardType.KeyDown) return;
            if (keyupMusic2_onlisten) { e.Handled = true; }
            if (is_alt() && (e.key == Keys.F4 || e.key == Keys.Tab)) { return; }

            //print_easy_read();
            if (judge_handled(e)) { e.Handled = true; }
            if (quick_replace_key(e)) return;
            if (hanling_keys.ContainsKey(e.key)) return;

            FreshProcessName();

            Task.Run(() =>
            {
                handle_special_or_normal_key(e);
                print_easy_read();
                //start_record(e);
            });
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

                    Music.hook_KeyDown_keyupMusic2(e);
                    cancel_sleep(e);
                });
            }
        }


        public void hook_KeyUp(KeyboardHookEventArgs e)
        {
            if (e.Type == KeyboardType.KeyDown) return;
            if (judge_handled(e)) { e.Handled = true; }
            lock (hanling_keys)
            {
                hanling_keys.Remove(e.key);
                if (!no_sleep) return;
                Invoke2(() =>
                {
                    label1.Text = label1.Text.Replace(easy_read(e.key), easy_read(e.key).ToLower());
                    keyupMusic2.KeyUp.yo(e);
                });
            }

            quick_replace_key(e, true);
        }
        private void F39(KeyboardHookEventArgs e)
        {
            play_sound_di();
            if (e.key == Keys.F9 && (keyupMusic2_onlisten || !no_sleep))
            {
                system_sleep();
            }
            else if (keyupMusic2_onlisten)
            {
                var aa = temp_visiable;
                temp_visiable = false;
                keyupMusic2_onlisten = false;
                Invoke(() => { SetVisibleCore(aa); });
            }
            else
            {
                form_move();
                super_listen();
            }
        }

        private void form_move()
        {
            Invoke2(() =>
            {
                if (Opacity == 0) { return; }
                var current_hwnd = ProcessName;
                if (!Visible) { temp_visiable = true; SetVisibleCore(true); }
                if (Focused) { FocusProcessSimple(current_hwnd); }
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

        public void start_catch(string msg)
        {
            if (msg.Contains("starting"))
            {
                play_sound_di2();
                if (Position.Y == 0)
                {
                    Invoke(() => { temp_visiable = !Visible; SetVisibleCore(temp_visiable); });
                    //press([Keys.LControlKey, Keys.F1]);
                }
                else if (Position.X == screenWidth1 && Position.Y == screenHeight1)
                {
                    //system_hard_sleep();
                }
                else if (Position.X == screenWidth1)
                {
                    string executablePath = Process.GetCurrentProcess().MainModule.FileName;
                    Process.Start(executablePath);
                    Environment.Exit(0);
                }
                else
                {
                    List<string> lines = new List<string>()
                        { devenv , Common.keyupMusic2, explorer, cs2 };
                    if (SearchHost.Contains(ProcessName2))
                    {
                        press([Keys.LControlKey, Keys.F1]);
                        press(Keys.LWin);
                    }
                    else if (lines.Contains(ProcessName2))
                        return;
                    else
                        LossScale();
                }
            }
            else if (msg.Contains("sleep"))
            {
                system_hard_sleep();
            }
        }

    }
}
