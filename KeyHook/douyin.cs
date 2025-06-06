using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public class DouyinClass : Default
    {
        public DouyinClass(Form parentForm)
        {
            huan = (Huan)parentForm;
        }
        public DouyinClass()
        {
        }
        public static Huan huan;
        bool signle = true;
        public static Keys[] judge_handled_key = { Keys.X, Keys.H, };
        int num;
        int num1222 = 1;
        bool not_in_class = false;
        public override bool judge_handled(KeyboardHookEventArgs e)
        {
            not_in_class = ProcessName != douyin
                && (ProcessName != ApplicationFrameHost || ProcessTitle?.IndexOf("照片") < 0)
                && ProcessTitle?.IndexOf("抖音") < 0
                //&& (ProcessName == msedge && ProcessTitle?.IndexOf("多多自走棋") < 0)
                ;
            if (not_in_class) return false;
            //if (judge_handled_key.Contains(e.key)) return true;
            if (is_down(Keys.F2)) return true;
            if (e.key == Keys.F1) return true;
            if (e.key == Keys.F2) return true;
            if (e.key == Keys.F4) return true;
            if (e.key == Keys.F5) return true;
            if (e.key == Keys.F6) return true;
            if (e.key == Keys.Oem3 && !is_shift()) return true;
            if ((Special_Input_tiem != init_time && Special_Input_tiem.AddMilliseconds(1000) > DateTime.Now)) return true;
            if (is_ctrl())
            {
                if (e.key == Keys.Left || e.key == Keys.Right)
                    return true;
            }
            return false;
        }
        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            string module_name = ProcessName;
            if (not_in_class) return;
            if (is_down(Keys.LWin)) return;
            if (e.Handled && e.key == Keys.PageDown) return;
            //if (!handling) return;
            Common.hooked = true;
            handling_keys = e.key;
            bool is_string_cmd = (Special_Input_tiem != init_time && Special_Input_tiem.AddMilliseconds(1000) > DateTime.Now) || Special_Input;
            switch (e.key)
            {
                case Keys.PageDown:
                case Keys.Right:
                case Keys.PageUp:
                case Keys.Left:
                    click_double_speed(e, num1222);
                    break;
            }

            Common.hooked = false;
            if (!handling) handling = true;
        }
        private void click_double_speed(KeyboardHookEventArgs e, int num1222)
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
            press("2345,1409;2345," + (1030 + (num * 50)), 101);
        }
    }
}