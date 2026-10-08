using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;

namespace keyupMusic4
{
    public class DevenvClass : Default
    {
        public void HookEvent(KeyboardMouseHook.KeyEventArgs e)
        {
            if (ProcessName != devenv) return;

            switch (e.key)
            {
                case Keys.F5:
                    if (Deven_runing()) press("583,74");
                    else press("116,69");
                    break;

                case Keys.Escape:
                    Sleep(100);
                    press([Keys.RControlKey, Keys.K, Keys.D]);
                    break;
            }
        }
    }
}

