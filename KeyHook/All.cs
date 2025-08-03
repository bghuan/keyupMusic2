using System.Drawing;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace keyupMusic2
{
    public class AllClass : Default
    {
        public void hook_KeyDown_ddzzq(KeyboardMouseHook.KeyEventArgs e)
        {
            string module_name = ProcessName;
            Common.hooked = true;
            handling_keys = e.key;
            bool right_top = Position.Y == 0 && Position.X == 2559;
            //if (!handling) return;

            quick_number(e);

            switch (e.key)
            {
                //case Keys.F1:
                //    SuperClass.get_point_color(); break;
                //case Keys.F2:
                //    quick_scale(); break;
                case Keys.F4:
                    quick_close(); quick_sleep(); break;
                //case Keys.F10:
                //    quick_what(); break;
                //case Keys.F11:
                //    quick_visiualstudio(); break;
                //case Keys.F12:
                //    quick_wechat_or_notify(); break;

                case Keys.Home:
                    copy_screen(); break;
                case Keys.End:
                    copy_secoed_screen(); break;
                case Keys.PageDown:
                    quick_gamg_alttab(e, module_name); break;

                case Keys.Delete:
                    clean(); DeleteCurrentWallpaper(); break;
                case Keys.Escape:
                    clean(); break;
                case Keys.LShiftKey:
                    shift(); break;

                case Keys.Up:
                    quick_next_image(); break;
                case Keys.Down:
                    quick_prix_image(); break;

                case Keys.OemPeriod:
                    if (is_down(Keys.RControlKey)) SS().KeyPress(Keys.Apps); break;
            }
            //if (Position.Y == 0)
            //    switch (e.key)
            //    {
            //        case Keys.Left:
            //        case Keys.Down:
            //        case Keys.Right:
            //            quick_number2(e.key); break;
            //    }

            Common.hooked = false;
        }

        public static void quick_scale()
        {
            //var asd = IsFullScreen();
            if (IsFullScreen())
            {
                LossScale();
            }
        }

        public void shift()
        {
            if (ProcessName == devenv) return;
            if (is_steam_game()) return;
            if (IsFullScreen()) return;
            if (!isctrl()) return;

            // 等待 Ctrl 和 Shift 键释放（超时 1 秒）
            if (!WaitForKeysReleased(1000, isctrl, is_shift))
                return;

            // 执行复制操作（Ctrl+A+C）
            press(new[] { Keys.LControlKey, Keys.A, Keys.C }, 50);

            // 获取剪贴板文本（在 UI 线程上执行）
            string clipboardText = GetClipboardText();
            if (string.IsNullOrEmpty(clipboardText))
                return;

            // 截断文本长度
            string processedText = clipboardText.Length > 20
                ? clipboardText.Substring(0, 20).ToUpper()
                : clipboardText.ToUpper();

            // 再次等待按键释放，然后按下 Shift 键
            //if (WaitForKeysReleased(1000, isctrl, is_shift))
            {
                press(new[] { Keys.LShiftKey }, 50);
                press(processedText);
            }
        }



        // 辅助方法：安全获取剪贴板文本
        public string GetClipboardText()
        {
            string result = "";
            try
            {
                huan.Invoke(new Action(() =>
                {
                    result = Clipboard.GetText() ?? "";
                    if (result.Length > 100) result = "";
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取剪贴板内容失败: {ex.Message}");
            }
            return result;
        }

        //static Dictionary<int, Keys[]> numkey = new Dictionary<int, Keys[]>()
        //{

        //};
        //var requestBody = new
        //{
        //    A1 = new Keys[] { Keys.Left },
        //    A2 = new Keys[] { Keys.Down },
        //};

        public void quick_prix_image()
        {
            if (!isctrl()) return;
            SetDesktopWallpaper(GetNextWallpaper(), WallpaperStyle.Fit, true);
        }

        public void quick_next_image()
        {
            if (!isctrl()) return;
            SetDesktopWallpaper(GetPreviousWallpaper(), WallpaperStyle.Fit, true);
        }

        public static void quick_what()
        {
            //if (TryFocusProcess(Common.cs2)) return;
            //if (TryFocusProcess(Common.SplitFiction)) return;
            ////if (TryFocusProcess(Common.steam)) break;
            //quick_max_chrome();
        }

        public static void quick_gamg_alttab(KeyboardMouseHook.KeyEventArgs e, string module_name)
        {
            var allow = is_steam_game() || module_name == chrome || module_name == PowerToysCropAndLock;
            if (!allow) return;
            quick_max_chrome(e.Pos);
        }

        public static void quick_sleep()
        {
            if (lock_err)
                system_hard_sleep();
        }

        public static void quick_close()
        {
            if (!lock_err)
                CloseProcess();
        }

        public static void quick_wechat_or_notify()
        {
            string module_name = ProcessName;
            if (is_down(Keys.Delete) || is_ctrl()) return;
            //if (ProcessName == Common.keyupMusic2)
            //{
            //    press(F12);
            //    return;
            //}
            if (Common.WeChat == module_name)
            {
                CloseProcess(module_name);
            }
            else if (GetWindowTitle() == UnlockingWindow || ProcessName == LockApp || ProcessName == err)
            {
                SuperClass.hook_KeyDown(Keys.N);
            }
            else
            {
                Common.FocusProcess(Common.WeChat);
                Thread.Sleep(10);
                if (ProcessName == Common.WeChat) return;
                run_wei();
            }
        }

        public static void quick_visiualstudio()
        {
            string module_name = ProcessName;
            if (is_down(Keys.Delete) || is_ctrl()) return;
            if (Common.devenv == module_name)
            {
                HideProcess(module_name);
            }
            else
            {
                if (Common.FocusProcessSimple(Common.devenv)) return;
                run_vis();
            }
        }

        public static void clean()
        {
            DaleyRun_stop = true;
            player.Stop();
            CleanMouseState();
            ready_to_sleep = false;
            system_sleep_count = 0;
        }

        private static void quick_go_back(KeyboardMouseHook.KeyEventArgs e)
        {
            {
                if (!biu.list_go_back.Contains(ProcessName)) return;

                if (e.key == Keys.MediaNextTrack && ProcessName == msedge)
                    press(Keys.Right);
                if (e.key == Keys.MediaNextTrack && ProcessName == chrome)
                    press(Keys.F);

                if (e.key == Keys.MediaNextTrack)
                    mousego();
                else
                    mouseback();
            }
        }

        private static void quick_number(KeyboardMouseHook.KeyEventArgs e)
        {
            if (is_down(Keys.F1))
                switch (e.key)
                {
                    case Keys.Oemcomma:
                        press(Keys.D1); break;
                    case Keys.OemPeriod:
                        press(Keys.D2); break;
                    case Keys.Oem2:
                        press(Keys.D3); break;
                    case Keys.K:
                        press(Keys.D4); break;
                    case Keys.L:
                        press(Keys.D5); break;
                    case Keys.OemSemicolon:
                        press(Keys.D6); break;
                    case Keys.I:
                        press(Keys.D7); break;
                    case Keys.O:
                        press(Keys.D8); break;
                    case Keys.P:
                        press(Keys.D9); break;
                    case Keys.Space:
                        press(Keys.D0); break;
                }
        }
        private static void run_wei()
        {
            if (!Common.ExistProcess(Common.WeChat))
            {
                press("LWin;100;WEI;en;100;Enter;", 50);
                return;
            }
            press([Keys.LControlKey, Keys.LMenu, Keys.W]);
        }

        public static void run_vis()
        {
            //press("LWin;VISUAL;en;100;Apps;100;Enter;1000;1271.654;", 100);
            ////DaleyRun(
            ////    () => GetPointTitle() == "Microsoft Visual Studio(管理员)",
            ////    () => press("100;Tab;Down;Enter;", 100),
            ////    3000, 200);
            //TaskRun(() => { press("Tab;Down;Enter;", 50); }, 800);
            ProcessRun(devenvexe, keyupMusicexe);
        }
    }
}
