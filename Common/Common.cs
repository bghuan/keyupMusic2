using System.Diagnostics;
using System.Drawing.Imaging;
using System.Media;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static keyupMusic2.Native;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Point = System.Drawing.Point;

namespace keyupMusic2
{
    public partial class Common
    {
        public static Dictionary<string, DateTime> KeyTime = new Dictionary<string, DateTime>();
        public const string keyupMusic2 = "keyupMusic4";
        public const string ElecHead = "ElecHead";
        public const string Windblown = "Windblown";
        public const string ACPhoenix = "ACPhoenix";
        public const string Dragonest = "DragonestGameLauncher";
        public const string devenv = "devenv";
        public const string WeChat = "WeChat";
        public const string douyin = "douyin";
        public const string msedge = "msedge";
        public const string chrome = "chrome";
        public const string Taskmgr = "Taskmgr";
        public const string explorer = "explorer";
        public const string SearchHost = "SearchHost";
        public const string QQMusic = "QQMusic";
        public const string HuyaClient = "HuyaClient";
        public const string QyClient = "QyClient";
        public const string ApplicationFrameHost = "ApplicationFrameHost";
        public const string QQLive = "QQLive";
        public const string vlc = "vlc";
        public const string v2rayN = "v2rayN";
        public const string Thunder = "Thunder";
        public const string VSCode = "Code";
        public const string AIoT = "AIoT IDE";
        public const string StartMenuExperienceHost = "StartMenuExperienceHost";
        public const string RadeonSoftware = "RadeonSoftware";
        public const string Glass = "Glass";
        public const string Glass2 = "Illusions";
        public const string Glass3 = "Glass Masquerade 3";
        public const string steam = "steamwebhelper";
        public const string Kingdom = "Kingdom Rush Vengeance";
        public const string Human = "Human";
        public const string ItTakesTwo = "ItTakesTwo";
        public const string Ghostrunner2 = "Ghostrunner2-Win64-Shipping";
        public const string bilibili = "bilibili";
        public const string UnlockingWindow = "UnlockingWindow";
        public const string LockApp = "LockApp";
        public const string Kingdom5 = "Kingdom Rush Alliance";
        public const string err = "err";
        public const string WeChatAppEx = "WeChatAppEx";
        public const string cs2 = "cs2";
        public const string PowerToysCropAndLock = "PowerToys.CropAndLock";
        public const string Broforce_beta = "Broforce_beta";
        public const string oriwotw = "oriwotw";
        public const string LosslessScaling = "LosslessScaling";
        public const string LosslessScalingexe = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Lossless Scaling\\LosslessScaling.exe";
        public const string wemeetapp = "wemeetapp";
        public const string SplitFiction = "SplitFiction";
        public const string gcc = "GCC";
        public const string gccexe = "C:\\Program Files\\GIGABYTE\\Control Center\\GCC.exe";
        public const string TwinkleTray = "Twinkle Tray";
        public const string TwinkleTrayexe = "C:\\Users\\bu\\AppData\\Local\\Programs\\twinkle-tray\\Twinkle Tray.exe";
        public const string androidstudio = "studio64";

        public static List<string> list_go_back = new List<string> { explorer, VSCode, msedge, chrome, devenv, androidstudio, ApplicationFrameHost, cs2 };

        public static SoundPlayer player = new SoundPlayer();
        public static SoundPlayer player2 = new SoundPlayer();
        public static bool hooked = false;
        public static bool stop_listen = false;
        public static bool ACPhoenix_mouse_hook = false;
        public static DateTime special_delete_key_time;
        public static bool gcc_restart = false;

        public static string ProcessName = "";
        public static string ProcessTitle = "";
        public static string ProcessPath = "";
        public static string system_sleep_string = "system_sleep";
        public static int system_sleep_count = 0;
        public static bool lock_err
        {
            get
            {
                var aaa =
                GetWindowText() == UnlockingWindow || ProcessName == LockApp || ProcessName == err;
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
        public static string ProcessName2
        {
            get
            {
                FreshProcessName();
                return ProcessName;
            }
            set
            {
                ProcessName = "";
            }
        }
        public static Point Position
        {
            get
            {
                return Cursor.Position;
            }
        }
        public static Point PositionMiddle = new Point(screenWidth2, screenHeight2);
        public static bool is_douyin()
        {
            return ProcessName == douyin || ProcessTitle?.IndexOf("抖音") >= 0 || (ProcessName == msedge && ProcessTitle?.IndexOf("多多自走棋") >= 0);
        }
        public static bool is_steam_game()
        {
            return ProcessPath != null && ProcessPath.Contains("steam");
        }
        static IntPtr old_hwnd = 0;

        //public static Dictionary<IntPtr, string> ProcessMap = new Dictionary<IntPtr, string>();
        //public static string FreshProcessName()
        //{
        //    IntPtr hwnd = Native.GetForegroundWindow(); // 获取当前活动窗口的句柄
        //    if (ProcessMap.ContainsKey(hwnd))
        //    {
        //        Common.ProcessName = ProcessMap[hwnd].Split(";;;;")[0];
        //        ProcessTitle = ProcessMap[hwnd].Split(";;;;")[1];
        //        ProcessPath = ProcessMap[hwnd].Split(";;;;")[2];
        //        if (Common.ProcessName == msedge)
        //            ProcessTitle = GetWindowText(hwnd); ;
        //        return Common.ProcessName;
        //    }
        //    //if (hwnd == old_hwnd) return Common.ProcessName;
        //    old_hwnd = hwnd;

        //    string windowTitle = GetWindowText(hwnd);
        //    ProcessTitle = string.IsNullOrEmpty(windowTitle) ? "" : windowTitle;
        //    //Console.WriteLine("当前活动窗口名称: " + windowTitle);

        //    var filePath = "a.txt";
        //    var fildsadsePath = "err";
        //    var module_name = "err";
        //    var ProcessName = "err";

        //    try
        //    {
        //        uint processId;
        //        Native.GetWindowThreadProcessId(hwnd, out processId);
        //        using (Process process = Process.GetProcessById((int)processId))
        //        {
        //            fildsadsePath = process.MainModule.FileName;
        //            ProcessPath = fildsadsePath;
        //            module_name = process.MainModule.ModuleName;
        //            ProcessName = process.ProcessName;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        fildsadsePath = ex.Message;
        //    }

        //    //log(DateTime.Now.ToString("") + " " + windowTitle + " " + fildsadsePath + module_nasme + "\n");
        //    Common.ProcessName = ProcessName;
        //    lock (ProcessMap)
        //    {
        //        if (!ProcessMap.ContainsKey(hwnd))
        //            ProcessMap.Add(hwnd, ProcessName + ";;;;" + ProcessTitle + ";;;;" + ProcessPath);
        //    }
        //    return ProcessName;
        //}



        public static Dictionary<IntPtr, ProcessWrapper> ProcessMap2 = new Dictionary<IntPtr, ProcessWrapper>();
        public class ProcessWrapper
        {
            public string name { get; set; }
            public string title { get; set; }
            public string path { get; set; }

            public ProcessWrapper(string name, string title, string path)
            {
                this.name = name;
                this.title = title;
                this.path = path;
            }
        }
        public static string FreshProcessName()
        {
            IntPtr hwnd = Native.GetForegroundWindow(); // 获取当前活动窗口的句柄
            if (ProcessMap2.ContainsKey(hwnd) && ProcessMap2[hwnd].title != null)
            {
                Common.ProcessName = ProcessMap2[hwnd].name;
                ProcessTitle = ProcessMap2[hwnd].title;
                ProcessPath = ProcessMap2[hwnd].path;
                if (Common.ProcessName == msedge)
                    ProcessTitle = GetWindowText(hwnd); ;
                return Common.ProcessName;
            }
            lock (ProcessMap2)
            {
                string windowTitle = GetWindowText(hwnd);
                ProcessTitle = string.IsNullOrEmpty(windowTitle) ? "" : windowTitle;
                var ProcessName = "none";

                try
                {
                    uint processId;
                    Native.GetWindowThreadProcessId(hwnd, out processId);
                    using (Process process = Process.GetProcessById((int)processId))
                    {
                        Common.ProcessName = process.ProcessName;

                        ProcessMap2.Add(hwnd, new ProcessWrapper(process.ProcessName, ProcessTitle, process.MainModule.FileName));
                    }
                }
                catch (System.Exception ex)
                {
                }
            }

            return ProcessName;
        }
        public static string GetWindowText()
        {
            IntPtr hwnd = Native.GetForegroundWindow(); // 获取当前活动窗口的句柄

            string windowTitle = GetWindowText(hwnd);
            //Console.WriteLine("当前活动窗口名称: " + windowTitle);

            return windowTitle;
        }
        public static string GetPointName(IntPtr hwnd)
        {
            IntPtr hWnd = Native.WindowFromPoint(Position);
            var aaa = GetWindowName(hWnd);
            return aaa;
        }
        public static string GetWindowName(IntPtr hwnd)
        {
            uint processId;
            string Name;
            Native.GetWindowThreadProcessId(hwnd, out processId);
            using (Process process = Process.GetProcessById((int)processId))
            {
                Name = process.ProcessName;
            }
            return Name;
        }
        static string proc_info = "";
        public static string log_process(string key = "")
        {
            IntPtr hwnd = Native.GetForegroundWindow();
            string Title = GetWindowText(hwnd);
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
        public static bool FocusProcess(string procName, bool front = true)
        {
            IntPtr current_hwnd = GetForegroundWindow(); // 获取当前活动窗口的句柄
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = IntPtr.Zero;
                hWnd = objProcesses[0].MainWindowHandle;
                if (current_hwnd == hWnd)
                    return true;
                ShowWindow((hWnd), SW.SW_RESTORE);
                if (procName != Dragonest && procName != chrome && procName != devenv)
                    ShowWindowAsync(new HandleRef(null, hWnd), SW_RESTORE);
                //ShowWindow((hWnd), SW.SW_SHOWMAXIMIZED);
                //ShowWindow((hWnd), SW.SW_SHOW);
                //ShowWindow((hWnd), SW.SW_SHOWNA);
                if (!front) return true;
                SetForegroundWindow(objProcesses[0].MainWindowHandle);
                Common.ProcessName = objProcesses[0].ProcessName;
                return true;
            }
            return false;
        }
        public static bool TryFocusProcess(string process)
        {
            var flag = ExistProcess(process);
            if (flag)
                if (process == ProcessName)
                    HideProcess(process);
                else
                    FocusProcess(process);
            return flag;
        }
        public static IntPtr GetProcessID(string procName)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                if (objProcesses[0].MainWindowHandle == IntPtr.Zero)
                {
                    for (int i = 1; i < objProcesses.Length; i++)
                    {
                        if (objProcesses[i].MainWindowHandle != IntPtr.Zero)
                            return objProcesses[i].MainWindowHandle;
                    }
                }
                return objProcesses[0].MainWindowHandle;
            }
            return nint.Zero;
        }
        public static bool SetWindowTitle(string window, string title)
        {
            string targetClassName = window;
            IntPtr hWnd = GetProcessID(targetClassName);
            bool result = SetWindowText(hWnd, title);
            return result;
        }
        public static bool ExistProcess(string procName, bool front = false)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                if (objProcesses[0].MainWindowHandle == 0 && front)
                    return false;
                return true;
            }
            return false;
        }

        public static void HideProcess(string procName)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = objProcesses[0].MainWindowHandle;
                Native.ShowWindow(hWnd, Native.SW.SW_MINIMIZE);
            }
        }

        public static void CloseProcess(string procName)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = objProcesses[0].MainWindowHandle;
                PostMessage(hWnd, (uint)WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static void CloseProcessFoce(string procName)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                foreach (Process proc in objProcesses)
                {
                    proc.Kill();
                }
            }
        }
        public static void CloseProcess()
        {
            IntPtr hwnd = Native.GetForegroundWindow();
            PostMessage(hwnd, (uint)WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
        public static string AltTabProcess()
        {
            altab(100);
            return FreshProcessName();
        }
        private void load_point()
        {
            string point = File.ReadAllText("point.txt");
            if (point == "") point = "0,0";
            int x = int.Parse(point.Split(',')[0]);
            int y = int.Parse(point.Split(',')[1]);
            //points[0] = new Point(x, y);
        }


        public static string GetWindowText(IntPtr hWnd)
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            if (Native.GetWindowText(hWnd, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return "";
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
        public static void copy_screen()
        {
            play_sound_di();
            Screen secondaryScreen = Screen.PrimaryScreen;
            Bitmap bmpScreenshot = new Bitmap(secondaryScreen.Bounds.Width, secondaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(new Point(0, 0), Point.Empty, secondaryScreen.Bounds.Size);
            string user_path = "C:\\Users\\bu\\Pictures\\Screenshots\\";
            string file_date_name = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            string path = "";
            if (ProcessName == Common.ACPhoenix) path = user_path + "dd\\" + file_date_name;
            else if (ProcessName == Common.chrome) path = "image\\encode\\" + file_date_name + "g";
            else path = user_path + file_date_name; ;
            bmpScreenshot.Save(path, ImageFormat.Png);
            TaskRun(() => play_sound_di(), 80);
            gfxScreenshot.Dispose();
            bmpScreenshot.Dispose();
        }
        public static void copy_secoed_screen(string path = "")
        {
            play_sound_di();
            Screen secondaryScreen = Screen.AllScreens.FirstOrDefault(scr => !scr.Primary);
            //int start_x = 2560;
            int start_x = -1920;
            if (secondaryScreen == null) { return; }
            Bitmap bmpScreenshot = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(new Point(start_x, 0), Point.Empty, secondaryScreen.Bounds.Size);
            bmpScreenshot.Save("image\\encode\\" + path + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png" + "g", ImageFormat.Png);
            TaskRun(() => play_sound_di(), 80);
            gfxScreenshot.Dispose();
            bmpScreenshot.Dispose();
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
        public static bool Special_Input { get { return is_down(Keys.F2) || Position.X == 0; } set { } }
        public static bool Special_Input2 = false;
        public static DateTime init_time = DateTime.Now;
        public static DateTime Special_Input_tiem = init_time;
        public static void DaleyRun(Func<bool> flag_action, Action action2, int alltime, int tick)
        {
            DaleyRun_stop = false;
            int i = 0;
            while (alltime >= 0)
            {
                if (i > 6000) break;
                if (DaleyRun_stop) break;
                Thread.Sleep(tick);
                alltime -= tick;
                var asd = flag_action.Invoke();
                if (asd) { action2(); break; }
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
        public static Point ProcessPosition(string pro)
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
            if (hWnd == IntPtr.Zero)
            {
                // No foreground window found  
                return false;
            }

            Native.RECT windowRect;
            if (!Native.GetWindowRect(hWnd, out windowRect))
            {
                // Failed to get window rectangle  
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

        public static bool key_sound = true;
        public static void play_sound(Keys key)
        {
            if (is_down(Keys.LWin)) return;
            if (Position.Y == 0) return;
            //if (key_sound && keys.Contains(e.key))
            if (key_sound)
            {
                string wav = "wav\\" + key.ToString().Replace("D", "").Replace("F", "") + ".wav";
                if (!File.Exists(wav)) return;

                player = new SoundPlayer(wav);
                player.Play();
            }
        }
        public static void play_sound_bongocat(Keys key)
        {
            string wav = "wav\\bongocat\\keyboard" + key.ToString().Replace("D", "") + ".wav";
            if (!File.Exists(wav)) return;

            player = new SoundPlayer(wav);
            player.Play();
        }
        static string wav = "wav\\d2.wav";
        static bool is_di = File.Exists(wav);
        public static void play_sound_di(int tick = 0)
        {
            if (!is_di) return;

            player = new SoundPlayer(wav);
            player.Play();
            Sleep(tick);
        }
        public static void play_sound_di2(int tick = 0)
        {
            play_sound_di();
            TaskRun(() => play_sound_di(), 80);
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
                if (i == (int)Keys.Menu || i == (int)Keys.LMenu)
                    continue;
                if (Native.GetAsyncKeyState(i) < 0)
                {
                    keys.Add((Keys)i);
                }
            }
            return keys;
        }
        public static IEnumerable<Keys> release_all_keydown()
        {
            IEnumerable<Keys> pressedKeys = GetPressedKeys();
            foreach (var key in pressedKeys) SSSS.KeyUp(key);
            return pressedKeys;
        }
        public static bool Deven_runing()
        {
            return (ProcessTitle?.IndexOf("正在运行") >= 0 || ProcessTitle == "");
        }
        public static bool IsMouseStopClick = false;
        public static bool isMouseStopped = true;
        public static bool QTCheck(DateTime dateTime, int ms)
        {
            var flag = DateTime.Now - dateTime < TimeSpan.FromMilliseconds(ms);
            return flag;
        }/// <summary>
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
            if (ProcessName2 == chrome)
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
            FreshProcessName();
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
            FreshProcessName();
            List<string> lines = new List<string>()
                { devenv , keyupMusic2, explorer };
            if (lines.Contains(ProcessName))
            {
                release_all_keydown();
                //ProcessRun(Application.ExecutablePath);
                return;
            }
            if (!ExistProcess(LosslessScaling))
                ProcessRun(LosslessScalingexe);
            press([Keys.LControlKey, Keys.F2]);

            if (Position.X > 0) press_middle_bottom();
        }
        public static void system_hard_sleep()
        {
            Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,1");
            gcc_restart = true;
            KeyTime[system_sleep_string] = DateTime.Now;
        }
        public static void bland_title()
        {
            //SetWindowTitle(Common.devenv, "");
            SetWindowTitle(Common.chrome, "");
            SetWindowTitle(Common.PowerToysCropAndLock, "");
            SetWindowTitle(Common.wemeetapp, "");
        }

        public static void quick_left_right(int arraw)
        {
            IntPtr hwnd = GetForegroundWindow();
            var full = IsFullScreen(hwnd);
            if (full)
            {
                SS().KeyPress(Keys.F11);
                Sleep(500);
            }

            ShowWindow((hwnd), SW.SW_SHOWNORMAL);
            Sleep(500);

            var pp = new Point(100, 100);
            if (arraw == 2) pp = new Point(screen2Width + 100, 100);
            MoveProcessWindow(ProcessName, pp);
            Sleep(500);

            RECT windowRect;
            GetWindowRect(hwnd, out windowRect);
            int windowWidth = windowRect.Right - windowRect.Left;
            if (windowWidth > 3000 || windowWidth < 1000)
            {
                SetWindowPos(hwnd, IntPtr.Zero, pp.X, pp.Y, 1800, 920, SWP_FRAMECHANGED | SWP_NOMOVE);
                Sleep(500);
            }

            ShowWindow((hwnd), SW.SW_SHOWMAXIMIZED);
            Sleep(500);

            if (full)
                SS().KeyPress(Keys.F11);
        }

    }
}
