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
        public static Huan huan => Huan.Instance;

        public static bool stop_listen { get => AppState.stop_listen; set => AppState.stop_listen = value; }
        public static bool ACPhoenix_mouse_hook { get => AppState.ACPhoenix_mouse_hook; set => AppState.ACPhoenix_mouse_hook = value; }
        public static bool gcc_restart { get => AppState.gcc_restart; set => AppState.gcc_restart = value; }

        public static string system_sleep_string { get => AppState.system_sleep_string; set => AppState.system_sleep_string = value; }
        public static int system_sleep_count { get => AppState.system_sleep_count; set => AppState.system_sleep_count = value; }
        public static bool ready_to_sleep { get => AppState.ready_to_sleep; set => AppState.ready_to_sleep = value; }
        public static Point Position { get { return Cursor.Position; } }
        public static Point PositionMiddle { get => AppState.PositionMiddle; set => AppState.PositionMiddle = value; }

        public static void CleanMouseState()
        {
            biu.catch_off();
            CleanVirMouseState();
        }

        static string proc_info = "";
        public static string process_and_log(string key = "")
        {
            IntPtr hwnd = Native.GetForegroundWindow();
            string Title = GetWindowTitle(hwnd);
            bool IsFull = IsFullScreen(hwnd);

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
            return Name;
        }
        public static void log(string message)
        {
            Log.log(message);
        }




        static string token = "ssssssssssidiii32ii323dcds";
        public static async Task<string> GetClashVergeStatusAsync()
        {
            using var client = new HttpClient();
            var url = "http://127.0.0.1:9097/configs";
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch
            {
                return "Clash Verge API 不可用或未启动";
            }
        }
        public static async Task<bool> IsClashVergeSystemProxyOn()
        {
            // 1. 解析 mixed-port
            int mixedPort = 0;
            try
            {
                var aa = await GetClashVergeStatusAsync();
                using var doc = JsonDocument.Parse(aa);
                if (doc.RootElement.TryGetProperty("mixed-port", out var portElem))
                    mixedPort = portElem.GetInt32();
            }
            catch { }

            if (mixedPort == 0) return false; // 没有监听端口

            // 2. 检查注册表
            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings"))
            {
                if (key != null)
                {
                    object enable = key.GetValue("ProxyEnable");
                    object server = key.GetValue("ProxyServer");
                    if (enable is int && (int)enable == 1 && server is string s)
                    {
                        return s.Contains("127.0.0.1:" + mixedPort) || s.Contains("localhost:" + mixedPort);
                    }
                }
            }
            return false;
        }




        public static Keys LongPressKey { get => AppState.LongPressKey; set => AppState.LongPressKey = value; }
        public static DateTime NotityTime { get => AppState.NotityTime; set => AppState.NotityTime = value; }
        public static bool no_move { get => AppState.no_move; set => AppState.no_move = value; }
        public static string DeviceName { get => AppState.DeviceName; set => AppState.DeviceName = value; }
        public static string DeviceName2 { get => AppState.DeviceName2; set => AppState.DeviceName2 = value; }
        public static string timestamp => DateTime.Now.ToString("HH:mm:ss.fff ");
        public static void WriteLine(string? value)
        {
            Console2.WriteLine(timestamp + value);
        }
        public static void Write(string? value)
        {
            Console2.Write(timestamp + value);
        }
        public static void Con(string? value)
        {
            Console2.WriteLine(timestamp + value);
        }
        public static class Console2
        {
            public static void WriteLine(string? value)
            {
                System.Console.WriteLine(timestamp + value);
            }
            public static void Write(string? value)
            {
                System.Console.Write(timestamp + value);
            }

        }
    }
}
