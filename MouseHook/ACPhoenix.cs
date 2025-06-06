using System.Timers;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    partial class biu
    {
        bool listen_move = false;
        public void ACPhoenix(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (ProcessName != keyupMusic2.Common.ACPhoenix) return;
            MoveStop(e);

            if (e.Msg == MouseMsg.click)
            {
                listen_move = true;
                if (e.Y == 0 || e.Y == screenHeight - 1) { press(Keys.Space); }
                else if (e.X == 0) { press(Keys.Tab); }
            }
            else if (e.Msg == MouseMsg.click_up)
            {
                listen_move = false;
            }
            else if (e.Msg == MouseMsg.click_r)
            {
                //downing2 = true;
                if (!(right_top_exit(e)))
                    Task.Run(() =>
                    {
                        mouse_click2(50);
                        mouse_click2(50);
                        Thread.Sleep(50);
                        for (var i = 0; i < 50; i++)
                        {
                            //if (!downing2) break;
                            mouse_click2(50);
                        }
                    });
            }
            else if (e.Msg == MouseMsg.click_r_up)
            {
                //downing2 = false;
                if ((e.Y < (493 * screenHeight / 1440) && e.Y > (190 * screenHeight / 1440)) && e.X < (2066 * screenWidth / 2560))
                    press(Keys.Space);
                if (try_press(1433, 1072, Color.FromArgb(245, 194, 55), () => { }))
                { }
                //退出观战
                if ((right_top_exit(e)))
                {
                    //press("111"); press(Keys.F4); press("1625.1078");
                    if (is_alt()) return;
                    press("2478,51;2492,1299;1625.1078", 200);
                }
            }
            if (!listen_move) { return; }

            if (e.Msg == MouseMsg.move)
            {
                //var aaa = 1430 * 2160 / 1440;
                if (!listen_move || (e.Y < screenHeight - 10) || (e.X > screenWidth)) { return; }
                if (ProcessName2 == keyupMusic2.Common.ACPhoenix) { press(Keys.S, 0); ; }
                listen_move = false;
            }
        }

        private bool right_top_exit(MouseKeyboardHook.MouseHookEventArgs e)
        {
            return e.Y == 0 && (e.X <= screenWidth - 1 && e.X > screenWidth - 120);
        }
    }
}
