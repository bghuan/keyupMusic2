using System.Timers;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    partial class biu
    {
        public void MoveStopClick()
        {
            if (!IsMouseStopClick) return;
            //if (!isMouseStopped) return;
            Point currentMousePosition = e.Pos;
            int distanceMoved = CalculateDistance(lastMousePosition, currentMousePosition);
            lastMoveTime = DateTime.Now;

            //if (distanceMoved > 75 && !is_down(Native.VK_LBUTTON))
            if (distanceMoved > 55)
            {
                lastMoveTime = DateTime.Now;
                lastMousePosition = currentMousePosition;
                isMouseStopped = false;
                long_mouse_down = false;
                huan.Invoke2(() => { huan.label1.Text = ++tickaaa + "," + tickccc; });
            }
        }
        private void timer1_Tick(object? sender, ElapsedEventArgs e2)
        {
            if (!IsMouseStopClick) return;
            //if (isMouseStopped) return;
            if (!lastMoveTime.HasValue) return;
            if (!long_mouse_down && is_down(Keys.LButton)) return;
            if (is_down(Keys.RButton)) return;
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsedTime = currentTime - lastMoveTime.Value;
            if (!isMouseStopped && elapsedTime.TotalMilliseconds > 100) // 假设超过1秒没移动算停了下来
            {
                //if (long_mouse_down)
                //    up_mouse();
                //else
                mouse_click();
                huan.Invoke2(() =>
                {
                    huan.label1.Text = tickaaa + "," + ++tickccc + "mouse_click";
                    play_sound_di();
                });
                lastMousePosition = e.Pos;
                isMouseStopped = true;
            }
            //else if (isMouseStopped && !long_mouse_down && elapsedTime.TotalMilliseconds > 600)
            //{
            //    long_mouse_down_double_flag = long_mouse_down_double_flag + 1;
            //    if (long_mouse_down_double_flag % 2 == 1)
            //    {
            //        down_mouse();
            //        huan.Invoke2(() => { huan.label1.Text = tickaaa + "--" + ++tickccc + "-" + long_mouse_down_double_flag;
            //            play_sound_di(130);
            //            play_sound_di();
            //        });
            //    }
            //    lastMousePosition = e.Pos;
            //    long_mouse_down = true;
            //}
            //else if (long_mouse_down && elapsedTime.TotalMilliseconds > 5000)
            //{
            //    up_mouse();
            //    huan.Invoke2(() => { huan.label1.Text = tickaaa + "++" + ++tickccc; });
            //    lastMousePosition = e.Pos;
            //    isMouseStopped = false;
            //}
        }
        static int long_mouse_down_double_flag = 0;
        private Point lastMousePosition;
        private DateTime? lastMoveTime;
        private int CalculateDistance(Point p1, Point p2)
        {
            int dx = p2.X - p1.X;
            int dy = p2.Y - p1.Y;
            return (int)Math.Sqrt(dx * dx + dy * dy);
        }
        int tickccc = 0;
        int tickaaa = 0;

        bool long_mouse_down = false;
        public void MoveStopClickListen()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 50;
            timer.Elapsed += timer1_Tick;
            timer.Start();
        }
        void MoveStop()
        {
            if (e.Msg == MouseMsg.move)
            {
                MoveStopClick();
            }
            //else if (e.Msg == MouseMsg.WM_LBUTTONDOWN || e.Msg == MouseMsg.WM_RBUTTONDOWN)
            //{
            //    lastMousePosition = e.Pos;
            //    isMouseStopped = true;
            //}
            else if (e.Msg == MouseMsg.click_up)
            {
                lastMousePosition = e.Pos;
                isMouseStopped = true;
            }
            else if (e.Msg == MouseMsg.click_r_up)
            {
                if (QTCheck(last_right_time, 200))
                {
                    IsMouseStopClick = !IsMouseStopClick;
                    huan.Invoke2(() => { huan.label1.Text = "IsMouseStopClick:" + IsMouseStopClick; });
                }
                last_right_time = DateTime.Now;
                lastMousePosition = e.Pos;
                isMouseStopped = true;
            }
            //右键双击取消
        }
        DateTime last_right_time = DateTime.MinValue;
    }
}
