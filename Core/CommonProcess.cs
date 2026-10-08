using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using static keyupMusic4.Native;
using Point = System.Drawing.Point;
using RECT = keyupMusic4.Native.RECT;

namespace keyupMusic4
{
    public partial class Common
    {
        public const string keyupMusic = "keyupMusic";
        public static readonly string keyupMusicexe = LoadPath("Path.keyupMusicSln", "C:\\Users\\bu\\source\\repos\\keyupMusic4\\keyupMusic4.sln");
        public const string AppName = "keyupMusic4";
        public const string scrcpy = "scrcpy";
        public const string ShapeofDreams = "Shape of Dreams";
        public const string TrialsSurvivors = "Trials Survivors";
        public const string kmRead = "Read";
        public const string OpenWith = "OpenWith";
        public const string _哔哩哔哩 = "哔哩哔哩";
        public const string PotPlayerMini64 = "PotPlayerMini64";
        public const string QuickLook = "QuickLook";
        public const string msedgewebview2 = "msedgewebview2";
        public const string ShellExperienceHost = "ShellExperienceHost";
        public const string SIBJumpView = "SIBJumpView";
        public const string ElecHead = "ElecHead";
        public const string Windblown = "Windblown";
        public const string ACPhoenix = "ACPhoenix";
        public const string Dragonest = "DragonestGameLauncher";
        public const string mhtab = "mhtab";
        public const string mhmain = "mhmain";
        public const string devenv = "devenv";
        public static readonly string devenvexe = LoadPath("Path.devenv", "C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Common7\\IDE\\devenv.exe");
        public const string WeChat = "Weixin"; //"WeChat";
        public const string douyin = "douyin";
        public const string msedge = "msedge";
        public const string chrome = "chrome";
        public const string KingOfHill = "KingOfHill";
        public const string Taskmgr = "Taskmgr";
        public const string explorer = "explorer";
        public static readonly string explorerexe = LoadPath("Path.explorer", "C:\\Windows\\explorer.exe");
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
        public const string Human = "Human";
        public const string ItTakesTwo = "ItTakesTwo";
        public const string Ghostrunner2 = "Ghostrunner2-Win64-Shipping";
        public const string bilibili = "bilibili";
        public const string UnlockingWindow = "UnlockingWindow";
        public const string LockApp = "LockApp";
        public const string err = "err";
        public const string WeChatAppEx = "WeChatAppEx";
        public const string cs2 = "cs2";
        public const string PowerToysCropAndLock = "PowerToys.CropAndLock";
        public const string spacedeskServiceTray = "spacedeskServiceTray";
        public const string Broforce_beta = "Broforce_beta";
        public const string oriwotw = "oriwotw";
        public const string LosslessScaling = "LosslessScaling";
        public static readonly string LosslessScalingexe = LoadPath("Path.losslessScaling", "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Lossless Scaling\\LosslessScaling.exe");
        public const string wemeetapp = "wemeetapp";
        public const string SplitFiction = "SplitFiction";
        public const string gcc = "GCC";
        public static readonly string gccexe = LoadPath("Path.gcc", "C:\\Program Files\\GIGABYTE\\Control Center\\GCC.exe");
        public const string TwinkleTray = "Twinkle Tray";
        public static readonly string TwinkleTrayexe = LoadPath("Path.twinkleTray", "C:\\Users\\bu\\AppData\\Local\\Programs\\twinkle-tray\\Twinkle Tray.exe");
        public const string androidstudio = "studio64";
        public const string cloudmusic = "cloudmusic";
        public static readonly string cloudmusicexe = LoadPath("Path.cloudmusic", "C:\\Program Files (x86)\\Netease\\CloudMusic\\cloudmusic.exe");
        public const string clashverge = "clash-verge";
        public static readonly string clashvergeexe = LoadPath("Path.clashVerge", "C:\\Program Files\\Clash Verge\\clash-verge.exe");
        public const string FolderView = "FolderView";
        public const string RSG = "RSG-Win64-Shipping";
        public const string b1 = "b1-Win64-Shipping";
        public const string KingdomRush = "Kingdom Rush";
        public const string KingdomRush1 = "Kingdom Rush";
        public const string KingdomRushFrontiers = "Kingdom Rush Frontiers";
        public const string Kingdom = "Kingdom Rush Vengeance";
        public const string Kingdom5 = "Kingdom Rush Alliance";
        public const string Progman = "Progman";
        public static readonly string lz_image_downloadexe = LoadPath("Path.lzImageDownload", "C:\\Users\\bu\\source\\repos\\lz_image_download\\bin\\Debug\\net8.0\\lz_image_download.exe");
        public const string PhotoApps = "PhotoApps";
        public const string BandiView = "Honeyview";
        public const string Honeyview = "Honeyview";
        public const string lz_image_download = "lz_image_download";

        public const string acer = "acer";
        public const string acerdevice = "VID_320F&PID_5088&MI_00";

        public const string logi = "logi";
        public const string logidevice = "_Dev_VID&02046d_PID&b034_REV";

        public const string coocaa = "coocaa";
        public const string coocaadevice = "_Dev_VID&010094_PID&0013_REV";

        public const string airkeyboard = "airkeyboard";
        public const string airkeyboarddevice = "VID_1915&PID_1025";

        public const string airmouse = "airmouse";
        public const string airmousedevice = "VID_1915&PID_1025&MI_03&Col01#7";

        public const string numberric = "numberric";
        public const string numberricdevice = "VID_1EA7&PID_0166&MI_00#7";

        public static string ProcessName = "";
        public static string ProcessTitle = "";

        /// <summary>从 config.txt 读取路径配置，未配置时回退到默认值（重构前硬编码路径）。</summary>
        private static string LoadPath(string key, string defaultValue)
        {
            try
            {
                var v = GetConfigValue(key);
                return string.IsNullOrWhiteSpace(v) ? defaultValue : v;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>坐标/分辨率适配的设计基准分辨率（默认 2560x1440，可在 config.txt 覆盖）。</summary>
        public static readonly int DesignWidth = LoadInt("Screen.designWidth", 2560);
        public static readonly int DesignHeight = LoadInt("Screen.designHeight", 1440);

        private static int LoadInt(string key, int defaultValue)
        {
            try
            {
                var v = GetConfigValue(key);
                return int.TryParse(v, out var r) ? r : defaultValue;
            }
            catch { return defaultValue; }
        }
        public static string ProcessPath = "";
        public static ProcessWrapper processWrapper;
        public static bool iswinopen { get { return new[] { StartMenuExperienceHost, SearchHost }.Contains(ProcessName); } }
        public static async Task FreshProcessName2(bool force = false)
        {
            FreshProcessName(force);
            await Task.Delay(10);
            FreshProcessName(force);
            await Task.Delay(100);
            FreshProcessName(force);
            await Task.Delay(1000);
            FreshProcessName(force);
        }
        private static string FreshProcessName(bool force = false)
        {
            IntPtr hwnd = Native.GetForegroundWindow(); // 获取当前活动窗口的句柄
            if (force) hwnd = Native.WindowFromPoint(Position);
            if (hwnd == IntPtr.Zero) return "";
            if (!ProcessMap.ContainsKey(hwnd))
                ProcessMap[hwnd] = new ProcessWrapper(hwnd);
            FreshProcessNameByMap(hwnd);
            return ProcessName;
        }
        public static HashSet<string> refreshTitleList = new HashSet<string> { msedge, chrome, Honeyview, };
        public static void FreshProcessNameByMap(IntPtr hwnd)
        {
            if (ProcessMap.ContainsKey(hwnd))
            {
                Common.ProcessName = ProcessMap[hwnd].name;
                ProcessTitle = ProcessMap[hwnd].title;
                ProcessPath = ProcessMap[hwnd].path;
                if (refreshTitleList.Contains(ProcessName))
                {
                    ProcessTitle = GetWindowTitle(hwnd);
                    ProcessMap[hwnd].title = ProcessTitle;
                }
                processWrapper = ProcessMap[hwnd];
            }
        }
        public static void FreshProcessNameByMap()
        {
            IntPtr hwnd = Native.GetForegroundWindow(); // 获取当前活动窗口的句柄
            if (ProcessMap.ContainsKey(hwnd))
            {
                Common.ProcessName = ProcessMap[hwnd].name;
                ProcessTitle = ProcessMap[hwnd].title;
                ProcessPath = ProcessMap[hwnd].path;
                if (refreshTitleList.Contains(ProcessName))
                {
                    ProcessTitle = GetWindowTitle(hwnd);
                    ProcessMap[hwnd].title = ProcessTitle;
                }
                processWrapper = ProcessMap[hwnd];
            }
        }
        public static bool IsDiffProcess()
        {
            IntPtr hwnd = Native.GetForegroundWindow();
            IntPtr point_hwnd = GetPointProcessHwnd();
            if (hwnd == point_hwnd) return false;
            if (ProcessMap.ContainsKey(hwnd) && ProcessMap.ContainsKey(point_hwnd))
                if (ProcessMap[hwnd].name == ProcessMap[point_hwnd].name)
                    return false;
            return true;
        }
        public static string GetPointName()
        {
            IntPtr hWnd = Native.WindowFromPoint(Position);
            var aaa = GetWindowName(hWnd);
            return aaa;
        }
        public static string GetPointTitle()
        {
            IntPtr hWnd = Native.WindowFromPoint(Position);
            var aaa = GetWindowTitle(hWnd);
            return aaa;
        }
        public static IntPtr GetPointProcessHwnd()
        {
            IntPtr hwnd = Native.WindowFromPoint(Position);
            if (!ProcessMap.ContainsKey(hwnd))
                ProcessMap[hwnd] = new ProcessWrapper(hwnd);
            return hwnd;
        }
        public static string GetWindowName(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero) return "";
            // 优先用 ProcessMap 缓存，同一窗口只查一次进程，避免高频创建 Process 对象
            if (!ProcessMap.ContainsKey(hwnd))
            {
                try { ProcessMap[hwnd] = new ProcessWrapper(hwnd); }
                catch (Exception) { return ""; }
            }
            return ProcessMap.TryGetValue(hwnd, out var wrapper) ? wrapper?.name ?? "" : "";
        }
        public static bool is_douyin()
        {
            return ProcessName == douyin || (ProcessName == msedge && ProcessTitle?.IndexOf("抖音") >= 0) || (ProcessName == chrome && ProcessTitle?.IndexOf("抖音") >= 0);
        }
        public static bool is_steam_game()
        {
            return ProcessPath != null && ProcessPath.Contains("steam") && ProcessName != steam;
        }

        public static ConcurrentDictionary<IntPtr, ProcessWrapper> ProcessMap = new ConcurrentDictionary<IntPtr, ProcessWrapper>();
        public class ProcessWrapper
        {
            public string name { get; set; }
            public string title { get; set; }
            public string path { get; set; }
            public string classname { get; set; }

            public ProcessWrapper(IntPtr hwnd)
            {
                uint processId;
                Native.GetWindowThreadProcessId(hwnd, out processId);

                if (processId == 0) return;
                StringBuilder className = new StringBuilder(256);
                GetClassName(hwnd, className, className.Capacity);
                string classNameStr = className.ToString();

                Process process = Process.GetProcessById((int)processId);
                this.name = process.ProcessName;
                this.title = GetWindowTitle(hwnd);
                this.path = process.MainModule.FileName;
                this.classname = classNameStr;
                process.Dispose();
            }
            public override string ToString()
            {
                return this.name + " " + this.title + " " + this.path + " " + this.classname;
            }
        }

        public static void FocusPointProcess()
        {
            IntPtr hwnd = GetPointProcessHwnd();
            SetForegroundWindow(hwnd);
            FreshProcessNameByMap(hwnd);
            CleanMouseState();
        }
        public static bool FocusProcessSimple(string procName)
        {
            IntPtr current_hwnd = GetForegroundWindow();
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = IntPtr.Zero;
                hWnd = objProcesses[0].MainWindowHandle;
                if (current_hwnd == hWnd)
                    return true;
                SetForegroundWindow(objProcesses[0].MainWindowHandle);
                FreshProcessNameByMap(objProcesses[0].MainWindowHandle);
                return true;
            }
            return false;
        }
        public static bool FocusProcess(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero) return false;
            SetForegroundWindow(hwnd);
            FreshProcessNameByMap();
            return false;
        }
        public static bool FocusProcess2(string procName, bool front = true)
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
                    ShowWindowAsync(new HandleRef(null, hWnd), (int)SW.SW_RESTORE);
                if (!front) return true;
                SetForegroundWindow(objProcesses[0].MainWindowHandle);
                FreshProcessNameByMap();
                if (ProcessName != procName) ProcessRun(procName);

                return true;
            }
            return false;
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
                if (!front) return true;
                SetForegroundWindow(objProcesses[0].MainWindowHandle);
                FreshProcessNameByMap();
                if (ProcessName != procName) ProcessRun(procName);

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
        public static bool SetWindowTitle2(string window)
        {
            string targetClassName = window;
            IntPtr hWnd = GetProcessID(targetClassName);
            string title = GetWindowTitle(hWnd);
            string name = ExtractStreamerName(title);
            title = name;
            bool result = SetWindowText(hWnd, title);
            return result;
        }
        public static bool SetWindowTitle3(string window)
        {
            string targetClassName = window;
            IntPtr hWnd = GetProcessID(targetClassName);
            string title = GetWindowTitle(hWnd);
            if (title == null || title == "") return false;
            if (title.IndexOf("(") > 0)
                title = title.Substring(0, title.IndexOf("(") - 1);
            else if (title.IndexOf("-") > 0)
                title = title.Substring(0, title.IndexOf("-") - 1);
            bool result = SetWindowText(hWnd, title);
            return result;
        }
        public static bool SetWindowTitle()
        {
            IntPtr hWnd = GetForegroundWindow();
            bool result = SetWindowText(hWnd, "");
            return result;
        }
        public static bool ExistProcess(string procName, bool front = false)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            foreach (Process proc in objProcesses)
            {
                if (!front || proc.MainWindowHandle != 0)
                    return true;
            }
            return false;
        }

        public static void HideProcess()
        {
            IntPtr hwnd = GetForegroundWindow(); // 获取当前活动窗口的句柄
            Native.ShowWindow(hwnd, Native.SW.SW_MINIMIZE);
        }

        public static void HideProcess(bool force)
        {
            IntPtr hwnd = GetForegroundWindow(); // 获取当前活动窗口的句柄
            var sw = SW.SW_MINIMIZE;
            if (force) sw = SW.SW_HIDE;
            else if (GetPointName().Contains(Common.AppName)) sw = SW.SW_HIDE;
            Native.ShowWindow(hwnd, sw);
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

        public static IntPtr MainHandle(string procName)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                return objProcesses[0].MainWindowHandle;
            }
            return 0;
        }

        public static void HideProcess2(string procName)
        {
            IntPtr hWnd = MainHandle(procName);

            RECT windowRect;
            GetWindowRect(hWnd, out windowRect);

            var small = windowRect.Left == 2550 && windowRect.Top == 290;
            if (small) return;

            ShowWindow(hWnd, SW.SW_MINIMIZE);

            Task.Run(() =>
            {
                int tick = 200;
                for (int i = 0; i < 2000 / tick; i++)
                {
                    IntPtr hWnd = MainHandle(procName);

                    RECT windowRect;
                    GetWindowRect(hWnd, out windowRect);
                    Sleep(tick);
                    if (windowRect.Left > -10000)
                    {
                        CloseProcess(procName);
                        return;
                    }
                }
            });





        }

        public static void HideProcess3(string procName)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = objProcesses[0].MainWindowHandle;
                ShowWindow(hWnd, SW.SW_MINIMIZE);
            }
        }

        public static void HomeProcess(string procName)
        {
            IntPtr hwnd = GetForegroundWindow();
            FocusProcessSimple(procName);
            press(Keys.BrowserHome, 100);
            FocusProcess(hwnd);
        }
        public static void CloseProcess(string procName)
        {
            if (ProcessName == Common.mhtab) return;
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = objProcesses[0].MainWindowHandle;
                PostMessage(hWnd, (uint)WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                objProcesses[0].Kill();
            }
        }
        public static void CloseProcess2(string procName)
        {
            if (ProcessName == Common.mhtab) return;
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = objProcesses[0].MainWindowHandle;
                PostMessage(hWnd, (uint)WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }
        public static void RestartProcess(string procName, string exe)
        {
            if (ProcessName == Common.mhtab) return;
            CloseProcess(procName);
            ProcessRun(exe);
        }

        public static void CloseProcessFoce(string procName)
        {
            if (ProcessName == Common.mhtab) return;
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
            if (ProcessName == Common.mhtab) return;
            IntPtr hwnd = Native.GetForegroundWindow();
            PostMessage(hwnd, (uint)WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
        public static string GetWindowTitle()
        {
            IntPtr hwnd = Native.GetForegroundWindow(); // 获取当前活动窗口的句柄

            string windowTitle = GetWindowTitle(hwnd);

            return windowTitle;
        }
        public static string GetWindowTitle(IntPtr hWnd)
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            if (Native.GetWindowText(hWnd, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return "";
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
        public static bool Deven_runing()
        {
            return (ProcessTitle?.IndexOf("正在运行") >= 0 || ProcessTitle == "");
        }


        public static void bland_title()
        {
            SetWindowTitle3(Common.devenv);
            SetWindowTitle3(Common.VSCode);
            SetWindowTitle2(Common.chrome);
            SetWindowTitle(Common.PowerToysCropAndLock, "");
        }
        public static bool IsDesktopFocused()
        {
            if (ProcessName != explorer) return false;
            string classNameStr = processWrapper?.classname;
            var title = GetPointTitle();
            var istitle = title == FolderView || Position.Y == screenHeight1;
            var aa = classNameStr == "Progman" || classNameStr == "WorkerW";
            if ((classNameStr == "Shell_TrayWnd" || classNameStr == "CabinetWClass") && istitle) aa = true;
            aa = aa && istitle;
            return aa;
        }
        public static bool is_lizhi => (ProcessTitle.Contains("荔枝") && !ProcessTitle.Contains("分类"));
        public static bool is_tran_powertoy = false;
        public static int is_tran_alpha { get => AppState.is_tran_alpha; set => AppState.is_tran_alpha = value; }
        public static bool is_tran_powertoy2 { get => AppState.is_tran_powertoy2; set => AppState.is_tran_powertoy2 = value; }
        public static string is_tran_process { get => AppState.is_tran_process; set => AppState.is_tran_process = value; }

        private const double BytesToMB = 1024 * 1024; // 1MB = 1048576字节
        public static string CounterName;
        public static float GetUsage(string TargetProcessName, string usageName = "Thread Count")
        {
            var processes = Process.GetProcessesByName(TargetProcessName);
            if (!processes.Any())
                return -1;

            if (string.IsNullOrEmpty(CounterName))
            {

                Task.Run(() =>
                {
                    var category = new PerformanceCounterCategory("Process");
                    var counterNames = category.GetCounters(processes[0].ProcessName)
                        .Select(c => c.CounterName)
                        .ToList();
                    CounterName = counterNames.FirstOrDefault(name => name.Contains(usageName));
                });
                return -2;

            }

            // 创建性能计数器并获取值
            using (var counter = new PerformanceCounter("Process", CounterName, processes[0].ProcessName, true))
            {
                if (counter.NextValue() == 0)
                {
                    counter.NextValue();
                    Thread.Sleep(100);
                }

                float value = counter.NextValue();
                if (value > 10000)
                    value = (float)Math.Round(value / BytesToMB, 2);

                Console2.WriteLine($"{TargetProcessName} {value} MB/s");
                return (float)value;
            }
        }
    }
}
