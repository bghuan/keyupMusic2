using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public partial class Super
    {
        public static bool judge_handled(KeyboardHookEventArgs e)
        {
            if (is_down(Keys.LWin) && e.key == Keys.Q) return true;
            return false;
        }
        public void hook(KeyboardHookEventArgs e)
        {
            if (!is_down(Keys.LWin)) return;
            Common.hooked = true;

            switch (e.key)
            {
                case Keys.Q:
                    run_chrome();
                    break;
            }
            Common.hooked = false;
        }
    }
}
