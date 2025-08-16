using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using static keyupMusic2.KeyboardMouseHook;
using static keyupMusic2.Native;
using Point = System.Drawing.Point;

namespace keyupMusic2
{
    public partial class Common
    {
        public static int[] deal_size_x_y(int x, int y, bool puls_one = true)
        {
            if (screenWidth == 2560 && screenHeight == 1440) return new int[] { x, y };
            if (puls_one)
            {
                x = x + 1;
                y = y + 1;
            }
            x = x * screenWidth / 2560;
            y = y * screenHeight / 1440;
            return new int[] { x, y };
        }
        public static void mouse_move(int x, int y, int tick = 0)
        {
            x = deal_size_x_y(x, y)[0];
            y = deal_size_x_y(x, y)[1];

            //if (x > 0 && x < screenWidth)
            //    mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
            //else
            SetCursorPos(x, y);
            Thread.Sleep(tick);
        }
        public static void mouse_move_to(int x, int y, int tick = 0)
        {
            var Pos = Position;
            x += Pos.X;
            y += Pos.Y;

            x = deal_size_x_y(x, y, false)[0];
            y = deal_size_x_y(x, y, false)[1];
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
            Thread.Sleep(tick);
        }

        public static Rectangle ScreenPrimary = Screen.PrimaryScreen.Bounds;
        public static Rectangle ScreenSecond =
       Screen.AllScreens.FirstOrDefault(scr => !scr.Primary)?.Bounds ?? Rectangle.Empty;

        public static int screenWidth = ScreenPrimary.Width;
        public static int screenHeight = ScreenPrimary.Height;
        public static int screenWidth1 = ScreenPrimary.Width - 1;
        public static int screenHeight1 = ScreenPrimary.Height - 1;
        public static int screenWidth2 = ScreenPrimary.Width / 2;
        public static int screenHeight2 = ScreenPrimary.Height / 2;

        //public static int screen2Width = -2880;
        //public static int screen2Height = 1620;
        //public static int screen2Height1 = 1619;

        public static int screen2X = ScreenSecond.X;
        public static int screen2Y = ScreenSecond.Y;
        public static int screen2Width = screen2X + ScreenSecond.Width - 1;
        public static int screen2Height = ScreenSecond.Height + screen2Y;
        public static int screen2Height1 = screen2Height - 1;
        public static int screenHeightMax = Math.Max(screenHeight1, screen2Height1);


        public static int screen3Width = -1920;
        public static int screen3Height = 1080;
        public static int screen3Height1 = 1079;


        public static void mouse_move_center(int tick = 0)
        {

            int x = screenWidth / 2;
            int y = screenHeight / 2;
            Thread.Sleep(tick);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
        }
        public static void mouse_move(Point point, int tick = 0)
        {
            SetCursorPos(point.X, point.Y);
            Thread.Sleep(tick);
        }
        public static void mouse_click(int tick = 10)
        {
            if (tick > 0)
            {
                down_mouse(tick);
                up_mouse(tick);
                return;
            }
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        public static void mouse_middle()
        {
            mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
        }
        public static void mousewhell(int x, int y, int num)
        {
            var p = Position;
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE | MOUSEEVENTF_WHEEL, x * 65536 / screenWidth, y * 65536 / screenHeight, WHEEL_DELTA * num, 0);
            Sleep(100);
            mouse_move(p);
        }
        public static void mousewhell(int x, int y, int x2, int y2, int num)
        {
            var p = Position;
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE | MOUSEEVENTF_WHEEL, x * 65536 / screenWidth, y * 65536 / screenHeight, WHEEL_DELTA * num, 0);
            Sleep(1);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE | MOUSEEVENTF_WHEEL, x2 * 65536 / screenWidth, y2 * 65536 / screenHeight, WHEEL_DELTA * num, 0);
            Sleep(1);
            mouse_move(p);
        }
        public static void mousewhell(int num)
        {
            mouse_event2(MOUSEEVENTF_WHEEL, 0, 0, WHEEL_DELTA * num, 0);
        }
        public static void mousewhell(decimal num)
        {
            mouse_event2(MOUSEEVENTF_WHEEL, 0, 0, (int)(WHEEL_DELTA * num), 0);
        }
        public static int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo)
        {
            var aa = Native.mouse_event(dwFlags, dx, dy, cButtons, (int)isVir);
            //if ((dwFlags & MOUSEEVENTF_LEFTDOWN) == 1) { Sleep(10); FreshProcessName(); }
            //if ((dwFlags & MOUSEEVENTF_MOVE) == 0) { FreshProcessName(); Task.Run(() => { Sleep(100); FreshProcessName(); }); }
            if (dwFlags != MOUSEEVENTF_MOVE && dwFlags != MOUSEEVENTF_WHEEL) { FreshProcessName2(); }
            return aa;
        }
        public static int mouse_event2(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo)
        {
            if (dx != 0 || dy != 0)
            {
                dx = dx * 65536 / screenWidth;
                dx = dx * 65536 / screenHeight;
            }
            return Native.mouse_event(dwFlags, dx, dy, cButtons, (int)isVir);
        }
        public static void mouse_click_right(int x, int y)
        {
            mouse_move(x, y);
            mouse_click_right();
        }
        public static void mouse_click_right(int tick = 10)
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            Thread.Sleep(tick);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            Thread.Sleep(tick);
        }
        public static void mouse_click_right2()
        {
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }
        public static void mousego()
        {
            mouse_event(MOUSEEVENTF_XDOWN, 0, 0, XBUTTON2, 0);
            mouse_event(MOUSEEVENTF_XUP, 0, 0, XBUTTON2, 0);
        }
        public static void mousegoback(bool go = true)
        {
            mouse_event(MOUSEEVENTF_XDOWN, 0, 0, go ? XBUTTON2 : XBUTTON1, 0);
            mouse_event(MOUSEEVENTF_XUP, 0, 0, go ? XBUTTON2 : XBUTTON1, 0);
        }

        public static void mouseback()
        {
            mouse_event(MOUSEEVENTF_XDOWN, 0, 0, XBUTTON1, 0);
            mouse_event(MOUSEEVENTF_XUP, 0, 0, XBUTTON1, 0);
        }
        public static void mouse_click(int x, int y, int tick = 10)
        {
            mouse_move(x, y);
            if (tick > 0)
            {
                down_mouse(tick);
                up_mouse(tick);
                return;
            }
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        public static void mouse_click2(int tick = 0)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(tick);
        }
        public static void mouse_middle_click(int tick = 0)
        {
            mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
            Thread.Sleep(tick);
        }
        public static void down_mouse(int tick = 0)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(tick);
        }
        public static void up_mouse(int tick = 0)
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(tick);
        }
        public static Dictionary<Keys, string> VirMouseStateKey = new Dictionary<Keys, string>();
        public static void VirKeyState(KeyboardMouseHook.KeyEventArgs e)
        {
            if (e.Type == KeyType.Up)
                VirMouseStateKey.Remove(e.key);
            else
                VirMouseStateKey[e.key] = ProcessName;
        }
        public static void VirKeyState(Keys key, bool up = false)
        {
            if (up)
                VirMouseStateKey.Remove(key);
            else
                VirMouseStateKey[key] = ProcessName;
        }
        public static void VirKeyState(MouseMsg msg)
        {
            if (!mousekeymap.ContainsKey(msg)) return;
            //bool up = msg.ToString().Contains("up");
            Keys key = mousekeymap[msg];
            if (IsUpEvent(msg))
                VirMouseStateKey.Remove(key);
            else
                VirMouseStateKey[key] = ProcessName;
        }
        public static Dictionary<MouseMsg, Keys> mousekeymap = new Dictionary<MouseMsg, Keys>() {
           { MouseMsg.click,            Keys.LButton},
           { MouseMsg.click_up,         Keys.LButton},
           { MouseMsg.click_r,          Keys.RButton},
           { MouseMsg.click_r_up,       Keys.RButton},
           { MouseMsg.go,               Keys.XButton2},
           { MouseMsg.go_up,            Keys.XButton2},
           { MouseMsg.back,             Keys.XButton1},
           { MouseMsg.back_up,          Keys.XButton1},
           { MouseMsg.middle,           Keys.MButton},
           { MouseMsg.middle_up,        Keys.MButton},
        };
        public static void CleanVirMouseState()
        {
            if (VirMouseStateKey == null || VirMouseStateKey.Count == 0) return;
            lock (VirMouseStateKey)
            {
                VirMouseStateKey = new Dictionary<Keys, string>();
            }
        }
        //public static bool is_down(int key)
        //{
        //    return GetAsyncKeyState(key) < 0;
        //}

        public static bool is_down(Keys key)
        {
            return GetAsyncKeyState(key) < 0;
        }
        public static bool is_down_vir(Keys key)
        {
            return is_down(key) || (VirMouseStateKey.ContainsKey(key) && VirMouseStateKey[key] == ProcessName);
        }
        // job 一次按键监听一次缓存
        public static bool is_ctrl()
        {
            return is_down(Keys.ControlKey);
        }
        public static bool isctrl()
        {
            return is_ctrl();
        }
        public static bool is_lbutton()
        {
            return is_down(Keys.LButton);
        }
        public static bool is_ctrl_shift_alt()
        {
            return is_ctrl() || is_shift() || is_alt();
        }
        public static bool is_esc(Keys keys = Keys.ControlKey)
        {
            return is_down(Keys.Escape);
        }
        public static bool is_alt()
        {
            return is_down(Keys.LMenu) || is_down(Keys.RMenu);
        }

        public static bool is_shift()
        {
            return is_down(Keys.ShiftKey);
        }

        public static void press_rate(Keys num, int tick = 0)
        {
            RateLimiter.Execute(press, num, 0);
        }
        public static void press(Keys num, int tick = 0)
        {
            if (is_down(Keys.Delete)) return;
            if (num == Keys.MediaPlayPause) { if (!IsAnyMusicPlayerRunning()) StartNeteaseCloudMusic(); }
            press([num], tick);
        }
        public static void press(Keys num, int times, int tick = 0)
        {
            if (is_down(Keys.Delete)) return;
            for (Int32 i = 0; i < times; i++)
            {
                _press(num);
            }
            Thread.Sleep(tick);
        }
        public static void altab(int tick = 10)
        {
            press([Keys.LMenu, Keys.Tab], tick);
            //Thread.Sleep(tick);
        }
        public static void altshiftab(int tick = 10)
        {
            press([Keys.LMenu, Keys.LShiftKey, Keys.Tab], tick);
            //Thread.Sleep(tick);
        }
        public static void altabtab(int tick = 10)
        {
            press([Keys.LMenu, Keys.Tab, Keys.Tab], tick);
            //Thread.Sleep(tick);
        }
        //job isvir move to keybd_event
        public static void press_raw(Keys num, int tick = 0)
        {
            isVir = 0;
            press(num, tick);
            isVir = isVirConst;
        }
        public static void press_raw2(Keys num, int tick = 0)
        {
            isVir = isVirConst + 1;
            press(num, tick);
            isVir = isVirConst;
        }
        public static void press_raw(Keys[] keys, int tick = 10)
        {
            isVir = 0;
            press(keys, tick);
            isVir = isVirConst;
        }
        public static void press_task(Keys[] keys, int tick = 10)
        {
            TaskRun(() => press(keys, 10), tick);
        }
        public static void press(Keys[] keys, int tick = 10)
        {
            if (keys.Length is 0 or > 100)
                return;
            foreach (var item in keys)
            {
                keybd_event((byte)item, 0, 0, 0);
                Sleep(tick);
            }
            Array.Reverse(keys);
            foreach (var item in keys)
            {
                keybd_event((byte)item, 0, 2, 0);
            }
            Thread.Sleep(tick);
        }
        static Point mousePosition;
        public static Point lastPosition;
        public static void press_middle_bottom()
        {
            biu.RECTT.release();
            press(middle_bottom, 0);
            biu.RECTT.release();
        }
        public static void press_middle_bottom3()
        {
            biu.RECTT.release();
            mouse_move(screenWidth1, screenHeight1);
            biu.RECTT.release();
        }
        public static string middle_bottom = "1333.1439";
        public static string middle_bottom_last_position = "1333.1439";
        public static void press_middle_bottom2()
        {
            if (Position != new Point(1333, 1439))
            {
                middle_bottom_last_position = Position.X + "." + Position.Y;
                press(middle_bottom, 0);
                return;
            }
            press(middle_bottom_last_position, 0);
        }

        public static void ctrl_shift_win_search(bool zh = true)
        {
            //var _zh = (judge_color(2290, 1411, Color.FromArgb(242, 242, 242)));
            //var _zh = (judge_color(2281, 1413, Color.FromArgb(242, 242, 242)));
            //var _zh = (judge_color(1397, 419, Color.FromArgb(255, 255, 255)));
            var _zh = (judge_color(980, 420, Color.FromArgb(255, 255, 255)));
            var _en = !_zh;
            if (zh && _en)
                press(Keys.LShiftKey, 10);
            else if (!zh && !_en)
                press(Keys.LShiftKey, 10);
            return;
        }

        public static void press(int tick, params object[] inputs)
        {
            foreach (var item in inputs)
            {
                if ("en" == item)
                    ctrl_shift_win_search(false);
                else if (item is Keys keys)
                    press(keys, tick);
                else if (item is string str)
                    press_str(str, tick);
                else if (item is Keys[] ks)
                    press(ks, tick);
                else if (item is int num)
                    Sleep(num);
            }
        }
        public static void press_str(string str, int tick = 100)
        {
            press(str.ToUpper(), tick);
        }

        //1 返回原来鼠标位置
        //2
        //3 跳过delete return
        public static void press(string str, int tick = 100, bool force = false)
        {
            if (is_down(Keys.Delete) && !force) return;
            //KeyboardHook.stop_next = false;
            bool isLastDigitOne = (tick % 10) == 1;
            if (isLastDigitOne) mousePosition = Cursor.Position;
            var list = str.Split(";");
            list = list.Where(s => s != null && s != "").ToArray();
            if (list.Length == 0) return;
            foreach (var item in list)
            {
                var click = item.IndexOf(',');
                var move = item.IndexOf('.');

                if (item == "LWin")
                {
                    if (ProcessName == "SearchHost")
                        press([Keys.LControlKey, Keys.A, Keys.Back]);
                    else
                        press(Keys.LWin);
                    Thread.Sleep(100);
                    //ctrl_shift(false);
                }
                else if (item == "close")
                    CloseProcess();
                else if (item == "zh")
                    ctrl_shift_win_search(true);
                else if (item == "en")
                    ctrl_shift_win_search(false);
                else if (item == "_") down_mouse();
                else if (item == "-") up_mouse();
                else if (click > 0 || move > 0)
                {
                    if (!int.TryParse(item.Substring(0, click + move + 1), out int x)) continue;
                    if (!int.TryParse(item.Substring(click + move + 1 + 1), out int y)) continue;
                    //var x = int.Parse(item.Substring(0, click + move + 1));
                    //var y = int.Parse(item.Substring(click + move + 1 + 1));
                    mouse_move(x, y, 10);
                    if (click > 0) mouse_click(30);
                }
                else if ((int.TryParse(item, out int number)))
                {
                    Thread.Sleep(number);
                }
                else if (Enum.TryParse(typeof(Keys), item, out object asd))
                {
                    press((Keys)asd, tick);
                }
                else if (item.Length > 1)
                {
                    press(item.Substring(0, 1), 10);
                    press(item.Substring(1), 10);
                }
                if (!ReferenceEquals(item, list.Last()) || list.Length == 1)
                    Thread.Sleep(tick);
            }
            if (isLastDigitOne && mousePosition.X < 2560)
                mouse_move(mousePosition.X, mousePosition.Y, 100);
        }
        public static void _press(Keys keys)
        {
            keybd_event((byte)keys, (byte)(MapVirtualKey((ushort)keys, 0) & 0xFFU), 0, 0);
            keybd_event((byte)keys, (byte)(MapVirtualKey((ushort)keys, 0) & 0xFFU), 2, 0);
        }
        public static void press_dump(Keys keys, int tick = 10)
        {
            keybd_event((byte)keys, 0, 0, 0);
            Thread.Sleep(tick);
            keybd_event((byte)keys, 0, 2, 0);
        }
        public static void press_dump_task(Keys keys, int tick = 100, int tick2 = 0)
        {
            TaskRun(() => press_dump(keys, tick), tick2);
        }
        public static void down_press(Keys keys)
        {
            keybd_event((byte)keys, 0, 0, 0);
        }
        public static void up_press(Keys keys)
        {
            keybd_event((byte)keys, 0, 2, 0);
        }
        public static void down_press(Keys keys, bool raw)
        {
            if (!raw)
            {
                down_press(keys);
                return;
            }
            isVir = 0;
            keybd_event((byte)keys, 0, 0, 0);
            isVir = isVirConst;
        }
        public static void up_press(Keys keys, bool raw)
        {
            if (!raw)
            {
                up_press(keys);
                return;
            }
            isVir = 0;
            keybd_event((byte)keys, 0, 2, 0);
            isVir = isVirConst;
        }
        static ConcurrentDictionary<byte, byte> MapVirtualKey = new ConcurrentDictionary<byte, byte>();
        private static readonly HashSet<byte> ExtendedKeys = new HashSet<byte>
{
    0xA5, // RMenu
    0xA4, // LMenu
    0xAE, // 可能是VolumeDown
    0xAF, // 可能是VolumeUp
    0xA3, // RControl
    0xA2, // LControl
    0x5B, // LWin
    0x5C, // RWin
    0x2D, // Insert
    0x2E, // Delete
    0x25, // Left
    0x26, // Up
    0x27, // Right
    0x28, // Down
    0x20, // Space (在数字键盘上需要扩展标志)
    0x21, // PageUp
    0x22, // PageDown
    0x23, // End
    0x24, // Home
    0x90, // NumLock
    0x91, // ScrollLock
    // 可以根据需要添加更多扩展键
};
        static HashSet<byte> FreshProcessNameKeys = new HashSet<byte> { 0xA5, 0xA4, 0x5C, 0x5B, 0x09, 0x73 };

        private static void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo)
        {
            if (ExtendedKeys.Contains(bVk))
                dwFlags |= (uint)KeyboardFlag.ExtendedKey;
            //else if (is_douyin())
            //    dwFlags |= (uint)KeyboardFlag.ExtendedKey;

            if (!MapVirtualKey.ContainsKey(bVk))
                MapVirtualKey[bVk] = (byte)(MapVirtualKey(bVk, 0) & 0xFFU);

            Native.keybd_event(bVk, MapVirtualKey[bVk], dwFlags, isVir);

            if (!FreshProcessNameKeys.Contains(bVk)) return;
            FreshProcessName2();
        }
        [Flags]
        public enum KeyboardFlag : uint
        {
            None = 0x0000,
            KeyDown = 0x000,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Unicode = 0x0004,
            ScanCode = 0x0008,
        }
        private static readonly HashSet<MouseMsg> UpEvents = new HashSet<MouseMsg>
        {
            MouseMsg.click_up,
            MouseMsg.middle_up,
            MouseMsg.click_r_up,
            MouseMsg.back_up,
            MouseMsg.go_up
        };
        public static readonly HashSet<MouseMsg> NoUp = new HashSet<MouseMsg>
        {
            MouseMsg.move,
            MouseMsg.wheel,
            MouseMsg.wheel_h
        };
        public static bool IsUpEvent(MouseMsg msg)
        {
            return UpEvents.Contains(msg);
        }
        // 辅助方法：等待指定按键释放，带超时
        public static bool WaitForKeysReleased(int timeoutMs, params Func<bool>[] keyChecks)
        {
            int maxAttempts = timeoutMs / 100;
            for (int i = 0; i < maxAttempts; i++)
            {
                // 检查所有按键是否都已释放
                if (keyChecks.All(check => !check()))
                    return true;

                Thread.Sleep(100);
            }

            // 超时处理
            return false;
        }
    }
}
