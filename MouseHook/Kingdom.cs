using System.Timers;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    partial class biu
    {
        public void Kingdom(MouseKeyboardHook.MouseEventArgs e)
        {
            if (ProcessName != keyupMusic2.Common.Kingdom) return;
            //MoveStop();

            if (e.Msg == MouseMsg.click_r_up)
            {
                //if (is_ctrl())
                //{
                //    mouse_click2(10);
                //    mouse_click2(10);
                //    mouse_click2(10);
                //    return;
                //}
                //mouse_click2(10);
                //press(Keys.Enter, 0);
                mouse_click2(10);
                SS(200).KeyPress(Keys.Down, true)
                    .KeyPress(Keys.Down, true)
                    .KeyPress(Keys.Down, true)
                    .KeyPress(Keys.Down, true)
                    .KeyPress(Keys.Right, true)
                    .KeyPress(Keys.Return);
            }
        }
    }
}
