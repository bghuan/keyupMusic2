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

        public static string ExtractStreamerName(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // 去掉开头所有的括号组，例如 "(27) " 或 "(a)(b) "
            string s = Regex.Replace(input, @"^\s*(\([^)]*\)\s*)+", string.Empty);

            // 若出现“主播”，截取其前面的内容
            int idx = s.IndexOf("主播", StringComparison.Ordinal);
            if (idx >= 0) s = s.Substring(0, idx);

            // 去除尾部的中文/英文冒号、引号、空白等
            s = s.Trim();
            s = Regex.Replace(s, @"[:：\s""'”“]+$", string.Empty);
            s = s.Trim();

            // 限制最终长度为最多12个字符
            if (s.Length > 12)
            {
                s = s.Substring(0, 12);
            }
            // 若中文字符超过 2 个，返回空字符串
            {
                int chineseCount = 0;
                ReadOnlySpan<char> span = s.AsSpan();
                int offset = 0;
                while (offset < span.Length)
                {
                    var slice = span.Slice(offset);
                    var status = Rune.DecodeFromUtf16(slice, out Rune rune, out int charsUsed);
                    if (status != System.Buffers.OperationStatus.Done) break;
                    uint v = (uint)rune.Value;
                    // 常用汉字与 CJK 扩展区间
                    if ((v >= 0x4E00 && v <= 0x9FFF) || (v >= 0x3400 && v <= 0x4DBF) ||
                        (v >= 0x20000 && v <= 0x2A6DF) || (v >= 0x2A700 && v <= 0x2B73F) ||
                        (v >= 0x2B740 && v <= 0x2B81F) || (v >= 0x2B820 && v <= 0x2CEAF))
                    {
                        chineseCount++;
                        if (chineseCount > 2) return string.Empty;
                    }
                    offset += charsUsed;
                }
            }
            return s;
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
        public static bool DaleyRun_stop { get => AppState.DaleyRun_stop; set => AppState.DaleyRun_stop = value; }
        public static bool Special_Input2 { get => AppState.Special_Input2; set => AppState.Special_Input2 = value; }
        public static DateTime init_time { get => AppState.init_time; set => AppState.init_time = value; }
        public static DateTime Special_Input_tiem { get => AppState.Special_Input_tiem; set => AppState.Special_Input_tiem = value; }
        public static void DelayRun(Func<bool> flag_action, Action action2, int alltime, int tick)
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

        public static void Sleep(int tick)
        {
            Thread.Sleep(tick);
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

                    Console2.WriteLine(refJson);
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
            //return VirMouseStateKey;
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

        public static void quick_left_right(int arraw)
        {
            if (Screen.AllScreens.Length < 2) return;
            var pp = new Point(100, 100);
            if (ScreenSecond.X < ScreenPrimary.X)
            {
                if (arraw == 1) pp = new Point(ScreenSecond.X + 100, 100);
                if (arraw == 2) pp = new Point(ScreenPrimary.X + 100, 100);
            }
            else if (ScreenSecond.X > ScreenPrimary.X)
            {
                if (arraw == 1) pp = new Point(ScreenPrimary.X + 100, 100);
                if (arraw == 2) pp = new Point(ScreenSecond.X + 100, 100);
            }

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
        public static class RateLimiter2
        {
            private static DateTime lastExecuteTime = DateTime.MinValue;
            private static readonly TimeSpan ExecutionInterval = TimeSpan.FromMilliseconds(200);

            private static readonly object _lock = new object();

            public static void Execute<T1>(Action<T1> action, T1 arg1)
            {
                var now = DateTime.Now;

                bool canExecuteBase = now - lastExecuteTime >= ExecutionInterval;

                if (canExecuteBase)
                {
                    lastExecuteTime = now;
                    action(arg1);
                }
            }
        }
        public const uint WM_USER = 0x0400;
        public const uint CUSTOM_MESSAGE = WM_USER + 100;

        // 向指定窗口发送消息

        public static void PrintDictSortedByValueDesc()
        {
            // 处理字典为空的情况
            if (Huan.mmmm == null || Huan.mmmm.Count == 0)
            {
                File.WriteAllText("log/log2.txt", "【提示】字典 mmmm 为空或未初始化");
                return;
            }

            // 1. 构建日志内容（按值从大到小排序）
            var logBuilder = new StringBuilder();
            logBuilder.AppendLine($"===== 字典打印日志 | 时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss} =====");
            logBuilder.AppendLine($"总键值对数量：{Huan.mmmm.Count}");
            logBuilder.AppendLine("--------------------------------------");
            logBuilder.AppendLine("序号 | 键（Key） | 值（Value）");
            logBuilder.AppendLine("--------------------------------------");

            // 2. 按值降序排序（核心：OrderByDescending(kvp => kvp.Value)）
            var sortedKvps = Huan.mmmm.OrderByDescending(kvp => kvp.Value);

            // 3. 遍历排序后的键值对，拼接日志
            int index = 1;
            foreach (var kvp in sortedKvps)
            {
                logBuilder.AppendLine($"{index,2}   | {kvp.Key,6}    | {kvp.Value,6}");
                index++;
            }

            // 4. 写入文件
            try
            {
                File.WriteAllText("log2.txt", logBuilder.ToString());
                //Console.WriteLine("字典已按值降序打印到 log2.txt");
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"写入失败：{ex.Message}");
            }
        }
        public static HashSet<Keys> number_button = new HashSet<Keys> { Keys.Oemcomma, Keys.OemPeriod, Keys.Oem2, Keys.K, Keys.L, Keys.OemSemicolon, Keys.I, Keys.O, Keys.P, Keys.Space };

        public const KeyType Upp = KeyType.Up;
        public const KeyType Downn = KeyType.Down;
        public static DateTime start_catch_time = DateTime.MinValue;


        [DllImport("user32.dll")]
        private static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [DllImport("user32.dll")]
        private static extern int ChangeDisplaySettings(ref DEVMODE lpDevMode, uint dwFlags);

        private const int ENUM_CURRENT_SETTINGS = -1;
        private const uint CDS_UPDATEREGISTRY = 0x01;
        private const uint CDS_TEST = 0x02;
        private const int DISP_CHANGE_SUCCESSFUL = 0;
        private const int DISP_CHANGE_BADMODE = 0x02;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
        }

        // dmFields 标志位
        private const int DM_PELSWIDTH = 0x80000;
        private const int DM_PELSHEIGHT = 0x100000;
        private const int DM_BITSPERPEL = 0x40000;
        private const int DM_DISPLAYFREQUENCY = 0x400000;

        /// <summary>
        /// 根据参数设置分辨率和刷新率
        /// </summary>
        /// <param name="mode">1=1920x1080@144Hz, 2=3840x2160@120Hz</param>
        /// <returns>是否设置成功</returns>
    }
}
