using static keyupMusic2.Common;
using KeyEvent = keyupMusic2.MouseKeyboardHook.KeyEventArgs;

namespace keyupMusic2
{
    public class ChromeClass : Default
    {
        public static Keys[] judge_handled_key = {  Keys.F1, };
        public override bool judge_handled(KeyEvent e)
        {
            if (Common.ProcessName != chrome) return false;
            if (judge_handled_key.Contains(e.key)) return true;
            return false;
        }
        public void handlehandle(KeyEvent e)
        {
            if (Common.ProcessName != chrome) return;
            pre_handling(e);
            do_handling(e);
            fin_handling(e);
        }
        public override void do_handling(KeyEvent e)
        {
            switch (e.key)
            {
                //case Keys.F2:
                //    CenterWindowOnScreen2(chrome, true);
                //    break;
                case Keys.PageUp:
                    LossScale();
                    break;
                //case Keys.Z:
                //    SS().KeyDown(Keys.X);
                //    break;
                //case Keys.VolumeDown:
                //    if (Position.Y == 0 || Position.X == 0 || Position.X == 6719) { press(Keys.Left); deal(); }
                //    break;
                //case Keys.VolumeUp:
                //    if (Position.Y == 0 || Position.X == 0 || Position.X == 6719) { press(Keys.Right); deal(); }
                //    break;
                //case Keys.F:
                //    if (Position.Y == 1619)
                //        copy_secoed_screen();
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
