using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

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
            string nothing3 = "1280.634";
            string nothing4 = "1284.640";
            handling_keys = e.key;

            switch (e.key)
            {
                case Keys.Space:
                    //if (Position.Y == 0) { press(Keys.MediaNextTrack); break; }
                    if (try_press(Color.FromArgb(255, 162, 16))) break;
                    if (try_press(Color.FromArgb(32, 104, 234))) break;
                    if (try_press(Color.FromArgb(220, 163, 51))) break;
                    if (try_press(1433, 1072, Color.FromArgb(245, 194, 55), () => { })) break;//中间确定
                    if (try_press(1431, 1205, Color.FromArgb(255, 202, 57), () => { })) break;//中间确定
                    if (try_press(1384, 1199, Color.FromArgb(241, 141, 20), () => { })) { break; }//确定查看图鉴
                    if (is_alt() && try_press(2079, 1280, Color.FromArgb(220, 163, 48), () => { })) break;//匹配游戏
                    if (try_press(1422, 942, Color.FromArgb(245, 194, 55), () => { })) { break; }//匹配进入游戏
                    if (is_alt() && try_press(2497, 1328, Color.FromArgb(148, 185, 195), () => { })) { break; }//匹配取消
                    if (judge_color(127, 177, Color.FromArgb(255, 227, 132))) { press(Keys.Escape); break; }//关闭图鉴
                    if (!is_ctrl() && !is_alt() && judge_color(1307, 85, Color.FromArgb(36, 39, 54), null, 10) && judge_color(2450, 80, Color.FromArgb(194, 198, 226))) { press(Keys.Tab); break; }//关闭tab
                    raw_press();
                    break;
                case Keys.Tab:
                    if (is_alt()) { break; }
                    if (is_down(Keys.LWin)) { break; }
                    if (try_press(137, 278, Color.FromArgb(118, 196, 30), () => { }))
                    {
                        if (judge_color(592, 67, Color.FromArgb(255, 255, 255)))
                        {
                            press("100;203, 66; ", 0);
                        }
                        press("157,359;" + nothing2, 10);
                        break;
                    }
                    //打开关闭好友列表
                    if (is_ctrl() && try_press(47, 90, Color.FromArgb(231, 232, 231), () => { press("100;203, 66; 157,359;" + nothing4, 0); })) break;
                    if (judge_color(0, 1426, Color.FromArgb(13, 39, 75)) && judge_color(2402, 97, Color.FromArgb(36, 39, 54)) && judge_color(858, 177, Color.FromArgb(32, 104, 234)))
                    {
                        press("1342, 1112"); break;
                    }
                    break;
                case Keys.Escape:
                    if (judge_color(2487, 936, Color.FromArgb(49, 218, 255))) break;
                    if (judge_color(967, 114, Color.FromArgb(19, 62, 165))
                        && judge_color(1741, 110, Color.FromArgb(19, 62, 165)))
                    {
                        press("2407,186");
                        break;
                    }
                    if (try_press(870, 1243, Color.FromArgb(26, 125, 222), () => { press("2452,185;2452,185;" + nothing, 100); })) break;
                    break;
                case Keys.Oem3:
                    if (is_ctrl() || is_alt()) { mouse_click(); mouse_click(); break; }
                    mouse_downing = true;
                    down_mouse();
                    break;
                case Keys.Enter:
                    if (try_press(138, 1149, Color.FromArgb(222, 35, 10), () => { press("200,710", 101); })) break;
                    break;
                case Keys.Oem7:
                    if (!judge_color(1353, 1407, Color.FromArgb(255, 162, 16), () => { })) break;
                    SS(10).KeyPress(new Keys[] { Keys.LControlKey, Keys.A }, "谢谢老板");
                    break;
                case Keys.Z:
                    if (judge_color(2141, 214, Color.FromArgb(215, 214, 216), null, 10)) { press(Keys.E); break; }
                    //bug 滑动
                    //每次重置
                    if (!is_ctrl() && !is_alt()) break;
                    press("2473,50;2472,50;1115,324;", 100);
                    press("_;272.700;272.600;272.400;272.330;100;-;272,330", 100);
                    //Ssss.MouseWhell(-120 * 10);
                    break;
                case Keys.X:
                    if (judge_color(2141, 214, Color.FromArgb(215, 214, 216), null, 10)) { press(Keys.E); break; }
                    if (!is_ctrl() && !is_alt()) break;
                    press("2325, 53", 101);
                    break;
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                    int num = int.Parse(e.key.ToString().Replace("D", ""));
                    if (!is_ctrl() && !is_alt()) break;
                    press("300," + (num * 104 + 100), 1);
                    break;
                case Keys.Q:
                    //if (judge_color(2202, 644, Color.FromArgb(239, 116, 108), () => { press("334,944"); })) break;//装备1
                    //if (try_press(1360, 1369, Color.FromArgb(255, 162, 16))) break;
                    //if (!is_ctrl() && !is_alt()) break;
                    //press("10;2134,1275;10", 101);
                    break;
                case Keys.E:
                    //if (judge_color(2202, 644, Color.FromArgb(239, 116, 108), () => { press("1580,932"); })) break;//装备3
                    if (Position.Y == 0) { press(Keys.MediaNextTrack); break; }
                    if ((is_ctrl() || is_alt()) && judge_color(2524, 210, Color.FromArgb(39, 61, 118), null, 10)) { mouse_move(2139, 336); break; }
                    if (is_ctrl() || is_alt()) { mouse_move(2139, 336); }
                    raw_press();
                    break;
                case Keys.R:
                    //if (judge_color(2202, 644, Color.FromArgb(239, 116, 108), () => { press("2220,938"); })) break;//装备4
                    //(2413,1089, Color.FromArgb(231,125,8)(1807,1125, Color.FromArgb(32,52,75)(2002,349, Color.FromArgb(255,139,0)
                    if (judge_color(2007, 340, Color.FromArgb(255, 139, 0)))
                    {
                        if (judge_color(2103, 1130, Color.FromArgb(140, 255, 85))) break;
                        if (judge_color(2105, 1129, Color.FromArgb(140, 255, 85))) break;
                        press("1800, 1119;2130, 327;2130, 327", 100);
                        break;
                    }//装备重铸
                    break;
                //case Keys.F2:
                //    mouse_click();
                //    press("1525,1072;", 0);
                //    //if (try_press(Color.FromArgb(220, 163, 51))) break;
                //    //if (try_press(1447, 1068, Color.FromArgb(245, 194, 55), () => { })) break;
                //    break;
                case Keys.F4:
                    if (is_alt()) break;
                    if (ProcessName2 != keyupMusic2.Common.ACPhoenix) { break; }
                    press("2478,51;2492,1299;", 201);
                    break;
                case Keys.F5:
                    press("2450,73;2107,229;1302,253;2355,237;2408,1000;", 201);
                    break;
                case Keys.F6:
                    //(2381, 805, Color.FromArgb(60, 68, 82)
                    press("2494,68;2135,805;1087,235;56,67;", 501);
                    break;
                //case Keys.Home:
                case Keys.PageUp:
                    //if (judge_color(2098, 188, Color.FromArgb(109, 189, 205)))
                    {
                        //(1555, 1150, Color.FromArgb(250, 198, 131)(2074, 386, Color.FromArgb(246, 250, 253)(2219, 231, Color.FromArgb(201, 202, 201)
                        press("2094, 187;2219, 231;2074, 386;1555, 1150", 200);
                    }
                    break;
                case Keys.PageDown:
                    var altTabProcess = AltTabProcess();
                    switch (altTabProcess)
                    {
                        case msedge:
                            press(Keys.PageDown, 100);
                            break;
                        case QyClient:
                            press(Keys.Space, 100);
                            break;
                    }
                    altab();
                    break;
                case Keys.A:
                    if (is_ctrl()) { press(Keys.A); press(Keys.A); press(Keys.A); break; }
                    break;
                case Keys.Delete:
                    if (judge_color(2409, 265, Color.FromArgb(93, 199, 250)) && judge_color(2358, 263, Color.FromArgb(93, 199, 250)))
                    {
                        press("2409,265", 1, true);
                    }
                    else
                        press("2460,50;2460,200", 101, true);
                    break;
            }
            Common.hooked = false;
            if (!handling) handling = true;
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
