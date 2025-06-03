using System.Diagnostics;
using System.Security.Policy;
using System.Windows.Forms;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    partial class biu
    {

        Dictionary<MouseMsg, DateTime> MouseMsgTime = new Dictionary<MouseMsg, DateTime>();
        static bool is_PowerToysCropAndLock_down = false;
        public void Other()
        {
            //bool double_click = e.Msg == MouseMsg.WM_LBUTTONUP && is_double_click();

            if (ProcessName == Common.PowerToysCropAndLock)
                PowerToysCropAndLock();
            else if (ProcessName == Common.chrome)
                Chrome();
            else if (ProcessName == Common.cs2)
                Cs2();
            else if (ProcessName == Common.devenv)
                Devenv();
            else if (is_douyin())
                Douyin();
            else if (ProcessName == Common.msedge)
                Msedge();
        }

        private void Douyin()
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

        private void Msedge()
        {
        }

        private void Cs2()
        {
        }
        private void Devenv()
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
                        if (Deven_runing()) press("116,69");
                        else press("898,71");
                    }
                    else if (!catch_ed)
                        mouse_click_right();
                }
                catch_off();
            }
        }
        private void PowerToysCropAndLock()
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
        private void Chrome()
        {
            //if (e.Msg == MouseMsg.WM_LBUTTONUP && is_double_click() && ExsitProcess(PowerToysCropAndLock, true))
            //    quick_max_chrome(e.Pos);
            //else if (e.Msg == MouseMsg.WM_LBUTTONUP && (e.X == screenWidth1 && e.Y < screenHeight2))
            //    CenterWindowOnScreen(chrome);
            //else if (e.Msg == MouseMsg.WM_LBUTTONUP && (e.X == screenWidth1 && e.Y >= screenHeight2))
            //    CenterWindowOnScreen(chrome, true); 
            if (e.Msg == MouseMsg.click_up && (e.X == screenWidth1))
                CenterWindowOnScreen(chrome, e.Y >= screenHeight2);
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
            else if (e.Msg == MouseMsg.wheel && e.X == screenWidth1)
            {
                Keys keys = Keys.Right;
                if (e.data > 0) keys = Keys.Left;
                press(keys);
            }
        }
    }
}
