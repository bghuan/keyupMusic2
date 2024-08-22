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
                    if (Position.Y == 0) press(Keys.VolumeDown);
                    break;
                case Keys.Right:
                    if (Position.Y == 0) press(Keys.VolumeUp);
                    break;
                case Keys.X:
                    if (Position.X == 0 && Position.Y == 0) { HideProcess(module_name); break; }
                    if (Position.X == 2559 && Position.Y == 0) { close(); break; }
                    break;
            }
            raw_press();
            Common.hooked = false;
            if (!handling) handling = true;
        }

    }
}
