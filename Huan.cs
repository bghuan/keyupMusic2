using Microsoft.Win32;
using NAudio.Gui;
using System;
using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;

namespace keyupMusic2
{
    public partial class Huan
    {
        private static Huan _instance;
        public static Huan Instance => _instance;

        public bool judge_handled(KeyboardMouseHook.KeyEventArgs e)
        {
            //if (is_alt() && is_down(Keys.Tab)) return false;
            if (e.key == Keys.LWin) return false;

            if (e.key == Keys.BrowserHome) return true;
            //if (e.key == Keys.MediaPlayPause) return true;
            if (e.key == Keys.Apps) return true;

            if (e.key == Keys.F1 && !isctrl()) return true;
            if (e.key == Keys.F2 && !ProcessTitle.Contains("tudio") && IsFullScreen()) return true;
            if (e.key == Keys.F3) return true;
            if (e.key == Keys.F9) return true;
            if (keyupMusic2_onlisten) { e.Handled = true; }

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
            if (ProcessName == Common.msedge) if (e.key == Keys.Tab && !is_alt()) return true;

            if (KeyFunc.judge(e)) return true;
            var flag = Chrome.judge_handled(e) || Douyin.judge_handled(e) || WinClass.judge_handled(e) || CoocaaClass.judge_handled(e) || AirKeyboardClass.judge_handled(e);
            return flag;
        }

        private void KeyBoardHookProc(KeyboardMouseHook.KeyEventArgs e)
        {
            if (e.key != F1) FreshProcessName2();
            if (lock_err) play_sound(D8);
            //if (ProcessName == SearchHost) return;
            if (judge_handled(e)) { e.Handled = true; VirKeyState(e); }
            var ha = deal_handilngkey(e.key, e.Type == KeyType.Down);

            if (!ha && (!is_steam_game() || (e.key == Tab && e.Type == KeyType.Up)))
                VirtualKeyboardForm.Instance?.TriggerKey(e.key, e.Type == KeyType.Up);

            Task.Run(() =>
            {
                if (!ha)
                    print_easy_read(e);

                //if (quick_replace_key(e)) return;
                if (KeyFunc.HookEvent(e)) return;
                if (ha) { return; }

                keyupMusic2.KeyUp.yo(e);

                if (e.Type == KeyType.Up) return;
                //{
                //if (CoocaaClass.Hooked(e)) return;
                //keyupMusic2.KeyUp.yo(e);
                //    return;
                //}
                Console2.WriteLine($"Hook, {e.key}, {Common.DeviceName}, {e.Type.ToString()} ");

                if (CoocaaClass.Hooked(e)) return;
                if (AirKeyboardClass.Hooked(e)) return;
                if (Super.HookEvent(e)) return;
                // job f3 f9 to var special key and customs
                if (e.key == Keys.F3 || e.key == Keys.F9)
                {
                    if (e.key == F9) play_sound_di();
                    super_listen();
                    form_move(); return;
                }

                Devenv.HookEvent(e);
                Douyin.HookEvent(e);
                Chrome.handlehandle(e);

                Other.HookEvent(e);
                All.HookEvent(e);
                Win.HookEvent(e);

                //KeyFunc.HookEvent(e);

                Music.HookEvent(e);

                if (GetPointName() == msedge && ProcessName != msedge)
                    if (e.key == Keys.PageDown || e.key == Space) mousewhell(-4);
                    else if (e.key == Keys.PageUp || e.key == Keys.Up) mousewhell(4);
            });
        }
        public void start_catch(string msg)
        {
            play_sound_di2();
            if (msg.Contains(start_check_str))
            {
                start_catch_time = DateTime.Now;
                //log("start_catch " + ProcessName);
                //string[] list_f1 = [StartMenuExperienceHost, SearchHost, clashverge,];
                string[] list_f1 = [clashverge,];
                //string[] list_nothing = [devenv, Common.keyupMusic2, explorer, cs2];

                if (Position.X == 0 && Position.Y == 0)
                {
                    AllClass.quick_visiualstudio();
                    //SetTransparency();
                }
                else if (Position.X == screenWidth1 && Position.Y == screenHeight1)
                {
                    system_sleep(true);
                }
                else if (Position.X == 0)
                {
                    string executablePath = Process.GetCurrentProcess().MainModule.FileName;
                    Process.Start(executablePath);
                    Environment.Exit(0);
                }
                else if (Position.X == screenWidth1)
                {
                    SetTransparency();
                }
                else if (Position.Y == 0)
                {
                    LossScale();
                }

                else if (ProcessName == cs2)
                    press("1301,48;100;1274,178;2260,1374");
                else if (list_f1.Contains(ProcessName))
                    changeClash();
                //else if (list_nothing.Contains(ProcessName)) { }
                //else if (IsDesktopFocused()) { }
                //else if (is_steam_game())
                //    LossScale();
                //else
                //{ }
            }
            else if (msg.Contains(start_check_str2))
            {
                system_hard_sleep();
            }
        }

    }
}
