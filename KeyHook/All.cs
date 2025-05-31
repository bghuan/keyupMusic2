using System.Drawing;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public class All : Default
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
                //case Keys.MediaNextTrack:
                //case Keys.MediaPreviousTrack:
                //    quick_go_back(e);
                //    break;
                case Keys.Home:
                    copy_screen(); break;
                case Keys.End:
                    copy_secoed_screen(); break;
                //case Keys.VolumeDown:
                //    if (right_top) press(Keys.F7); break;
                //case Keys.VolumeUp:
                //    if (right_top) press(Keys.F8); break;
                case Keys.Delete:
                case Keys.Escape:
                    Special_Input = false;
                    DaleyRun_stop = true;
                    special_delete_key_time = DateTime.Now;
                    KeyTime[system_sleep_string] = DateTime.MinValue;
                    player.Stop();
                    break;
                case Keys.LMenu:
                case Keys.Tab:
                case Keys.F4:
                    Sleep(100); FreshProcessName(); break;
                //case Keys.LWin:
                //case Keys.M:
                //case Keys.B:
                //case Keys.Oem1:
                //case Keys.D9:
                //    play_sound_di(); break;
                case Keys.OemPeriod:
                    if (is_down(Keys.RControlKey)) SS().KeyPress(Keys.Apps); break;
                    //case Keys.RControlKey:
                    //    log(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()); break;
            }

            Common.hooked = false;
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
                        var pp = new Point(100, 100);
                        if(e.key == Keys.Left) pp = new Point(screen2Width, 100);
                        MoveProcessWindow(ProcessName, pp);
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
    }
}
