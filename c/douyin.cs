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
            if (is_down(Keys.LWin)) return;
            //if (!handling) return;
            Common.hooked = true;
            handling_keys = e.key;

            switch (e.key)
            {
                case Keys.Right:
                case Keys.PageUp:
                    if (Position.Y == 0 && Position.X != 2559) { press(Keys.VolumeUp); break; }
                    if (module_name == ClassName())
                    {
                        handling = false;
                        press_dump(e.key, 210);
                        press_dump(e.key, 210);
                        Thread.Sleep(10);
                        break;
                    }
                    raw_press();
                    break;
                case Keys.Left:
                case Keys.PageDown:
                    if (Position.Y == 0 && Position.X != 2559) { press(Keys.VolumeDown); break; }
                    raw_press();
                    break;
                case Keys.X:
                    //if (module_name == Common.msedge) { break; }
                    //if (Position.X == 0 && Position.Y == 0) { HideProcess(module_name); break; }
                    //if (Position.X == 2559 && Position.Y == 0) { close(); break; }
                    //if (module_name == ClassName()) { }
                    //raw_press();
                    break;
                case Keys.D1:
                    if (module_name != ClassName()) break;
                    press("2236.1400;2226,1062", 101);
                    break;
                case Keys.D2:
                    if (module_name != ClassName()) break;
                    press("2236.1400;2226,1284", 101);
                    break;
                case Keys.D3:
                    if (module_name != ClassName()) break;
                    press("2236.1400;2226,1333", 101);
                    break;
            }
            Common.hooked = false;
            if (!handling) handling = true;
        }

    }
}
