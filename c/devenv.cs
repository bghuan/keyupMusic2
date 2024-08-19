using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = keyupMusic2.Common;

namespace keyupMusic2
{
    public class devenv : Default
    {
        static int is_oem = 0;
        public void hook_KeyDown_ddzzq(object? sender, KeyEventArgs e)
        {
            if (pagedown_edge.yo() != ClassName()) return;
            Common.hooked = true;

            switch (e.KeyCode)
            {
                case Keys.F10:
                    Common.press([Keys.LControlKey, Keys.LShiftKey, Keys.F5]);
                    break;
                case Keys.F6:
                    Common.press([Keys.LShiftKey, Keys.F5]);
                    break;
            }
            Common.hooked = false;
        }
    }
}

