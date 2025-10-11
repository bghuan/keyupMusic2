using keyupMusic2.fantasy;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
using static keyupMusic2.Native;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace keyupMusic2
{
    public partial class SuperClass
    {
        Keys[] keys = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.PageUp, Keys.Home, Keys.End };
        public static bool start_record = false;

        public static void hook_KeyDown(Keys keys)
        {
            Huan.keyupMusic2_onlisten = true;
            var e = new KeyboardMouseHook.KeyEventArgs(KeyType.Down, keys, 0, new Native.keyboardHookStruct());
            new SuperClass().HookEvent(e);
            Huan.keyupMusic2_onlisten = false;
        }
        public bool HookEvent(KeyboardMouseHook.KeyEventArgs e)
        {
            if (!Huan.keyupMusic2_onlisten) return false;
            if (e.key == (Keys.LButton | Keys.OemClear)) return false;

            bool catched = true;

            if (e.key == Huan.super_key && DeviceName != acer)
                press(MediaPlayPause);
            else if (e.key == Huan.super_key)
                huan.SetVisibleCore2(!huan.Visible);
            else if (e.key == Huan.super_key2)
                huan.system_sleep();
            else
                switch (e.key)
                {
                    case Keys.Q:
                        //Blob.Instance.changeFlag(false);
                        mousewhell(4);
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
                            if (is_down(Keys.CapsLock)) return true;
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
                        //BatchCompressImages("C:\\Users\\bu\\Pictures\\Screenshots\\dd\\20241022", "C:\\Users\\bu\\Desktop", 80, 1920, 1080);
                        //VirtualKeyboardForm.Instance.TriggerKey(Q, false);
                        break;
                    case Keys.J:
                        //SetDesktopWallpaperAli(GetCurrentWallpaperPath());
                        MoonTime.Instance.ChangeColor();
                        break;
                    case Keys.K:
                        //huan.release_all_key(1000);
                        VirtualKeyboardForm.Instance?.TriggerKey(Y, e.Type == KeyType.Up);
                        break;
                    case Keys.L:
                        string windowTitle = "PowerToys.CropAndLock"; // 示例：记事本程序
                        byte transparency = 160; // 半透明效果
                        if (is_tran_powertoy)
                            transparency = 255;
                        is_tran_powertoy = !is_tran_powertoy;
                        bool success = SetWindowTransparency(windowTitle, transparency);
                        break;
                    case Keys.Z:
                        SetTransparency();
                        break;
                    case Keys.X:
                        //AllClass.run_vis();
                        //IntPtr hwnd = Native.GetForegroundWindow();
                        //const uint WHITE_COLOR = 0x000000;
                        //SetLayeredWindowAttributes(hwnd, WHITE_COLOR, 255, 0x1);
                        huan.Invoke(() => {
                            Read read = new Read();
                            read.Show();
                            read.Activate();         // 激活窗口
                            read.BringToFront();     // 放到最前
                            read.Focus();            // 设置输入焦点
                        });
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
                        get_point_color();
                        //if (VirMouseStateKey.Count > 0)
                        //    log("VirMouseStateKey: " + string.Join(", ", VirMouseStateKey));
                        break;
                    case Keys.F2:
                        //LossScale();
                        CloseProcess(explorer);
                        ProcessRun(explorer);
                        break;
                    //case Keys.F3:
                    //    huan.SetVisibleCore2(!huan.Visible);
                    //    break;
                    case Keys.F4:
                        huan.Invoke(() => {
                            Read read = new Read();
                            read.Show();
                            read.Activate();         // 激活窗口
                            read.BringToFront();     // 放到最前
                            read.Focus();            // 设置输入焦点
                        });
                        break;
                    case Keys.F5:
                        huan.Invoke((Delegate)(() =>
                        {
                            Native.AllocConsole();
                            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
                            Console.SetError(new StreamWriter(Console.OpenStandardError()) { AutoFlush = true });
                            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
                        }));
                        break;
                    case Keys.F6:
                        play_sound(Keys.D2);
                        press(Keys.LWin);
                        Sleep(180);
                        mouse_click(1062, 899);
                        //steam://rungameid/730
                        //ProcessRun("steam://rungameid/730");
                        break;
                    //case Keys.F9:
                    //    huan.system_sleep();
                    //    break;
                    case Keys.F10:
                        Common.no_move = !Common.no_move;
                        HideProcess(true);
                        break;
                    //case Keys.F11:
                    //case Keys.F12:
                    //    press(e.key);
                    //    break;

                    case Keys.Left:
                        press(Keys.MediaPreviousTrack);
                        //altshiftab();
                        break;
                    case Keys.Right:
                        press(Keys.MediaNextTrack);
                        //altab();
                        break;
                    case Keys.Down:
                        press(Keys.VolumeDown, 2, 10);
                        catched = false;
                        break;
                    case Keys.Up:
                        press(Keys.VolumeUp, 2, 10);
                        catched = false;
                        break;
                    case Keys.Enter:
                        press(MediaPlayPause);
                        break;

                    case Keys.PageDown:
                        press(Keys.MediaPlayPause);
                        break;
                    case Keys.D0:
                        play_sound_bongocat(Keys.D0);
                        NotityTime = DateTime.Now.AddMinutes(10);
                        break;

                    case Keys.Escape:
                        huan._mouseKbdHook.ChangeMouseHooks();
                        break;
                    case Keys.Delete:
                        DeleteCurrentWallpaper();
                        break;
                    case Keys.LWin:
                    case Keys.RWin:
                        press(e.key);
                        break;

                    case Keys.D1:
                        RestartProcess(TwinkleTray, TwinkleTrayexe);
                        break;
                    case Keys.D2:
                        RestartProcess(cloudmusic, cloudmusicexe);
                        break;
                    case Keys.D3:
                        SetResolution(1);
                        break;
                    case Keys.D4:
                        SetResolution(2);
                        break;
                    case Keys.D5:
                        RestartProcess(explorer, cloudmusicexe);
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
            return true;
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
                Console2.WriteLine($"Error opening image: {ex.Message}");
            }
        }
        public static void notify(string title = "拯救锁屏无登录", string msg = "这是一个系统通知内容。")
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.Visible = true;
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = msg;
            notifyIcon.ShowBalloonTip(10000);
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

        public static void get_point_color()
        {
            Point mousePosition = Cursor.Position;
            var last_x = Cursor.Position.X;
            var last_y = Cursor.Position.Y;
            if (last_x > screenWidth)
            {
                Screen currentScreen = Screen.FromPoint(mousePosition);
                int relativeX = (mousePosition.X - currentScreen.Bounds.X) * 1920 / currentScreen.Bounds.Width;
                int relativeY = (mousePosition.Y - currentScreen.Bounds.Y) * 1080 / currentScreen.Bounds.Height;
                Console2.WriteLine($"相对坐标：({relativeX}, {relativeY})");
                last_x = screenWidth + relativeX;
                last_y = screenHeight + relativeY;

                Bitmap bmpScreenshot = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
                Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.CopyFromScreen(new Point(screenWidth, 0), Point.Empty, currentScreen.Bounds.Size);

                var color = bmpScreenshot.GetPixel(relativeX, relativeY);
                string asd = $"({mousePosition.X},{mousePosition.Y}, Color.FromArgb({color.R},{color.G},{color.B}))";
                log(processWrapper.ToString() + " " + GetPointName() + " " + asd);
                //process_and_log(e?.key.ToString());
                huan.Invoke(() => Clipboard.SetText(asd));
                return;
            }
            using (Bitmap bitmap = new Bitmap(1, 1))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(last_x, last_y, 0, 0, new System.Drawing.Size(1, 1));
                    var color = bitmap.GetPixel(0, 0);
                    string asd = $"({mousePosition.X},{mousePosition.Y}, Color.FromArgb({color.R},{color.G},{color.B}))";
                    //process_and_log(e?.key.ToString());
                    log(processWrapper.ToString() + " " + GetPointName() + " " + asd);
                    asd = $"{mousePosition.X},{mousePosition.Y}";
                    huan.Invoke(() => Clipboard.SetText(asd));
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

        public void Invoke(Action action)
        {
            huan.Invoke(action);
        }
    }
}