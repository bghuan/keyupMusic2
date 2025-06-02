using System.Drawing;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public class AllClass : Default
    {
        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            string module_name = ProcessName;
            Common.hooked = true;
            handling_keys = e.key;
            bool right_top = Position.Y == 0 && Position.X == 2559;
            //if (!handling) return;

            quick_open(e, module_name);
            quick_number(e);

            switch (e.key)
            {
                case Keys.F4:
                    quick_close(); fresh_name(); break;
                case Keys.F10:
                    quick_what(); break;
                case Keys.F11:
                    quick_visiualstudio(module_name); break;
                case Keys.F12:
                    quick_wechat_or_notify(module_name); break;

                case Keys.Home:
                    copy_screen(); break;
                case Keys.End:
                    copy_secoed_screen(); break;
                case Keys.PageDown:
                    quick_gamg_alttab(e, module_name); break;

                case Keys.Delete:
                case Keys.Escape:
                    esc(); break;

                case Keys.LMenu:
                case Keys.Tab:
                    fresh_name(); break;

                case Keys.OemPeriod:
                    if (is_down(Keys.RControlKey)) SS().KeyPress(Keys.Apps); break;
            }

            Common.hooked = false;
        }

        private static void quick_what()
        {
            //if (TryFocusProcess(Common.cs2)) return;
            //if (TryFocusProcess(Common.SplitFiction)) return;
            ////if (TryFocusProcess(Common.steam)) break;
            //quick_max_chrome();
        }

        private static void quick_gamg_alttab(KeyboardHookEventArgs e, string module_name)
        {
            var allow = is_steam_game() || module_name == chrome || module_name == PowerToysCropAndLock;
            if (!allow) return;
            quick_max_chrome(e.Pos);
        }

        private static void quick_close()
        {
            //if (ProcessName == Common.keyupMusic2) return;
            //if (lock_err) { system_hard_sleep(); return; }
            CloseProcess();
        }

        private static void fresh_name()
        {
            Sleep(100);
            FreshProcessName();
        }

        private static void quick_wechat_or_notify(string module_name)
        {
            if (is_down(Keys.Delete) || is_ctrl()) return;
            if (Common.WeChat == module_name)
            {
                CloseProcess(module_name);
            }
            else if (GetWindowText() == UnlockingWindow || ProcessName == LockApp || ProcessName == err)
            {
                SuperClass.hook_KeyDown(Keys.N);
            }
            else
            {
                Common.FocusProcess(Common.WeChat);
                Thread.Sleep(10);
                if (ProcessName2 == Common.WeChat) return;
                run_wei();
            }
        }

        private static void quick_visiualstudio(string module_name)
        {
            if (is_down(Keys.Delete) || is_ctrl()) return;
            if (Common.devenv == module_name)
            {
                HideProcess(module_name);
            }
            else
            {
                if (Common.FocusProcess(Common.devenv)) return;
                run_vis();
            }
        }

        private static void esc()
        {
            Special_Input = false;
            DaleyRun_stop = true;
            special_delete_key_time = DateTime.Now;
            KeyTime[system_sleep_string] = DateTime.MinValue;
            player.Stop();
        }

        private static void quick_go_back(KeyboardHookEventArgs e)
        {
            {
                if (!list_go_back.Contains(ProcessName)) return;

                if (e.key == Keys.MediaNextTrack && ProcessName == msedge)
                    press(Keys.Right);
                if (e.key == Keys.MediaNextTrack && ProcessName == chrome)
                    press(Keys.F);

                if (e.key == Keys.MediaNextTrack)
                    MouseForward();
                else
                    MouseBack();
            }
        }

        private static void quick_open(KeyboardHookEventArgs e, string module_name)
        {
            if (is_down(Keys.LWin))
                switch (e.key)
                {
                    case Keys.D1:
                        break;
                    case Keys.D2:
                        break;
                    case Keys.Right:
                    case Keys.Left:
                        int arraw = e.key == Keys.Left ? 2 : 1;
                        quick_left_right(arraw);
                        break;
                }
        }

        private static void quick_number(KeyboardHookEventArgs e)
        {
            if (is_down(Keys.F1))
                switch (e.key)
                {
                    case Keys.Oemcomma:
                        press(Keys.D1); break;
                    case Keys.OemPeriod:
                        press(Keys.D2); break;
                    case Keys.Oem2:
                        press(Keys.D3); break;
                    case Keys.K:
                        press(Keys.D4); break;
                    case Keys.L:
                        press(Keys.D5); break;
                    case Keys.OemSemicolon:
                        press(Keys.D6); break;
                    case Keys.I:
                        press(Keys.D7); break;
                    case Keys.O:
                        press(Keys.D8); break;
                    case Keys.P:
                        press(Keys.D9); break;
                    case Keys.Space:
                        press(Keys.D0); break;
                }
        }
        private static void run_wei()
        {
            if (!Common.ExistProcess(Common.WeChat))
            {
                press("LWin;100;WEI;en;100;Enter;", 50);
                return;
            }
            press([Keys.LControlKey, Keys.LMenu, Keys.W]);
        }

        private static void run_vis()
        {
            press("LWin;VISUAL;en;100;Apps;100;Enter;", 100);
            TaskRun(() => { press("Tab;Down;Enter;", 100); }, 1800);
        }
    }
}
