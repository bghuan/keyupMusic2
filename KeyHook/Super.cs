﻿using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;
using static keyupMusic2.Native;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace keyupMusic2
{
    public partial class SuperClass
    {
        Keys[] keys = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.PageUp, Keys.Home, Keys.End };
        public static bool start_record = false;
        string commnd_record = "";

        public static void hook_KeyDown(Keys keys)
        {
            Huan.keyupMusic2_onlisten = true;
            var e = new MouseKeyboardHook.KeyEventArgs(KeyType.Down, keys, 0, new Native.keyboardHookStruct());
            new SuperClass().hook_KeyDown_keyupMusic2(e);
        }
        public void hook_KeyDown_keyupMusic2(MouseKeyboardHook.KeyEventArgs e)
        {
            if (!Huan.keyupMusic2_onlisten) return;
            Common.hooked = true;
            bool catched = true;
            switch (e.key)
            {
                case Keys.Q:
                    //Blob.Instance.changeFlag(false);
                    break;
                case Keys.W:
                    //SSSS.KeyPress(Keys.LWin, "openvpn", Keys.Enter);
                    //SSSS.KeyPress(Keys.LWin, "verge", Keys.Enter);
                    mouse_move(2186, 1403, 100);
                    var need_win = GetPointName() != explorer;
                    if (need_win) press(LWin, 100);
                    mouse_click_right();
                    if (e.key == Q)
                        press("2259,1112;300;2109,1107", 100);
                    else
                        press("2259,1112;300;2109,1180", 100);
                    break;
                    //2083,1180 2109,1107 2259,1112 2186,1403
                    mouse_click_right(2186, 1403);
                    press("2259,1112;2083,1180", 100);
                    break;
                case Keys.E:
                    play_sound(Keys.D0);
                    winBinWallpaper.changeImg();
                    break;
                case Keys.R:
                    sound_setting();
                    break;
                case Keys.T:
                    gcc_restart = true;
                    break;
                case Keys.Y:
                    Common.cmd($"/c start ms-settings:taskbar");
                    press("100;978,1042;978,1044;907,1227;2500,32;", 501);
                    break;
                case Keys.U:
                    Common.cmd($"/c start ms-settings:personalization");
                    press("200;1056,588;2118,530;2031,585;2516,8;", 801);
                    break;
                case Keys.I:
                    SS().KeyPress(Keys.RButton);
                    break;
                case Keys.O:
                    play_sound(Keys.D5);
                    change_file_last(true);
                    break;
                case Keys.P:
                    play_sound(Keys.D3);
                    change_file_last(false);
                    break;
                case Keys.A:
                    //start_record = !start_record;
                    //Log.logcachesave();
                    //string path = "";
                    //Invoke(() =>
                    //{
                    //    path = Clipboard.GetText();
                    //});
                    //var a = GetAllFiles(path, false);
                    //download_image(a);
                    //start_listen_to_word();
                    break;
                case Keys.S:
                    //SetWindowTitle();
                    MoveSmallFilesRecursive();
                    break;
                case Keys.D:
                    break;
                case Keys.F:
                    var aasad = "C:\\Users\\bu\\source\\repos\\keyupMusic2\\bin\\Debug\\net8.0-windows\\image\\downloaded_images\\33084\\0f4702772dd8fff945d5b066.png";
                    FindDuplicatewebps(aasad, "C:\\Users\\bu\\source\\repos\\keyupMusic2\\bin\\Debug\\net8.0-windows\\image\\downloaded_images");
                    break;
                case Keys.G:
                    var keys = new List<Keys>();
                    for (int i = 0; i < 256; i++)
                        keys.Add((Keys)i);
                    huan.Invoke(() => { huan.label1.Text = "all key up " + keys.Count + keys.ToString(); });
                    Sleep(200);
                    foreach (var key in keys)
                    {
                        if (key == Keys.Apps) continue;
                        if (is_down(Keys.CapsLock)) return;
                        huan.Invoke(() => { huan.label1.Text = ProcessName + " " + key.ToString(); });
                        SS(10).KeyUp(key);
                    }
                    break;
                case Keys.H:
                    //string sourceImage = "input.png";
                    //string targetImage = "output.png";
                    ////ConvertAndResize(sourceImage, targetImage);
                    ////ShowFadeInWallpaper(targetImage);
                    //var aaa = GetWallpaperFromRegistry();
                    //targetImage = "output.png";
                    BatchCompressImages("C:\\Users\\bu\\Pictures\\Screenshots\\dd\\20241022", "C:\\Users\\bu\\Desktop", 80, 1920, 1080);
                    break;
                case Keys.J:
                    SetDesktopWallpaperAli(GetCurrentWallpaperPath());
                    break;
                case Keys.K:
                    huan.release_all_key(1000);
                    break;
                case Keys.L:
                    //RemoveWebpWhiteBorder(GetCurrentWallpaperPath(), "a.webp");
                    //var mp3Path = "C:\\Users\\bu\\Desktop\\a\\爸(19523708410)_20230728213046.mp3";
                    //var wavPath = "C:\\Users\\bu\\Desktop\\b\\爸(19523708410)_20230728213046.wav";
                    //ConvertMp3ToWav(mp3Path, wavPath);
                    string windowTitle = "PowerToys.CropAndLock"; // 示例：记事本程序
                    byte transparency = 160; // 半透明效果
                    if (is_tran_powertoy)
                        transparency = 255;
                    is_tran_powertoy = !is_tran_powertoy;
                    bool success = SetWindowTransparency(windowTitle, transparency);
                    break;
                case Keys.Z:
                    GoodDesktopWallpaper(); break;
                case Keys.X:
                    AllClass.run_vis();
                    break;
                case Keys.C:
                    press_middle_bottom();
                    break;
                case Keys.V:
                    cmd_v();
                    break;
                case Keys.B:
                    var pressedKeys = release_all_keydown();
                    if (pressedKeys.Any())
                        huan.Invoke2(() => { huan.label1.Text = "relese: " + string.Join(", ", pressedKeys); });
                    Huan.handling_keys = new();
                    break;
                case Keys.M:
                    chrome_m();
                    break;
                case Keys.N:
                    notify();
                    break;
                case Keys.Space:
                    Invoke(() =>
                    {
                        press([Keys.LControlKey, Keys.A, Keys.C], 200);
                        press([Keys.LShiftKey], 100);
                        string ddd = Clipboard.GetText().ToUpper();
                        if (ddd.Length < 20)
                            press(ddd);
                    });
                    break;
                case Keys.F1:
                    get_point_color(e);
                    if (VirMouseStateKey.Count > 0)
                        log("VirMouseStateKey: " + string.Join(", ", VirMouseStateKey));
                    break;
                //case Keys.F2:
                //    //LossScale();
                //    break;
                case Keys.F4:
                    press(Keys.MediaPlayPause);
                    break;
                case Keys.F5:
                    play_sound(Keys.D2);
                    ProcessRun("C:\\Program Files (x86)\\Steam\\steam.exe");
                    break;
                case Keys.F6:
                    play_sound(Keys.D2);
                    press(Keys.LWin);
                    Sleep(180);
                    mouse_click(1062, 899);
                    break;
                case Keys.F10:
                    //Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,1");
                    //huan.system_sleep();
                    break;
                //case Keys.Up:
                //    Invoke(() => huan.Opacity = huan.Opacity >= 1 ? 1 : huan.Opacity + 0.5);
                //    break;
                //case Keys.Down:
                //    Invoke(() => huan.Opacity = huan.Opacity <= 0 ? 0 : huan.Opacity - 0.5);
                //    break;
                case Keys.Escape:
                    //if (is_ctrl() && is_shift()) { Process.Start(new ProcessStartInfo("taskmgr.exe")); break; }
                    //press_middle_bottom();
                    huan._mouseKbdHook.ChangeMouseHooks();
                    break;
                case Keys.Delete:
                    DeleteCurrentWallpaper();
                    break;
                case Keys.F11:
                case Keys.F12:
                    press(e.key);
                    break;

                case Keys.Left:
                    press(Keys.MediaPreviousTrack);
                    break;
                case Keys.Right:
                    press(Keys.MediaNextTrack);
                    break;
                case Keys.Down:
                    press(Keys.VolumeDown, 2, 10);
                    break;
                case Keys.Up:
                    press(Keys.VolumeUp, 2, 10);
                    break;
                case Keys.PageDown:
                    press(Keys.MediaPlayPause);
                    break;

                default:
                    catched = false;
                    break;
            }

            //if (key_sound && keys.Contains(e.key)) { play_sound(e.key); catched = true; }
            if (keys.Contains(e.key)) { play_sound(e.key); catched = true; }

            if (catched)
            {
                huan.Invoke2(() => { Huan.keyupMusic2_onlisten = false; huan.BackColor = Color.White; /*huan.label1.Text = e.key.ToString();*/ }, 10);
                //Invoke((() => { label1.Text = label_backup; }));
                //KeyboardHook.stop_next = true;
            }
            Common.hooked = false;
        }

        public static void chrome_m()
        {
            TaskRun(() =>
            {
                string name = ProcessName;
                if (!FocusProcessSimple(Common.chrome)) return;
                play_sound_di();
                Simm.KeyPress(Keys.M).Sleep(100);
                FocusProcessSimple(name);
            }, 100);
        }

        private void quick_onekey()
        {
            Invoke(() =>
            {
                string ddd = Clipboard.GetText();
                string dddd = ddd.Substring(ddd.IndexOf("app/") + 4, 8);
                string ddddd = dddd.Substring(0, dddd.IndexOf("/"));
                ProcessRun("C:\\Program Files\\other\\Onekey---v1.3.5.exe");
                Simm.Wait(2000).KeyPress(ddddd).KeyPress(Keys.Enter);
            });
        }

        private void cmd_v()
        {
            if (ProcessName == Common.devenv && ProcessTitle.Contains("在运行") && (is_ctrl() || Position.X == 0))
            {
                var txt = "Common.";
                Invoke(() => Clipboard.SetText(txt));
                press([Keys.LControlKey, Keys.V]);
                return;
            }
            Invoke(() => { try { press(Clipboard.GetText()); } catch { } });
            //play_sound(Keys.D1);
        }

        private static void quick_dir_file()
        {
            string imagePath = @"C:\Users\bu\Pictures\Screenshots\屏幕截图 2024-10-15 204332.png";
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"\"{imagePath}\"",
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening image: {ex.Message}");
            }
        }
        private static void notify()
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.Visible = true;
            notifyIcon.BalloonTipTitle = "拯救锁屏无登录";
            notifyIcon.BalloonTipText = "这是一个系统通知内容。";
            notifyIcon.ShowBalloonTip(10000);
        }

        private static void dragonest()
        {
            if (!Common.ExistProcess(Common.Dragonest))
            {
                dragonest_init();
                dragonest_max(10000);
            }
            else
            {
                dragonest_notity_click();
                if (!judge_color(71, 199, Color.FromArgb(242, 95, 99)))
                {
                    dragonest_notity_click();
                }
                if (!judge_color(2223, 1325, Color.FromArgb(22, 155, 222)))
                    dragonest_max(100);
            }
            dragonest_run();
        }

        private static void sound_setting()
        {
            var judge = () =>
            {
                huan.Invoke(() => { huan.label1.Text = DateTimeNow2(); });
                Simm.MouseWhell(-120 * 10);
                return (judge_color(775, 1265, Color.FromArgb(26, 26, 25)))
                     && judge_color(2124, 1327, Color.FromArgb(243, 243, 243), 2);
            };
            var run = () => { press("200;2220,1070", 10); };
            var action2 = () =>
                    DelayRun(judge, run, 3222, 122);

            mouse_move(2220, 1070);
            Common.cmd($"/c start ms-settings:sound", action2, 200);
        }

        public void get_point_color(MouseKeyboardHook.KeyEventArgs e)
        {
            Point mousePosition = Cursor.Position;
            var last_x = Cursor.Position.X;
            var last_y = Cursor.Position.Y;
            if (last_x > screenWidth)
            {
                Screen currentScreen = Screen.FromPoint(mousePosition);
                int relativeX = (mousePosition.X - currentScreen.Bounds.X) * 1920 / currentScreen.Bounds.Width;
                int relativeY = (mousePosition.Y - currentScreen.Bounds.Y) * 1080 / currentScreen.Bounds.Height;
                Console.WriteLine($"相对坐标：({relativeX}, {relativeY})");
                last_x = screenWidth + relativeX;
                last_y = screenHeight + relativeY;

                Bitmap bmpScreenshot = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
                Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.CopyFromScreen(new Point(screenWidth, 0), Point.Empty, currentScreen.Bounds.Size);

                var color = bmpScreenshot.GetPixel(relativeX, relativeY);
                string asd = $"({mousePosition.X},{mousePosition.Y}, Color.FromArgb({color.R},{color.G},{color.B}))";
                log(processWrapper.ToString() + " " + asd);
                //process_and_log(e?.key.ToString());
                Invoke(() => Clipboard.SetText(asd));
                return;
            }
            using (Bitmap bitmap = new Bitmap(1, 1))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(last_x, last_y, 0, 0, new System.Drawing.Size(1, 1));
                    var color = bitmap.GetPixel(0, 0);
                    string asd = $"({mousePosition.X},{mousePosition.Y}, Color.FromArgb({color.R},{color.G},{color.B}))";
                    log(processWrapper.ToString() + " " + asd);
                    //process_and_log(e?.key.ToString());
                    asd = $"{mousePosition.X},{mousePosition.Y}";
                    Invoke(() => Clipboard.SetText(asd));
                }
            }
        }

        private void start_listen_to_word()
        {
            Listen.is_listen = !Listen.is_listen;
            //Invoke(() => huan.SetVisibleCore2(Listen.is_listen));
            //Listen.aaaEvent += huan.handle_word;
            if (Listen.is_listen) Task.Run(() => Listen.listen_word(new string[] { }, (string deal, int a) => { }));
            Listen.speak_word = "";
        }

        private static void dragonest_run()
        {
            //press("2280,1314;LWin;3222;LWin;", 500); 
            press("2280,1314;LWin", 0);
            Task.Run(() =>
            {
                DaleyRun_stop = false;
                Thread.Sleep(3500);
                if (DaleyRun_stop) return;
                altab();
                press("500;2525,40;100", 0);
                mouse_move_center();
            });
            return;
        }

        private static void dragonest_init()
        {
            var judge = () => judge_color(1063, 529, Color.FromArgb(199, 71, 69));
            var run = () => { press("1076,521"); };
            var action2 = () => DelayRun(judge, run, 3222, 122);

            press("LWin", 0);
            action2();
            //press("10;LWin;zh;DUODUO;Space;Apps;100;Enter", 101);
        }
        private static void dragonest_max(int tick)
        {
            DelayRun(
                () => (
                        //yo() == Common.Dragonest &&
                        judge_color(71, 199, Color.FromArgb(242, 95, 99)) &&
                        !judge_color(2223, 1325, Color.FromArgb(22, 155, 222))),
                () => { press("2323, 30"); },
                tick, 10);
        }

        public void Invoke(Action action)
        {
            huan.Invoke(action);
        }
    }
}