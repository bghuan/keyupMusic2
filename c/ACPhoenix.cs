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
        public bool ACPhoenix_mouse_down = false;

        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            if (is_alt() && is_down(Keys.Tab)) return;
            if (ProcessName != ClassName()) return;
            Common.hooked = true;
            string nothing = "1834.1103";
            string nothing2 = screenWidth2 + "." + screenHeight2;
            handling_keys = e.key;

            switch (e.key)
            {
                case Keys.Space:
                    if (Position.Y == 0) { press(Keys.MediaNextTrack); break; }
                    if (try_press(Color.FromArgb(255, 162, 16))) break;
                    if (try_press(Color.FromArgb(32, 104, 234))) break;
                    if (try_press(Color.FromArgb(220, 163, 51))) break;
                    if (try_press(1433, 1072, Color.FromArgb(245, 194, 55))) break;
                    if (try_press(1384, 1199, Color.FromArgb(241, 141, 20))) { break; }//确定查看图鉴
                    if (try_press(1422, 942, Color.FromArgb(245, 194, 55))) { break; }//匹配进入游戏
                    if (judge_color(127, 177, Color.FromArgb(255, 227, 132))) { press(Keys.Escape); break; }//关闭图鉴
                    raw_press();
                    break;
                case Keys.Tab:
                    if (is_alt()) { break; }
                    if (is_ctrl() && try_press(47, 90, Color.FromArgb(231, 232, 231), () => { press("100;203, 66; 157,359;" + nothing, 0); })) break;
                    //主页打开关闭好友列表
                    if (try_press(137, 278, Color.FromArgb(118, 196, 30), () => { press("50;203, 66;10; 157,359; 800.850", 10); })) break;
                    if (judge_color(0, 1426, Color.FromArgb(13, 39, 75), () => { mouse_click(screenWidth2, screenHeight2); })) break;
                    //raw_press();
                    break;
                case Keys.Escape:
                    if (try_press(870, 1243, Color.FromArgb(26, 125, 222), () => { press("2452,185;2452,185;" + nothing, 100); })) break;
                    break;
                case Keys.Oem3:
                    if (is_ctrl() || is_alt()) { mouse_click(); mouse_click(); break; }
                    mouse_downing = true;
                    down_mouse();
                    break;
                case Keys.Enter:
                    //(135,1152, Color.FromArgb(212,29,14)
                    if (try_press(138, 1149, Color.FromArgb(222, 35, 10), () => { press("200,710", 101); })) break;
                    break;
                case Keys.Z:
                    if (!is_ctrl() && !is_alt()) break;
                    press("2473,50;2472,50;1115,324;", 100);
                    press("_;272.700;272.600;272.400;272.330;100;-;272,330", 100);
                    break;
                case Keys.X:
                    if (!is_ctrl() && !is_alt()) break;
                    press("2325, 53", 101);
                    break;
                case Keys.F2:
                    mouse_click();
                    press("100;1525,1072;", 0);
                    break;
                case Keys.F4:
                    if (is_alt()) break;
                    press("2478,51;2492,1299;", 201);
                    break;
                case Keys.F5:
                    press("2450,73;2107,229;1302,253;2355,237;2408,1000;", 201);
                    break;
                case Keys.F6:
                    press("2494,68;2135,668;1087,235;56,67;", 501);
                    break;
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                    if (!is_ctrl() && !is_alt()) break;
                    int num = int.Parse(e.key.ToString().Replace("D", ""));
                    press("300," + (num * 100 + 100), 1);
                    break;
                case Keys.Q:
                    if (try_press(1360, 1369, Color.FromArgb(255, 162, 16))) break;
                    if (!is_ctrl() && !is_alt()) break;
                    press("10;2134,1275;10", 101);
                    break;
                case Keys.E:
                    if (Position.Y == 0) { press(Keys.MediaPreviousTrack); break; }
                    if (is_ctrl() || is_alt()) mouse_move(2139, 336);
                    raw_press();
                    break;
            }
            Common.hooked = false;
            if(!handling) handling = true;
        }


        public void MouseHookProc(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.WM_LBUTTONUP)
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
