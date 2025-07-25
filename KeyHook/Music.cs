﻿using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public class Music
    {
        static Keys[] keys = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0, Keys.PageUp, Keys.Home, Keys.End };
        static Keys last_key_sound = new Keys();
        static DateTime last_key_sound_time = DateTime.Now.AddDays(-1);
        static DateTime last_key_sound_time2 = DateTime.Now.AddDays(-1);
        static Dictionary<string, Keys[]> expect = new Dictionary<string, Keys[]> {{
                Common.cs2,
                    [Keys.D3, Keys.D4]},{
                Common.msedge,
                    [Keys.Next, Keys.Space, Keys.D0]},{
                Common.douyin,
                    [Keys.Space, Keys.D0]
        }};

        public static void hook_KeyDown_keyupMusic2(MouseKeyboardHook.KeyEventArgs e)
        {
            return;
            hook_KeyDown_keyupMusic2222(e);
            if (!key_sound) return;
            if (!keys.Contains(e.key)) return;
            if (expect.ContainsKey(ProcessName) && expect[ProcessName].Contains(e.key)) return;
            Common.hooked = true;

            if (last_key_sound == e.key)
            {
                if (last_key_sound_time.AddMilliseconds(500) > DateTime.Now)
                {
                    play_sound(e.key);
                    last_key_sound_time2 = DateTime.Now.AddMilliseconds(1200);
                }
                else if (last_key_sound_time2 > DateTime.Now)
                {
                    player.Stop();
                    last_key_sound_time2 = DateTime.Now;
                }

            }

            last_key_sound = e.key;
            last_key_sound_time = DateTime.Now;
            Common.hooked = false;
        }
        static Keys[] keys2 = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0 };
        public static void hook_KeyDown_keyupMusic2222(MouseKeyboardHook.KeyEventArgs e)
        {
            if (!keys2.Contains(e.key)) return;
            if (Common.hooked) return;

            play_sound_bongocat(e.key);
        }
    }
}
