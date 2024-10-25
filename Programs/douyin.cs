﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;

using static keyupMusic2.Common;
using System.Media;
using System.Windows.Forms;

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
            bool is_string_cmd = Special_Input && is_douyin_live_and_input();
            is_string_cmd = Special_Input2;

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
                case Keys.F1:
                    zan = !zan;
                    Task.Run(() =>
                    {
                        while (zan)
                        {
                            int tick = 220 + new Random().Next(1, 6);
                            mouse_click2(tick);
                            if (FreshProcessName() != ClassName()) zan = false;
                        }
                    });
                    break;
                //case Keys.F1:
                //    var aaa = Position;
                //    press("2220,1385", 100);
                //    press([Keys.LControlKey, Keys.V]);
                //    press([Keys.Enter]);
                //    press(aaa.X + "." + aaa.Y, 100);
                //    break;
                case Keys.F2:
                    Special_Input2 = !Special_Input2;
                    if (Special_Input2) play_sound_di();
                    break;
                case Keys.F4:
                    send_input("揭竿而起");
                    break;
                case Keys.F5:
                    send_input("全军出击");
                    //send_input("隔山打牛");
                    break;
                case Keys.F6:
                    send_input("休养生息");
                    //send_input("勇冠三军");
                    break;
                case Keys.D1:
                    if (is_string_cmd) send_input("隔山打牛"); break;
                case Keys.D2:
                    if (is_string_cmd) send_input("兵不厌诈"); break;
                case Keys.D3:
                    if (is_string_cmd) send_input("勇冠三军"); break;
                case Keys.D4:
                    if (is_string_cmd) send_input("固若金汤"); break;
                case Keys.D5:
                    if (is_string_cmd) send_input("破釜沉舟"); break;
                case Keys.D6:
                    if (is_string_cmd) send_input("急速冷却"); break;
                case Keys.D7:
                    if (is_string_cmd) send_input("招贤纳士"); break;
                case Keys.D8:
                    if (is_string_cmd) send_input("战无不胜"); break;
                case Keys.D9:
                    if (is_string_cmd) send_input("天降神兵"); break;
                case Keys.D0:
                    if (is_string_cmd) send_input("极寒领域"); break;
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

        private void play_sound_di()
        {
            string wav = "wav\\d.wav";
            if (!File.Exists(wav)) return;

            player = new SoundPlayer(wav);
            player.Play();
        }
        private void send_input(string txt)
        {
            play_sound_di();
            Invoke(() => Clipboard.SetText(txt));

            if (!is_douyin_live_and_input()) return;
            if (is_ctrl()) return;

            var old_pos = Position;
            press("2220,1385", 10);
            press([Keys.LControlKey, Keys.A]);
            press([Keys.Back]);
            press([Keys.LControlKey, Keys.V]);
            //press("2519.1384", 10);
            //press([Keys.Enter]);
            press(old_pos.X + "." + old_pos.Y, 0);
        }

        private static bool is_douyin_live_and_input()
        {
            if (!judge_color(2030, 1209, Color.FromArgb(37, 38, 50), null, 10)) return false;
            if (!judge_color(2295, 1383, Color.FromArgb(51, 52, 63), null, 10)) return false;
            return true;
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