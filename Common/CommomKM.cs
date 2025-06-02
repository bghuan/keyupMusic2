using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using static keyupMusic2.Native;
using Point = System.Drawing.Point;

namespace keyupMusic2
{
    public partial class Common
    {
        public static int[] deal_size_x_y(int x, int y, bool puls_one = true)
        {
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

            if (x > 0 && x < screenWidth)
                mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
            else
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

        public static int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        public static int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        public static int screenWidth1 = Screen.PrimaryScreen.Bounds.Width - 1;
        public static int screenHeight1 = Screen.PrimaryScreen.Bounds.Height - 1;
        public static int screenWidth2 = Screen.PrimaryScreen.Bounds.Width / 2;
        public static int screenHeight2 = Screen.PrimaryScreen.Bounds.Height / 2;

        public static int screen2Width = -2880;
        public static int screen2Height = 1620;
        public static int screen2Height1 = 1619;

        public static void mouse_move_center(int tick = 0)
        {
            int x = screenWidth / 2;
            int y = screenHeight / 2;
            Thread.Sleep(tick);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
        }
        public static void mouse_move(Point point, int tick = 0)
        {
            Thread.Sleep(tick);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, point.X * 65536 / screenWidth, point.Y * 65536 / screenHeight, 0, 0);
        }
        public static void mouse_move2(int x, int y, int tick = 0)
        {
            x += Cursor.Position.X;
            y += Cursor.Position.Y;
            Thread.Sleep(tick);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
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
        public static int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo)
        {
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
        // 模拟鼠标前进（点击前进按钮）
        public static void MouseForward()
        {
            // 按下前进按钮
            mouse_event(MOUSEEVENTF_XDOWN, 0, 0, XBUTTON2, 0);
            // 释放前进按钮
            mouse_event(MOUSEEVENTF_XUP, 0, 0, XBUTTON2, 0);
        }

        // 模拟鼠标后退（点击后退按钮）
        public static void MouseBack()
        {
            // 按下后退按钮
            mouse_event(MOUSEEVENTF_XDOWN, 0, 0, XBUTTON1, 0);
            // 释放后退按钮
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
        public static void mouse_click3()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        public static DateTime mouse_click_not_repeat_time = DateTime.Now;
        public static void mouse_click_not_repeat()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            mouse_click_not_repeat_time = DateTime.Now;
        }
        public static bool not_repeat()
        {
            if (mouse_click_not_repeat_time.AddSeconds(1) > DateTime.Now) return false;
            mouse_click_not_repeat_time = DateTime.Now;
            return true;
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
        public static bool is_down(Keys key)
        {
            return Native.GetAsyncKeyState(key) < 0;
        }
        public static bool is_down(int key)
        {
            return Native.GetAsyncKeyState(key) < 0;
        }

        public static bool is_ctrl()
        {
            return Native.GetAsyncKeyState(Keys.ControlKey) < 0;
        }
        public static bool isctrl()
        {
            return is_ctrl();
        }
        public static bool is_ctrl_shift_alt()
        {
            return is_ctrl() || is_shift() || is_alt();
        }
        public static bool is_esc()
        {
            return Native.GetAsyncKeyState(Keys.Escape) < 0;
        }
        public static bool is_alt()
        {
            return Native.GetAsyncKeyState(Keys.LMenu) < 0 || Native.GetAsyncKeyState(Keys.RMenu) < 0;
        }

        public static bool is_shift()
        {
            return Native.GetAsyncKeyState(Keys.ShiftKey) < 0;
        }
        public static readonly object _lockObject2 = new object();

        public static void press_rate(Keys num, int tick = 0)
        {
            RateLimiter.Execute(press, num, 0);
        }
        public static void press(Keys num, int tick = 0)
        {
            if (is_down(Keys.Delete)) return;
            lock (_lockObject2)
            {
                press([num], tick);
            }
        }
        public static void press(Keys num, int times, int tick = 0)
        {
            if (is_down(Keys.Delete)) return;
            lock (_lockObject2)
            {
                for (Int32 i = 0; i < times; i++)
                {
                    _press(num);
                }
            }
            Thread.Sleep(tick);
        }
        public static bool _Not_F10_F11_F12_Delete = true;
        public static bool Not_F10_F11_F12_Delete(bool refresh = false, Keys current_key = new Keys())
        {
            if (refresh)
            {
                var keys = new[] { Keys.F10, Keys.F11, Keys.F12, Keys.Delete, Keys.LControlKey, Keys.RControlKey };
                var sss = false;
                var filteredKeys = keys.Where(key => key != current_key).ToArray();
                foreach (Keys key in filteredKeys)
                {
                    if (is_down(key))
                    {
                        sss = true;
                    }
                }
                _Not_F10_F11_F12_Delete = !sss;
                //_Not_F10_F11_F12_Delete = !is_down(Keys.F10) && !is_down(Keys.F11) && !is_down(Keys.Delete);
            }
            return _Not_F10_F11_F12_Delete;
        }
        public static void press_close()
        {
            if (!not_repeat()) return;
            press([Keys.LMenu, Keys.F4]);
        }
        public static void altab(int tick = 0)
        {
            press([Keys.LMenu, Keys.Tab]);
            Thread.Sleep(tick);
        }
        public static void press_raw(Keys num, int tick = 0)
        {
            isVir = 0;
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
            if (keys == null || keys.Length == 0 || keys.Length > 100)
                return;
            if (keys.Length == 1)
            {
                _press(keys[0]);
            }
            else if (keys.Length > 1 && keys[0] == keys[1])
            {
                foreach (var key in keys)
                {
                    _press(key);
                };
            }
            else
            {
                foreach (var item in keys)
                {
                    //Thread.Sleep(10);
                    keybd_event((byte)item, 0, 0, 0);
                    Sleep(10);
                }
                Array.Reverse(keys);
                foreach (var item in keys)
                {
                    //Thread.Sleep(10);
                    keybd_event((byte)item, 0, 2, 0);
                }
            }
            Thread.Sleep(tick);
        }
        static Point mousePosition;
        public static Point lastPosition;
        public static void press_middle_bottom()
        {
            press(middle_bottom, 0);
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
            var _zh = (judge_color(1397, 419, Color.FromArgb(255, 255, 255)));
            var _en = !_zh;
            if (zh && _en)
                press(Keys.LShiftKey, 10);
            else if (!zh && !_en)
                press(Keys.LShiftKey, 10);
            return;
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
                    {
                        press([Keys.LControlKey, Keys.A]);
                        press([Keys.Back]);
                    }
                    else
                        press(Keys.LWin);
                    Thread.Sleep(100);
                    //ctrl_shift(false);
                }
                else if (item == "zh")
                {
                    ctrl_shift_win_search(true);
                }
                else if (item == "en")
                {
                    ctrl_shift_win_search(false);
                }
                else if (item == "_") down_mouse();
                else if (item == "-") up_mouse();
                //else if (click > 0 && item.Substring(0, click + 1).IndexOf(",") > 0)
                //{ }
                else if (click > 0 || move > 0)
                {

                    var x = int.Parse(item.Substring(0, click + move + 1));
                    var y = int.Parse(item.Substring(click + move + 1 + 1));
                    mouse_move(x, y, 10);
                    if (click > 0) mouse_click(30);
                }
                else if ((int.TryParse(item, out int number)))
                {
                    Thread.Sleep(number);
                }
                else if (Enum.TryParse(typeof(Keys), item, out object asd))
                {
                    press((Keys)asd);
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

            keybd_event((byte)keys, (byte)(Native.MapVirtualKey((ushort)keys, 0) & 0xFFU), 0, 0);
            keybd_event((byte)keys, (byte)(Native.MapVirtualKey((ushort)keys, 0) & 0xFFU), 2, 0);
        }
        public static void press_dump(Keys keys, int tick = 500)
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
        static Dictionary<byte, byte> MapVirtualKey = new Dictionary<byte, byte>();
        private static void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo)
        {
            if (!MapVirtualKey.ContainsKey(bVk))
            {
                MapVirtualKey.Add(bVk, (byte)(Native.MapVirtualKey(bVk, 0) & 0xFFU));
            }
            Native.keybd_event(bVk, MapVirtualKey[bVk], dwFlags, isVir);
        }
    }
}
