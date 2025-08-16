using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;

namespace keyupMusic2
{
    public class DouyinClass : Default
    {
        bool signle = true;
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
            //if (!handling) return;
            handling_keys = e.key;
            switch (e.key)
            {
                case Keys.PageDown:
                case Keys.Right:
                case Keys.PageUp:
                case Keys.Left:
                    click_double_speed(e, num1222);
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
            //if (judge_color(2469, 646, Color.FromArgb(254, 44, 85)) 
            //    || judge_color(1996, 1400, Color.FromArgb(254, 21, 89))
            //    || judge_color(1996, 1400, Color.FromArgb(117, 46, 66)))
            int x = 2345;
            x -= 50;
            //x -= 50;
            press($"{x},1409;{x}," + (1030 + (num * 50)), 101);
        }
    }
}