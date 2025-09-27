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
        public bool judge_handled(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.move && Common.no_move && !is_down(LButton)) return true;
            if (e.Msg == MouseMsg.move) return false;
            if (e.Msg == MouseMsg.click) return false;
            if (e.Msg == MouseMsg.click_up) return false;
            if (e.Msg == MouseMsg.middle) return true;
            if (e.Msg == MouseMsg.middle_up) return true;

            if (go_back_keys.Contains(e.Msg)) return true;
            if (e.Msg == MouseMsg.wheel)
            {
                if (is_lbutton()) return false;
                if (e.Y == 0) return true;
                if (is_douyin() && (e.X == 0 || is_down(LButton))) return true;
                if (IsFullVedio()) return true;
                var list = new[] { Glass2, vlc, /*Honeyview*/ };
                if (list.Contains(Common.ProcessName) && !GetPointTitle().Contains("设置"))
                    return true;
            }
            return false;
        }

        public void MouseHookProc(KeyboardMouseHook.MouseEventArgs e)
        {
            //if (e.Msg == MouseMsg.move) return;
            //need point hwnd and process name
            if (judge_handled(e)) { e.Handled = true; VirKeyState(e.Msg); }

            Task.Run(() =>
            {
                if (!NoUp.Contains(e.Msg))
                {
                    FreshProcessName2();
                    if (huan.deal_handilngkey(mousekeymap[e.Msg], !IsUpEvent(e.Msg))) return;
                }

                easy_read(e);

                Area(e);
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
            if (ProcessName == b1) return;

            if (e.Msg == MouseMsg.middle)
            {
                if (ProcessName == b1) mouse_middle();
                else press(Keys.MediaPlayPause);
            }
            else if (e.Msg == MouseMsg.wheel)
            {
                if (e.Y == 0 && e.X == 0)
                    press([LMenu, e.data > 0 ? F8 : F7]);
                else if (e.Y == 0 && e.X > screenWidth)
                    press([LShiftKey, e.data > 0 ? F8 : F7]);
                else if (e.Y == 0)
                    press(e.data > 0 ? F8 : F7);
                else if (is_douyin() && (e.X == 0 || is_down(LButton)))
                    press(e.data > 0 ? Left : Right);
                else if (IsFullVedio() && !GetPointTitle().Contains("设置"))
                    press(e.data > 0 ? Left : Right);
                else if (ProcessName == Common.vlc)
                    press(e.data > 0 ? Left : Right);
                else if (ProcessName == Common.ApplicationFrameHost && (ProcessTitle.Contains("png") || GetPointName() == PhotoApps))
                    press(e.data > 0 ? Left : Right);
                else if (ProcessName == msedge && e.Y == screenHeight1)
                    press(e.data > 0 ? Keys.PageUp : Keys.PageDown);
            }
            else if (e.Msg == MouseMsg.wheel_h)
                press(e.data > 0 ? VolumeDown : VolumeUp);
            else if (e.Msg == MouseMsg.click_r && Common.no_move)
                Common.no_move = false;
            else if (e.Msg == MouseMsg.click_r)
                click_r_point = e.Pos;
            else if (e.Msg == MouseMsg.click_up && LongPressClass.long_press_lbutton)
                LongPressClass.long_press_lbutton = false;
            else if (e.Msg == MouseMsg.click_r_up && LongPressClass.long_press_rbutton)
                LongPressClass.long_press_rbutton = false;

            if (catch_state && catch_key == e.Msg) catch_ed = true;
        }

    }
}
