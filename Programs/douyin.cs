using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;

using static keyupMusic2.Common;

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
        public override bool judge_handled(KeyboardHookEventArgs e)
        {
            if (Common.ProcessName != ClassName()) return false;
            //if (judge_handled_key.Contains(e.key)) return true;
            if (is_ctrl())
            {
                if (e.key == Keys.Left || e.key == Keys.Right || e.key == Keys.Enter)
                    return true;
            }
            return false;
        }
        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            string module_name = ProcessName;
            if (module_name != ClassName()) return;
            //if (module_name != ClassName() && module_name != Common.msedge) return;
            if (is_down(Keys.LWin)) return;
            //if (!handling) return;
            Common.hooked = true;
            handling_keys = e.key;

            switch (e.key)
            {
                case Keys.PageUp:
                    if (Position.Y == 0 && Position.X == 2559) { break; }
                    if (Position.Y == 0 && Position.X != 2559) { press(Keys.VolumeUp); press(Keys.VolumeUp); press(Keys.VolumeUp); press(Keys.VolumeUp); break; }
                    if (module_name == ClassName())
                    {
                        if (num1222 <= 3 && num1222 > 1) num1222--;
                        if (num1222 == 2) num = 5;
                        else if (num1222 == 3) num = 6;
                        else num = 1;
                        press("2236.1400;2226," + (1030 + (num * 50)), 101);
                        break;
                    }
                    raw_press();
                    break;
                case Keys.PageDown:
                    if (Position.Y == 0 && Position.X != 2559) { press(Keys.VolumeDown); press(Keys.VolumeDown); press(Keys.VolumeDown); press(Keys.VolumeDown); break; }
                    if (module_name == ClassName())
                    {
                        if (num1222 < 3 && num1222 >= 1) num1222++;
                        if (num1222 == 2) num = 5;
                        else if (num1222 == 3) num = 6;
                        else num = 1;
                        press("2236.1400;2226," + (1030 + (num * 50)), 101);
                        break;
                    }
                    raw_press();
                    break;
                //case Keys.Oem3:
                //case Keys.D1:
                //case Keys.D2:
                //case Keys.D3:
                //case Keys.D4:
                //case Keys.D5:
                //case Keys.D6:
                //    if (module_name != ClassName()) break;
                //    num = int.Parse(e.key.ToString().Replace("D", "").Replace("Oem3", "0"));
                //    press("2236.1400;2226," + (1030 + (num * 50)), 101);
                //    break;
                //case Keys.LControlKey:
                //    if (module_name == Common.douyin)
                //        press_middle_bottom();
                //    break;
                //case Keys.D3:
                //case Keys.D4:
                //case Keys.D5:
                //case Keys.D6:
                //    if (module_name != ClassName()) break;
                //    num = int.Parse(e.key.ToString().Replace("D", "").Replace("Oem3", "0"));
                //    press("2236.1400;2226," + (1030 + (num * 50)), 101);
                //    break;
                case Keys.Right:
                    //case Keys.D:
                    if (!is_ctrl()) break;
                    if (num1222 < 3 && num1222 >= 1) num1222++;
                    if (num1222 == 2) num = 5;
                    else if (num1222 == 3) num = 6;
                    else num = 1;
                    press("2236.1400;2226," + (1030 + (num * 50)), 101);
                    break;
                case Keys.Left:
                    //case Keys.A:
                    if (!is_ctrl()) break;
                    if (num1222 <= 3 && num1222 > 1) num1222--;
                    if (num1222 == 2) num = 5;
                    else if (num1222 == 3) num = 6;
                    else num = 1;
                    press("2236.1400;2226," + (1030 + (num * 50)), 101);
                    break;
                //case Keys.F5:
                //    zan = !zan;
                //    Task.Run(() =>
                //    {
                //        while (zan)
                //        {
                //            int tick = 20 + new Random().Next(1, 6);
                //            mouse_click(tick);
                //            if (yo() != ClassName()) zan = false;
                //        }
                //    });
                //    break;
                case Keys.F1:
                    var aaa = Position;
                    press("2220,1385", 100);
                    press([Keys.LControlKey, Keys.V]);
                    press([Keys.Enter]);
                    press(aaa.X + "." + aaa.Y, 100);
                    break;
                //case Keys.F2:
                //case Keys.F5:
                //    string txt = "攻击";
                //    if (is_ctrl()) txt = "全军出击";
                //    Invoke(() => Clipboard.SetText(txt));
                //    press([Keys.LControlKey, Keys.V]);
                //    break;
                //case Keys.F6:
                //    Invoke(() => Clipboard.SetText("增援"));
                //    press([Keys.LControlKey, Keys.V]);
                //    break;
                case Keys.Enter:
                    if (!is_ctrl()) break;
                    string old_clipboard = "";
                    Invoke(() => old_clipboard = Clipboard.GetText());
                    press([Keys.LControlKey, Keys.A]);
                    press([Keys.LControlKey, Keys.C], 100);
                    Invoke(() =>
                    {
                        string curr_clipboard = Clipboard.GetText();
                        //bool blank = "" == curr_clipboard || old_clipboard == curr_clipboard;
                        //if ("" == curr_clipboard) Clipboard.SetText(old_clipboard);
                        if (old_clipboard == curr_clipboard)
                            press([Keys.LControlKey, Keys.V], 100);
                    });
                    press([Keys.Enter]);
                    break;
            }
            Common.hooked = false;
            if (!handling) handling = true;
        }
        bool zan = false;
        public void Invoke(Action action)
        {
            huan.Invoke(action);
        }
    }
}

//case Keys.X:
//    //if (Position.X == 0)
//    //{
//    //    num1222++;
//    //    num = num1222;
//    //    press("2236.1400;2226," + (1030 + (num * 50)), 101);
//    //    break;
//    //}
//    raw_press2();
//    break;
//case Keys.H:
//    if (Position.X == 0)
//    {
//        num1222--;
//        num = num1222;
//        press("2236.1400;2226," + (1030 + (num * 50)), 101);
//        break;
//    }
//    raw_press();
//    break;
//case Keys.VolumeDown:
//    if (special_delete_key_time.AddSeconds(2) > DateTime.Now)
//    {
//        press(Keys.VolumeDown);
//        special_delete_key_time = DateTime.Now;
//        break;
//    }
//    if (Position.Y == 0 && Position.X != 2559) { press(Keys.VolumeDown); break; }
//    //if (signle)
//        press(Keys.PageDown);
//    //signle = !signle;
//    break;
//case Keys.VolumeUp:
//    if (special_delete_key_time.AddSeconds(2) > DateTime.Now)
//    {
//        press(Keys.VolumeUp);
//        special_delete_key_time = DateTime.Now;
//        break;
//    }
//    if (Position.Y == 0 && Position.X != 2559) { press(Keys.VolumeUp); break; }
//    press(Keys.PageUp);
//    break;