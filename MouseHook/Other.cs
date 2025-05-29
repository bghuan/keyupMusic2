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

            switch (ProcessName2)
            {
                case PowerToysCropAndLock:
                    //if (!is_no_title(PowerToysCropAndLock))
                    //    break;
                    if (is_down(Keys.LWin)) break;
                    if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
                        is_PowerToysCropAndLock_down = true;
                    else if (e.Msg == MouseMsg.WM_LBUTTONUP)
                    {
                        if (e.X == 0)
                        {
                            hideProcessTitle(PowerToysCropAndLock);
                            MoveProcessWindow2(PowerToysCropAndLock);
                            CenterWindowOnScreen(chrome, true);
                            break;
                        }
                        if (e.Y == screenHeight1)
                        {
                            CloseProcess(PowerToysCropAndLock);
                            break;
                        }
                        //if (is_double_click()) quick_max_chrome(e.Pos);
                        is_PowerToysCropAndLock_down = false;
                    }
                    else if (e.Msg == MouseMsg.WM_RBUTTONUP)
                    {
                        quick_max_chrome(e.Pos);
                        if (e.X == 0)
                        {
                            SS().KeyPress(Keys.F);
                            SS().MouseWhell(-120 * 9);
                        }
                    }
                    else if (e.Msg == MouseMsg.WM_MOUSEMOVE && is_PowerToysCropAndLock_down)
                    {
                        IntPtr targetWindowHandle = GetProcessID(ProcessName);
                        var lastCursor = e.Pos;
                        Native.ScreenToClient(targetWindowHandle, ref lastCursor);
                        IntPtr lParam = ((lastCursor.Y << 16) | lastCursor.X);
                        Native.SendMessage(targetWindowHandle, Native.WM_NCLBUTTONDOWN, Native.HTCAPTION, lParam);
                    }
                    else if (e.Msg == MouseMsg.WM_MOUSEWHEEL)
                    {
                        //HandleMouseWheel(e);
                    }
                    break;
                case chrome:
                    //if (e.Msg == MouseMsg.WM_LBUTTONUP && is_double_click() && ExsitProcess(PowerToysCropAndLock, true))
                    //    quick_max_chrome(e.Pos);
                    //else if (e.Msg == MouseMsg.WM_LBUTTONUP && (e.X == screenWidth1 && e.Y < screenHeight2))
                    //    CenterWindowOnScreen(chrome);
                    //else if (e.Msg == MouseMsg.WM_LBUTTONUP && (e.X == screenWidth1 && e.Y >= screenHeight2))
                    //    CenterWindowOnScreen(chrome, true); 
                    if (e.Msg == MouseMsg.WM_LBUTTONUP && (e.X == screenWidth1))
                        CenterWindowOnScreen(chrome, e.Y >= screenHeight2);
                    else if (e.Msg == MouseMsg.WM_RBUTTONUP && ExistProcess(PowerToysCropAndLock, true))
                    {
                        if (e.X == 0) { press(Keys.OemPeriod); break; }
                        quick_max_chrome(e.Pos);
                    }
                    else if (e.Msg == MouseMsg.WM_XBUTTONUP)
                    {
                        if (judge_color(26, 94, Color.FromArgb(120, 123, 117)))
                        {
                            press([Keys.LControlKey, Keys.W]);
                        }
                    }
                    break;
                case cs2:
                    if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
                        huan.Invoke2(() => { huan.label1.Text = "click_down"; });
                    else if (e.Msg == MouseMsg.WM_LBUTTONUP)
                        huan.Invoke2(() => { huan.label1.Text = "click_up"; });
                    break;
                case msedge:
                    if (e.Msg == MouseMsg.WM_XBUTTONUP)
                    {
                        if (judge_color(33, 80, Color.FromArgb(204, 204, 204), 0))
                        //if (judge_color(92, 73, Color.FromArgb(0, 0, 0), null, 0))
                        {
                            press([Keys.LControlKey, Keys.W]);
                        }
                    }
                    break;
            }
        }

        private bool is_double_click()
        {
            var result = MouseMsgTime != null && MouseMsgTime.ContainsKey(e.Msg) && MouseMsgTime[e.Msg] != DateTime.MinValue && MouseMsgTime[e.Msg].AddMilliseconds(200) > DateTime.Now;

            result = result && !IsClickOnTitleBar(ProcessName, e.Pos);
            if (MouseMsgTime != null) MouseMsgTime[e.Msg] = DateTime.Now;
            if (result) MouseMsgTime[e.Msg] = DateTime.MinValue;
            return result;
        }
    }
}
