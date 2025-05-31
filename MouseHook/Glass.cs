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
        int line = 580;
        int line2 = 1980;
        public void Glass()
        {
            if (e.Msg == MouseMsg.move) return;
            var asdads = new string[] { Common.Glass, Common.Glass2, Common.Glass3, };
            if (!asdads.Contains(ProcessName)) return;

            //const int VK_XBUTTON1 = 0x05; // 鼠标侧键前进键的虚拟键码
            //const int VK_XBUTTON2 = 0x06; // 鼠标侧键后退键的虚拟键码
            //var dfsaf = Native.GetAsyncKeyState(VK_XBUTTON1);
            //var dfsaf2 = Native.GetAsyncKeyState(VK_XBUTTON2);

            if (e.Msg == MouseMsg.click_up)
            {
                if (!(e.X > line && e.X < line2))
                {
                    last_left = e.Pos;
                }
            }
            else if (e.Msg == MouseMsg.click_r_up)
            {
                if (!(e.X > line && e.X < line2))
                {
                    last_left = e.Pos;
                    Thread.Sleep(100);
                    mouse_click2();
                }
                else
                {
                    var point = Position;
                    mouse_move(last_left);
                    mouse_click();
                    mouse_move(point, 10);
                }
            }
            else if (e.Msg == MouseMsg.back)
            {
                var dd = 297;
                if (e.X > screenWidth2) dd = 2245;
                if (!(e.X > line && e.X < line2))
                {
                    mouse_click();
                    mouse_click(dd, 680);
                    mouse_click();
                    mouse_move(screenWidth2, screenHeight2);
                }
                else
                {
                    var point = Position;
                    mouse_click();
                    mouse_click(dd, 680);
                    mouse_click();
                    mouse_move(point, 10);
                }
            }
        }
    }
}