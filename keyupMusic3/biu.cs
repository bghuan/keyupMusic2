using keyupMusic2;
using WGestures.Core.Impl.Windows;
using static keyupMusic2.Common;

namespace keyupMusic3
{
    public class biu
    {
        public biu(Form parentForm)
        {
            huan = (Form2)parentForm;
        }
        public Form2 huan;
        bool listen_move = false;
        bool downing = false;
        bool downing2 = false;
        bool handing = false;
        bool handing2 = false;
        bool handing3 = false;
        bool left_left_click = false;
        bool left_down_click = false;
        bool right_up_click = false;
        bool right_down_click = false;
        private static readonly object _lockObject_handing2 = new object();
        MouseKeyboardHook.MouseHookEventArgs e = null;
        private Point start = Point.Empty;
        private int threshold = 10;
        bool r_button_downing = false;
        bool x_button_dowing = false;


        public void MouseHookProc(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (hooked_mouse) return;
            if (handing3) return;
            if (handing) return;
            hooked_mouse = true;
            handing = true;
            handing3 = true;
            this.e = e;
            if (e.Msg != MouseMsg.WM_MOUSEMOVE) Task.Run(() => { FreshProcessName(); });

            Task.Run(() => { ACPhoenix(e); });
            //Douyin(e);
            Douyin(e, Common.msedge);
            Task.Run(Devenv);
            Task.Run(Cornor);
            Task.Run(() => { ScreenEdgeClick(); handing3 = false; });
            Task.Run(UnderLine);
            Task.Run(QQMusic);
            Task.Run(Other);

            handing = false;
            hooked_mouse = false;
        }
        public void Douyin(MouseKeyboardHook.MouseHookEventArgs e, string allow = Common.douyin)
        {
            if (ProcessName != allow && !is_douyin()) return;
            if (e.Msg == MouseMsg.WM_MOUSEMOVE && !downing) { return; }

            if (e.Msg == MouseMsg.WM_RBUTTONDOWN)
            {
                if (e.Y > 1370) return;
                if (ProcessName == msedge && ProcessTitle.Contains("抖音"))
                {
                    e.Handled = true;
                    r_button_downing = true;
                }
                if (e.X != 0) return;
                if (ProcessName2 != allow) return;
                e.Handled = true;
                start = Position;
                downing = true;
            }
            else if (e.Msg == MouseMsg.WM_RBUTTONUP)
            {
                if (e.Y > 1370) return;
                if (r_button_downing && ProcessName == msedge && ProcessTitle.Contains("抖音"))
                {
                    e.Handled = true;
                    r_button_downing = false;
                }
                if (ProcessName2 != allow) return;
                if (!downing) return;
                e.Handled = true;
                downing = false;
            }
            else if (e.Msg == MouseMsg.WM_MOUSEMOVE && downing == true)
            {
                int y = e.Y - start.Y;
                if (Math.Abs(y) > threshold)
                {
                    if (y > 0)
                        press(Keys.VolumeDown);
                    if (y < 0)
                        press(Keys.VolumeUp);
                    start = Position;
                }
            }
            else if (e.Msg == MouseMsg.WM_XBUTTONDOWN && is_douyin())
            {
                string dasad = Common.ProcessTitle;
                string dasasssd = Common.ProcessName;

                e.Handled = true;
                x_button_dowing = true;
                Task.Run(() =>
                {
                    //if (e.Y == 0 && e.X == 0)
                    //    press("2222,1410;100;2222,1120", 1);
                    //else if ((e.Y == 0 || e.Y + 1 == screenHeight) && e.X < screenWidth)
                    Simulate.Sim.KeyPress(Keys.X);
                    //else
                    //    KeyboardInput.PressKey(Keys.H);
                    mouse_move(2221, 1407);
                });
            }
            else if (x_button_dowing && e.Msg == MouseMsg.WM_XBUTTONUP && is_douyin())
            {
                e.Handled = true;
                x_button_dowing = false;
            }
        }

        public void ACPhoenix(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (ProcessName != keyupMusic2.Common.ACPhoenix) return;

            if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            {
                listen_move = true;
                if (e.Y == 0 || e.Y == screenHeight - 1) { press(Keys.Space); }
                if (e.X == 0) { press(Keys.Tab); }
            }
            else if (e.Msg == MouseMsg.WM_LBUTTONUP)
            {
                listen_move = false;
            }
            else if (e.Msg == MouseMsg.WM_RBUTTONDOWN)
            {
                downing2 = true;
                if (!(e.Y == 0 && (e.X <= screenWidth - 1 && e.X > screenWidth - 120)))
                    Task.Run(() =>
                    {
                        mouse_click2(50);
                        mouse_click2(50);
                        Thread.Sleep(50);
                        for (var i = 0; i < 50; i++)
                        {
                            if (!downing2) break;
                            mouse_click2(50);
                        }
                    });
            }
            else if (e.Msg == MouseMsg.WM_RBUTTONUP)
            {
                downing2 = false;
                if ((e.Y < (493 * screenHeight / 1440) && e.Y > (190 * screenHeight / 1440)) && e.X < (2066 * screenWidth / 2560))
                    press(Keys.Space);
                if (try_press(1433, 1072, Color.FromArgb(245, 194, 55), () => { }))
                { }
                //退出观战
                if ((e.Y == 0 && (e.X <= screenWidth - 1 && e.X > screenWidth - 120)))
                {
                    //press("111"); press(Keys.F4); press("1625.1078");
                    if (is_alt()) return;
                    press("2478,51;2492,1299;1625.1078", 200);
                }
            }
            if (!listen_move) { return; }

            if (e.Msg == MouseMsg.WM_MOUSEMOVE)
            {
                //var aaa = 1430 * 2160 / 1440;
                if (!listen_move || (e.Y < screenHeight - 10) || (e.X > screenWidth)) { return; }
                if (ProcessName2 == keyupMusic2.Common.ACPhoenix) { press(Keys.S, 0); ; }
                listen_move = false;
            }
        }
        public void Devenv()
        {
            //if (ProcessName != keyupMusic2.Common.devenv) return;

            //if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            //{
            //    if ((e.Y == 0) && (e.X < (2560 / 2)))
            //    {
            //        if (judge_color(82, 68, Color.FromArgb(189, 64, 77)))
            //            press([Keys.LControlKey, Keys.LShiftKey, Keys.F5]);
            //        else
            //            press([Keys.F5]);
            //    }
            //    else if ((e.Y == 0) && (e.X < 2560))
            //    {
            //        press([Keys.LShiftKey, Keys.F5]);
            //    }
            //}
        }
        int expect_cornor_edge = 50;
        public void ScreenEdgeClick()
        {
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN || cornor != 0)
            {
                left_left_click = false;
                left_down_click = false;
                right_up_click = false;
                right_down_click = false;
            }
            else if (e.Msg == MouseMsg.WM_MOUSEMOVE)
            {
                if (e.X > (2560 * screenWidth / 2560) / 4 && left_left_click == false)
                    left_left_click = true;
                else if (e.Y < (1440 * screenHeight / 1440) / 4 * 3 && left_down_click == false)
                    left_down_click = true;
                else if (e.X < (2560 * screenWidth / 2560) / 1 && right_up_click == false)
                    right_up_click = true;
                if (Math.Abs(e.X - (1333 * screenWidth / 2560)) < 2 && e.Y == screenHeight - 1)
                    left_down_click = false;

                var not_allow = (ProcessName != keyupMusic2.Common.ACPhoenix) && (ProcessName != msedge);

                if (left_left_click && e.X == 0 && (e.Y < expect_cornor_edge || e.Y > screenHeight - expect_cornor_edge))
                {
                    left_left_click = false;
                }
                else if (left_left_click && e.X == 0)
                {
                    //if (!not_allow && IsFullScreen()) return;
                    if (is_douyin()) return;
                    left_left_click = false;
                    mouse_click2(400);
                }
                else if ((left_down_click && e.Y + 1 == screenHeight && e.X < screenWidth) && (e.X < expect_cornor_edge))
                {
                    left_down_click = false;
                }
                else if (left_down_click && e.Y + 1 == screenHeight && e.X < screenWidth)
                {
                    if (!not_allow && IsFullScreen()) return;
                    if (is_douyin() && IsFullScreen()) return;
                    if (judge_color(Color.FromArgb(210, 27, 70))) { return; }
                    left_down_click = false;
                    mouse_click2(0);
                }
                else if (right_up_click && e.Y == 0 && e.X > screenWidth)
                {
                    right_up_click = false;
                    mouse_click2(0);
                    //press(Keys.Escape, 111);
                    Simulate.Sim.KeyPress(Keys.F);
                    //press(Keys.PageDown, 111);
                    //if (!judge_color(5534, 696, Color.FromArgb(0, 0, 0)))
                    //new SendKeyboardMouse().MouseWhell(-120 * 7);
                }
            }
        }
        int cornor = 0;
        public void UnderLine()
        {
            if (e.Msg == MouseMsg.WM_RBUTTONUP)
            {
                if (e.Y + 1 == screenHeight && !IsFullScreen())
                {
                    Sleep(322);
                    mouse_move_to(0, 1325 - screenHeight);
                    mouse_click();
                }
            }
        }
        int ffff = 0;
        public void Cornor()
        {
            lock (_lockObject_handing2)
            {
                FreshProcessName();
                if (handing2) { handing2 = false; return; }
                handing2 = true;
                if (ffff != 10) ffff++;
                if (ffff < 10) { handing2 = false; return; }
                if (e.Msg != MouseMsg.WM_MOUSEMOVE) { handing2 = false; return; }
                cornor = 0;
                if (e.X == 0 && e.Y == 1439) cornor = 1;
                else if (e.X == 0 && e.Y == 0) cornor = 2;
                else if (e.X == 2559 && e.Y == 0) cornor = 3;
                else if (e.X == 2559 && e.Y == 1439) cornor = 4;
                else { handing2 = false; return; }

                //if (cornor == 3 && ProcessName == ApplicationFrameHost) mouse_click_not_repeat();
                //else if (cornor == 3 && ProcessName == explorer) mouse_click_not_repeat();
                //else if (cornor == 3 && ProcessName == vlc) mouse_click_not_repeat();
                //else if (cornor == 3 && ProcessName == v2rayN) mouse_click_not_repeat();
                //else if (cornor == 3 && ProcessName == Common.devenv && ProcessTitle.Contains("正在运行"))
                //    press([Keys.LShiftKey, Keys.F5]);
                //else if (cornor == 3 && ProcessName == Common.devenv) HideProcess(Common.devenv);

                if (cornor == 2)
                {
                    if (mouse_click_not_repeat_time.AddSeconds(1) > DateTime.Now) return;

                    var list = new[] { msedge, chrome };

                    if (is_douyin())
                        Simulate.Sim.KeyPress(Keys.H);
                    else if (list.Contains(Common.ProcessName))
                        press([Keys.F11]);

                    mouse_click_not_repeat_time = DateTime.Now;
                    ffff = 0;
                    Common.ProcessName = "";
                }
                else if (cornor == 3)
                {
                    var list = new[] { ApplicationFrameHost, explorer, vlc, v2rayN, Common.QQMusic };

                    if (list.Contains(Common.ProcessName))
                        mouse_click_not_repeat();
                    else if (ProcessName == Common.devenv && ProcessTitle.Contains("正在运行"))
                        press([Keys.LShiftKey, Keys.F5]);
                    else if (ProcessName == Common.devenv)
                        HideProcess(Common.devenv);

                    ffff = 0;
                    Common.ProcessName = "";
                }
                //if (cornor == 4)
                //{
                //    if (is_douyin()) press("2462,843");
                //}
                handing2 = false;
            }
        }

        private void QQMusic()
        {
            if (ProcessName != keyupMusic2.Common.QQMusic) return;
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            {
                ctrl_shift(true);
            }
        }
        public void Other()
        {
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            {
                if (ProcessName == keyupMusic2.Common.msedge && (e.Y == (screenHeight - 1)))
                    press(Keys.PageDown, 0);
            }
            //else if (e.Msg == MouseMsg.WM_LBUTTONUP)
            //{
            //    if (e.X == 6719 || e.Y == 1619)
            //    {
            //        HideProcess(keyupMusic2.Common.chrome); return;
            //    };
            //}
        }
    }
}
