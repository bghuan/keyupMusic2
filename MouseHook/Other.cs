using System.Diagnostics;
using System.Security.Policy;
using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;

namespace keyupMusic2
{
    partial class biu
    {

        Dictionary<MouseMsg, DateTime> MouseMsgTime = new Dictionary<MouseMsg, DateTime>();
        static bool is_PowerToysCropAndLock_down = false;
        public void Other(KeyboardMouseHook.MouseEventArgs e)
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
            //else if (is_douyin())
            //    Douyin(e);
            else if (ProcessName == Common.msedge)
                Msedge(e);
            else if (ProcessName == Common.RSG)
                _Rsg(e);
            else if (ProcessName == Common.b1)
                _Rsg(e);
            //else if (ProcessName == Common.vlc)
            //    wheelleftright(e);
            //else if (ProcessName == Common.ApplicationFrameHost && ProcessTitle.Contains("png") || GetPointName() == PhotoApps)
            //    wheelleftright(e);
            else if (ProcessName.Contains(KingdomRush))
                _KingdomRush(e);
            else if (ProcessName == Common.BandiView)
                HandleBandiView(e);
            else if (ProcessName == Common.explorer)
                Handleexplorer(e);
            else if (ProcessName == Common._哔哩哔哩)
                Handle_哔哩哔哩(e);

            if (e.Msg != MouseMsg.move)
                if (IsDesktopFocused())
                    change_image(e);
        }

        private void Handle_哔哩哔哩(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;
            if (e.Msg == MouseMsg.click_r_up)
            {
                if (!is_down(LButton))
                    press([LControlKey, C]);
                else
                    press([Space, LControlKey, V]);
            }
        }

        private void Handleexplorer(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;
            if (e.Msg == MouseMsg.click_up)
            {
                if (LongPressClass.long_press_lbutton && (e.Y == screenHeight1 || e.Y == 0))
                    press(Delete);
            }
        }

        private void HandleBandiView(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;
            if (GetPointName() != Honeyview) return;

            //if (e.Msg == MouseMsg.click_up && !isctrl())
            //    press([LControlKey, D0]);
            if (e.Msg == MouseMsg.click_r)
            {
                press(Delete);
                var judge = () => { return GetPointTitle() == ""; };
                var run = () => { press(Escape); };
                DelayRun(judge, run, 1000, 1);
            }
            //if (e.Msg == MouseMsg.wheel)
            //{
            //    if (e.data > 0 && Common.ProcessTitle.IndexOf("id") == 0) press(Delete);
            //    else mousewhell(e.data);
            //}
            //press(10, 400, Escape, Delete, 200, Return);
        }

        private static void change_image(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.wheel)
            {
                string nextWallpaper = "";
                if (e.data < 0 && (e.X == 0 || e.X == screen2Width1)) nextWallpaper = GetNextIdFolder();
                else if (e.data > 0 && e.X == 0) nextWallpaper = GetPreviousIdFolder();
                else if (e.data < 0) nextWallpaper = GetNextWallpaper();
                else if (e.data > 0) nextWallpaper = GetPreviousWallpaper();
                SetDesktopWallpaper(nextWallpaper, WallpaperStyle.Fit);
            }
            if (e.Msg == MouseMsg.click_r_up)
            {
                if (e.Y == 0)
                {
                    DeleteCurrentWallpaper();
                    Sleep(20);
                    press(Escape);
                }
                else if (e.X == 0 || e.X == screen2Width1)
                {
                    Sleep(20);
                    press(Escape);
                    SetDesktopWallpaperAli(GetCurrentWallpaperPath());
                    //press(Escape);
                }
            }
        }

        private void _KingdomRush(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.click_r_up)
            {
                press(Keys.D2);
            }
        }

        private void _Rsg(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.middle)
            {
                mouse_middle();
            }
        }



        private void Msedge(KeyboardMouseHook.MouseEventArgs e)
        {
        }

        private void Cs2(KeyboardMouseHook.MouseEventArgs e)
        {
        }
        private void Devenv(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.click_up)
                if (is_down_vir(Keys.RButton))
                {
                    press(Keys.F12);
                    {
                        var judge = () => { return GetPointTitle() == ""; };
                        var run = () => { press(Escape); };
                        DelayRun(judge, run, 1000, 1);
                    }
                }
            //if (e.Msg == MouseMsg.click_r)
            //{
            //    catch_on(MouseMsg.click);
            //}
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
                    //else if (!catch_ed)
                    //    mouse_click_right();
                }
                //catch_off();
            }
        }
        private void PowerToysCropAndLock(KeyboardMouseHook.MouseEventArgs e)
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
        private void Chrome(KeyboardMouseHook.MouseEventArgs e)
        {
            //if (GetPointName() != chrome) return;
            if (e.Msg == MouseMsg.click_up && (e.X == screenWidth1))
                CenterWindowOnScreen(chrome, e.Y >= screenHeight2);
            else if (e.Msg == MouseMsg.click_r_up && !LongPressClass.long_press_rbutton && ExistProcess(Common.PowerToysCropAndLock, true) && e.Pos == click_r_point)
            {
                //if (e.X == 0) { press(Keys.OemPeriod); return; }
                if (judge_color(1840, 51, Color.FromArgb(162, 37, 45)))
                    press(Keys.F, 51);
                quick_max_chrome(e.Pos);
            }
            else if (e.Msg == MouseMsg.back_up && (judge_color(26, 94, Color.FromArgb(120, 123, 117))))
                press([Keys.LControlKey, Keys.W]);
            else if (e.Msg == MouseMsg.click_r_up && e.Y < 200 && (ProcessTitle.Contains("荔枝") && ProcessTitle.Contains("详情")))
            {
                var pos = Position;
                mouse_move_to(80, 80, 10);
                mouse_click(500);
                press(Enter);
                mouse_move(pos, 10);
            }
        }

        public static void wheelleftright(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.wheel) press(e.data > 0 ? Keys.Left : Keys.Right);
        }
    }
}
