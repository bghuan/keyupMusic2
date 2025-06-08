using System;
using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public partial class Huan
    {
        public static bool quick_replace_key(KeyboardHookEventArgs e)
        {
            for (int i = 0; i < replace.Count; i++)
            {
                if (ProcessName == replace[i].process && e.key == replace[i].defore)
                {
                    if (e.Type == KeyboardType.KeyDown) down_press(replace[i].after);
                    else up_press(replace[i].after);
                    return true;
                }
            }
            return false;
        }

        public static List<ReplaceKey> replace = new List<ReplaceKey> {
           new ReplaceKey( steam,Keys.D7,Keys.D8),
           new ReplaceKey( steam,Keys.D8,Keys.D7),
           new ReplaceKey( Windblown,Keys.W,Keys.S),
           new ReplaceKey( Windblown,Keys.S,Keys.W),
        };
    }
    public class ReplaceKey
    {
        public ReplaceKey(string key, Keys defore, Keys after) { this.process = key; this.defore = defore; this.after = after; }
        public string process;
        public Keys defore;
        public Keys after;
    }
}
