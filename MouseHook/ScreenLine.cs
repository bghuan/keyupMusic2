using static keyupMusic2.Common;

namespace keyupMusic2
{
    partial class biu
    {

        int expect_cornor_edge = 100;
        public void ScreenLine()
        {
            _ScreenLine();
            handing3 = false;
        }

        int[] special = new int[] {
                0,
                screenWidth,
                screenHeight,
                screenWidth1,
                screenHeight1,
                screenWidth2,
                screenHeight2,
                screen2Width,
                screen2Height,
            };
        public void _ScreenLine()
        {
            //if (e.Msg == MouseMsg.WM_MOUSEMOVE && left_side_click && left_down_click && left_up_click && right_up_click && right_down_click && right_side_click && (!special.Contains(e.X)&& special.Contains(e.Y)))
            //{
            //    return;
            //}
            if (is_ctrl()) return;
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN || cornor != 0)
            {
                left_side_click = false;
                left_down_click = false;
                left_up_click = false;

                right_up_click = false;
                right_down_click = false;
                right_side_click = false;
            }
            else if (e.Msg == MouseMsg.WM_MOUSEMOVE)
            {
                //if (e.X > (screenWidth) / 4 && left_side_click == false)
                if (e.X < 0 && left_side_click == false)
                    left_side_click = true;
                else if (e.Y < (screenHeight) / 4 * 3 && left_down_click == false)
                    left_down_click = true;
                else if (e.Y > (screenHeight) / 4 && left_up_click == false)
                    left_up_click = true;

                else if (e.Y > (screen2Height) / 4 && e.Y < (screenHeight) / 4 * 3 && right_up_click == false)
                    right_up_click = true;
                else if (e.Y < (screen2Height) / 4 * 3 && right_down_click == false)
                    right_down_click = true;
                else if (e.X > (screen2Width) / 4 * 3 && right_side_click == false)
                    right_side_click = true;

                if (Math.Abs(e.X - (1333 * screenWidth / 2560)) < 2 && e.Y == screenWidth1)
                    left_down_click = false;

                var not_allow = (ProcessName != keyupMusic2.Common.ACPhoenix) && (ProcessName != msedge);
                var catched = true;

                //if (left_side_click && e.X == 0 && (e.Y < expect_cornor_edge || e.Y > screenHeight - expect_cornor_edge))
                if (left_side_click && e.X == screenWidth1 && e.Y > expect_cornor_edge && e.Y < screen2Height - expect_cornor_edge)
                {
                    //if (ProcessName.Equals(chrome)) return;

                    if (is_douyin()) return;
                    if (ProcessTitle.Contains(bilibili)) return;
                    if (ProcessName.Equals(err)) return;
                    if (ProcessTitle.Contains(Ghostrunner2)) return;
                    if (ProcessTitle.Contains(ItTakesTwo)) return;
                    if (right_up_f && ProcessName == Common.chrome) SS().KeyPress(Keys.F);
                    right_up_f = false;
                    left_side_click = false;
                    mouse_click2(0);
                }
                //else if ((left_up_click && e.Y == 0 && e.X < screenWidth) && e.X >0)
                //{
                //    if (ProcessName.Equals(err)) return;
                //    //if (ProcessName.Equals(chrome) && e.X < screenWidth / 4 * 3 && e.X > 200)
                //    //    SS().KeyPress(Keys.F);
                //    left_up_click = false;
                //}
                else if ((left_down_click && e.Y + 1 == screenHeight && e.X > 0 && e.X < screenWidth) && (e.X < expect_cornor_edge || (e.X > screenWidth - 500)))
                {
                    left_down_click = false;
                }
                else if (left_down_click && e.Y + 1 == screenHeight && e.X > 0 && e.X < screenWidth)
                {
                    right_up_f = false;
                    left_down_click = false;
                    if (ProcessName.Equals(err)) return;
                    if (!not_allow && IsFullScreen()) return;
                    if (is_douyin() && IsFullScreen()) return;
                    if (judge_color(Color.FromArgb(210, 27, 70))) { return; }
                    if (right_up_f && ProcessName == Common.chrome) SS().KeyPress(Keys.F);
                    //if (ProcessName == Common.chrome)
                    //{
                    //    if (e.X > screenWidth2) return;
                    //    if (!judge_color(1840, 51, Color.FromArgb(162, 37, 45)))
                    //        SS().KeyPress(Keys.F);
                    //    SS().MouseWhell(-120 * 12);
                    //    return;
                    //}
                    mouse_click2(0);
                }
                else if (right_up_click && e.Y == 0 && e.X < 0 && e.X > screen2Width + expect_cornor_edge)
                {
                    play_sound_di();
                    catched = false;
                    right_up_click = false;
                    right_up_f = true;
                    mouse_click2(0);
                    SS().KeyPress(Keys.F)
                        .MouseWhell(120 * 12);
                }
                else if (right_down_click && e.Y == screen2Height && e.X < 0)
                {
                    play_sound_di();
                    catched = false;
                    right_down_click = false;
                    if (ProcessName != Common.chrome)
                    {
                        mouse_click2(0);
                    }
                    //mouse_click2(0);
                    if (!judge_color(-1783, 51, Color.FromArgb(162, 37, 45)))
                        SS().KeyPress(Keys.F);
                    SS().MouseWhell(-120 * 12);
                }
                else if (right_side_click && e.X == screen2Width && e.Y > expect_cornor_edge)
                {
                    right_side_click = false;
                    mouse_click2(0);
                }
                else
                {
                    catched = false;
                }

                if (catched) { play_sound_di(); }
            }
        }
    }
}
