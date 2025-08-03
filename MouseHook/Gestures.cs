using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
using static keyupMusic2.Native;
using static keyupMusic2.Simulate;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace keyupMusic2
{
    public partial class biu
    {
        List<string> list = new() { chrome };
        private void Gestures(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;
            if (e.Msg == MouseMsg.click) { menu_opened = false; return; }
            if (e.Msg == MouseMsg.click_r) r_down_x = e.Pos;
            if (e.Msg != MouseMsg.click_r_up) return;
            if (r_down_x == Point.Empty) return;
            if (!list.Contains(ProcessName)) return;

            int arraw = 0;
            int arraw_long = 110;
            bool catched = true;
            if (e.Y - r_down_x.Y > arraw_long) arraw = 3;
            else if (e.Y - r_down_x.Y < -arraw_long) arraw = 4;
            else if (e.X - r_down_x.X > arraw_long) arraw = 1;
            else if (e.X - r_down_x.X < -arraw_long) arraw = 2;

            var is_chrome = ProcessName == chrome && !menu_opened;

            switch (arraw)
            {
                case 0:
                    catched = false;
                    if (ProcessName == chrome)
                    {
                        menu_opened = true;
                        mouse_click_right();
                    }
                    break;
                case 1:
                    if (is_chrome) quick_left_right(arraw); break;// 右
                case 2:
                    if (is_chrome) quick_left_right(arraw); break;// 左
                //case 1:
                //    if (is_chrome) press_raw([LWin, Right]); break;// 右
                //case 2:
                //    if (is_chrome) press_raw([LWin, Left]); break;// 左
                case 3:
                    if (is_chrome) press([Keys.LControlKey, Keys.W]); break;//  下
                case 4:
                    if (is_chrome) SS().KeyPress(Keys.M); break;//  上
                default:
                    catched = false; break;
            }
            if (catched)
            {
                // log(e.X + " " + e.Y + " " + r_down_x.X + " " + r_down_x.Y + " " + arraw);
                r_down_x = Point.Empty;
                play_sound_di();
            }
        }


    }
}
