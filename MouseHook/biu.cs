using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;
using static keyupMusic2.Native;
using static keyupMusic2.Simulate;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace keyupMusic2
{
    public partial class biu
    {
        public biu()
        {
            ReplaceKey2.init();
            init();
        }
        public static Point r_down_x = Point.Empty;
        bool menu_opened = false;

        public bool judge_handled(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return false;
            if (e.Msg == MouseMsg.middle && !raw_middle) return true;
            if (e.Msg == MouseMsg.middle_up) return true;
            if (e.Msg == MouseMsg.wheel && e.Y == 0) return true;

            if (go_back_keys.Contains(e.Msg) && ReplaceKey2.Catched(ProcessName, e.Msg))
            {
                return true;
            }
            if (go_back_keys.Contains(e.Msg))
            {
                if (is_down(Keys.XButton1) || is_down(Keys.XButton2)) { return false; }
                if (!list_go_back.Contains(ProcessName)) return true;
                if (Common.cs2.Equals(ProcessName)) return true;
                if (is_douyin()) return true;
            }
            if (e.Msg == MouseMsg.click_r || e.Msg == MouseMsg.click_r_up)
            {
                var list = new[] { devenv, Glass2, Glass3, BandiView };
                if (list.Contains(Common.ProcessName))
                {
                    //if (is_down(Keys.RButton)) return false;
                    if (!list.Contains(GetPointName())) return false;
                    return true;
                }
            }
            if (e.Msg == MouseMsg.wheel)
            {
                if (e.X == 0)
                {
                    var list2 = new[] { chrome };
                    if (list2.Contains(Common.ProcessName))
                        return true;
                }
                var list = new[] { Glass2, vlc, Honeyview };
                if (list.Contains(Common.ProcessName))
                    return true;
            }
            return false;
        }

        public void MouseHookProc(MouseHookEventArgs e)
        {
            if (e.Msg != MouseMsg.move) FreshProcessName();
            if (judge_handled(e))
            {
                e.Handled = true;
                VirKeyState(e.Msg);
                //if (IsDiffProcess())
                //    FocusPointProcess();
            }
            //if (ProcessName==explorer)
            //    if (IsDiffProcess())
            //    FocusPointProcess();

            Task.Run(() =>
            {
                easy_read(e);

                Cornor(e);
                BottomLine(e);
                Other(e);

                Glass(e);

                GoBack(e);
                GoBack2(e);
                //Gestures(e);
                All(e);
            });
        }
        private void All(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;

            if (e.Msg == MouseMsg.middle && !raw_middle)
            {
                press(Keys.MediaPlayPause);
                if (!IsAnyMusicPlayerRunning())
                {
                    StartNeteaseCloudMusic();
                }
            }
            if (e.Msg == MouseMsg.wheel && e.Y == 0)
            {
                Keys keys = Keys.F7;
                if (e.data > 0) keys = Keys.F8;
                press(keys);
            }
            if (catch_state && catch_key == e.Msg) catch_ed = true;
        }

    }
}
