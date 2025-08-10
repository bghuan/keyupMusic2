using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
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

        public bool judge_handled(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.move && Common.no_move && !is_down(LButton)) return true;
            if (e.Msg == MouseMsg.move) return false;
            if (e.Msg == MouseMsg.click) return false;
            if (e.Msg == MouseMsg.click_up) return false;
            if (e.Msg == MouseMsg.middle) return true;
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
                //if (ProcessName == chrome && ExistProcess(Common.PowerToysCropAndLock, true))
                //    return true;
            }
            if (e.Msg == MouseMsg.wheel)
            {
                if (is_douyin() && (e.X == 0 || is_down(LButton)))
                    return true;
                if (IsFullVedio())
                    return true;
                var list = new[] { Glass2, vlc, /*Honeyview*/ };
                if (list.Contains(Common.ProcessName) && !GetPointTitle().Contains("设置"))
                    return true;
            }
            return false;
        }

        public void MouseHookProc(KeyboardMouseHook.MouseEventArgs e)
        {
            //need point hwnd and process name
            if (judge_handled(e)) { e.Handled = true; VirKeyState(e.Msg); }

            Task.Run(() =>
            {
                //if (!string.IsNullOrEmpty(Common.DeviceName)) Common.DeviceName = "";
                if (e.Msg != MouseMsg.move && e.Msg != MouseMsg.wheel) FreshProcessName();
                if (e.Msg != MouseMsg.move && e.Msg != MouseMsg.wheel && huan.deal_handilngkey(mousekeymap[e.Msg], !e.Msg.IsUpEvent())) return;

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
        private void All(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;

            if (e.Msg == MouseMsg.middle)
            {
                if (!IsAnyMusicPlayerRunning()) StartNeteaseCloudMusic();
                press(Keys.MediaPlayPause);
            }
            else if (e.Msg == MouseMsg.wheel && e.Y == 0)
            {
                Keys keys = Keys.F7;
                if (e.data > 0) keys = Keys.F8;
                press(keys);
            }
            else if (e.Msg == MouseMsg.wheel && is_douyin() && (e.X == 0 || is_down(LButton)))
            {
                Keys keys = Keys.Right;
                if (e.data > 0) keys = Keys.Left;
                press(keys);
            }
            else if (e.Msg == MouseMsg.wheel && IsFullVedio() && !GetPointTitle().Contains("设置"))
            {
                Keys keys = Keys.Right;
                if (e.data > 0) keys = Keys.Left;
                press(keys);
            }
            else if (Common.no_move && (e.Msg == MouseMsg.click_r))
                Common.no_move = false;
            else if (e.Msg == MouseMsg.click_up && LongPressClass.long_press_lbutton)
                LongPressClass.long_press_lbutton = false;
            else if (e.Msg == MouseMsg.click_r_up && LongPressClass.long_press_rbutton)
                LongPressClass.long_press_rbutton = false;
            else if (e.Msg == MouseMsg.click_r)
                click_r_point = e.Pos;

            if (catch_state && catch_key == e.Msg) catch_ed = true;
        }
        Point click_r_point = new Point();

    }
}
