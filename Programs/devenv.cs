using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;

using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class devenv : Default
    {
        static int is_oem = 0;
        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            if (ProcessName != ClassName()) return;
            Common.hooked = true;

            switch (e.key)
            {
                case Keys.F4:
                    press([Keys.LShiftKey, Keys.F5]);
                    break;
                case Keys.F5:
                    if (judge_color(82, 68, Color.FromArgb(189, 64, 77)))
                        press([Keys.LControlKey, Keys.LShiftKey, Keys.F5]);
                    break;
                case Keys.F6:
                    press([Keys.LControlKey, Keys.LShiftKey, Keys.F5]);
                    break;
                    //case Keys.F11:
                    //    ProcessStartInfo startInfo = new ProcessStartInfo("taskmgr.exe");
                    //    Process.Start(startInfo);
                    //    break;
            }
            Common.hooked = false;
        }
    }
}

