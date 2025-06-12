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
                // 支持全局（process为空或null）或指定进程
                if (e.key == replace[i].defore && (string.IsNullOrEmpty(replace[i].process) || ProcessName == replace[i].process))
                {
                    if (e.Type == KeyboardType.KeyDown) down_press(replace[i].after, replace[i].raw);
                    else up_press(replace[i].after, replace[i].raw);
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
           new ReplaceKey(string.Empty, Keys.RMenu, Keys.RWin,true)
        };
    }
    public class ReplaceKey
    {
        public ReplaceKey(string key, Keys defore, Keys after, bool raw = false) { this.process = key; this.defore = defore; this.after = after; this.raw = raw; }
        public string process;
        public Keys defore;
        public Keys after;
        public bool raw;
    }
}
