using System.Drawing;
using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace keyupMusic4
{
    public class AllClass : Default
    {
        public void HookEvent(KeyboardMouseHook.KeyEventArgs e)
        {
            string module_name = ProcessName;
            handling_keys = e.key;
            bool right_top = Position.Y == 0 && Position.X == 2559;

            quick_number(e);

            switch (e.key)
            {

                case Keys.PageDown:
                    quick_gamg_alttab(e, module_name); break;

                case Keys.Delete:
                    clean(); /*DeleteCurrentWallpaper();*/ break;
                case Keys.Escape:
                    clean(); break;

                case Keys.Up:
                    if (is_tran_process == ProcessName) { SetTransparency(16); break; }
                    /*quick_next_image();*/ break;
                case Keys.Down:
                    if (is_tran_process == ProcessName) { SetTransparency(-16); break; }
                    /*quick_prix_image(); */break;

                case Keys.OemPeriod:
                    if (is_down(Keys.RControlKey)) { press_raw(Apps); break; }
                    break;
            }

        }

        public static void quick_scale()
        {
            if (ProcessName == cs2)
            {
                return;
            }
            if (!ProcessTitle.Contains("tudio") && IsFullScreen())
            {
                LossScale();
            }
            if (ProcessTitle == kmRead)
            {
                LossScale();
            }
            else if (lock_err)
            {
                quick_sleep();
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
                Console2.WriteLine($"获取剪贴板内容失败: {ex.Message}");
            }
            return result;
        }

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
            if (ProcessName == "msedgewebview2")
            {
                return;
            }
            if (Common.WeChat == module_name)
            {
                CloseProcess2(module_name);
            }
            else if (GetWindowTitle() == UnlockingWindow || ProcessName == LockApp || ProcessName == err)
            {
                SuperClass.hook_KeyDown(Keys.N);
            }
            else
            {
                press([Keys.LControlKey, Keys.LMenu, Keys.W]);
                Thread.Sleep(10);
                if (ProcessName == Common.WeChat) return;
                run_wei();
            }
        }

        public static void quick_visiualstudio()
        {
            string module_name = ProcessName;
            if (is_down(Keys.Delete) || is_ctrl()) return;
            if (ProcessName == Common.AppName)
            {
                return;
            }
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
            Common.no_move = false;
            DaleyRun_stop = true;
            ready_to_sleep = false;
            system_sleep_count = 0;
            player.Stop();
            CleanMouseState();
            SetDesktopToBlack();
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
            ProcessRun("C:\\Program Files\\Tencent\\Weixin\\Weixin.exe");
        }

        public static void run_vis()
        {
            ProcessRun(devenvexe, keyupMusicexe);
        }
    }
}
