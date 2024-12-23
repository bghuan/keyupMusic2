using static keyupMusic2.Common;

namespace keyupMusic2
{
    partial class biu
    {

        int expect_cornor_edge = 50;
        public void ScreenLine()
        {
            _ScreenLine();
            handing3 = false;
        }

        public void _ScreenLine()
        {
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN || cornor != 0)
            {
                left_left_click = false;
                left_down_click = false;
                left_up_click = false;
                right_up_click = false;
                right_down_click = false;
            }
            else if (e.Msg == MouseMsg.WM_MOUSEMOVE)
            {
                if (e.X > (screenWidth) / 4 && left_left_click == false)
                    left_left_click = true;
                else if (e.Y < (screenHeight) / 4 * 3 && left_down_click == false)
                    left_down_click = true;
                else if (e.Y > (screenHeight) / 4 && left_up_click == false)
                    left_up_click = true;
                else if (e.X < (screenWidth) / 1 && right_up_click == false)
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
                    if (is_douyin()) return;
                    if (ProcessTitle.Contains(bilibili)) return;
                    if (ProcessTitle.Contains(Ghostrunner2)) return;
                    if (right_up_f && ProcessName == Common.chrome) SS().KeyPress(Keys.F);
                    right_up_f = false;
                    left_left_click = false;
                    mouse_click2(0);
                }
                else if ((left_up_click && e.Y == 0 && e.X < screenWidth))
                {
                    if (ProcessName.Equals(chrome))
                        SS().KeyPress(Keys.F);
                    left_up_click = false;
                }
                else if ((left_down_click && e.Y + 1 == screenHeight && e.X < screenWidth) && (e.X < expect_cornor_edge))
                {
                    left_down_click = false;
                }
                else if (left_down_click && e.Y + 1 == screenHeight && e.X < screenWidth)
                {
                    right_up_f = false;
                    left_down_click = false;
                    if (!not_allow && IsFullScreen()) return;
                    if (is_douyin() && IsFullScreen()) return;
                    if (judge_color(Color.FromArgb(210, 27, 70))) { return; }
                    if (right_up_f && ProcessName == Common.chrome) SS().KeyPress(Keys.F);
                    if (ProcessName == Common.chrome)
                    {
                        if (e.X > screenWidth2) return;
                        SS().KeyPress(Keys.F);
                        //if (!judge_color(1840, 51, Color.FromArgb(162, 37, 45)))
                        SS().MouseWhell(-120 * 7);
                        return;
                    }
                    mouse_click2(0);
                }
                else if (right_up_click && e.Y == 0 && e.X > screenWidth)
                {
                    right_up_click = false;
                    right_up_f = true;
                    mouse_click2(0);
                    SS().KeyPress(Keys.F);
                    //if (!judge_color(5534, 696, Color.FromArgb(0, 0, 0)))
                    if (e.X > 5106)
                        SS().MouseWhell(-120 * 7);
                }
            }
        }
    }
}
