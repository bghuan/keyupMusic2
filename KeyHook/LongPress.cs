using System.Drawing.Imaging;
using System.Text;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class LongPressClass
    {
        public static int long_press_tick = 300;
        public static int long_press_tick2 = 1500;
        public static int long_press_tick3 = 4000;
        public static bool long_press_lbutton = false;
        public static bool long_press_rbutton = false;
        public static void Every100msHandler()
        {
            //PrintDictSortedByValueDesc();
            if (Huan.handling_keys.Count == 0) return;

            var handling_keys = Huan.handling_keys;
            //if (is_steam_game()) return;
            foreach (var key in handling_keys)
            {
                if (DateTime.Now - key.Value > TimeSpan.FromMilliseconds(LongPressClass.long_press_tick))
                {
                    deal(key.Key);
                    handling_keys[key.Key] = DateTime.Now.AddDays(1);
                }
                if (key.Value.Day == DateTime.Now.AddDays(1).Day && DateTime.Now.AddDays(1) - key.Value > TimeSpan.FromMilliseconds(long_press_tick2))
                {
                    deal2(key.Key);
                    handling_keys[key.Key] = DateTime.Now.AddDays(2);
                }
                if (key.Value.Day == DateTime.Now.AddDays(2).Day && DateTime.Now.AddDays(2) - key.Value > TimeSpan.FromMilliseconds(long_press_tick3 - long_press_tick2))
                {
                    deal3(key.Key);
                    handling_keys[key.Key] = DateTime.Now.AddDays(3);
                }
            }
        }
        public static void deal(Keys keys)
        {
            //play_sound_di();
            huan.Invoke2(() => { huan.label1.Text = ProcessName + " longpress " + keys; });

            if (keys == Keys.F3)
                huan.Invoke(() => { if (!huan.Visible) huan.Show(); else huan.Hide(); });
            else if (keys == Keys.F4 && !lock_err)
                CloseProcess();
            else if (keys == Keys.F9)
                huan.system_sleep(true);

            else if (keys == Keys.Home || keys == Keys.End)
            {
                try { Common.bmpScreenshot?.Save(Common.bmpScreenshot_path, ImageFormat.Png); }
                catch (Exception ex) { }
                play_sound_di();
                //bmpScreenshot.Dispose();
            }
            else if (keys == Keys.Escape)
                HideSomething();

            else if (keys == Keys.LButton)
                long_press_lbutton = true;
            else if (keys == Keys.RButton)
                long_press_rbutton = true;

            if (IsDesktopFocused())
                play_sound_di();

            KeyFunc.LongPressFlag(keys);

            LongPressKey = keys;
        }
        public static void deal2(Keys keys)
        {
            //play_sound_di2();
            huan.Invoke2(() => { huan.label1.Text = ProcessName + " longlongpress " + keys; });

            if (keys == Keys.RButton)
            {
                play_sound_di2();
                Common.no_move = true;
            }

            if (keys == Keys.Escape)
            {
                play_sound_di2();
                huan._mouseKbdHook.ChangeMouseHooks();
            }

            if (keys == Keys.Delete)
            {
                play_sound_di2();
                var keysA = new List<Keys>();
                for (int i = 0; i < 256; i++)
                    keysA.Add((Keys)i);
                huan.Invoke(() => { huan.label1.Text = "all key up " + keysA.Count + keysA.ToString(); });
                Sleep(200);
                foreach (var key in keysA)
                {
                    if (key == Keys.Apps) continue;
                    //if (is_down(Keys.CapsLock)) return true;
                    huan.Invoke(() => { huan.label1.Text = ProcessName + " " + key.ToString(); });
                    SS(10).KeyUp(key);
                }
            }
        }
        public static void deal3(Keys keys)
        {
            //play_sound_di2();
            huan.Invoke2(() => { huan.label1.Text = ProcessName + " longlonglongpress " + keys; });

            if (keys == Keys.Escape)
            {
                play_sound_di2();
                Environment.Exit(0);
            }

            if (keys == LWin) return;
            VirtualKeyboardForm.Instance?.TriggerKey(keys, true);
        }

    }
}
