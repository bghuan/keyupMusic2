using System.Diagnostics;
using System.Drawing.Imaging;
using System.Management;
using static keyupMusic2.Common;
using static keyupMusic2.Huan;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;
using static keyupMusic2.Simulate;
using WGestures.Common.OsSpecific.Windows;

namespace keyupMusic2
{
    public class Super
    {
        public Super(Form parentForm)
        {
            huan = (Huan)parentForm;
        }
        public Super()
        {
        }
        public static Huan huan;
        Keys[] keys = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.PageUp, Keys.Next, Keys.Home, Keys.End, Keys.Space };
        bool start_record = false;
        string commnd_record = "";

        public static void hook_KeyDown(Keys keys)
        {
            huan.keyupMusic2_onlisten = true;
            var e = new KeyboardHookEventArgs(WGestures.Core.Impl.Windows.KeyboardEventType.KeyDown, keys, 0, new WGestures.Common.OsSpecific.Windows.Native.keyboardHookStruct());
            new Super().hook_KeyDown_keyupMusic2(e);
        }
        public void hook_KeyDown_keyupMusic2(KeyboardHookEventArgs e)
        {
            //if (ProcessName != Common.keyupMusic2) return;
            if (!huan.keyupMusic2_onlisten) return;
            //if (is_ctrl() && is_shift()) return;
            Common.hooked = true;
            //string label_backup = huan.label1.Text;
            bool catched = true;

            switch (e.key)
            {
                case Keys.Q:
                    //Sim.KeyPress([Keys.LControlKey, Keys.V]);
                    Sim.KeyPress(Keys.LWin).Wait(100)
                       .SendString("openvpn").Wait(100)
                       .KeyPress(Keys.Enter).Wait(500);
                    //press("LWin;OPEN;Enter;500;", 101);
                    //if (judge_color(1493, 1109, Color.FromArgb(237, 127, 34)))
                    //    press("1056, 411;1563, 191", 101);
                    break;
                case Keys.W:
                    start_listen_to_word();
                    break;
                case Keys.E:
                    paly_sound(Keys.D0);
                    winBinWallpaper.changeImg();
                    break;
                case Keys.R:
                    if (key_sound) player.Stop();
                    key_sound = !key_sound;
                    break;
                //case Keys.T:
                //    if (start_record && !string.IsNullOrEmpty(commnd_record))
                //    {
                //        Common.log(commnd_record);
                //        Invoke(() => Clipboard.SetText(commnd_record));
                //    }
                //    start_record = !start_record;
                //    commnd_record = "";
                //    break;
                case Keys.Y:
                    Common.cmd($"/c start ms-settings:taskbar");
                    press("200;978,1042;907,1227;2500,32;", 801);
                    break;
                case Keys.U:
                    Common.cmd($"/c start ms-settings:personalization");
                    press("200;1056,588;2118,530;2031,585;2516,8;", 801);
                    break;
                case Keys.I:
                    sound_setting();
                    break;
                case Keys.O:
                    paly_sound(Keys.D5);
                    change_file_last(true);
                    break;
                case Keys.P:
                    paly_sound(Keys.D3);
                    change_file_last(false);
                    break;
                case Keys.D:
                    new Other().hook_KeyDown_ddzzq(new KeyboardHookEventArgs(WGestures.Core.Impl.Windows.KeyboardEventType.KeyDown, Keys.F11, 0, new WGestures.Common.OsSpecific.Windows.Native.keyboardHookStruct()));
                    break;
                case Keys.F:
                    Sim.KeyPress(Keys.F);
                    Sim.MouseWhell(120);
                    break;
                case Keys.H:
                    press(Keys.F11);
                    break;
                case Keys.J:
                    if (!is_ctrl()) if (Common.FocusProcess(Common.chrome)) break;
                    press("LWin;CHR;Enter;", 100);
                    break;
                case Keys.K:
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity");

                    ManagementObjectCollection collection = searcher.Get();

                    foreach (ManagementObject mo in collection)
                    {
                        Console.WriteLine("Device ID: " + mo["DeviceID"]);
                        Console.WriteLine("Name: " + mo["Name"]);
                        Console.WriteLine("PNPDeviceID: " + mo["PNPDeviceID"]);
                        Console.WriteLine("PNPClass: " + mo["PNPClass"]);
                        Console.WriteLine("Caption: " + mo["Caption"]);
                        Console.WriteLine("----------------------------------");
                    }
                    break;
                case Keys.L:
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
                    break;
                case Keys.Z:
                    press("100;LWin;KK;Enter;", 110);
                    break;
                case Keys.X:
                    Thread.Sleep(3000);
                    mouse_move(1, 1);
                    press(Keys.Left);
                    break;
                case Keys.C:
                    press_middle_bottom();
                    break;
                case Keys.V:
                    ////KeyboardInput2.SendString("Hello, World!");
                    ////KeyboardInput3.SendString("Hello, World!");
                    ////SendKeyboardMouse sendKeyMouse = new SendKeyboardMouse();
                    ////sendKeyMouse.SendKeyPress(VKCODE.VK_A);
                    if (ProcessName == Common.devenv && ProcessTitle.Contains("在运行") && is_ctrl())
                    {
                        var txt = "Common.";
                        Invoke(() => Clipboard.SetText(txt));
                        press([Keys.LControlKey, Keys.V]);
                        break;
                    }

                    Invoke(() => { try { press(Clipboard.GetText()); } catch { } });
                    paly_sound(Keys.D1);
                    break;
                case Keys.B:
                    Invoke(() => { Clipboard.Clear(); });
                    stop_keys = new Dictionary<Keys,string>();
                    break;
                case Keys.N:
                    notify();
                    break;
                case Keys.M:
                    TaskRun(() =>
                    {
                        FocusProcess(Common.chrome);
                        Sim.KeyPress(Keys.M);
                        play_sound_di();
                        Sleep(100);
                        altab();
                    }, 100);
                    break;

                case Keys.F1:
                case Keys.G:
                    paly_sound(Keys.D4);
                    get_point_color(e);
                    break;
                    //Invoke(() => { huan.SetVisibleCore2(huan.last_visiable); });
                    ////hide_keyupmusic3();
                    //huan.last_visiable = false;
                    break;
                case Keys.F2:
                    //case Keys.S:
                    //if (!FocusProcess("keyupMusic3"))
                    //{
                    //    ProcessRun(Common.keyupMusic3exe);
                    //    HideProcess("keyupMusic3");
                    //}
                    huan._mouseKbdHook.ChangeMouseHooks();
                    break;
                case Keys.F5:
                case Keys.A:
                    paly_sound(Keys.D2);
                    if (ProcessName == Common.ACPhoenix) { Common.HideProcess(Common.ACPhoenix); break; }
                    if (Common.FocusProcess(Common.ACPhoenix)) break;
                    dragonest();
                    break;
                case Keys.F4:
                    //log_always = !log_always;
                    press(Keys.MediaPlayPause);
                    break;
                case Keys.F6:
                    Process[] processes = Process.GetProcessesByName(keyupMusic2.Common.keyupMusic2);
                    //Process[] processes2 = Process.GetProcessesByName(keyupMusic2.Common.keyupMusic3);
                    //processes2[0].Kill();
                    processes[0].Kill();
                    break;
                case Keys.Up:
                    Invoke(() => huan.Opacity = huan.Opacity >= 1 ? 1 : huan.Opacity + 0.1);
                    break;
                case Keys.Down:
                    Invoke(() => huan.Opacity = huan.Opacity <= 0 ? 0 : huan.Opacity - 0.1);
                    break;
                case Keys.Escape:
                    if (is_ctrl() && is_shift()) { Process.Start(new ProcessStartInfo("taskmgr.exe")); break; }
                    //press("LWin;1957,1015");
                    press_middle_bottom();
                    Special_Input = false; DaleyRun_stop = true; special_delete_key_time = DateTime.Now; player.Stop();
                    break;

                default:
                    catched = false;
                    break;
            }

            if (key_sound && keys.Contains(e.key)) { paly_sound(e.key); catched = true; }

            if (catched)
            {
                huan.Invoke2(() => { huan.keyupMusic2_onlisten = false; huan.BackColor = Color.White; /*huan.label1.Text = e.key.ToString();*/ }, 10);
                //Invoke((() => { label1.Text = label_backup; }));
                //KeyboardHook.stop_next = true;
            }
            Common.hooked = false;
        }

        //private void asd()
        //{
        //    unsafe
        //    {
        //        var text = "f";
        //        var input = stackalloc KeyboardInput.INPUT[1];
        //        var inputs = new KeyboardInput.INPUT[text.Length * 2];

        //        for (int i = 0; i < text.Length; i++)
        //        {
        //            ushort vk = KeyboardInput.VirtualKeyFromChar(text[i]);
        //            inputs[i * 2] = KeyboardInput.INPUT.CreateKeyDown(vk);
        //            inputs[i * 2 + 1] = KeyboardInput.INPUT.CreateKeyUp(vk);
        //            input[0] = KeyboardInput.INPUT.CreateKeyDown(vk);
        //        }
        //        SendInput((uint)inputs.Length, input, Marshal.SizeOf(typeof(KeyboardInput.INPUT)));

        //    }
        //}
        //[DllImport("user32.dll", SetLastError = true)]
        //public unsafe static extern UInt32 SendInput(UInt32 numberOfInputs, KeyboardInput.INPUT* inputs, Int32 sizeOfInputStructure);

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
            if (!Common.ExsitProcess(Common.Dragonest))
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
                Sim.MouseWhell(-120 * 10);
                return (judge_color(775, 1265, Color.FromArgb(26, 26, 25)))
                     && judge_color(2124, 1327, Color.FromArgb(243, 243, 243), null, 2);
            };
            var run = () => { press("200;2220,1070", 10); };
            var action2 = () =>
                    DaleyRun(judge, run, 3222, 122);

            mouse_move(2220, 1070);
            Common.cmd($"/c start ms-settings:sound", action2, 200);
        }

        private void get_point_color(KeyboardHookEventArgs e)
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
                log(ProcessName + asd);
                log_process(e.key.ToString());
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
                    log(ProcessName + asd);
                    log_process(e.key.ToString());
                    Invoke(() => Clipboard.SetText(asd));
                }
            }
        }

        private void start_listen_to_word()
        {
            Listen.is_listen = !Listen.is_listen;
            Invoke(() => huan.SetVisibleCore2(Listen.is_listen));
            Listen.aaaEvent += huan.handle_word;
            if (Listen.is_listen) Task.Run(() => Listen.listen_word(new string[] { }, (string asd, int a) => { }));
            speak_word = "";
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
            var action2 = () => DaleyRun(judge, run, 3222, 122);

            press("LWin", 0);
            action2();
            //press("10;LWin;zh;DUODUO;Space;Apps;100;Enter", 101);
        }
        private static void dragonest_max(int tick)
        {
            DaleyRun(
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
