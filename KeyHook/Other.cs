using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;
using static keyupMusic2.Native;

namespace keyupMusic2
{
    public class OtherClass : Default
    {
        public OtherClass()
        {
            var constants = GetPublicConstStrings(typeof(Common));
            var list = new List<string>();
            foreach (var constant in constants)
            {
                list.Add(constant.Value.ToString());
            }
            this.list = list.ToArray();
        }
        string[] list = new string[200];
        string[] list_wechat_visualstudio = { Common.WeChat, Common.ACPhoenix, explorer, Common.keyupMusic2, Common.douyin, Common.devenv, Common.QQMusic, Common.SearchHost, Common.ApplicationFrameHost, Common.vlc, Common.v2rayN, Common.cs2 };
        string[] list_volume = { Common.douyin, Common.msedge }; 

        public void hook_KeyDown(Keys keys)
        {
            new OtherClass().hook_KeyDown(new KeyboardHookEventArgs(KeyboardType.KeyDown, keys, 0, new Native.keyboardHookStruct()));
        }
        public void hook_KeyDown(KeyboardHookEventArgs e)
        {
            string module_name = ProcessName;
            Common.hooked = true;
            //Not_F10_F11_F12_Delete(true, e.key);
            handling_keys = e.key;

            switch (module_name)
            {
                case QQLive:
                    switch (e.key)
                    {
                        case Keys.Oem3:
                        case Keys.D1:
                            press("2431.1404;2431.1406;2431.1408;100;2343,923", 101); break;
                        case Keys.D2:
                            press("2431.1404;2431.1406;2431.1408;100;2269,959", 101); break;
                        case Keys.D3:
                            press("2431.1404;2431.1406;2431.1408;100;2475,957", 101); break;
                    }
                    break;
                case Common.msedge:
                    switch (e.key)
                    {
                        case Keys.Right:
                            if (ProcessTitle?.IndexOf("起点中文网") >= 0)
                                press(Keys.PageDown, 0); break;
                        case Keys.Left:
                            if (ProcessTitle?.IndexOf("起点中文网") >= 0)
                                press(Keys.PageUp, 0); break;
                        case Keys.VolumeDown:
                            if (e.X == screenWidth1 || e.Y == screenHeight1)
                                press_rate(Keys.PageDown, 0); break;
                        case Keys.VolumeUp:
                            if (e.X == screenWidth1 || e.Y == screenHeight1)
                                press_rate(Keys.PageUp, 0); break;
                    }
                    break;
                case Common.Glass:
                case Common.Glass2:
                case Common.Glass3:
                    switch (e.key)
                    {
                        case Keys.Left:
                        case Keys.Right:
                        case Keys.MediaPreviousTrack:
                        case Keys.MediaNextTrack:
                            int asdsa = 2245;
                            if (e.key == Keys.Left) asdsa = 297;
                            if (e.key == Keys.MediaNextTrack) asdsa = 297;
                            var point = Position;
                            mouse_click();
                            mouse_click(asdsa, 680);
                            mouse_move(point, 10);
                            break;
                    }
                    break;
                case Common.Kingdom:
                case Common.Kingdom5:
                    switch (e.key)
                    {
                        case Keys.Oem3:
                            mouse_click2();
                            if (is_ctrl())
                            {
                                mouse_move(Position.X + 115, Position.Y + 30, 100);
                                mouse_click2();
                            }
                            break;
                        case Keys.Tab:
                            S10.KeyPress(Keys.D4, true);
                            break;
                        case Keys.Space:
                            S10.KeyPress(Keys.D5, true);
                            break;
                    }
                    break;
                case Common.ItTakesTwo:
                    switch (e.key)
                    {
                        case Keys.Oem3:
                        case Keys.J:
                            mouse_click();
                            break;
                        case Keys.K:
                            mouse_click_right();
                            break;
                        case Keys.L:
                            press_dump(Keys.Space, 500);
                            break;
                    }
                    break;
                case Common.SearchHost:
                    switch (e.key)
                    {
                        case Keys.MediaNextTrack:
                        case Keys.MediaPreviousTrack:
                            press(Keys.LWin);
                            break;
                    }
                    break;
                case Common.cs2:
                    switch (e.key)
                    {
                        //case Keys.MediaPreviousTrack:
                        //    if (is_down(Keys.LButton)) break;
                        //    if (is_down(Keys.D1)) press("B;1243,699;1483,429;1483,568;1483,828;1483,696;1483,969;B;D3;");
                        //    else if (is_down(Keys.D5)) press("B;985,969;1483,429;1483,568;1483,696;1483,969;B;D3;");
                        //    else
                        //        press("B;985,699;1483,429;1483,568;1483,828;1483,696;1483,969;B;D3;");
                        //    //("B;930,962;1483,568;1483,696;1483,828;1483,969;B;D3;");
                        //    break;
                        //case Keys.MediaNextTrack:
                        //    if (is_down(Keys.LButton)) break;
                        //    press("Escape;");
                        //    break;
                        //case Keys.MediaNextTrack:
                        //    if (is_down(Native.VK_LBUTTON)) break;
                        //    press("B;1241,692;1483,568;1483,696;1483,828;1483,969;B;D3;", 50);
                        //    break;
                        case Keys.F1:
                            break;
                        case Keys.F2:
                            Sleep(100);
                            press(Keys.D3);
                            break;
                        case Keys.F5:
                            press("1301,48;100;1274,178;2260,1374");
                            break;
                        case Keys.F6:
                            press("Escape;1643,179;100;1466,818;1556,806");
                            break;
                        case Keys.OemPeriod:
                            var sfa = Position;
                            mouse_click();
                            press("1654, 555;1564, 970;");
                            mouse_move(sfa);
                            //if (is_ctrl()) press("170;Escape;");
                            break;
                    }
                    break;
                case Common.steam:
                    switch (e.key)
                    {
                        case Keys.F5:
                        case Keys.MediaNextTrack:
                            press("808,651;", 1);
                            CloseProcess(steam);
                            break;
                        case Keys.MediaPreviousTrack:
                            press("36,70", 1);
                            break;
                    }
                    break;
                case Common.Broforce_beta:
                    switch (e.key)
                    {
                        case Keys.PageDown:
                        case Keys.F1:
                            press("Escape;Right;Down;Down;Enter;Enter;", 200);
                            break;
                        case Keys.F4:
                            CloseProcess(Broforce_beta);
                            break;
                    }
                    break;
                case Common.PowerToysCropAndLock:
                    switch (e.key)
                    {
                        case Keys.Space:
                            hideProcessTitle(PowerToysCropAndLock);
                            MoveProcessWindow2(PowerToysCropAndLock);
                            //CenterWindowOnScreen(chrome, true);
                            break;
                    }
                    break;
                case Common.oriwotw:
                    switch (e.key)
                    {
                        case Keys.F1:
                            press("H;200;955,332;1179,335;1395,336;1613,332;" +
                                "732,922;902,939;1183,939;1060,780;H;");
                            break;
                        case Keys.F2:
                            press("H;200;955,332;1179,335;1395,336;1613,332;" +
                                "1378,1102;1519,1113;1685,785;1183,939;H;");
                            break;
                    }
                    break;
                case Common.wemeetapp:
                    switch (e.key)
                    {
                        case Keys.F1:
                            CenterWindowOnScreen(wemeetapp);
                            break;
                        case Keys.Oem3:
                            mouse_click();
                            break;
                            //case Keys.MediaNextTrack:
                            //   SS(0).KeyPress(Keys.C);
                            //    break;
                    }
                    break;
                case Common.vlc:
                    var tick = 100;
                    switch (e.key)
                    {
                        case Keys.D1:
                        case Keys.MediaPreviousTrack:
                        case Keys.PageUp:
                            var fdsafdas = e.Y;
                            if (e.key == Keys.MediaPreviousTrack && e.Y == screenHeight1) { press(Keys.Space); break; }
                            Sleep(tick);
                            mouse_click_right();
                            SS(tick)
                                //.KeyPress(Keys.Apps)
                                .KeyPress(Keys.P)
                                .KeyPress(Keys.E)
                                .KeyPress(Keys.W)
                                .KeyPress(Keys.Enter)
                                ;
                            break;
                        case Keys.D2:
                        case Keys.MediaNextTrack:
                        case Keys.PageDown:
                            Sleep(tick);
                            mouse_click_right();
                            SS(tick)
                                //.KeyPress(Keys.Apps)
                                .KeyPress(Keys.P)
                                .KeyPress(Keys.E)
                                .KeyPress(Keys.F)
                                .KeyPress(Keys.Enter)
                                ;
                            break;
                        case Keys.Right:
                        case Keys.Left:
                            var keyssss = Keys.V;
                            if (e.key == Keys.Right) keyssss = Keys.X;
                            if (!is_ctrl()) break;
                            Sleep(200);
                            mouse_click_right();
                            SS(tick)
                                .KeyPress(keyssss)
                                .KeyPress(Keys.Enter)
                                ;
                            break;
                    }
                    break;
            }



            Common.hooked = false;
        }
    }
}
