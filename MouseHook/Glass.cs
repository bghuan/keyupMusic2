using keyupMusic2;
using System.Timers;
using static keyupMusic2.Common;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace keyupMusic2
{
    partial class biu
    {
        static Point last_left = new Point(297, 680);
        static Point aaa = new Point(1277, 743);
        public void Glass()
        {
            if (e.Msg == MouseMsg.move) return;
            var list = new string[] { Common.Glass, Common.Glass2, Common.Glass3, };
            if (!list.Contains(ProcessName)) return;

            if (e.Msg == MouseMsg.click)
            {
                if (IsMouseInCircle()) return;
                last_left = Position;
            }
            else if (e.Msg == MouseMsg.click_r_up)
            {
                if (!IsMouseInCircle()) return;
                mouse_click();
                mouse_move(last_left);
                mouse_click();
                mouse_move(aaa);
            }
            if (e.Msg == MouseMsg.back || e.Msg == MouseMsg.go)
            {
                int x = 2245;
                if (e.Msg == MouseMsg.go) x = 297;
                var point = Position;
                mouse_click();
                mouse_click(x, 680);
                last_left = Position;
                mouse_move(aaa);
            }
        }
        public bool IsMouseInCircle(int centerX = 1285, int centerY = 695, int radius = 700)
        {
            int dx = e.X - centerX;
            int dy = e.Y - centerY;
            return dx * dx + dy * dy <= radius * radius;
        }
    }
}