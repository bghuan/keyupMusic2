using static keyupMusic2.Common;

namespace keyupMusic2
{
    partial class biu
    {

        int expect_cornor_edge = 150;
        public void ScreenLine()
        {
            if (is_ctrl_shift_alt()){ handing3 = false; return; }
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
                screen2Height1,
            };
        public void _ScreenLine()
        {
            //if (e.Msg == MouseMsg.WM_MOUSEMOVE && left_side_click && left_down_click && left_up_click && right_up_click && right_down_click && right_side_click && (!special.Contains(e.X)&& special.Contains(e.Y)))
            //{
            //    return;
            //}
            if (is_ctrl()) return;
            if (e.Msg == MouseMsg.click || cornor != 0)
            {
                left_side_click = false;
                left_down_click = false;
                left_up_click = false;

                right_up_click = false;
                right_down_click = false;
                right_side_click = false;
            }
            else if (e.Msg == MouseMsg.move)
            {
                //if (e.X > (screenWidth) / 4 && left_side_click == false)
                if (e.X < 0 && left_side_click == false)
                    left_side_click = true;
                else if (e.Y < (screenHeight) / 4 * 3 && left_down_click == false)
                    left_down_click = true;
                else if (e.Y > (screenHeight) / 4 && left_up_click == false)
                    left_up_click = true;

                else if (e.Y > (screen2Height1) / 4 && e.Y < (screenHeight) / 4 * 3 && right_up_click == false)
                    right_up_click = true;
                else if (e.Y < (screen2Height1) / 4 * 3 && right_down_click == false)
                    right_down_click = true;
                else if (e.X > 0 && right_side_click == false)
                    right_side_click = true;

                if (Math.Abs(e.X - (1333 * screenWidth / 2560)) < 2 && e.Y == screenWidth1)
                    left_down_click = false;

                var not_allow = (ProcessName != keyupMusic2.Common.ACPhoenix) && (ProcessName != msedge);
                var catched = true;

                //if (left_side_click && e.X == 0 && (e.Y < expect_cornor_edge || e.Y > screenHeight - expect_cornor_edge))
                if (left_side_click && e.X == screenWidth1 && e.Y > expect_cornor_edge && e.Y < screen2Height1 - expect_cornor_edge)
                {
                    //if (ProcessName.Equals(chrome)) return;

                    var aaa = ProcessName == Common.chrome && !(!judge_color(-1783, 51, Color.FromArgb(162, 37, 45)) || !judge_color(-645, 45, Color.FromArgb(162, 37, 45)));
                    if (is_douyin()) return;
                    if (ProcessTitle.Contains(bilibili)) return;
                    if (ProcessName.Equals(err)) return;
                    if (ProcessTitle.Contains(Ghostrunner2)) return;
                    if (ProcessTitle.Contains(ItTakesTwo)) return;
                    if (aaa && ProcessName == Common.chrome) SS().KeyPress(Keys.F);
                    right_up_f = false;
                    left_side_click = false;
                    mouse_click2(0);
                    //if (ProcessName2 == msedge && is_douyin()) mouse_click2(0);
                }
                else if ((left_up_click && e.Y == 0 && e.X < screenWidth) && e.X > 0)
                {
                    catched = false;
                    if (ProcessName.Equals(err)) return;
                    if (ProcessName.Equals(chrome) && e.X < screenWidth - expect_cornor_edge && e.X > expect_cornor_edge)
                        if (ProcessPosition(chrome).X >= 0)
                            SS().KeyPress(Keys.F);
                    left_up_click = false;
                }
                else if ((left_down_click && e.Y + 1 == screenHeight && e.X > 0 && e.X < screenWidth) && (e.X < expect_cornor_edge || (e.X > screenWidth - 500)))
                {
                    left_down_click = false;
                }
                else if (left_down_click && e.Y + 1 == screenHeight && e.X > 0 && e.X < screenWidth)
                {
                    var aaa = ProcessName == Common.chrome && !(!judge_color(-1783, 51, Color.FromArgb(162, 37, 45)) || !judge_color(-645, 45, Color.FromArgb(162, 37, 45)));
                    right_up_f = aaa;
                    left_down_click = false;
                    if (ProcessName.Equals(err)) return;
                    if (!not_allow && IsFullScreen()) return;
                    if (is_douyin() && IsFullScreen()) return;
                    if (judge_color(Color.FromArgb(210, 27, 70))) { return; }
                    if (right_up_f && ProcessName == Common.chrome) SS().KeyPress(Keys.F);
                    mouse_click2(0);
                    if (ProcessName2 == Common.chrome && ProcessPosition(chrome).X >= 0)
                    {
                        //if (e.X > screenWidth2) return;
                        if (!judge_color(1840, 51, Color.FromArgb(162, 37, 45)))
                            SS().KeyPress(Keys.F);
                        SS().MouseWhell(-120 * 12);
                        return;
                    }
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
                else if (right_down_click && e.Y == screen2Height1 && e.X < 0 && e.X > screen2Width + expect_cornor_edge)
                {
                    play_sound_di();
                    catched = false;
                    right_down_click = false;
                    if (ProcessName != Common.chrome)
                    {
                        mouse_click2(0);
                    }
                    //mouse_click2(0);
                    var aaa = ProcessName == Common.chrome && !(judge_color(-1783, 51, Color.FromArgb(162, 37, 45)) && judge_color(-645, 45, Color.FromArgb(162, 37, 45)));
                    if (aaa)
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
