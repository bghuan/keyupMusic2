﻿using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

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
                //case Keys.F4:
                //    if (ProcessTitle?.IndexOf("正在运行") >= 0)
                //        press([Keys.RShiftKey, Keys.F5]);
                //    break;
                case Keys.F5:
                    if (Deven_runing())
                        press([Keys.RControlKey, Keys.RShiftKey, Keys.F5]);
                    break;
                //case Keys.F11:
                //    ProcessStartInfo startInfo = new ProcessStartInfo("taskmgr.exe");
                //    Process.Start(startInfo);
                //    break;

                case Keys.F:
                    if (is_alt() && is_shift())
                    {
                        TaskRun(() =>
                        {
                            press([Keys.RControlKey, Keys.K]);
                            press([Keys.RControlKey, Keys.D]);
                        }, 300);
                    }
                    break;
            }
            Common.hooked = false;
        }
    }
}

