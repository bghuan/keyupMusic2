using System.Diagnostics;
using System.Diagnostics.Metrics;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
using Timer = System.Timers.Timer;

namespace keyupMusic2
{
    public class CoocaaClass
    {
        public static readonly HashSet<Keys> keys = new HashSet<Keys> { Up, Down, Keys.Right, Keys.Left, Keys.LButton | Keys.OemClear, BrowserHome, Apps, Keys.Sleep };
        public static bool handled(KeyboardMouseHook.KeyEventArgs e)
        {
            return ((Common.DeviceName == Common.coocaa && keys.Contains(e.key)) || e.key == Keys.Sleep);
        }
        public static bool judge_handled(KeyboardMouseHook.KeyEventArgs e)
        {
            if (handled(e)) return true;
            return false;
        }

        public static bool KeyUp(KeyboardMouseHook.KeyEventArgs e)
        {
            if (!handled(e)) return false;

            return true;
        }


        public static bool Hooked(KeyboardMouseHook.KeyEventArgs e)
        {
            if (e.Type == KeyType.Down)
                return KeyDown(e);
            //if (e.Type == KeyType.Up)
            return KeyUp(e);

        }
        public static int timer_tick = 500;
        public static DateTime timer_endtime = DateTime.MinValue;
        public static Timer timer;
        public static void timer_func()
        {
            if (DateTime.Now < timer_endtime) return;
            else if (is_down(LMenu)) up_press(LMenu);
            timer.Stop();
        }

        public static Timer timer_power;
        public static bool timer_power_flag = true;
        public static void timer_power_func()
        {
            if (timer_power_flag)
            {
                press_raw(F3);
                HideSomething();
            }
            timer_power.Stop();
        }
        public static bool KeyDown(KeyboardMouseHook.KeyEventArgs e)
        {
            if (!handled(e)) return false;

            if (e.key == (Keys.LButton | Keys.OemClear))
            {
                if (timer_power == null)
                {
                    timer_power = new() { Interval = 100, AutoReset = false };
                    timer_power.Elapsed += (s, e) => timer_power_func();
                }
                timer_power_flag = !timer_power.Enabled;
                timer_power.Start();
                return true;
            }

            if (IsDesktopFocused())
            {
                if (e.key == BrowserHome) { press_raw2(PageUp); return true; }
                else if (e.key == Apps) { press_raw2(Delete); return true; }
                else if (e.key == Keys.Sleep) { }
                else return false;
            }

            timer_endtime = DateTime.Now.AddMilliseconds(timer_tick);
            switch (e.key)
            {
                case Keys.BrowserHome:
                    mousego();
                    break;
                case Keys.Apps:
                    mouseback();
                    break;
                case Keys.Sleep:
                    if (ProcessName == msedge)
                    {
                        HideProcess();
                        var pos = e.Pos;
                        mouse_move(2300, 1200);
                        if (GetPointTitle() == FolderView)
                        {
                            mouse_click();
                            break;
                        }
                        else
                            mouse_move(pos);
                        FocusProcess(msedge);
                    }
                    if (!is_down(LMenu))
                        down_press(LMenu);
                    press(Tab);
                    if (timer == null)
                    {
                        timer = new() { Interval = 10, AutoReset = true };
                        timer.Elapsed += (s, e) => timer_func();
                    }
                    timer.Start();
                    break;

                case Keys.Up:
                    press(PageUp);
                    break;
                case Keys.Down:
                    if (!is_alt()) { press(PageDown); break; }
                    press([LShiftKey, Tab]);
                    if (is_down(LMenu)) up_press(LMenu);
                    Sleep(20);
                    if (ProcessName == msedge)
                    {
                        HideProcess();
                        var pos = e.Pos;
                        mouse_move(2300, 1200);
                        if (GetPointTitle() == FolderView)
                            mouse_click();
                    }
                    break;
                case Keys.Right:
                    press(Right);
                    break;
                case Keys.Left:
                    press(Left);
                    break;

            }
            return true;
        }
    }
}
