using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;

namespace keyupMusic4
{
    public class DouyinClass : Default
    {
        public static Keys[] judge_handled_key = { Keys.X, Keys.H, };
        int num;
        int num1222 = 1;
        bool not_in_class = false;
        public override bool judge_handled(KeyboardMouseHook.KeyEventArgs e)
        {
            not_in_class = ProcessName != douyin
                && ProcessTitle?.IndexOf("抖音") < 0
                ;
            if (not_in_class) return false;
            return false;
        }
        public void HookEvent(KeyboardMouseHook.KeyEventArgs e)
        {
            string module_name = ProcessName;
            if (not_in_class) return;
            if (is_down(Keys.LWin)) return;
            if (e.Handled && e.key == Keys.PageDown) return;
            handling_keys = e.key;
            switch (e.key)
            {
                case Keys.PageDown:
                case Keys.Right:
                case Keys.PageUp:
                case Keys.Left:
                    click_double_speed(e, num1222);
                    break;
                case Keys.F1:
                    mouseback();
                    break;
            }
        }
        private void click_double_speed(KeyboardMouseHook.KeyEventArgs e, int num1222)
        {
            var a = new[] { Keys.Left, Keys.Right };
            var b = new[] { Keys.PageDown, Keys.Right };
            var c = new[] { Keys.PageUp, Keys.Left };
            if (a.Contains(e.key) && !is_ctrl()) return;
            if (b.Contains(e.key))
                if (num1222 < 3 && num1222 >= 1)
                    this.num1222++;
            if (c.Contains(e.key))
                if (num1222 <= 3 && num1222 > 1)
                    this.num1222--;

            num1222 = this.num1222;
            if (num1222 == 2) num = 5;
            else if (num1222 == 3) num = 6;
            else num = 1;
            int x = 2345;
            x -= 50;
            x -= 50;
            press($"{x},1409;{x}," + (1025 + (num * 50)), 101);
        }
    }
}