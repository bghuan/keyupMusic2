using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = keyupMusic2.Common;

namespace keyupMusic2
{
    public class ACPhoenix: Default
    {
        static int is_oem = 0; 
        public void hook_KeyDown_ddzzq(object? sender, KeyEventArgs e)
        {
            if (pagedown_edge.yo() != ClassName()) return;
            Common.hooked = true;

            switch (e.KeyCode)
            {
                case Keys.Oem3:
                    if (is_oem != 0) break;
                    C.mouse_down();
                    is_oem = 30;
                    while (is_oem > 0)
                    {
                        Thread.Sleep(30);
                        if (!C.is_down(Keys.Oem3))
                        {
                            C.mouse_up();
                            is_oem = 0;
                        }
                    }
                    break;
                case Keys.F1: //打开好友列表
                    //Common.press("129,336;200;129,336;", 0);
                    Common.press("144,319;203,66;157,359;", 200);
                    break;
                case Keys.F2: //确认观战
                    C.mouse_click();
                    Common.press("100;1525,1072;", 0);
                    break;
                case Keys.F4: //退出观战 //如何避免退出游戏
                    if (C.is_down(Keys.LMenu)) break;
                    Common.press("2478,51;2492,1299;1545,1055;", 201);
                    break;
                case Keys.F5: //主页设置画面
                    Common.press("2494,68;2135,668;1087,235;56,67;", 501);
                    Common.ACPhoenix_mouse_hook = true;
                    break;
                case Keys.F6: //游戏设置画面
                    Common.press("2450,73;2107,229;1302,253;2355,237;", 101);
                    Common.ACPhoenix_mouse_hook = true;
                    break;
                case Keys.X:
                    if (!C.is_ctrl()) break;
                    Common.press("300;Enter;A;", 101);
                    break;
            }
            Common.hooked = false;
        }

    }
}
