using static keyupMusic2.Common;
using KeyEvent = keyupMusic2.KeyboardMouseHook.KeyEventArgs;

namespace keyupMusic2
{
    public class ChromeClass : Default
    {
        public static Keys[] judge_handled_key = {  Keys.F1, };
        public override bool judge_handled(KeyEvent e)
        {
            if (Common.ProcessName != chrome) return false;
            if (is_douyin()) return false;
            if (judge_handled_key.Contains(e.key)) return true;
            return false;
        }
        public void handlehandle(KeyEvent e)
        {
            if (Common.ProcessName != chrome) return;
            if (is_douyin()) return;
            pre_handling(e);
            do_handling(e);
            fin_handling(e);
        }
        public override void do_handling(KeyEvent e)
        {
            if (is_douyin()) return;
            switch (e.key)
            {
                case Keys.PageUp:
                    LossScale();
                    break;
                case Keys.PageDown:
                    SetTransparency();
                    break;
            }
        }
    }
}
