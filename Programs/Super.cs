using System;
using System.Diagnostics;
using System.Management;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.Huan;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;

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
        Keys[] keys = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
        bool key_sound = true;
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
                    press("LWin;OPEN;Enter;500;", 101);
                    if (judge_color(1493, 1109, Color.FromArgb(237, 127, 34)))
                        press("1056, 411;1563, 191", 101);
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
                case Keys.T:
                    if (start_record && !string.IsNullOrEmpty(commnd_record))
                    {
                        Common.log(commnd_record);
                        Invoke(() => Clipboard.SetText(commnd_record));
                    }
                    start_record = !start_record;
                    commnd_record = "";
                    break;
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
                    //KeyboardInput.SimulateMouseWheel(120);
                    new Other().hook_KeyDown_ddzzq(new KeyboardHookEventArgs(WGestures.Core.Impl.Windows.KeyboardEventType.KeyDown, Keys.F11, 0, new WGestures.Common.OsSpecific.Windows.Native.keyboardHookStruct()));
                    break;
                case Keys.F:
                    press("0.0", 100);
                    break;
                case Keys.G:
                    paly_sound(Keys.D4);
                    get_point_color(e);
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
                    Thread.Sleep(2000);
                    press(Keys.G);
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
                    Invoke(() => { try { press(Clipboard.GetText()); } catch { } });
                    paly_sound(Keys.D1);
                    break;
                case Keys.B:
                    Invoke(() => { Clipboard.Clear(); });
                    stop_keys = new List<Keys>();
                    break;
                case Keys.N:
                    notify();
                    break;
                case Keys.M:
                    //FocusProcess(Common.chrome);
                    KeyboardInput.SendString("f");
                    break;

                case Keys.F1:
                    Invoke(() => { huan.SetVisibleCore2(huan.last_visiable); });
                    //hide_keyupmusic3();
                    huan.last_visiable = false;
                    break;
                case Keys.F2:
                case Keys.S:
                    if (!FocusProcess("keyupMusic3"))
                    {
                        ProcessRun("C:\\Users\\bu\\source\\repos\\keyupMusic3\\bin\\Debug\\net8.0-windows\\keyupMusic3.exe");
                        HideProcess("keyupMusic3");
                    }
                    break;
                case Keys.F4:
                case Keys.A:
                    paly_sound(Keys.D2);
                    if (ProcessName == Common.ACPhoenix) { Common.HideProcess(Common.ACPhoenix); break; }
                    if (Common.FocusProcess(Common.ACPhoenix)) break;
                    dragonest();
                    break;
                case Keys.F5:
                    //log_always = !log_always;
                    press(Keys.MediaPlayPause);
                    break;
                case Keys.F6:
                    Process[] processes = Process.GetProcessesByName(keyupMusic2.Common.keyupMusic2);
                    Process[] processes2 = Process.GetProcessesByName(keyupMusic2.Common.keyupMusic3);
                    processes2[0].Kill();
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

            void paly_sound(Keys key)
            {
                if (is_down(Keys.LWin)) return;
                //if (key_sound && keys.Contains(e.key))
                if (key_sound)
                {
                    string wav = "wav\\" + key.ToString().Replace("D", "").Replace("F", "") + ".wav";
                    if (!File.Exists(wav)) return;

                    player = new SoundPlayer(wav);
                    player.Play();
                }
            }
        }

        private static void notify()
        {
            //ctrl_shift();
            //KeyboardInput.SendString("xiexielaoban");
            //huan.Invoke2(() =>
            //{
            // 创建一个通知图标对象
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.Visible = true;
            notifyIcon.BalloonTipTitle = "拯救锁屏无登录";
            notifyIcon.BalloonTipText = "这是一个系统通知内容。";
            notifyIcon.ShowBalloonTip(5000);
            //});
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
            var action = () =>
            {
                DaleyRun(
       () =>
       {
           return (judge_color(1072, 105, Color.FromArgb(26, 26, 25)));
       },
       () =>
       {
           Sleep(520);
           mouse_click(2303, 565);
           press(Keys.PageDown, 10);
           press(Keys.PageDown, 10);
           press("400;2211, 765", 10);
       },
       2211,
       100);
            };
            Common.cmd($"/c start ms-settings:sound", action);
        }

        private void get_point_color(KeyboardHookEventArgs e)
        {
            Point mousePosition = Cursor.Position;
            if (start_record)
            {
                commnd_record += $"{mousePosition.X},{mousePosition.Y};";
            }
            using (Bitmap bitmap = new Bitmap(1, 1))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(mousePosition.X, mousePosition.Y, 0, 0, new Size(1, 1));
                    //(1470, 1213, Color.FromArgb(245, 139, 0))
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
            //press("2280,1314;LWin;", 500);
            //Task.Run(() =>
            //{
            //    DaleyRun(() =>
            //    {
            //        return (judge_color(2463, 1281, Color.FromArgb(220, 163, 50)));
            //    },
            //    () =>
            //    {
            //        Thread.Sleep(200);
            //        //press("100;525.40;");
            //        //return;
            //        altab();
            //        Thread.Sleep(200);
            //        press("100;2525,40;100", 0); mouse_move_center();
            //    },
            //    15000,
            //    100);
            //});
            Task.Run(() =>
            {
                if (DaleyRun_stop) return;
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
            press("10;LWin;500;1076,521", 101);
            //press("10;LWin;zh;DUODUO;Space;Apps;100;Enter", 101);
        }
        private static void dragonest_max(int tick)
        {
            DaleyRun(
                () =>
                {
                    return (
                        //yo() == Common.Dragonest &&
                        judge_color(71, 199, Color.FromArgb(242, 95, 99)) &&
                        !judge_color(2223, 1325, Color.FromArgb(22, 155, 222)));
                },
                () => { press("2323, 30"); },
                tick,
                10);
        }

        public void Invoke(Action action)
        {
            huan.Invoke(action);
        }
        public void change_file_last(bool pngg)
        {
            // 指定要处理的文件夹路径  
            string folderPath = "image\\encode\\";

            // 指定旧后缀和新后缀（不包含点号）  
            string oldExtension = "pngg";
            string newExtension = "png";
            if (pngg) { oldExtension = "png"; newExtension = "pngg"; }

            // 确保文件夹路径存在  
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("指定的文件夹不存在。");
                return;
            }

            // 遍历文件夹下的所有文件  
            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                // 检查文件是否匹配旧后缀  
                if (Path.GetExtension(filePath)?.TrimStart('.') == oldExtension)
                {
                    // 构建新文件名  
                    string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + "." + newExtension);

                    // 重命名文件  
                    try
                    {
                        File.Move(filePath, newFilePath);
                        Console.WriteLine($"文件 {filePath} 已更改为 {newFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"无法重命名文件 {filePath}。错误：{ex.Message}");
                    }
                }
            }

            Console.WriteLine("所有匹配的文件后缀已更改。");
        }
        [DllImport("user32.dll")]
        public static extern int GetKeyboardLayoutList(int nBuff, byte[] lpList);

        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(uint dwLayout);

        [DllImport("user32.dll")]
        public static extern int GetKeyboardLayoutName(StringBuilder pwszKLID, int cchKLID);

    }
}
