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
        public static bool RunProcess(string str, string arg = "", bool waitForExit = false) { return ProcessRun(str, arg, waitForExit); }
        public static bool ProcessRun(string str, string arg = "", bool waitForExit = false)
        {
            Process process = null;
            try
            {
                if (!(str.Contains(".") || str.Contains("exe")))
                    str = ProcessMap.Where(o => o.Value.name == str).First().Value.path;

                ProcessStartInfo startInfo = new ProcessStartInfo(str)
                {
                    UseShellExecute = true,
                    Verb = "open" // 请求管理员权限
                };

                if (!string.IsNullOrEmpty(arg))
                    startInfo.Arguments = arg;

                process = Process.Start(startInfo);

                if (process != null && waitForExit)
                {
                    process.WaitForExit(); // 等待进程退出
                    return process.ExitCode == 0; // 返回进程退出状态
                }

                return true; // 不等待时默认返回成功
            }
            catch (Exception ex)
            {
                Console2.WriteLine($"启动进程时出错: {ex.Message}");
                return false;
            }
            finally
            {
                // 释放资源
                process?.Dispose();
            }
        }

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void LossScale()
        {

            if (!ExistProcess(LosslessScaling))
            {
                ProcessRun(LosslessScalingexe);
                play_sound_bongocat(D9);
            }


            press([Keys.LControlKey, Keys.F2]);

        }
        // 已禁用（基线行为：原实现被 return 截断，现为空实现；恢复功能可从 git 历史取回原实现）
        public static void system_hard_sleep()
        {
        }


        public static void changeClash()
        {
            press([Keys.LControlKey, Keys.F1]);
            bool isProxyOn = Common.IsClashVergeSystemProxyOn().GetAwaiter().GetResult();
            Keys keys = isProxyOn ? Keys.D0 : D1;
            play_sound_bongocat(keys);
            if (iswinopen)
                press(LWin);
        }

        public static bool IsAnyMusicPlayerRunning()
        {
            // 可根据实际需求添加更多音乐软件进程名
            string[] musicProcesses = { "cloudmusic", "QQMusic", "KuGou", "酷我音乐" };
            foreach (var proc in System.Diagnostics.Process.GetProcesses())
            {
                foreach (var name in musicProcesses)
                {
                    if (proc.ProcessName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                }
            }
            return false;
        }

        // 启动网易云音乐
        public static void StartNeteaseCloudMusic()
        {
            ProcessRun(cloudmusicexe);
        }
        public static void record_screen()
        {
            press([LControlKey, LMenu, S]);
        }

        public static void HideSomething()
        {
            HideProcess(PowerToysCropAndLock);
        }
    }
}
