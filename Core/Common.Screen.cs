using Microsoft.Win32;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using static keyupMusic4.Native;
using Point = System.Drawing.Point;
using RECT = keyupMusic4.Native.RECT;
namespace keyupMusic4
{
    public partial class Common
    {
        public static bool judge_color(Color color, Action action = null, int similar = 50)
        {
            int x = Position.X;
            int y = Position.Y;
            var asd = get_mouse_postion_color(new Point(x, y));
            var flag = AreColorsSimilar(asd, color, similar);
            if (flag && action != null) action();
            return flag;
        }
        public static bool judge_color(int x, int y, Color color, int similar = 50)
        {
            x = deal_size_x_y(x, y, false)[0];
            y = deal_size_x_y(x, y, false)[1];
            var asd = get_mouse_postion_color(new Point(x, y));
            var flag = AreColorsSimilar(asd, color, similar);
            return flag;
        }

        public static bool try_press(int x, int y, Color color, Action action = null)
        {
            x = deal_size_x_y(x, y)[0];
            y = deal_size_x_y(x, y)[1];
            var asd = get_mouse_postion_color(new Point(x, y));
            var flag = AreColorsSimilar(asd, color);
            if (flag)
            {
                var flag2 = action != null;
                press(x + "," + y, flag2 ? 100 : 101);
                if (flag2) action();
                else lastPosition = Position;
            }
            return flag;
        }

        public static Color get_mouse_postion_color(Point point)
        {
            try
            {
                if (point.X > screenWidth)
                {
                    Screen currentScreen = Screen.FromPoint(point);
                    int relativeX = (point.X - currentScreen.Bounds.X) * 1920 / currentScreen.Bounds.Width;
                    int relativeY = (point.Y - currentScreen.Bounds.Y) * 1080 / currentScreen.Bounds.Height;

                    Bitmap bmpScreenshot = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
                    Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                    gfxScreenshot.CopyFromScreen(new Point(screenWidth, 0), Point.Empty, currentScreen.Bounds.Size);

                    return bmpScreenshot.GetPixel(relativeX, relativeY);
                }
                if (point.X < 0)
                {
                    point = new Point((int)(point.X / 1.5), (int)(point.Y / 1.5));
                }
                using (Bitmap bitmap = new Bitmap(1, 1))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(point.X, point.Y, 0, 0, new System.Drawing.Size(1, 1));
                        return bitmap.GetPixel(0, 0);
                    }
                }
            }
            catch (Exception) { }
            return Color.Black;
        }

        public static bool try_press(Color color, Action action = null)
        {
            return try_press(Position.X, Position.Y, color, action);
        }

        public static bool try_press2(int x, int y, Color color, Action action = null, int similar = 70)
        {
            mouse_move(x, y);
            Thread.Sleep(10);
            var asd = get_mouse_postion_color(new Point(x, y));
            var flag = AreColorsSimilar(asd, color, similar);
            if (flag)
            {
                var flag2 = action != null;
                press(x + "," + y, flag2 ? 100 : 101);
                if (flag2) action();
                else lastPosition = Position;
            }
            log("try_press:" + x + "," + y + "," + color.R + "," + color.G + "," + color.B + " " + asd.R + "," + asd.G + "," + asd.B);
            return flag;
        }

        public static bool AreColorsSimilar(Color color1, Color color2, int threshold = 50)
        {
            if (color1 == color2) return true;
            int rDiff = Math.Abs(color1.R - color2.R);
            int gDiff = Math.Abs(color1.G - color2.G);
            int bDiff = Math.Abs(color1.B - color2.B);

            return (rDiff + gDiff + bDiff) <= threshold;
        }

        public static void dragonest_notity_click(bool repeat = false)
        {
            Bitmap bitmap = new Bitmap(500, 1);
            int startX = 1800;
            int startY = 1397;
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(startX, startY, 0, 0, new System.Drawing.Size(500, 1));
            Rectangle rect = new Rectangle(0, 0, 500, 1);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = Math.Abs(bmpData.Stride) * bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
            bitmap.UnlockBits(bmpData);
            bool flag = false;
            var ds11a = DateTime.Now.ToString("ssfff");

            for (int i = 0; i < 500; i++)
            {
                int baseIndex = i * 4; // 每个像素4个字节（ARGB）  
                if (rgbValues[baseIndex + 2] == 233 && rgbValues[baseIndex + 1] == 81 && rgbValues[baseIndex] == 81)
                {
                    press($"{startX + i}, {startY}", 0);
                    i = 600;
                    flag = true;
                    break;
                }
            }
            if (flag == false && repeat == false)
            {
                press(Keys.LWin);
                Thread.Sleep(500);
                dragonest_notity_click(true);
            }
        }

        public static Bitmap bmpScreenshot;
        public static string bmpScreenshot_path;
        public static void copy_screen()
        {
            play_sound_di();
            Screen secondaryScreen = Screen.PrimaryScreen;
            Bitmap bmpScreenshot = new Bitmap(secondaryScreen.Bounds.Width, secondaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(new Point(0, 0), Point.Empty, secondaryScreen.Bounds.Size);
            string user_path = "C:\\Users\\bu\\Pictures\\Screenshots\\";
            string file_date_name = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            string name = ExtractStreamerName(processWrapper.title);
            if (!string.IsNullOrEmpty(name)) name += "_";
            file_date_name = name + file_date_name;
            var path = "";
            if (ProcessName == Common.ACPhoenix) path = user_path + "dd\\" + file_date_name;
            else if (ProcessName == Common.chrome) path = "image\\encode\\" + file_date_name + "g";
            else path = user_path + file_date_name;
            bmpScreenshot_path = path;
            bmpScreenshot.Save(path, ImageFormat.Png);
            gfxScreenshot.Dispose();
            bmpScreenshot.Dispose();
        }
        public static void copy_secoed_screen()
        {
            Screen secondaryScreen = Screen.AllScreens.FirstOrDefault(scr => !scr.Primary);
            int start_x = screenWidth;
            if (screen2Width < 0)
                start_x = -1920;
            if (secondaryScreen == null) { return; }
            play_sound_di();
            Bitmap bmpScreenshot = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(new Point(start_x, 0), Point.Empty, secondaryScreen.Bounds.Size);
            string path = "";
            path = "image\\encode\\" + path + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png" + "g";
            bmpScreenshot_path = path;
            bmpScreenshot.Save(path, ImageFormat.Png);
            gfxScreenshot.Dispose();
            bmpScreenshot.Dispose();
        }
        public static void copy_ddzzq_screen()
        {
            Screen secondaryScreen = Screen.PrimaryScreen;
            if (secondaryScreen != null)
            {
                Bitmap bmpScreenshot = new Bitmap(DesignWidth, DesignHeight, PixelFormat.Format32bppArgb);
                Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.CopyFromScreen(new Point(0, 0), Point.Empty, secondaryScreen.Bounds.Size);
                gfxScreenshot.Dispose();
                bmpScreenshot.Save("C:\\Users\\bu\\Pictures\\Screenshots\\dd\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png", ImageFormat.Png);
            }
        }

        public static void change_file_last(bool pngg)
        {
            // 要处理的文件夹列表（自动跳过不存在的目录）
            string[] folders = new[]
            {
        "image\\encode\\",
        "image\\encode\\2024\\",
        "image\\encode\\2025\\",
        "image\\encode\\2026\\",
        "image\\encode\\2027\\",
        "image\\encode\\2028\\",
        "image\\encode\\2029\\"
    };

            // 指定旧后缀和新后缀（不包含点号）
            string oldExtension = "pngg";
            string newExtension = "png";
            if (pngg) { oldExtension = "png"; newExtension = "pngg"; }

            int processedCount = 0;

            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    Console2.WriteLine($"目录不存在，已跳过: {folder}");
                    continue;
                }

                try
                {
                    // 使用 EnumerateFiles 流式遍历，节省内存
                    foreach (var filePath in Directory.EnumerateFiles(folder))
                    {
                        try
                        {
                            var ext = Path.GetExtension(filePath)?.TrimStart('.').ToLowerInvariant();
                            if (ext != oldExtension) continue;

                            var fi = new FileInfo(filePath);

                            //// 非空判断与大小范围过滤（仅处理大小在 2000 ~ 2100 字节之间的文件）

                            string newFilePath = Path.Combine(fi.DirectoryName ?? folder, Path.GetFileNameWithoutExtension(fi.Name) + "." + newExtension);

                            // 避免覆盖已存在文件
                            if (File.Exists(newFilePath))
                            {
                                Console2.WriteLine($"目标文件已存在，跳过: {newFilePath}");
                                continue;
                            }

                            File.Move(filePath, newFilePath);
                            Console2.WriteLine($"文件已更改: {filePath} -> {newFilePath}");
                            processedCount++;
                        }
                        catch (Exception exFile)
                        {
                            Console2.WriteLine($"处理文件失败: {filePath}，错误: {exFile.Message}");
                        }
                    }
                }
                catch (Exception exDir)
                {
                    Console2.WriteLine($"遍历目录失败: {folder}，错误: {exDir.Message}");
                }
            }

            Console2.WriteLine($"完成：共处理 {processedCount} 个文件（大小范围 2000~2100 字节，依据扩展名筛选）。");
        }


        public static bool SetResolution(int mode)
        {
            int width = 1920, height = 1080, freq = 144;
            if (mode == 2)
            {
                width = 3840;
                height = 2160;
                freq = 120;
            }

            DEVMODE devMode = new DEVMODE();
            devMode.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));

            if (!EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref devMode))
            {
                Console.WriteLine("无法获取当前显示设置");
                return false;
            }

            devMode.dmPelsWidth = width;
            devMode.dmPelsHeight = height;
            devMode.dmDisplayFrequency = freq;
            devMode.dmBitsPerPel = 32; // 推荐设置为32位色

            devMode.dmFields = DM_PELSWIDTH | DM_PELSHEIGHT | DM_DISPLAYFREQUENCY | DM_BITSPERPEL;

            int testResult = ChangeDisplaySettings(ref devMode, CDS_TEST);
            if (testResult != DISP_CHANGE_SUCCESSFUL)
            {
                Console.WriteLine($"不支持{width}×{height} @{freq}Hz模式，错误码: {testResult}");
                return false;
            }

            int result = ChangeDisplaySettings(ref devMode, CDS_UPDATEREGISTRY);
            if (result == DISP_CHANGE_SUCCESSFUL)
            {
                Console.WriteLine($"成功设置为{width}×{height} @{freq}Hz");
                return true;
            }
            else if (result == DISP_CHANGE_BADMODE)
            {
                Console.WriteLine("该显示模式不受支持");
            }
            else
            {
                Console.WriteLine($"设置失败，错误码: {result}");
            }
            return false;
        }        /// <summary>
                 /// 判断窗口是否已经置顶
                 /// </summary>
    }
}
