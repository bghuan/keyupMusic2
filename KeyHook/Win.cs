using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public class WinClass
    {
        public WinClass(Form parentForm)
        {
            huan = (Huan)parentForm;
        }
        public static Huan huan;
        public static List<Keys> keys = new List<Keys> { Q, Left, Right,W };
        public static bool judge_handled(KeyboardHookEventArgs e)
        {
            if (!is_down(Keys.LWin)) return false;
            if (keys.Contains(e.key)) return true;
            return false;
        }
        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            if (!is_down(Keys.LWin)) return;

            switch (e.key)
            {
                case Keys.Q:
                    run_chrome();
                    break;
                case Keys.Right:
                case Keys.Left:
                    int arraw = e.key == Keys.Left ? 2 : 1;
                    quick_left_right(arraw);
                    break;
            }

        }
        private static void run_chrome()
        {
            //if (!is_ctrl()) if (Common.FocusProcess(Common.chrome)) return;
            //press("LWin;chrome;Enter;", 100);
            //press(100, Keys.LWin);
            press(100, LWin, "chrome", "en", Enter);
            //SS().KeyPress(Keys.LWin)
            //    .KeyPress("chrome")
            //    .KeyPress(Keys.Enter);
        }
    }
}
