using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using static keyupMusic2.Native;
using Point = System.Drawing.Point;
using RECT = keyupMusic2.Native.RECT;

namespace keyupMusic2
{
    public partial class Common
    {
        public static ConcurrentDictionary<string, DateTime> KeyTime = new ConcurrentDictionary<string, DateTime>();
        

        public static List<string> list_go_back = new List<string> { explorer, VSCode, msedge, chrome, devenv, androidstudio, ApplicationFrameHost, cs2, steam, Glass, Glass2, Glass3 };


        public static bool hooked = false;
        public static bool stop_listen = false;
        public static bool ACPhoenix_mouse_hook = false;
        public static bool gcc_restart = false;

        public static string system_sleep_string = "system_sleep";
        public static int system_sleep_count = 0;
        public static bool ready_to_sleep = false;
          public static Point Position { get { return Cursor.Position; } }
        public static Point PositionMiddle = new Point(screenWidth2, screenHeight2);
        
        public static void CleanMouseState()
        {
            biu.catch_off();
            //biuCL.RECTT.release();
            CleanVirMouseState();
            biu.r_down_x = Point.Empty;
        }
      
        static string proc_info = "";
        public static string process_and_log(string key = "")
        {
            IntPtr hwnd = Native.GetForegroundWindow();
            string Title = GetWindowTitle(hwnd);
            bool IsFull = IsFullScreen(hwnd);

            var filePath = "a.txt";

            var Path = "err";
            var module_name = "err";
            var Name = "err";

            uint processId = 0;
            try
            {
                Native.GetWindowThreadProcessId(hwnd, out processId);
                using (Process process = Process.GetProcessById((int)processId))
                {
                    Path = process.MainModule.FileName;
                    module_name = process.MainModule.ModuleName;
                    Name = process.ProcessName;
                }
            }
            catch (System.Exception ex)
            {
                Path = ex.Message;
            }

            if (Name == "err") Name = "err,processId:" + processId;
            string txt = key;
            var curr_proc_info = new { key, Name, Title, Path, IsFull }.ToString();
            if (proc_info != curr_proc_info) txt = curr_proc_info;
            proc_info = curr_proc_info;

            log(txt);
            //log(DateTime.Now.ToString("") + " " + windowTitle + " " + fildsadsePath + module_nasme + "\n");
            return Name;
        }
        public static void log(string message)
        {
            Log.log(message);
        }
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
            //if (flag && action != null) action();
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
        public static void cmd(string cmd, Action action = null, int tick = 10)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = cmd,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };

            using (Process process = Process.Start(startInfo))
            {
                if (action == null) return;
                Sleep(tick);
                action();
            }
        }
      
      
        public static void TaskRun(Action action, int tick)
        {
            Task.Run(() =>
            {
                Thread.Sleep(tick);
                action();
            });
        }
        public static Color get_mouse_postion_color(Point point)
        {
            if (point.X > screenWidth)
            {
                Screen currentScreen = Screen.FromPoint(point);
                int relativeX = (point.X - currentScreen.Bounds.X) * 1920 / currentScreen.Bounds.Width;
                int relativeY = (point.Y - currentScreen.Bounds.Y) * 1080 / currentScreen.Bounds.Height;
                Console.WriteLine($"相对坐标：({relativeX}, {relativeY})");

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
        public static bool try_press(Color color, Action action = null)
        {
            return try_press(Position.X, Position.Y, color, action);
        }
        public static bool try_press2(int x, int y, Color color, Action action = null, int similar = 70)
        {
            //var pos = Position;
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
            //mouse_move(pos);
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
            try { if (!Debugger.IsAttached) bmpScreenshot.Dispose(); } catch (NullReferenceException e) { }
            play_sound_di();
            Screen secondaryScreen = Screen.PrimaryScreen;
            bmpScreenshot = new Bitmap(secondaryScreen.Bounds.Width, secondaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(new Point(0, 0), Point.Empty, secondaryScreen.Bounds.Size);
            string user_path = "C:\\Users\\bu\\Pictures\\Screenshots\\";
            string file_date_name = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            var path = "";
            if (ProcessName == Common.ACPhoenix) path = user_path + "dd\\" + file_date_name;
            else if (ProcessName == Common.chrome) path = "image\\encode\\" + file_date_name + "g";
            else path = user_path + file_date_name;
            bmpScreenshot_path = path;
            //bmpScreenshot.Save(path, ImageFormat.Png);
            //TaskRun(() => play_sound_di(), 80);
            gfxScreenshot.Dispose();
            //bmpScreenshot.Dispose();
        }
        public static void copy_secoed_screen(string path = "")
        {
            try { if (!Debugger.IsAttached) bmpScreenshot.Dispose(); } catch (NullReferenceException e) { }
            play_sound_di();
            Screen secondaryScreen = Screen.AllScreens.FirstOrDefault(scr => !scr.Primary);
            //int start_x = 2560;
            int start_x = -1920;
            if (secondaryScreen == null) { return; }
            bmpScreenshot = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(new Point(start_x, 0), Point.Empty, secondaryScreen.Bounds.Size);
            path = "image\\encode\\" + path + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png" + "g";
            bmpScreenshot_path = path;
            //bmpScreenshot.Save(path, ImageFormat.Png);
            //TaskRun(() => play_sound_di(), 80);
            gfxScreenshot.Dispose();
            //bmpScreenshot.Dispose();
        }
        public static void copy_ddzzq_screen()
        {
            Screen secondaryScreen = Screen.PrimaryScreen;
            if (secondaryScreen != null)
            {
                Bitmap bmpScreenshot = new Bitmap(2560, 1440, PixelFormat.Format32bppArgb);
                Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.CopyFromScreen(new Point(0, 0), Point.Empty, secondaryScreen.Bounds.Size);
                gfxScreenshot.Dispose();
                bmpScreenshot.Save("C:\\Users\\bu\\Pictures\\Screenshots\\dd\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png", ImageFormat.Png);
            }
        }
        public static void ProcessRun(string str)
        {
            try
            {
                ProcessStartInfo startInfo2 = new ProcessStartInfo(str);
                startInfo2.UseShellExecute = true;
                startInfo2.Verb = "runas";
                Process.Start(startInfo2);
            }
            catch { }
        }
        public static void DaleyRun(Func<bool> action, int alltime, int tick)
        {
            while (alltime > 0)
            {
                Thread.Sleep(tick);
                alltime -= tick;
                var asd = action.Invoke();
                if (asd) break;
            }
        }
        public static bool DaleyRun_stop = false;
        public static bool Special_Input2 = false;
        public static DateTime init_time = DateTime.Now;
        public static DateTime Special_Input_tiem = init_time;
        public static void DaleyRun(Func<bool> flag_action, Action action2, int alltime, int tick)
        {
            DaleyRun_stop = false;
            int i = 0;
            while (alltime >= 0)
            {
                if (i++ > 1000) break;
                if (DaleyRun_stop) break;
                Thread.Sleep(tick);
                alltime -= tick;
                var asd = flag_action.Invoke();
                if (asd) { Thread.Sleep(tick); action2(); break; }
            }
        }
        public static string DateTimeNow()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string DateTimeNow2()
        {
            return DateTime.Now.ToString("HH:mm:ss fff");
        }
        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        public static void Sleep(int tick)
        {
            Thread.Sleep(tick);
        }
        public static Point ProcessLocation(string pro)
        {
            IntPtr targetWindowHandle = GetProcessID(pro);

            if (targetWindowHandle != IntPtr.Zero)
            {
                RECT rect;
                GetWindowRect(targetWindowHandle, out rect);
                return new Point(rect.Left, rect.Top);
            }
            return new Point(0, 0);
        }
        public static bool IsFullScreen(IntPtr hWnd = 0)
        {
            if (hWnd == 0)
                hWnd = Native.GetForegroundWindow();
            Native.RECT windowRect;
            if (!Native.GetWindowRect(hWnd, out windowRect))
            {
                return false;
            }

            int screenWidth = Native.GetSystemMetrics(Native.SM_CXSCREEN);
            int screenHeight = Native.GetSystemMetrics(Native.SM_CYSCREEN);

            if (Position.X < 0 || Position.X > screenWidth)
            {
                return windowRect.Left == screen2Width &&
                   windowRect.Top == 0 &&
                   windowRect.Right == 0 &&
                   windowRect.Bottom == screen2Height;
            }
            //Thread.Sleep(1000); 
            // Check if the window covers the entire screen  
            return windowRect.Left == 0 &&
                   windowRect.Top == 0 &&
                   windowRect.Right == screenWidth &&
                   windowRect.Bottom == screenHeight;
        }
        public static void change_file_last(bool pngg)
        {
            // 指定要处理的文件夹路径  
            string folderPath = "image\\encode\\";
            string folderPath2 = "image\\encode\\2024\\";
            string folderPath3 = "image\\encode\\2025\\";

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
            var a = Directory.GetFiles(folderPath);
            var a2 = Directory.GetFiles(folderPath2);
            var a3 = Directory.GetFiles(folderPath3);
            var combinedArray = a.Concat(a2).Concat(a3).ToArray();

            foreach (string filePath in combinedArray)
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

        public static void HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream stream = response.GetResponseStream();

                using (StreamReader reader = new StreamReader(stream))
                {
                    string refJson = reader.ReadToEnd();

                    Console.WriteLine(refJson);
                    Console.Read();
                }
            }
        }
        public static bool IsPointClose(Point point1, Point point2, int diff = 100)
        {
            return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y) < 2 * diff;
        }
        public static Simulate SSSS { get { return new Simulate(100); } }
        public static Simulate S10 { get { return new Simulate(10); } }
        public static Simulate S100 { get { return new Simulate(100); } }
        public static Simulate Simm = new Simulate(0);
        public static Simulate SS(int tick = 100)
        {
            return new Simulate(tick);
        }
        public static IEnumerable<Keys> GetPressedKeys()
        {
            var keys = new List<Keys>();
            for (int i = 0; i < 256; i++)
            {
                //if (i == (int)Keys.Menu || i == (int)Keys.LMenu)
                //    continue;
                if (Native.GetAsyncKeyState(i) < 0)
                {
                    keys.Add((Keys)i);
                }
            }
            return keys;
        }
        public static Dictionary<Keys, string> GetVirPressedKeys()
        {
            IEnumerable<Keys> keys = GetPressedKeys();
            Dictionary<Keys, string> virPressedKeys = VirMouseStateKey;
            var mergedDictionary = virPressedKeys
                .Concat(keys.Select(k => new KeyValuePair<Keys, string>(k, null)))
                .ToLookup(kv => kv.Key, kv => kv.Value)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault());
            return mergedDictionary;
        }
        public static IEnumerable<Keys> release_all_keydown()
        {
            IEnumerable<Keys> pressedKeys = GetPressedKeys();
            foreach (var key in pressedKeys) SSSS.KeyUp(key);
            return pressedKeys;
        }
        public static bool IsMouseStopClick = false;
        public static bool isMouseStopped = true;
        public static bool QTCheck(DateTime dateTime, int ms)
        {
            var flag = DateTime.Now - dateTime < TimeSpan.FromMilliseconds(ms);
            return flag;
        }
        /// <summary>
         /// 获取指定类型中所有 public const string 成员
         /// </summary>
         /// <param name="type">要检查的类型</param>
         /// <returns>一个字典，键为常量名，值为常量值</returns>
        public static Dictionary<string, string> GetPublicConstStrings(Type type)
        {
            var result = new Dictionary<string, string>();
            // 获取该类型的所有公共字段
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                // 检查字段是否为常量且类型为 string
                if (field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(string))
                {
                    // 将常量名和值添加到结果字典中
                    result[field.Name] = (string)field.GetValue(null);
                }
            }
            return result;
        }
        public static void quick_max_chrome(Point point = new Point())
        {
            if (!ExistProcess(PowerToysCropAndLock, true)) return;
            if (point == new Point()) point = Position;
            if (ProcessName == chrome)
            {
                press(Keys.F11);
                CenterWindowOnScreen(chrome, true);
                altab();
                FocusProcess(PowerToysCropAndLock, false);
                if (ExistProcess(cs2)) { Sleep(10); mouse_click(); }
            }
            else
            {
                //放大
                HideProcess(PowerToysCropAndLock);
                if (ProcessName == cs2) point = PositionMiddle;
                var pp = new Point(point.X - 450, point.Y - 450);
                MoveProcessWindow(chrome, pp);
                mouse_click2(2);
                CenterWindowOnScreen(chrome, true);
                press(Keys.F11);
                if (ExistProcess(cs2)) { press_middle_bottom(); }

                //if (ProcessName2 != chrome)
                //{
                //    quick_max_chrome(point);
                //}
            }
            //FreshProcessName();
        }
        public static uint isVir = 3;
        public const uint isVirConst = 3;

        public static bool is_no_title(string targetWindowTitle)
        {
            IntPtr targetWindowHandle = GetProcessID(targetWindowTitle);

            if (targetWindowHandle != IntPtr.Zero)
            {
                int currentStyle = GetWindowLong(targetWindowHandle, GWL_STYLE);
                bool hasCaption = (currentStyle & WS_CAPTION) != 0;

                if (!hasCaption)
                    return true;
                return false;
            }
            return false;
        }
        public static int PowerToysCropAndLock_Height = 0;
        public static int PowerToysCropAndLock_Wight = 0;
        public static double PowerToysCropAndLock_delta = 1;
        public static bool hideProcessTitle(string targetWindowTitle)
        {
            IntPtr targetWindowHandle = GetProcessID(targetWindowTitle);

            if (targetWindowHandle != IntPtr.Zero)
            {
                // 获取当前窗口样式
                int currentStyle = GetWindowLong(targetWindowHandle, GWL_STYLE);
                bool hasCaption = (currentStyle & WS_CAPTION) != 0;

                if (!hasCaption)
                    return false;
                // 移除标题栏、边框和调整大小边框的样式
                int newStyle = currentStyle & ~(WS_CAPTION | WS_BORDER | WS_THICKFRAME);

                // 设置新的窗口样式
                SetWindowLong(targetWindowHandle, GWL_STYLE, newStyle);

                // 获取当前窗口的位置和大小
                RECT rect;
                GetWindowRect(targetWindowHandle, out rect);

                int newWidth = rect.Right - rect.Left - 22;
                int newHeight = rect.Bottom - rect.Top - 55;
                PowerToysCropAndLock_Height = newHeight;
                PowerToysCropAndLock_Wight = newWidth;


                // 更新窗口布局，使修改生效，并调整窗口大小
                SetWindowPos(targetWindowHandle, IntPtr.Zero, rect.Left, rect.Top, newWidth, newHeight, SWP_FRAMECHANGED | SWP_NOMOVE);
                Console.WriteLine("标题栏和边框已隐藏");
                return true;
            }
            else
            {
                Console.WriteLine("未找到目标窗口");
            }
            return false;
        }
        public static void change_process_size(double delta)
        {
            IntPtr targetWindowHandle = GetProcessID(ProcessName);
            if (targetWindowHandle != IntPtr.Zero)
            {
                // 获取当前窗口的矩形
                RECT rect;
                Native.GetWindowRect(targetWindowHandle, out rect);

                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;
                if (PowerToysCropAndLock_Wight != 0)
                {
                    width = PowerToysCropAndLock_Wight;
                    height = PowerToysCropAndLock_Height;
                }
                else
                {
                    PowerToysCropAndLock_Wight = width;
                    PowerToysCropAndLock_Height = height;
                }

                double scaleFactor = 1.1; // 缩放比例，可根据需要调整

                double aspectRatio = (double)width / height; // 计算原始宽高比

                int newWidth, newHeight;

                newWidth = (int)(width / delta);
                newHeight = (int)(newWidth / delta);

                // 计算新的左上角位置，以左下角为基准
                int newLeft = rect.Left;
                int newTop = rect.Bottom - newHeight;

                // 移动窗口并调整大小
                Native.MoveWindow(targetWindowHandle, newLeft, newTop, newWidth, newHeight, true);
            }
        }
        public static void CenterWindowOnScreen(string targetWindowTitle, bool right = false)
        {
            // 查找目标窗口的句柄
            IntPtr targetWindowHandle = GetProcessID(targetWindowTitle);
            if (targetWindowHandle == IntPtr.Zero)
            {
                Console.WriteLine($"未找到标题为 '{targetWindowTitle}' 的窗口。");
                return;
            }

            // 获取目标窗口的矩形信息
            RECT windowRect;
            if (!GetWindowRect(targetWindowHandle, out windowRect))
            {
                Console.WriteLine("无法获取窗口的矩形信息。");
                return;
            }

            // 获取屏幕的矩形信息
            RECT screenRect;
            GetWindowRect(Native.GetDesktopWindow(), out screenRect);

            // 计算窗口的宽度和高度
            int windowWidth = windowRect.Right - windowRect.Left;
            int windowHeight = windowRect.Bottom - windowRect.Top;

            // 计算窗口在屏幕中间的位置
            int newX = (screenWidth - windowWidth) + 12;
            int newY = (screenHeight - windowHeight) / 2 + 1;
            if (right) newX = screenWidth1 - 9;

            if (targetWindowTitle == chrome)
            {
                windowWidth = 1301;
                windowHeight = 861;
            }
            if (targetWindowTitle == wemeetapp)
            {
                windowWidth = 1927;
                windowHeight = 1132;
            }
            //if (targetWindowTitle == chrome)
            //{
            //    windowWidth = 1937;
            //    windowHeight = 1175;
            //}

            // 移动窗口到屏幕中间
            if (!MoveWindow(targetWindowHandle, newX, newY, windowWidth, windowHeight, true))
            {
                Console.WriteLine("无法移动窗口到指定位置。");
            }
        }
        public static void CenterWindowOnScreen2(string targetWindowTitle, bool right = false)
        {
            // 查找目标窗口的句柄
            IntPtr targetWindowHandle = GetProcessID(targetWindowTitle);
            if (targetWindowHandle == IntPtr.Zero)
            {
                Console.WriteLine($"未找到标题为 '{targetWindowTitle}' 的窗口。");
                return;
            }
            //// 获取当前窗口样式
            //int currentStyle = GetWindowLong(targetWindowHandle, GWL_STYLE);
            //bool hasCaption = (currentStyle & WS_CAPTION) != 0;
            //if (hasCaption)
            //{
            //    int newStyle = currentStyle & ~(WS_CAPTION | WS_BORDER | WS_THICKFRAME);
            //    SetWindowLong(targetWindowHandle, GWL_STYLE, newStyle);
            //}

            // 获取目标窗口的矩形信息
            RECT windowRect;
            if (!GetWindowRect(targetWindowHandle, out windowRect))
            {
                Console.WriteLine("无法获取窗口的矩形信息。");
                return;
            }


            double windowWidth = 2904;
            double windowHeight = 1762;

            // 更新窗口布局，使修改生效，并调整窗口大小
            SetWindowPos(targetWindowHandle, IntPtr.Zero, 0, 0, (int)windowWidth, (int)windowHeight, SWP_FRAMECHANGED | SWP_NOMOVE);
            //// 移动窗口到屏幕中间
            //if (!MoveWindow(targetWindowHandle, newX, newY, windowWidth, windowHeight, true))
            //{
            //    Console.WriteLine("无法移动窗口到指定位置。");
            //}
        }
        public static void MoveProcessWindow(IntPtr targetWindowHandle, Point point)
        {
            RECT windowRect;
            GetWindowRect(targetWindowHandle, out windowRect);

            int windowWidth = windowRect.Right - windowRect.Left;
            int windowHeight = windowRect.Bottom - windowRect.Top;

            MoveWindow(targetWindowHandle, point.X, point.Y, windowWidth, windowHeight, true);
        }
        public static void MoveProcessWindow(string targetWindowTitle, Point point)
        {
            IntPtr targetWindowHandle = GetProcessID(targetWindowTitle);
            RECT windowRect;
            GetWindowRect(targetWindowHandle, out windowRect);

            int windowWidth = windowRect.Right - windowRect.Left;
            int windowHeight = windowRect.Bottom - windowRect.Top;

            MoveWindow(targetWindowHandle, point.X, point.Y, windowWidth, windowHeight, true);
        }
        public static void MoveProcessWindow2(string targetWindowTitle)
        {
            IntPtr targetWindowHandle = GetProcessID(targetWindowTitle);
            RECT windowRect;
            GetWindowRect(targetWindowHandle, out windowRect);

            int windowWidth = windowRect.Right - windowRect.Left;
            int windowHeight = windowRect.Bottom - windowRect.Top;
            int dsad = screenHeight - windowHeight;

            MoveWindow(targetWindowHandle, 0, dsad, windowWidth, windowHeight, true);
        }
        // 系统指标常量
        private const int SM_CYCAPTION = 4;
        public static bool IsClickOnTitleBar(string ProcessName, Point clickPosition)
        {
            // 查找目标窗口的句柄
            IntPtr windowHandle = GetProcessID(ProcessName);
            // 获取窗口的矩形信息
            RECT windowRect;
            if (!GetWindowRect(windowHandle, out windowRect))
            {
                return false;
            }

            // 获取标题栏的高度
            int captionHeight = GetSystemMetrics(SM_CYCAPTION);

            // 将鼠标点击的屏幕坐标转换为窗口的客户区坐标
            Point clientPoint = clickPosition;
            ScreenToClient(windowHandle, ref clientPoint);

            // 判断点击位置是否在标题栏内
            return clientPoint.Y >= 0 && clientPoint.Y < captionHeight;
        }
        public static void LossScale()
        {
            //press([Keys.LControlKey, Keys.F2]);return;
            if (!ExistProcess(LosslessScaling))
                ProcessRun(LosslessScalingexe);
            press([Keys.LControlKey, Keys.F2]);

            if (Position.X > 0) 
                press_middle_bottom();
        }
        public static void system_hard_sleep()
        {
            gcc_restart = true;
            KeyTime[system_sleep_string] = DateTime.Now;
            system_sleep_count = 0;
            Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0");
        }

        public static void quick_left_right(int arraw)
        {
            IntPtr hwnd = GetForegroundWindow();
            int tick = 100;
            Sleep(tick);

            var full = IsFullScreen(hwnd);
            if (full)
            {
                press(Keys.F11);
                Sleep(tick);
            }

            ShowWindow((hwnd), SW.SW_SHOWNORMAL);
            Sleep(tick);

            var pp = new Point(100, 100);
            if (arraw == 2) pp = new Point(screen2Width + 100, 100);
            MoveProcessWindow(hwnd, pp);
            Sleep(tick);

            RECT windowRect;
            GetWindowRect(hwnd, out windowRect);
            int windowWidth = windowRect.Right - windowRect.Left;
            if (windowWidth > 3000 || windowWidth < 1000)
            {
                SetWindowPos(hwnd, IntPtr.Zero, pp.X, pp.Y, 1800, 920, SWP_FRAMECHANGED | SWP_NOMOVE);
                Sleep(tick);
            }

            ShowWindow((hwnd), SW.SW_SHOWMAXIMIZED);
            Sleep(tick);

            if (full)
                SS().KeyPress(Keys.F11);
        }
        public static class RateLimiter
        {
            // 基础策略：记录上次执行时间
            private static DateTime lastExecuteTime = DateTime.MinValue;
            private static readonly TimeSpan ExecutionInterval = TimeSpan.FromMilliseconds(400);

            // 过载策略：记录1秒内的调用次数
            private static int callCount = 0;
            private static DateTime windowStart = DateTime.MinValue;
            private const int OverloadThreshold = 5;

            // 线程安全锁
            private static readonly object _lock = new object();

            public static void Execute<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2)
            {
                lock (_lock)
                {
                    var now = DateTime.Now;

                    // 重置窗口计数器（如果窗口已过期）
                    if (now - windowStart > ExecutionInterval)
                    {
                        windowStart = now;
                        callCount = 0;
                    }

                    callCount++;

                    // 策略1：基础频率限制（每秒最多1次）
                    bool canExecuteBase = now - lastExecuteTime >= ExecutionInterval;

                    // 策略2：过载放行（1秒内调用超过5次时，放行第6次）
                    bool canExecuteOverload = callCount % OverloadThreshold == 0;

                    if (callCount >= 1.4 * OverloadThreshold)
                    {
                        lastExecuteTime = now;
                        callCount = 0; // 执行后重置计数器
                        action(arg1, arg2); // 执行带参数的方法
                    }
                    else if (callCount >= 2 * OverloadThreshold)
                    {
                        lastExecuteTime = now;
                        callCount = 0; // 执行后重置计数器
                        action(arg1, arg2); // 执行带参数的方法
                        action(arg1, arg2); // 执行带参数的方法
                    }
                    else if (canExecuteBase || canExecuteOverload)
                    {
                        lastExecuteTime = now;
                        callCount = 0; // 执行后重置计数器
                        action(arg1, arg2); // 执行带参数的方法
                    }
                }
            }
        }
        public const uint WM_USER = 0x0400;
        public const uint CUSTOM_MESSAGE = WM_USER + 100;

        // 向指定窗口发送消息
        public static void SendMessageToWindow(string procName, string message)
        {
            int currentProcessId = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcessesByName(procName);
            foreach (Process process in processes)
            {
                if (process.Id != currentProcessId && !process.MainWindowHandle.Equals(IntPtr.Zero))
                {
                    // 获取主窗口句柄
                    IntPtr hWnd = process.MainWindowHandle;

                    // 发送消息
                    IntPtr lParam = Marshal.StringToHGlobalUni(message);
                    PostMessage(hWnd, CUSTOM_MESSAGE, IntPtr.Zero, lParam);
                    Marshal.FreeHGlobal(lParam);
                }
            }
        }
        //public static List<ReplaceKey> replace = ReplaceKey.replace;
        public static bool lock_err
        {
            get
            {
                var aaa =
                 ProcessName == err || ProcessName == LockApp || GetWindowText() == UnlockingWindow;
                if (aaa)
                {
                    string _ProcessName = ProcessName;
                    if (_ProcessName == "err")
                    {
                        uint processId = 0;
                        Native.GetWindowThreadProcessId(Native.GetForegroundWindow(), out processId);
                        _ProcessName = "err,processId:" + processId;
                    }
                    Log.log("GetWindowText():" + GetWindowText() + ",ProcessName:" + ProcessName);
                    if (ProcessName == err)
                        play_sound_di2();
                }
                return aaa;
            }
        }
        // 获取当前窗口的缩放比例
        public static double GetWindowScalingFactor(Form form)
        {
            uint dpi = GetDpiForWindow(form.Handle);
            return dpi / 96.0;
        }
        
        public static void Show(string msg)
        {
            Socket.socket_write(Huan.huan_invoke + ProcessName + " " + msg);
        }


    }
}
