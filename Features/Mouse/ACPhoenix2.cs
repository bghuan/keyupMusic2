using System.Timers;
using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;

namespace keyupMusic4
{
    partial class biu
    {
        public void MoveStopClick(KeyboardMouseHook.MouseEventArgs e)
        {
            if (!IsMouseStopClick) return;
            Point currentMousePosition = e.Pos;
            int distanceMoved = CalculateDistance(lastMousePosition, currentMousePosition);
            lastMoveTime = DateTime.Now;

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
            if (!lastMoveTime.HasValue) return;
            if (!long_mouse_down && is_down(Keys.LButton)) return;
            if (is_down(Keys.RButton)) return;
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsedTime = currentTime - lastMoveTime.Value;
            if (!isMouseStopped && elapsedTime.TotalMilliseconds > 100) // 假设超过1秒没移动算停了下来
            {
                mouse_click();
                huan.Invoke2(() =>
                {
                    huan.label1.Text = tickaaa + "," + ++tickccc + "mouse_click";
                    play_sound_di();
                });
                isMouseStopped = true;
            }
        }
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
        void MoveStop(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.move)
            {
                MoveStopClick(e);
            }
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
