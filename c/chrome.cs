﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;
using static keyupMusic2.Common;
using KeyEvent = WGestures.Core.Impl.Windows.MouseKeyboardHook.KeyboardHookEventArgs;

namespace keyupMusic2
{
    public class Chrome : Default
    {
        public static string ClassName = "chrome";
        //public chrome()
        //{
        //    Common.list2.Add(ClassName);
        //}
        public static Keys[] judge_handled_key = { Keys.VolumeDown, Keys.VolumeUp, };
        public override bool judge_handled(KeyEvent e)
        {
            if (Common.ProcessName != ClassName) return false;
            if (judge_handled_key.Contains(e.key)) return true;
            return false;
        }
        public void handlehandle(KeyEvent e)
        {
            if (Common.ProcessName != ClassName) return;
            pre_handling(e);
            do_handling(e);
            fin_handling(e);
        }
        public override void do_handling(KeyEvent e)
        {
            switch (e.key)
            {
                case Keys.VolumeDown:
                    if (Position.Y == 0 || Position.X == 0 || Position.X == 6719) {press(Keys.Left); deal(); }
                    break;
                case Keys.VolumeUp:
                    if (Position.Y == 0 || Position.X == 0 || Position.X == 6719) { press(Keys.Right); deal(); }
                    break;
            }
            if (catched == false && judge_handled_key.Contains(e.key))
            {
                raw_press();
            }
        }
        //public void press(Keys keys) {
        //    catched = true;
        //    Common.press(keys);
        //}
        public void deal()
        {
            catched = true;
        }
    }
}
