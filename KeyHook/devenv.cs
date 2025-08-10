using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;

namespace keyupMusic2
{
    public class DevenvClass : Default
    {
        static int is_oem = 0;
        public void hook_KeyDown_ddzzq(KeyboardMouseHook.KeyEventArgs e)
        {
            if (ProcessName != devenv) return;

            switch (e.key)
            {
                //case Keys.F4:
                //    if (ProcessTitle?.IndexOf("正在运行") >= 0)
                //        press([Keys.RShiftKey, Keys.F5]);
                //    break;
                case Keys.F5:
                    //if (Deven_runing())
                    //    press([Keys.RControlKey, Keys.RShiftKey, Keys.F5]);
                    if (Deven_runing()) press("116,69");
                    //else press("898,71");
                    break;

                case Keys.Escape:
                    Sleep(100);
                    press([Keys.RControlKey, Keys.K, Keys.D]);
                    break;
                //case Keys.F2:
                //    MessageBox.Show("sss");
                //    //var aaa = (GetPointTitle() == "");
                //    //if (aaa)
                //    //    mouse_move(Position.X + 500, Position.Y);
                //    //SS(1000).KeyPress(Keys.Down)
                //    //    .KeyPress(Keys.Enter)
                //    //    .KeyPress(Keys.Enter);
                //    //if (aaa)
                //    //    mouse_move(Position.X - 500, Position.Y);
                //    break;
            }
        }
    }
}

