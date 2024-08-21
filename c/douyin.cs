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

            switch (e.key)
            {
                case Keys.PageDown:
                case Keys.Left:
                    e.Handled = true;
                    if (module_name == Common.msedge && e.key == Keys.Left) break;
                    if (Cursor.Position.Y < 100)
                    {
                        press(Keys.VolumeDown);
                        press(Keys.VolumeDown);
                    }
                    break;
                case Keys.PageUp:
                case Keys.Right:
                    e.Handled = true;
                    if (module_name == Common.msedge && e.key == Keys.Right) break;
                    if (Cursor.Position.Y < 100)
                    {
                        press(Keys.VolumeUp);
                        press(Keys.VolumeUp);
                    }
                    break;
            }
            Common.hooked = false;
        }

    }
}
