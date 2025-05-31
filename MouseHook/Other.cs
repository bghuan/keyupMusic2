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
                    if (e.Msg == MouseMsg.click)
                        is_PowerToysCropAndLock_down = true;
                    else if (e.Msg == MouseMsg.click_up)
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
                    break;
                case chrome:
                    //if (e.Msg == MouseMsg.WM_LBUTTONUP && is_double_click() && ExsitProcess(PowerToysCropAndLock, true))
                    //    quick_max_chrome(e.Pos);
                    //else if (e.Msg == MouseMsg.WM_LBUTTONUP && (e.X == screenWidth1 && e.Y < screenHeight2))
                    //    CenterWindowOnScreen(chrome);
                    //else if (e.Msg == MouseMsg.WM_LBUTTONUP && (e.X == screenWidth1 && e.Y >= screenHeight2))
                    //    CenterWindowOnScreen(chrome, true); 
                    if (e.Msg == MouseMsg.click_up && (e.X == screenWidth1))
                        CenterWindowOnScreen(chrome, e.Y >= screenHeight2);
                    else if (e.Msg == MouseMsg.click_r_up && ExistProcess(PowerToysCropAndLock, true))
                    {
                        if (e.X == 0) { press(Keys.OemPeriod); break; }
                        quick_max_chrome(e.Pos);
                    }
                    else if (e.Msg == MouseMsg.back_up)
                    {
                        if (judge_color(26, 94, Color.FromArgb(120, 123, 117)))
                        {
                            press([Keys.LControlKey, Keys.W]);
                        }
                    }
                    break;
                case cs2:
                    if (e.Msg == MouseMsg.click)
                        huan.Invoke2(() => { huan.label1.Text = "click_down"; });
                    else if (e.Msg == MouseMsg.click_up)
                        huan.Invoke2(() => { huan.label1.Text = "click_up"; });
                    break;
                case Common.devenv:
                    if (e.Msg == MouseMsg.click_r)
                    {
                        if ((e.Y != 0)) break;
                        if (Deven_runing())
                            //press_task([Keys.RControlKey, Keys.RShiftKey, Keys.F5], 200);
                            press("116,69");
                        //Task.Run(() => Sim.KeyPress([Keys.RControlKey, Keys.RShiftKey, Keys.F5]));
                        //press("115, 69",101);
                        else
                            press("898,71");
                            //press([Keys.F5]);
                    }
                    break;
                case Common.msedge:
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
                        break;
                    }
                    if (e.Msg == MouseMsg.back_up)
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

    }
}
