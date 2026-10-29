using System.Windows.Forms;
using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;
using static keyupMusic4.Native;
using static keyupMusic4.Simulate;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace keyupMusic4
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
                if (is_douyin()) return true;
                if (IsFullScreen() && e.Y == screenHeight1) return true;
                if (GetPointName() == Common.mhmain && ProcessName == Common.mhtab) return true;
            }
            return false;
        }

        public void MouseHookProc(KeyboardMouseHook.MouseEventArgs e)
        {
            //need point hwnd and process name
            var handleddd = judge_handled(e);
            if (handleddd) { e.Handled = true; VirKeyState(e.Msg); }
            if (!handleddd && !huan.Focused) Native.ReleaseCapture();

            // easy_read 内部已限频(200ms + hwnd 缓存)；Area 保持每帧保证边缘检测精度；Other 保持每帧（PowerToysCropAndLock 拖窗依赖 move）
            if (e.Msg == MouseMsg.move)
            {
                easy_read(e);
                Area(e);
                Other(e);
                return;
            }

            Task.Run(() =>
            {
                if (!NoUp.Contains(e.Msg))
                {
                    FreshProcessName2();
                    if (huan.deal_handilngkey(mousekeymap[e.Msg], !IsUpEvent(e.Msg))) return;
                }

                easy_read(e);

                Area(e);
                Other(e);

                Glass(e);

                GoBack(e);
                GoBack2(e);
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
                if (ProcessName == Common.mhtab)
                {
                    if (GetPointName() == Common.mhtab && e.data > 0)
                        press([LMenu, K]);
                    if (GetPointName() == Common.mhtab && e.data < 0)
                        press(Keys.Tab);
                    else if (GetPointName() == Common.mhmain && e.data > 0)
                        press_s(LControlKey, Keys.Tab, 4, 1);
                    else if (GetPointName() == Common.mhmain && e.data < 0)
                        press([LControlKey, Keys.Tab]);
                    else if (e.Y == 0 && e.data > 0)
                        press([LMenu, K]);
                }
                else if (e.Y == 0 && e.X == 0)
                    press([LControlKey, e.data > 0 ? Keys.Oemplus : OemMinus]);
                else if (e.Y == 0 && e.X < 200)
                    press([LControlKey, e.data > 0 ? F8 : F7]);
                else if (e.Y == 0)
                    press([e.data > 0 ? F8 : F7]);
                else if (is_douyin() && (e.X == 0))
                    press(e.data > 0 ? Left : Right);
                else if (is_douyin())
                    press(e.data > 0 ? Keys.Up : Keys.Down);
                else if (ProcessName == msedge && e.Y == screenHeight1)
                    press(e.data > 0 ? Keys.PageUp : Keys.PageDown);
                else if (IsFullVedio() && !GetPointTitle().Contains("设置"))
                    press(e.data > 0 ? Left : Right);
                else if (ProcessName == Common.ApplicationFrameHost && (ProcessTitle.Contains("png") || GetPointName() == PhotoApps))
                    press(e.data > 0 ? Left : Right);
                else if (ProcessTitle == kmRead && e.Y == screenHeight1)
                    press(e.data > 0 ? Keys.PageUp : Keys.PageDown);
                else if (e.Y == screenHeight1 && IsFullScreen())
                    press(e.data > 0 ? Left : Right);
                else if (e.X == 0)
                    press(e.data > 0 ? VolumeUp : VolumeDown);
                else if (is_tran_process == ProcessName && e.X == screenWidth1)
                    SetTransparency(e.data > 0 ? 16 : -16);
                else if (IsDesktopFocused() && e.X == 0)
                    SetDesktopWallpaper(e.data > 0 ? SwitchWallpaperFolder() : SwitchWallpaperFolder(), WallpaperStyle.Fit, true);
                else if (IsDesktopFocused())
                    SetDesktopWallpaper(e.data > 0 ? GetPreviousWallpaper() : GetNextWallpaper(), WallpaperStyle.Fit, true);
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
