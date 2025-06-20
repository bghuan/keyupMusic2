using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;
using static keyupMusic2.Native;

namespace keyupMusic2
{
    public class OtherClass : Default
    {
        private readonly string[] list;
        private static readonly string[] list_wechat_visualstudio = {
            Common.WeChat, Common.ACPhoenix, explorer, Common.keyupMusic2, Common.douyin,
            Common.devenv, Common.QQMusic, Common.SearchHost, Common.ApplicationFrameHost,
            Common.vlc, Common.v2rayN, Common.cs2
        };
        private static readonly string[] list_volume = { Common.douyin, Common.msedge };

        public OtherClass()
        {
            // 只初始化一次，避免每次实例化都反射
            var constants = GetPublicConstStrings(typeof(Common));
            var tempList = new List<string>(constants.Count);
            foreach (var constant in constants)
                tempList.Add(constant.Value.ToString());
            list = tempList.ToArray();
        }

        public void hook_KeyDown(KeyboardHookEventArgs e)
        {
            string module_name = ProcessName;
            handling_keys = e.key;

            switch (module_name)
            {
                case QQLive:
                    HandleQQLive(e.key);
                    break;
                case Common.msedge:
                    HandleMsEdge(e);
                    break;
                case Common.Glass:
                case Common.Glass2:
                case Common.Glass3:
                    HandleGlass(e.key);
                    break;
                case Common.Kingdom:
                case Common.Kingdom5:
                    HandleKingdom(e);
                    break;
                case Common.ItTakesTwo:
                    HandleItTakesTwo(e.key);
                    break;
                case Common.SearchHost:
                    if (e.key is Keys.MediaNextTrack or Keys.MediaPreviousTrack)
                        press(Keys.LWin);
                    break;
                case Common.cs2:
                    HandleCS2(e);
                    break;
                case Common.steam:
                    HandleSteam(e.key);
                    break;
                case Common.Broforce_beta:
                    HandleBroforceBeta(e.key);
                    break;
                case Common.PowerToysCropAndLock:
                    if (e.key == Keys.Space)
                    {
                        hideProcessTitle(PowerToysCropAndLock);
                        MoveProcessWindow2(PowerToysCropAndLock);
                    }
                    break;
                case Common.oriwotw:
                    HandleOriwotw(e.key);
                    break;
                case Common.wemeetapp:
                    HandleWemeetApp(e.key);
                    break;
                case Common.Taskmgr:
                    if (e.key == Keys.F5)
                        Simm.KeyPress(Common.keyupMusic2);
                    break;
                case Common.vlc:
                    HandleVlc(e);
                    break;
                case Common.VSCode:
                    HandleVSCode(e);
                    break;
                case Common.explorer:
                case Common.keyupMusic2:
                    HandleProgman(e);
                    break;
                case Common.BandiView:
                    HandleBandiView(e);
                    break;
            }

            Common.hooked = false;
        }

        private void HandleBandiView(KeyboardHookEventArgs e)
        {
            if (e.key == Delete)
            {
                Sleep(200);
                press(Return);
            }
        }

        private void HandleProgman(KeyboardHookEventArgs e)
        {
            if (!IsDesktopFocused() && ProcessName != Common.keyupMusic2) return;
            switch (e.key)
            {
                case Keys.Down:
                    string nextWallpaper = GetNextWallpaper();
                    SetDesktopWallpaper(nextWallpaper, WallpaperStyle.Fit);
                    break;
                case Keys.Up:
                    string nextWallpaper2 = GetPreviousWallpaper();
                    SetDesktopWallpaper(nextWallpaper2, WallpaperStyle.Fit);
                    break;
                case Keys.Left:
                    string nextWallpaper3 = GetPreviousIdFolder();
                    SetDesktopWallpaper(nextWallpaper3, WallpaperStyle.Fit);
                    break;
                case Keys.Right:
                    string nextWallpaper4 = GetNextIdFolder();
                    SetDesktopWallpaper(nextWallpaper4, WallpaperStyle.Fit);
                    break;
            }
        }

        private void HandleQQLive(Keys key)
        {
            switch (key)
            {
                case Keys.Oem3:
                case Keys.D1:
                    press("2431.1404;2431.1406;2431.1408;100;2343,923", 101); break;
                case Keys.D2:
                    press("2431.1404;2431.1406;2431.1408;100;2269,959", 101); break;
                case Keys.D3:
                    press("2431.1404;2431.1406;2431.1408;100;2475,957", 101); break;
            }
        }

        private void HandleMsEdge(KeyboardHookEventArgs e)
        {
            switch (e.key)
            {
                case Keys.Right:
                    if (ProcessTitle?.Contains("起点中文网") == true)
                        press(Keys.PageDown, 0);
                    break;
                case Keys.Left:
                    if (ProcessTitle?.Contains("起点中文网") == true)
                        press(Keys.PageUp, 0);
                    break;
                case Keys.VolumeDown:
                    if (e.X == screenWidth1 || e.Y == screenHeight1)
                        press_rate(Keys.PageDown, 0);
                    break;
                case Keys.VolumeUp:
                    if (e.X == screenWidth1 || e.Y == screenHeight1)
                        press_rate(Keys.PageUp, 0);
                    break;
            }
        }

        private void HandleGlass(Keys key)
        {
            if (key is Keys.Left or Keys.Right or Keys.MediaPreviousTrack or Keys.MediaNextTrack)
            {
                int x = (key == Keys.Left || key == Keys.MediaNextTrack) ? 297 : 2245;
                var point = Position;
                mouse_click();
                mouse_click(x, 680);
                mouse_move(point, 10);
            }
        }

        private void HandleKingdom(KeyboardHookEventArgs e)
        {
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
        }

        private void HandleItTakesTwo(Keys key)
        {
            switch (key)
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
        }

        private void HandleCS2(KeyboardHookEventArgs e)
        {
            switch (e.key)
            {
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
                    break;
            }
        }

        private void HandleSteam(Keys key)
        {
            switch (key)
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
        }

        private void HandleBroforceBeta(Keys key)
        {
            switch (key)
            {
                case Keys.PageDown:
                case Keys.F1:
                    press("Escape;Right;Down;Down;Enter;Enter;", 200);
                    break;
                case Keys.F4:
                    CloseProcess(Broforce_beta);
                    break;
            }
        }

        private void HandleOriwotw(Keys key)
        {
            switch (key)
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
        }

        private void HandleWemeetApp(Keys key)
        {
            switch (key)
            {
                case Keys.F1:
                    CenterWindowOnScreen(wemeetapp);
                    break;
                case Keys.Oem3:
                    mouse_click();
                    break;
            }
        }

        private void HandleVlc(KeyboardHookEventArgs e)
        {
            int tick = 100;
            switch (e.key)
            {
                case Keys.D1:
                case Keys.MediaPreviousTrack:
                case Keys.PageUp:
                    if (e.key == Keys.MediaPreviousTrack && e.Y == screenHeight1)
                    {
                        press(Keys.Space);
                        break;
                    }
                    Sleep(tick);
                    mouse_click_right();
                    SS(tick)
                        .KeyPress(Keys.P)
                        .KeyPress(Keys.E)
                        .KeyPress(Keys.W)
                        .KeyPress(Keys.Enter);
                    break;
                case Keys.D2:
                case Keys.MediaNextTrack:
                case Keys.PageDown:
                    Sleep(tick);
                    mouse_click_right();
                    SS(tick)
                        .KeyPress(Keys.P)
                        .KeyPress(Keys.E)
                        .KeyPress(Keys.F)
                        .KeyPress(Keys.Enter);
                    break;
                case Keys.Right:
                case Keys.Left:
                    if (!is_ctrl()) break;
                    var key = e.key == Keys.Right ? Keys.X : Keys.V;
                    Sleep(200);
                    mouse_click_right();
                    SS(tick)
                        .KeyPress(key)
                        .KeyPress(Keys.Enter);
                    break;
            }
        }

        private void HandleVSCode(KeyboardHookEventArgs e)
        {
            switch (e.key)
            {
                case Keys.Escape:
                    Sleep(100);
                    press([Keys.LMenu, Keys.LShiftKey, Keys.F]);
                    break;
                case Keys.Q:
                    if (!isctrl()) return;
                    press(Keys.Oem2);
                    break;
            }
        }
    }
}