using System.Diagnostics;
using System.Diagnostics.Metrics;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
using Timer = System.Timers.Timer;

namespace keyupMusic2
{
    public class AirKeyboardClass
    {
        public static readonly HashSet<Keys> keys = new HashSet<Keys> { Up, Down, Keys.Right, Keys.Left, Enter, BrowserBack, BrowserHome, Apps, Back, VolumeMute };
        public const Keys super_key = Keys.Enter;
        public static bool handled(KeyboardMouseHook.KeyEventArgs e)
        {
            return Common.DeviceName == Common.airkeyboard && keys.Contains(e.key);
        }
        public static bool judged;
        public static bool judge_handled(KeyboardMouseHook.KeyEventArgs e)
        {
            //judged = handled(e);
            //return judged;
            if (handled(e)) return true;
            return false;
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
        public static bool KeyUp(KeyboardMouseHook.KeyEventArgs e)
        {
            return true;
        }

        public static bool Hooked(KeyboardMouseHook.KeyEventArgs e)
        {
            if (Huan.keyupMusic2_onlisten && e.key != super_key) return false;
            if (!handled(e)) return false;
            try
            {
                if (e.Type == KeyType.Down)
                    return KeyDown(e);
                //if (e.Type == KeyType.Up)
                return KeyUp(e);
            }
            finally
            {
                judged = false;
            }
        }
        public static bool KeyDown(KeyboardMouseHook.KeyEventArgs e)
        {
            if (IsDesktopFocused())
            {
                if (e.key == BrowserBack) { press_raw2(PageUp); return true; }
                else if (e.key == Apps) { press_raw2(Delete); return true; }
                //else if (e.key == Keys.Sleep) { }
                else return false;
            }

            timer_endtime = DateTime.Now.AddMilliseconds(timer_tick);
            switch (e.key)
            {
                //case Keys.MediaPlayPause:
                //    press_raw2(F3);
                //    HideSomething();
                //    break;
                case super_key:
                    press_raw2(F3);
                    HideSomething();
                    break;
                case Keys.BrowserBack:
                    if (is_douyin())
                    {
                        press(X);
                        break;
                    }
                    else if (ProcessName == msedge)
                    {
                        press(MediaPreviousTrack);
                        break;
                    }
                    else if (ProcessName == chrome)
                    {
                        press(F);
                        break;
                    }
                    mousego();
                    break;
                case Keys.Apps:
                    if (ProcessName == BandiView)
                    {
                        press(Delete);
                        var judge = () => { return GetPointTitle() == ""; };
                        var run = () => { press(Escape); };
                        DelayRun(judge, run, 1000, 100);
                        break;
                    }
                    else if (ProcessName == msedge)
                    {
                        press(MediaNextTrack);
                        break;
                    }
                    else if (ProcessName == chrome)
                    {
                        press(M);
                        break;
                    }
                    mouseback();
                    break;
                case Keys.Back:
                    if (ProcessName == msedge && !ExistProcess(cs2))
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
                    if (ProcessName == msedge && !is_douyin())
                    {
                        press(PageUp);
                        break;
                    }
                    press(Up);
                    break;
                case Keys.Down:
                    //job process name + title judge
                    if (ProcessName == msedge && !is_douyin())
                    {
                        press(PageDown);
                        break;
                    }
                    press(Down);
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
