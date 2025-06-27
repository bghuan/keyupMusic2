using System.Diagnostics;
using System.Security.Policy;
using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    partial class biu
    {

        Dictionary<MouseMsg, DateTime> MouseMsgTime = new Dictionary<MouseMsg, DateTime>();
        static bool is_PowerToysCropAndLock_down = false;
        public void Other(MouseHookEventArgs e)
        {
            //bool double_click = e.Msg == MouseMsg.WM_LBUTTONUP && is_double_click();

            if (ProcessName == Common.PowerToysCropAndLock)
                PowerToysCropAndLock(e);
            else if (ProcessName == Common.chrome)
                Chrome(e);
            else if (ProcessName == Common.cs2)
                Cs2(e);
            else if (ProcessName == Common.devenv)
                Devenv(e);
            else if (is_douyin())
                Douyin(e);
            else if (ProcessName == Common.msedge)
                Msedge(e);
            else if (ProcessName == Common.RSG)
                _Rsg(e);
            else if (ProcessName == Common.vlc)
                wheelleftright(e);
            else if (ProcessName == Common.ApplicationFrameHost && ProcessTitle.Contains("png") || GetPointName() == PhotoApps)
                wheelleftright(e);
            else if (ProcessName.Contains(KingdomRush))
                _KingdomRush(e);
            else if (ProcessName == Common.BandiView)
                HandleBandiView(e);
            else if (ProcessName == Common.explorer)
                Handleexplorer(e);

            if (e.Msg != MouseMsg.move)
                if (IsDesktopFocused())
                    change_image(e);
        }

        private void Handleexplorer(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;
            if (e.Msg == MouseMsg.click_up)
            {
                if (LongPressClass.long_press_lbutton && e.Y == screenHeight1)
                    press(Delete);
                LongPressClass.long_press_lbutton = false;
            }
        }

        private void HandleBandiView(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;
            if (GetPointName() != Honeyview) return;

            if (e.Msg == MouseMsg.click_up)
                press([LControlKey, D0]);
            if (e.Msg == MouseMsg.click_r)
                press(Delete);
            if (e.Msg == MouseMsg.wheel)
            {
                if (e.data > 0 && Common.ProcessTitle.IndexOf("id") == 0) press(Delete);
                else mousewhell(e.data);
            }
            //press(10, 400, Escape, Delete, 200, Return);
        }

        private static void change_image(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.wheel)
            {
                string nextWallpaper = "";
                if (e.data < 0) nextWallpaper = GetNextWallpaper();
                if (e.data > 0) nextWallpaper = GetPreviousWallpaper();
                SetDesktopWallpaper(nextWallpaper, WallpaperStyle.Fit);
            }
            if (e.Msg == MouseMsg.click_r)
                if (e.Y == 0 && IsDesktopFocused())
                {
                    DeleteCurrentWallpaper();
                    Sleep(100);
                    press(Escape);
                }
        }

        private void _KingdomRush(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.click_r_up)
            {
                press(Keys.D2);
            }
        }

        private void _Rsg(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.middle)
            {
                mouse_middle();
            }
        }

        private void Douyin(MouseHookEventArgs e)
        {
            if (is_douyin())
            {
                if (e.Msg == MouseMsg.back)
                {
                    SS().KeyPress(Keys.X);
                }
                else if (e.Msg == MouseMsg.go)
                {
                    SS().KeyPress(Keys.H);
                }
                else if (e.Msg == MouseMsg.click_up)
                {
                    Common.isVir = 0;
                    if (e.Y == screenHeight1 && e.X < screenWidth2)
                        SS().KeyPress(Keys.PageUp);
                    else if (e.Y == screenHeight1 && e.X < screenWidth1)
                        SS().KeyPress(Keys.PageDown);
                    Common.isVir = 3;
                }
                return;
            }
        }

        private void Msedge(MouseHookEventArgs e)
        {
        }

        private void Cs2(MouseHookEventArgs e)
        {
        }
        private void Devenv(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.click_up)
                if (is_down_vir(Keys.RButton))
                    press(Keys.F12);
            if (e.Msg == MouseMsg.click_r)
            {
                catch_on(MouseMsg.click);
            }
            if (e.Msg == MouseMsg.click_r_up)
            {
                if (!is_down(Keys.LButton))
                {
                    if (e.Y == 0)
                    {
                        if (Deven_runing()) press("583,74");
                        else press("116,69");
                        //else press("898,71");
                    }
                    else if (!catch_ed)
                        mouse_click_right();
                }
                catch_off();
            }
        }
        private void PowerToysCropAndLock(MouseHookEventArgs e)
        {
            //if (!is_no_title(PowerToysCropAndLock))
            //    break;
            if (is_down(Keys.LWin)) return;
            if (e.Msg == MouseMsg.click)
                is_PowerToysCropAndLock_down = true;
            else if (e.Msg == MouseMsg.click_up)
            {
                if (e.X == 0)
                {
                    hideProcessTitle(Common.PowerToysCropAndLock);
                    MoveProcessWindow2(Common.PowerToysCropAndLock);
                    CenterWindowOnScreen(chrome, true);
                    return;
                }
                if (e.Y == screenHeight1)
                {
                    CloseProcess(Common.PowerToysCropAndLock);
                    return;
                }
                //if (is_double_click()) quick_max_chrome(e.Pos);
                is_PowerToysCropAndLock_down = false;
            }
            else if (e.Msg == MouseMsg.click_r_up)
            {
                quick_max_chrome(e.Pos);
                if (e.X == 0)
                {
                    SS().KeyPress(Keys.F);
                    SS().MouseWhell(-120 * 9);
                }
            }
            else if (e.Msg == MouseMsg.move && is_PowerToysCropAndLock_down)
            {
                IntPtr targetWindowHandle = GetProcessID(ProcessName);
                var lastCursor = e.Pos;
                Native.ScreenToClient(targetWindowHandle, ref lastCursor);
                IntPtr lParam = ((lastCursor.Y << 16) | lastCursor.X);
                Native.SendMessage(targetWindowHandle, Native.WM_NCLBUTTONDOWN, Native.HTCAPTION, lParam);
            }
            else if (e.Msg == MouseMsg.wheel)
            {
                //HandleMouseWheel(e);
            }
        }
        private void Chrome(MouseHookEventArgs e)
        {
            //if (e.Msg == MouseMsg.WM_LBUTTONUP && is_double_click() && ExsitProcess(PowerToysCropAndLock, true))
            //    quick_max_chrome(e.Pos);
            //else if (e.Msg == MouseMsg.WM_LBUTTONUP && (e.X == screenWidth1 && e.Y < screenHeight2))
            //    CenterWindowOnScreen(chrome);
            //else if (e.Msg == MouseMsg.WM_LBUTTONUP && (e.X == screenWidth1 && e.Y >= screenHeight2))
            //    CenterWindowOnScreen(chrome, true); 
            //if (e.Msg == MouseMsg.click_up && (e.X == screenWidth1))
            //    CenterWindowOnScreen(chrome, e.Y >= screenHeight2);
            //else if (e.Msg == MouseMsg.click_r_up && ExistProcess(Common.PowerToysCropAndLock, true))
            //{
            //    if (e.X == 0) { press(Keys.OemPeriod); return; }
            //    quick_max_chrome(e.Pos);
            //}
            //else if (e.Msg == MouseMsg.back_up)
            //{
            //    if (judge_color(26, 94, Color.FromArgb(120, 123, 117)))
            //    {
            //        press([Keys.LControlKey, Keys.W]);
            //    }
            //}
            if (e.Msg == MouseMsg.wheel && e.X == 0)
            {
                wheelleftright(e);
            }

            //if (e.Msg == MouseMsg.wheel && (ProcessTitle.Contains("荔枝") && !ProcessTitle.Contains("分类")))
            //{
            //    //int aa = screenWidth / 20;
            //    //int a = (e.X - screenWidth2) / aa;
            //    //if (a < 1) a = 1;
            //    if (e.X < 1300)
            //    { }
            //    else// if (e.X < 1800)
            //        mousewhell(e.data * 3);
            //    //else if (e.X < 2200)
            //    //    mousewhell(e.data * 1300 / 120);
            //    //else if (e.X < screenWidth)
            //    //    mousewhell(e.data * 12);
            //}
        }

        public static void wheelleftright(MouseHookEventArgs e)
        {
            if (e.Msg != MouseMsg.wheel) return;
            Keys keys = Keys.Right;
            if (e.data > 0) keys = Keys.Left;
            press(keys);
        }
    }
}
