using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;

using static keyupMusic2.Common;
using System.Diagnostics;

namespace keyupMusic2
{
    public class BBB : Default
    {
        public void hook_KeyDown_ddzzq(KeyboardHookEventArgs e)
        {
            string module_name = ProcessName;
            Common.hooked = true;
            handling_keys = e.key;
            bool right_top = Position.Y == 0 && Position.X == 2559;
            //if (!handling) return;

            switch (e.key)
            {
                case Keys.D1:
                    if (!is_down(Keys.LWin))
                    {
                        raw_press();
                        break;
                    }
                    if (Common.devenv == module_name)
                    {
                        HideProcess(module_name);
                    }
                    else if (!Common.FocusProcess(Common.devenv))
                    {
                        if (Common.FocusProcess(Common.devenv)) break;
                        press("LWin;VIS;Apps;100;Enter;", 100);
                        //TaskRun(() => { press("Tab;Down;Enter;", 100); }, 1500);
                        DaleyRun(() =>
                        {
                            var flag = judge_color(519, 717, Color.FromArgb(115, 97, 236), null, 10)
                                    && judge_color(571, 460, Color.FromArgb(250, 250, 250));
                            return flag;
                        }, 3000, 100);
                        press("Tab;Down;Enter;", 100);
                    }
                    break;
                case Keys.D2:
                    if (is_down(Keys.LWin))
                    {
                        if (!Common.FocusProcess(Common.douyin))
                        {
                            press(Keys.MediaStop);
                            ProcessRun(douyinexe);
                            DaleyRun(() => { return judge_color(2318, 1258, Color.FromArgb(111, 112, 120), null, 10); }, 3000, 100);
                            press("311, 1116", 0);
                            mouse_click(11);
                            press("2227, 1245", 0);
                            mouse_click(11);
                            press("1333.1444", 0);
                        }
                        break;
                    }
                    raw_press();
                    break;
            }

            switch (e.key)
            {
                case Keys.Home:
                    copy_screen(); break;
                case Keys.End:
                    copy_secoed_screen(); break;
                case Keys.Left:
                    if (right_top) press(Keys.F7); break;
                case Keys.PageDown:
                    if (right_top) press(Keys.F7); break;
                case Keys.Right:
                    if (right_top) press(Keys.F8); break;
                case Keys.PageUp:
                    if (right_top) press(Keys.F8); break;
                //case Keys.VolumeDown:
                //    if (right_top) { press(Keys.F7); break; } else { raw_press(); break; }
                //case Keys.VolumeUp:
                //    if (right_top) { press(Keys.F8); break; } else { raw_press(); break; }
                case Keys.Delete:
                    DaleyRun_stop = true; special_delete_key_time = DateTime.Now; player.Stop(); break;
                case Keys.LMenu:
                    yo(); break;
                case Keys.Escape:
                    player.Stop(); break;
            }

            Common.hooked = false;
        }

    }
}
