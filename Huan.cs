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

            for (int i = 0; i < replace.Count; i++)
            {
                if (ProcessName == replace[i].process && e.key == replace[i].defore)
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
        public class ReplaceKey
        {
            public ReplaceKey(string key, Keys defore, Keys after) { this.process = key; this.defore = defore; this.after = after; }
            public string process;
            public Keys defore;
            public Keys after;
        }
        public static List<ReplaceKey> replace = new List<ReplaceKey> {
           //new ReplaceKey( msedge,Keys.A,Keys.D),
           //new ReplaceKey( msedge,Keys.D,Keys.A),
           new ReplaceKey( Windblown,Keys.W,Keys.S),
           new ReplaceKey( Windblown,Keys.S,Keys.W),
        };
        private void hook_KeyDown(KeyboardHookEventArgs e)
        {
            if (e.Type != KeyboardType.KeyDown) return;
            if (keyupMusic2_onlisten) { e.Handled = true; }
            if (is_alt() && (e.key == Keys.F4 || e.key == Keys.Tab)) { return; }

            //print_easy_read();
            if (judge_handled(e)) { e.Handled = true; }
            for (int i = 0; i < replace.Count; i++)
            {
                if (ProcessName == replace[i].process && e.key == replace[i].defore)
                {
                    down_press(replace[i].after);
                    return;
                }
            }

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

            for (int i = 0; i < replace.Count; i++)
            {
                if (ProcessName == replace[i].process && e.key == replace[i].defore)
                    up_press(replace[i].after);
            }
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
        public static bool start_check()
        {
            if (IsAdministrator()) return false;
            int currentProcessId = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcessesByName(Common.keyupMusic2);
            foreach (Process process in processes)
            {
                if (process.Id != currentProcessId)
                {
                    TcpServer.socket_run = false;
                    TcpServer.socket_write("starting");
                    TcpServer.close();
                    //SendMessageToWindow(Common.keyupMusic2,"3214421432");
                    //press_raw([Keys.LControlKey, Keys.F3]);
                    //TaskRun(() => { Application.Exit(); }, 5550);
                    //MessageBox.Show("ddd"); 
                    return true;
                }
            }
            return false;
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
            if (!IsAdministrator()) Text = Text + "(非管理员)";
        }

    }
}
