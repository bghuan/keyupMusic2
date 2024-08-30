using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;

using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class douyin : Default
    {
        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            string module_name = ProcessName;
            if (module_name != ClassName() && module_name != Common.msedge) return;
            Common.hooked = true;
            handling_keys = e.key;

            switch (e.key)
            {
                case Keys.Left:
                case Keys.PageUp:
                    if (Position.Y == 0) { press(Keys.VolumeDown); break; }
                    raw_press();
                    break;
                case Keys.Right:
                case Keys.PageDown:
                    if (Position.Y == 0) { press(Keys.VolumeUp); break; }
                    //raw_press();
                    if (module_name == ClassName())
                    {
                        handling = false;
                        press_dump(e.key, 210);
                        press_dump(e.key, 210);
                        Thread.Sleep(10);
                    }
                    break;
                case Keys.X:
                    //if (module_name == Common.msedge) { break; }
                    //if (Position.X == 0 && Position.Y == 0) { HideProcess(module_name); break; }
                    //if (Position.X == 2559 && Position.Y == 0) { close(); break; }
                    //if (module_name == ClassName()) { }
                    //raw_press();
                    break;
            }
            Common.hooked = false;
            if (!handling) handling = true;
        }

    }
}
