using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WGestures.Core.Impl.Windows;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;

using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class ACPhoenix : Default
    {
        public static int is_oem = 0;
        public static int move_length = 120;

        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            //if (ProcessName != ClassName() && (ProcessName != Common.explorer)) return;
            if (ProcessName != ClassName()) return;
            Common.hooked = true;

            switch (e.key)
            {
                case Keys.Oem3:
                    if (is_oem != 0) break;
                    mouse_down();
                    is_oem = 30;
                    while (is_oem > 0)
                    {
                        Thread.Sleep(30);
                        if (!is_down(Keys.Oem3))
                        {
                            mouse_up();
                            is_oem = 0;
                        }
                    }
                    break;
                case Keys.F1: //打开好友列表
                    press("1800,1000;144,319;203,66;157,359;", 200);
                    Thread.Sleep(30);
                    mouse_move(800, 850);
                    break;
                case Keys.F2: //确认观战
                    mouse_click();
                    press("100;1525,1072;100;1300,930;", 0);
                    break;
                case Keys.F3:
                    press("100,60;100,60;200,360;", 301);
                    break;
                case Keys.F4: //退出观战 //如何避免退出游戏
                    if (is_alt()) break;
                    press("2478,51;2492,1299;1545,1055;", 201);
                    break;
                case Keys.F5: //主页设置画面
                    press("2450,73;2107,229;1302,253;2355,237;2408,1000;", 101);
                    Common.ACPhoenix_mouse_hook = true;
                    break;
                case Keys.F6: //游戏设置画面
                    press("2494,68;2135,668;1087,235;56,67;", 501);
                    Common.ACPhoenix_mouse_hook = true;
                    break;
                case Keys.F12:
                    Common.FocusProcess(Common.WeChat);
                    Thread.Sleep(100);
                    if (ProcessName2 == Common.WeChat) break;
                    press("LWin;WEI;Enter;", 50);
                    break;
                case Keys.X:
                    if (!is_ctrl()) break;
                    press("300;Enter;A;", 101);
                    break;
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    if (!is_ctrl() && !is_alt()) break;
                    int num = int.Parse(e.key.ToString().Replace("D", ""));
                    press("300," + (num * 100 + 100), 1);
                    break;
                case Keys.Up:
                    mouse_move2(0, -move_length);
                    break;
                case Keys.Down:
                    mouse_move2(0, move_length);
                    break;
                case Keys.Left:
                    mouse_move2(-move_length, 0);
                    break;
                case Keys.Right:
                    mouse_move2(move_length, 0);
                    break;
                case Keys.P:
                    press("2325,53", 101);
                    break;
                case Keys.Q:
                    press("2134,1275", 101);
                    break;
                case Keys.LControlKey:
                    mouse_downing = true;
                    mouse_down();
                    break;
            }
            Common.hooked = false;
        }
        public bool ACPhoenix_mouse_down = false;
        public void MouseHookProc(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            {
                if (ACPhoenix_mouse_down) ACPhoenix_mouse_down = false;
            }
            else if (e.Msg == MouseMsg.WM_RBUTTONDOWN)
            {
                if (ProcessName2 != Common.ACPhoenix) return;
                //if (ACPhoenix_mouse_down == false) mouse_down();
                //else mouse_up();
                //ACPhoenix_mouse_down = !ACPhoenix_mouse_down;
                mouse_click();
                press("100;1525,1072;100;1300,930;", 0);
            }
        }
        //if (Common.ACPhoenix_mouse_hook && ProcessName == Common.ACPhoenix && (_mouseKbdHook == null || !_mouseKbdHook.is_install))
        //{
        //    _mouseKbdHook = new MouseKeyboardHook();
        //    _mouseKbdHook.MouseHookEvent += aCPhoenix.MouseHookProc;
        //    _mouseKbdHook.Install();
        //}
        //else if (_mouseKbdHook != null && _mouseKbdHook.is_install && ProcessName != Common.ACPhoenix)
        //{
        //    _mouseKbdHook.Uninstall();
        //    _mouseKbdHook.Dispose();
        //    Common.ACPhoenix_mouse_hook = false;
        //}
    }
}
