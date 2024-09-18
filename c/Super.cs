using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static WGestures.Core.Impl.Windows.MouseKeyboardHook;
using System.Windows.Forms;
using WGestures.Core.Impl.Windows;
using static keyupMusic2.Common;
using static keyupMusic2.Huan;
using System.Net;

namespace keyupMusic2
{
    public class Super
    {
        public Super(Form parentForm)
        {
            huan = (Huan)parentForm;
        }
        public Huan huan;
        Keys[] keys = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };
        bool key_sound = true;
        SoundPlayer player = new SoundPlayer();
        bool start_record = false;
        string commnd_record = "";

        public void hook_KeyDown_keyupMusic2(KeyboardHookEventArgs e)
        {
            //if (ProcessName != Common.keyupMusic2) return;
            if (!huan.keyupMusic2_onlisten) return;
            Common.hooked = true;
            string label_backup = huan.label1.Text;
            bool catched = false;

            huan.Invoke2(() => { huan.keyupMusic2_onlisten = false; huan.BackColor = Color.White; /*huan.label1.Text = e.key.ToString();*/ }, 10);

            switch (e.key)
            {
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.D0:
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                    if (is_down(Keys.LWin)) break;
                    if (key_sound && keys.Contains(e.key))
                    //if (key_sound)
                    {
                        string wav = "wav\\" + e.key.ToString().Replace("D", "").Replace("F", "") + ".wav";
                        if (!File.Exists(wav)) return;

                        player = new SoundPlayer(wav);
                        player.Play();
                    }
                    break;
            }
            switch (e.key)
            {
                case Keys.A:
                    if (ProcessName == Common.ACPhoenix) { close(); break; }
                    if (Common.FocusProcess(Common.ACPhoenix)) break;
                    if (!Common.FocusProcess(Common.Dragonest)) { dragonest_init(); }
                    //if (ProcessName2 != Common.Dragonest) break;
                    if (!judge_color(2223, 1325, Color.FromArgb(22, 155, 222))) { Task.Run(() => dragonest_notity_click()); }
                    dragonest_run();
                    break;
                case Keys.Q:
                    //handle_word("连接", 0, false);
                    press("LWin;OPEN;Enter;500;1056, 411;1563, 191", 101);
                    break;
                case Keys.W:
                    Listen.is_listen = !Listen.is_listen;
                    Invoke(() => huan.SetVisibleCore2(Listen.is_listen));
                    Listen.aaaEvent += huan.handle_word;
                    if (Listen.is_listen) Task.Run(() => Listen.listen_word(new string[] { }, (string asd, int a) => { }));
                    break;
                case Keys.E:
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
                    break;
                case Keys.O:
                    change_file_last(true);
                    break;
                case Keys.P:
                    change_file_last(false);
                    break;
                case Keys.D:
                    break;
                case Keys.F:
                    break;
                case Keys.G:
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
                            string asd = $"({mousePosition.X},{mousePosition.Y}, Color.FromArgb({color.R},{color.G},{color.B})";
                            log(ProcessName + asd);
                            log_process(e.key.ToString());
                            Invoke(() => Clipboard.SetText(asd));
                        }
                    }
                    break;
                case Keys.H:
                    press(Keys.F11);
                    break;
                case Keys.J:
                    if (!is_ctrl()) if (Common.FocusProcess(Common.chrome)) break;
                    press("LWin;CHR;Enter;", 100);
                    break;
                case Keys.K:
                    break;
                case Keys.L:
                    Thread.Sleep(2000);
                    press(Keys.G);
                    break;
                case Keys.Z:
                    press("100;LWin;KK;Enter;", 110);
                    break;
                case Keys.X:
                    break;
                case Keys.M:
                    FocusProcess(Common.chrome);
                    press_dump(Keys.M,200);
                    break;

                case Keys.F1:
                    Invoke(() => { huan.SetVisibleCore2(!huan.Visible); });
                    break;
                case Keys.F2:
                    if (!FocusProcess("keyupMusic3"))
                    {
                        ProcessRun("C:\\Users\\bu\\source\\repos\\keyupMusic3\\bin\\Debug\\net8.0-windows\\keyupMusic3.exe");
                        HideProcess("keyupMusic3");
                    }
                    break;
                //case Keys.F4:
                //    Common.stop_listen = !Common.stop_listen;
                //    break;
                case Keys.F5:
                    log_always = !log_always;
                    break;
                case Keys.Up:
                    Invoke(() => huan.Opacity = huan.Opacity >= 1 ? 1 : huan.Opacity + 0.1);
                    break;
                case Keys.Down:
                    Invoke(() => huan.Opacity = huan.Opacity <= 0 ? 0 : huan.Opacity - 0.1);
                    break;
                case Keys.Escape:
                    if (is_ctrl() && is_shift()) { Process.Start(new ProcessStartInfo("taskmgr.exe")); break; }
                    press("LWin;1957,1015");
                    break;

                default:
                    catched = true;
                    break;
            }
            if (catched)
            {
                //Invoke((() => { label1.Text = label_backup; }));
                //KeyboardHook.stop_next = true;
            }
            Common.hooked = false;
        }
        private static void dragonest_run()
        {
            int asdf = 1000;
            while (asdf > 0)
            {
                if (judge_color(2223, 1325, Color.FromArgb(22, 155, 222)))
                {
                    press("2280,1314;LWin", 0);
                    break;
                }
                asdf -= 50;
                Thread.Sleep(50);
            }
            //press("2280,1314", 0);
            Task.Run(() =>
            {
                Thread.Sleep(3500);
                //Common.FocusProcess(Common.ACPhoenix);
                altab();
                //Common.FocusProcess(Common.Dragonest);
                press("500;2525,40;100", 0);
                mouse_move3();
            });
        }

        private static void dragonest_init()
        {
            press("10;LWin;500;1076,521", 101);
            var asd = 15000;
            int tick = 500;
            while (asd > 0)
            {
                if (judge_color(1797, 55, Color.FromArgb(18, 23, 33))) { press("2323, 30"); break; }
                Thread.Sleep(tick);
                asd -= tick;
            }
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
    }
}
