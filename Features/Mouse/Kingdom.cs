using System.Timers;
using static keyupMusic4.Common;

namespace keyupMusic4
{
    partial class biu
    {
        public void Kingdom(KeyboardMouseHook.MouseEventArgs e)
        {
            if (ProcessName != keyupMusic4.Common.Kingdom) return;

            if (e.Msg == MouseMsg.click_r_up)
            {
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
