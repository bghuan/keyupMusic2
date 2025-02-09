using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public class Other : Default
    {
        public Other()
        {
            var constants = GetPublicConstStrings(typeof(Common));
            var list = new List<string>();
            foreach (var constant in constants)
            {
                list.Add(constant.Value.ToString());
            }
            this.list =list.ToArray();
        }
        string[] list = new string[200];
        string[] list_wechat_visualstudio = { Common.WeChat, Common.ACPhoenix, explorer, Common.keyupMusic2, Common.douyin, Common.devenv, Common.QQMusic, Common.SearchHost, Common.ApplicationFrameHost, Common.vlc, Common.v2rayN, Common.cs2 };
        string[] list_volume = { Common.douyin, Common.msedge };
        static bool flag_special = false;

        public void hook_KeyDown(Keys keys)
        {
            new Other().hook_KeyDown(new KeyboardHookEventArgs(KeyboardEventType.KeyDown, keys, 0, new Native.keyboardHookStruct()));
        }
        public void hook_KeyDown(KeyboardHookEventArgs e)
        {
            string module_name = ProcessName;
            //flag_special = is_down(Keys.Delete);
            if (!list.Contains(module_name) || flag_special) return;
            Common.hooked = true;
            Not_F10_F11_F12_Delete(true, e.key);
            handling_keys = e.key;

            switch (e.key)
            {
                case Keys.F11:
                    //if (is_down(Keys.Delete)) {run_vis(); break; }
                    if (is_ctrl()) break;
                    if (!Not_F10_F11_F12_Delete()) break;
                    if (is_shift())
                    {
                        press("500;", 100);
                        run_vis();
                    }
                    //else if (is_douyin())
                    //{
                    //    if (Common.FocusProcess(Common.devenv)) break;
                    //    run_vis();
                    //}
                    else if (Common.devenv == module_name)
                    {
                        HideProcess(module_name);
                    }
                    else if (list_wechat_visualstudio.Contains(module_name) || flag_special || Huan.keyupMusic2_onlisten)
                    {
                        if (Common.FocusProcess(Common.devenv)) break;
                        if (e.X == screenWidth1) break;
                        run_vis();
                    }
                    break;
                case Keys.F12:
                    if (is_ctrl()) break;
                    if (!Not_F10_F11_F12_Delete()) break;
                    // if (is_douyin())
                    //{
                    //    Common.FocusProcess(Common.WeChat);
                    //    Thread.Sleep(10);
                    //    if (ProcessName2 == Common.WeChat) break;
                    //    run_wei();
                    //}
                    //else 
                    if (Common.WeChat == module_name)
                    {
                        CloseProcess(module_name);
                    }
                    else if (GetWindowText() == UnlockingWindow || ProcessName == LockApp || ProcessName == err)
                    {
                        Super.hook_KeyDown(Keys.N);
                    }
                    else if (list_wechat_visualstudio.Contains(module_name) || flag_special || Huan.keyupMusic2_onlisten)
                    {
                        Common.FocusProcess(Common.WeChat);
                        Thread.Sleep(10);
                        if (ProcessName2 == Common.WeChat) break;
                        run_wei();
                    }
                    break;
                case Keys.MediaPreviousTrack:
                    if (module_name == HuyaClient) { press("587,152", 1); break; }
                    break;
                case Keys.PageDown:
                    if (module_name == QyClient) press("563, 894", 1); break;
                case Keys.F4:
                    if (module_name == vlc) CloseProcess(vlc); break;
            }
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
                        //case Keys.PageDown:
                        //    if (e.X > screenWidth || is_down(Native.VK_RBUTTON))
                        //        press(Keys.VolumeDown, 5, 0); break;
                        //case Keys.PageUp:
                        //    if (e.X > screenWidth)
                        //        press(Keys.VolumeUp, 5, 0); break;
                        case Keys.Right:
                            if (ProcessTitle?.IndexOf("起点中文网") >= 0)
                                press(Keys.PageDown, 0); break;
                        case Keys.Left:
                            if (ProcessTitle?.IndexOf("起点中文网") >= 0)
                                press(Keys.PageUp, 0); break;
                        case Keys.VolumeDown:
                            if (e.X == screenWidth1 || e.Y == screenHeight1)
                                press(Keys.PageDown, 0); break;
                        case Keys.VolumeUp:
                            if (e.X == screenWidth1 || e.Y == screenHeight1)
                                press(Keys.PageUp, 0); break;
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
                        case Keys.MediaPreviousTrack:
                            if (is_down(Native.VK_LBUTTON)) break;
                            press("B;930,962;B;");
                            break;
                        case Keys.MediaNextTrack:
                            if (is_down(Native.VK_LBUTTON)) break;
                            press("B;1241,692;B;");
                            break;
                        case Keys.F4:
                            CloseProcess(cs2);
                            break;
                        case Keys.F5:
                            press("1301,48;100;1274,178;2260,1374");
                            break;
                        case Keys.F6:
                            press("Escape;1643,179");
                            break;
                    }
                    break;
                case Common.steam:
                    switch (e.key)
                    {
                        //case Keys.Space:
                        case Keys.F5:
                        case Keys.MediaNextTrack:
                            press("808,651;");
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
            }



            Common.hooked = false;
        }
        private static void run_wei()
        {
            if (!Common.ExsitProcess(Common.WeChat))
            {
                press("LWin;100;WEI;100;Enter;", 50, flag_special);
                return;
            }
            press([Keys.LControlKey, Keys.LMenu, Keys.W]);
        }

        private static void run_vis()
        {
            press("LWin;VISUAL;100;Apps;100;Enter;", 100, flag_special);
            TaskRun(() => { press("Tab;Down;Enter;", 100); }, 1600);
        }
    }
}
