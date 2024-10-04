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
        bool signle = true;
        public static Keys[] judge_handled_key = { Keys.X, Keys.H, };
        int num;
        int num1222 = 2;
        public override bool judge_handled(KeyboardHookEventArgs e)
        {
            if (Common.ProcessName != ClassName()) return false;
            //if (judge_handled_key.Contains(e.key)) return true;
            return false;
        }
        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            string module_name = ProcessName;
            if (module_name != ClassName() && module_name != Common.msedge) return;
            if (is_down(Keys.LWin)) return;
            //if (!handling) return;
            Common.hooked = true;
            handling_keys = e.key;

            switch (e.key)
            {
                //case Keys.Right:
                case Keys.PageUp:
                    if (Position.Y == 0 && Position.X == 2559) { break; }
                    if (Position.Y == 0 && Position.X != 2559) { press(Keys.VolumeUp); press(Keys.VolumeUp); press(Keys.VolumeUp); press(Keys.VolumeUp); break; }
                    Common.share();
                    if (share_string == "WM_LBUTTONDOWN") { press(Keys.VolumeUp); break; }
                    if (share_string == "WM_RBUTTONDOWN") { press(Keys.VolumeUp); break; }
                    if (module_name == ClassName())
                    {
                        handling = false;
                        press_dump(e.key, 210);
                        press_dump(e.key, 210);
                        Thread.Sleep(10);
                        break;
                    }
                    raw_press();
                    break;
                //case Keys.Left:
                case Keys.PageDown:
                    if (Position.Y == 0 && Position.X != 2559) { press(Keys.VolumeDown); press(Keys.VolumeDown); press(Keys.VolumeDown); press(Keys.VolumeDown); break; }
                    raw_press();
                    break;
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
                case Keys.Oem3:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                    if (module_name != ClassName()) break;
                    num = int.Parse(e.key.ToString().Replace("D", "").Replace("Oem3", "0"));
                    press("2236.1400;2226," + (1030 + (num * 50)), 101);
                    break;
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
                case Keys.LControlKey:
                    if (module_name == Common.douyin)
                        press_middle_bottom();
                    break;
            }
            Common.hooked = false;
            if (!handling) handling = true;
        }

    }
}
