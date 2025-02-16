using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public class douyin : Default
    {
        public douyin(Form parentForm)
        {
            huan = (Huan)parentForm;
        }
        public douyin()
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
            not_in_class = ProcessName != ClassName()
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
                case Keys.F4:
                    press_close();
                    break;
                case Keys.LControlKey:
                    {
                        var k = "douyin_space";
                        if (KeyTime.ContainsKey(k) && DateTime.Now - KeyTime[k] < TimeSpan.FromMilliseconds(300))
                        {
                            mouse_click();
                        }

                        KeyTime[k] = DateTime.Now;
                    }
                    break;
            }
            //douyin_game_key(e, is_string_cmd);
            //if (e.key == Keys.Right)
            //{
            //    if (e.X < 800)
            //    {
            //        press(Keys.X);
            //    }
            //    if (e.X < 1600) { }
            //    if (e.X < 2600) { }
            //}

            Common.hooked = false;
            if (!handling) handling = true;
        }

        private void douyin_game_key(KeyboardHookEventArgs e, bool is_string_cmd)
        {
            switch (e.key)
            {
                case Keys.F1:
                    press(Keys.Enter);
                    break;
                case Keys.F2:
                    Special_Input2 = !Special_Input2;
                    Special_Input_tiem = DateTime.Now;
                    play_sound_di();
                    break;

                case Keys.F4:
                    send_input("揭竿而起");
                    break;
                case Keys.F5:
                    send_input("全军出击");
                    break;
                case Keys.F6:
                    send_input("休养生息");
                    break;

                case Keys.Oem3:
                    if (is_string_cmd) { raw_press(); break; }
                    else if (!is_shift()) send_input("x", false); break;

                case Keys.D1:
                    if (is_string_cmd) send_input("隔山打牛"); break;//远
                case Keys.Q:
                    if (is_string_cmd) send_input("雷霆万钧"); break;//         2

                case Keys.D2:
                    if (is_string_cmd) send_input("兵不厌诈"); break;//+2       pick
                case Keys.W:
                    if (is_string_cmd) send_input("无中生有"); break;//         1

                case Keys.D3:
                    if (is_string_cmd) send_input("勇冠三军"); break;//+2
                case Keys.E:
                    if (is_string_cmd) send_input("如影随形"); break;//         3

                case Keys.D4:
                    if (is_string_cmd) send_input("固若金汤"); break;//+3
                case Keys.R:
                    if (is_string_cmd) send_input("溃不成军"); break;//no

                case Keys.D5:
                    if (is_string_cmd) send_input("救死扶伤"); break;//铁
                case Keys.T:
                    if (is_string_cmd) send_input("殃及池鱼"); break;//         01

                case Keys.D6:
                    if (is_string_cmd) send_input("急速冷却"); break;//辅
                case Keys.Y:
                    if (is_string_cmd) send_input("调兵遣将"); break;//no

                case Keys.D7:
                    if (is_string_cmd) send_input("指鹿为马"); break;//+3       pick
                case Keys.U:
                    if (is_string_cmd) send_input("偷梁换柱"); break;//no

                case Keys.D8:
                    if (is_string_cmd) send_input("破釜沉舟"); break;//逆风

                case Keys.D9:
                    if (is_string_cmd) send_input("割地称臣"); break;//逆风


                case Keys.D0:
                    if (is_string_cmd) send_input("招贤纳士"); break;//

                case Keys.C:
                    if (is_string_cmd) send_input("水淹七军"); break;//
                case Keys.Z:
                    if (is_string_cmd) send_input("战无不胜"); break;//
                case Keys.X:
                    if (is_string_cmd) send_input("殃及池鱼"); break;//
                case Keys.V:
                    if (is_string_cmd) send_input("神之守护"); break;//
            }
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
            press("2290.1400;2290," + (1030 + (num * 50)), 101);
        }

        static string last_clip = "";
        private void send_input(string txt, bool click_input = true)
        {
            Special_Input_tiem = init_time;
            play_sound_di();
            if (last_clip != txt)
                Invoke(() => Clipboard.SetText(txt));
            last_clip = txt;

            if (is_ctrl())
            {
                string url = "https://bghuan.cn/api/save.php/?namespace=douyin_game&format=string&str=" + txt;
                HttpGet(url);
                return;
            }

            var old_pos = Position;
            if (click_input)
            {
                press("2220,1385", 10);
                press([Keys.LControlKey, Keys.A]);
                press([Keys.Back]);
            }
            press([Keys.LControlKey, Keys.V]);
            press(old_pos.X + "." + old_pos.Y, 0);
        }
        public void Invoke(Action action)
        {
            huan.Invoke(action);
        }
    }
}