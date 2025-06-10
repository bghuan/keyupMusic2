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
        public static List<Keys> keys = new List<Keys> { Q, Left, Right, W };
        public static bool handling = false;
        public static bool judge_handled(KeyboardHookEventArgs e)
        {
            if (!is_down(Keys.LWin)) return false;
            if (keys.Contains(e.key)) return true;
            return false;
        }
        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            if (!is_down(Keys.LWin)) return;
            // 发送无效 Win + Key 事件,使当前 Win 键无效
            if (keys.Contains(e.key)) press(Keys.LControlKey);
            bool catched = true;

            switch (e.key)
            {
                case Keys.Q:
                    run_chrome();
                    break;
                case Keys.Right:
                case Keys.Left:
                    int arraw = e.key == Keys.Left ? 1 : 2;
                    quick_left_right(arraw);
                    break;
                default: catched = false; break;
            }
        }
        private static void run_chrome()
        {
            press(100, LWin, "chrome", "en", Enter);
        }
    }
}
