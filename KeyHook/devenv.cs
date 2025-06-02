using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public class DevenvClass : Default
    {
        static int is_oem = 0;
        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            if (ProcessName != devenv) return;
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

                case Keys.F1:
                case Keys.Escape:
                    Sleep(100);
                    press([Keys.RControlKey, Keys.K, Keys.D]);
                    break;
                case Keys.F2:
                    press("73,242", 1);
                    break;
                case Keys.F:
                    if (is_alt() && is_shift())
                    {
                        TaskRun(() =>
                        {
                            press([Keys.RControlKey, Keys.K, Keys.D]);
                        }, 300);
                    }
                    break;
            }
            Common.hooked = false;
        }
    }
}

