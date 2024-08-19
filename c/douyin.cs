using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = keyupMusic2.Common;

namespace keyupMusic2
{
    public class douyin: Default
    {
        public void hook_KeyDown_ddzzq(object? sender, KeyEventArgs e)
        {
            if (pagedown_edge.yo() != ClassName()) return;
            Common.hooked = true;

            switch (e.KeyCode)
            {
                case Keys.F6: 
                    Common.press("2450,73;2107,229;1302,253;2355,237;", 101);
                    break;
            }
            Common.hooked = false;
        }

    }
}
