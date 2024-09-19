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
                //case Keys.Right:
                case Keys.PageUp:
                    if (Position.Y == 0 && Position.X == 2559) { break; }
                    if (Position.Y == 0 && Position.X != 2559) { press(Keys.VolumeUp); break; }
                    Common.share();
                    if (share_string == "WM_LBUTTONDOWN") { press(Keys.VolumeUp); break; }
                    if (share_string == "WM_RBUTTONDOWN") { press(Keys.VolumeUp); break; }
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
                //case Keys.Left:
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
                case Keys.Oem3:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                    if (module_name != ClassName()) break;
                    int num = int.Parse(e.key.ToString().Replace("D", "").Replace("Oem3", "0"));
                    press("2236.1400;2226," + (1030 + (num * 50)), 101);
                    break;
            }
            Common.hooked = false;
            if (!handling) handling = true;
        }

    }
}
