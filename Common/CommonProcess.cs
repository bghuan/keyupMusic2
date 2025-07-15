using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
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
        public const string keyupMusic = "keyupMusic";
        public const string keyupMusicexe = "C:\\Users\\bu\\source\\repos\\keyupMusic2\\keyupMusic2.sln";
        public const string keyupMusic2 = "keyupMusic4";
        public const string _哔哩哔哩 = "哔哩哔哩";
        public const string PotPlayerMini64 = "PotPlayerMini64";
        public const string QuickLook = "QuickLook";
        public const string msedgewebview2 = "msedgewebview2";
        public const string ShellExperienceHost = "ShellExperienceHost";
        public const string ElecHead = "ElecHead";
        public const string Windblown = "Windblown";
        public const string ACPhoenix = "ACPhoenix";
        public const string Dragonest = "DragonestGameLauncher";
        public const string devenv = "devenv";
        public const string devenvexe = "C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Common7\\IDE\\devenv.exe";
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
        public const string cloudmusic = "cloudmusic";
        public const string cloudmusicexe = "C:\\Program Files (x86)\\Netease\\CloudMusic\\cloudmusic.exe";
        public const string clashverge = "clash-verge";
        public const string FolderView = "FolderView";
        public const string RSG = "RSG-Win64-Shipping";
        public const string KingdomRush = "Kingdom Rush";
        public const string KingdomRush1 = "Kingdom Rush";
        public const string KingdomRushFrontiers = "Kingdom Rush Frontiers";
        public const string Kingdom = "Kingdom Rush Vengeance";
        public const string Kingdom5 = "Kingdom Rush Alliance";
        public const string Progman = "Progman";
        public const string lz_image_downloadexe = "C:\\Users\\bu\\source\\repos\\lz_image_download\\bin\\Debug\\net8.0\\lz_image_download.exe";
        public const string PhotoApps = "PhotoApps";
        public const string BandiView = "Honeyview";
        public const string Honeyview = "Honeyview";
        public const string lz_image_download = "lz_image_download";


        public static string ProcessName = "";
        public static string ProcessTitle = "";
        public static string ProcessPath = "";
        public static ProcessWrapper processWrapper;
        public static bool iswinopen { get { return new[] { StartMenuExperienceHost, SearchHost }.Contains(ProcessName); } }
        public static HashSet<string> middle_str = new() { RSG };
        public static bool raw_middle => middle_str.Contains(ProcessName);
        public static string FreshProcessName()
        {
            IntPtr hwnd = Native.GetForegroundWindow(); // 获取当前活动窗口的句柄
            if (hwnd == IntPtr.Zero) return "";
            if (ProcessMap.ContainsKey(hwnd) && ProcessMap[hwnd].name == Common.ProcessName)
            {
                if (refreshTitleList.Contains(ProcessName))
                    FreshProcessNameByMap(hwnd);
                //log(hwnd + ProcessName + 1);
                return Common.ProcessName;
            }
            if (!ProcessMap.ContainsKey(hwnd))
            {
                ProcessMap[hwnd] = new ProcessWrapper(hwnd);
            }
            FreshProcessNameByMap(hwnd);

            CleanMouseState();

            //log(hwnd + ProcessName+2);
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
        public static bool IsDiffProcess()
        {
            IntPtr hwnd = Native.GetForegroundWindow();
            IntPtr point_hwnd = Native.WindowFromPoint(Position);
            if (hwnd == point_hwnd) return false;
            if (ProcessMap.ContainsKey(hwnd) && ProcessMap.ContainsKey(point_hwnd))
                if (ProcessMap[hwnd].name == ProcessMap[point_hwnd].name)
                    return false;
            IntPtr point_process_hwnd = GetPointProcessHwnd();
            if (hwnd == point_process_hwnd) return false;
            //if (ProcessMap.ContainsKey(hwnd) && ProcessMap[hwnd].name == LosslessScaling)
            //    return false;
            //if (ProcessMap.ContainsKey(hwnd) && ProcessMap.ContainsKey(point_process_hwnd))
            //    if (ProcessMap[hwnd].name == ProcessMap[point_process_hwnd].name)
            //        return false;
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
            IntPtr point_hwnd = Native.WindowFromPoint(Position);
            //IntPtr hwnd = IntPtr.Zero;
            //uint processId;
            //Native.GetWindowThreadProcessId(point_hwnd, out processId);
            //using (Process process = Process.GetProcessById((int)processId))
            //{
            //    hwnd = process.MainWindowHandle;
            //    ProcessMap[point_hwnd] = ProcessMap[hwnd] = new ProcessWrapper(point_hwnd);
            //}
            ProcessMap[point_hwnd] = ProcessMap[point_hwnd] = new ProcessWrapper(point_hwnd);
            //FreshProcessNameByMap(hwnd);
            return point_hwnd;
        }
        public static string GetWindowName(IntPtr hwnd)
        {
            uint processId;
            string Name = "";
            Native.GetWindowThreadProcessId(hwnd, out processId);
            try
            {
                Process process = Process.GetProcessById((int)processId);
                {
                    Name = process.ProcessName;
                }
            }
            catch (Exception e) { }
            return Name;
        }
        public static bool is_douyin()
        {
            return ProcessName == douyin || (ProcessName == msedge && ProcessTitle?.IndexOf("抖音") >= 0);
        }
        public static bool is_steam_game()
        {
            return ProcessPath != null && ProcessPath.Contains("steam");
        }

        public static ConcurrentDictionary<IntPtr, ProcessWrapper> ProcessMap = new ConcurrentDictionary<IntPtr, ProcessWrapper>();
        public class ProcessWrapper
        {
            public string name { get; set; }
            public string title { get; set; }
            public string path { get; set; }
            public string classname { get; set; }

            //public ProcessWrapper(string name, string title, string path, string classname)
            //{
            //    this.name = name;
            //    this.title = title;
            //    this.path = path;
            //    this.classname = classname;
            //}
            public ProcessWrapper(IntPtr hwnd)
            {
                uint processId;
                Native.GetWindowThreadProcessId(hwnd, out processId);

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
            public string ToString()
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
        public static bool FocusProcessSimple(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero) return false;
            SetForegroundWindow(hwnd);
            FreshProcessNameByMap(hwnd);
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
                ShowWindow((hWnd), SW.SW_RESTORE);
                if (procName != Dragonest && procName != chrome && procName != devenv)
                    ShowWindowAsync(new HandleRef(null, hWnd), (int)SW.SW_RESTORE);
                //ShowWindow((hWnd), SW.SW_SHOWMAXIMIZED);
                //ShowWindow((hWnd), SW.SW_SHOW);
                //ShowWindow((hWnd), SW.SW_SHOWNA);
                if (!front) return true;
                SetForegroundWindow(objProcesses[0].MainWindowHandle);
                FreshProcessNameByMap(objProcesses[0].MainWindowHandle);
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
            if (title == null || title == "") return false;
            if (title.IndexOf(")") >= 0)
                title = title.Substring(title.IndexOf(")") + 1, title.Length - title.IndexOf(")") - 1);
            if (title.IndexOf("主") > 0)
                title = title.Substring(0, title.IndexOf("主"));
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
            //if (objProcesses.Length > 0)
            //{
            //    if (objProcesses[0].MainWindowHandle == 0 && front)
            //        return false;
            //    return true;
            //}
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

        public static void HideProcess(string procName)
        {
            Process[] objProcesses = Process.GetProcessesByName(procName);
            if (objProcesses.Length > 0)
            {
                IntPtr hWnd = objProcesses[0].MainWindowHandle;
                Native.ShowWindow(hWnd, Native.SW.SW_MINIMIZE);
            }
        }

        public static void HomeProcess(string procName)
        {
            IntPtr hwnd = GetForegroundWindow();
            FocusProcessSimple(procName);
            press(Keys.BrowserHome, 100);
            FocusProcessSimple(hwnd);
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
        public static string GetWindowTitle()
        {
            IntPtr hwnd = Native.GetForegroundWindow(); // 获取当前活动窗口的句柄

            string windowTitle = GetWindowTitle(hwnd);
            //Console.WriteLine("当前活动窗口名称: " + windowTitle);

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
        public static IntPtr FindEdgeWindow(string procName)
        {
            IntPtr current_hwnd = GetForegroundWindow();
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

            return IntPtr.Zero;
        }
        public static bool Deven_runing()
        {
            return (ProcessTitle?.IndexOf("正在运行") >= 0 || ProcessTitle == "");
        }


        public static void bland_title()
        {
            //SetWindowTitle(Common.devenv, "");
            //SetWindowTitle(Common.chrome, "");
            SetWindowTitle2(Common.chrome);
            //SetWindowTitle(Common.PowerToysCropAndLock, "");
            //SetWindowTitle(Common.wemeetapp, "");
        }
        public static bool IsDesktopFocused()
        {
            if (ProcessName != explorer) return false;
            string classNameStr = processWrapper?.classname;
            var title = GetPointTitle();
            var istitle = title == FolderView;
            //istitle = istitle || title == "Program Manager";
            var aa = classNameStr == "Progman" || classNameStr == "WorkerW";
            if ((classNameStr == "Shell_TrayWnd" || classNameStr == "CabinetWClass") && istitle) aa = true;
            aa = aa && istitle;
            //aa = aa || ProcessName == Common.keyupMusic2;
            return aa;
        }
        public static bool is_lizhi => (ProcessTitle.Contains("荔枝") && !ProcessTitle.Contains("分类"));

    }
}
