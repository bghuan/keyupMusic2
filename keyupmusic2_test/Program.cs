using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WindowTransparencySetter
{
    public partial class  asdsa
    {
        // 导入Windows API函数
        [DllImport("user32.dll")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        // 常量定义
        private const int GWL_EXSTYLE = -20;               // 扩展窗口样式索引
        private const int WS_EX_LAYERED = 0x80000;         // 分层窗口样式
        private const int LWA_ALPHA = 0x2;                 // 透明度调节标志

        public static void Main()
        {
            SetTransparencyBtn_Click();
        }

        private static void SetTransparencyBtn_Click()
        {
            // 1. 目标程序的窗口标题（可通过任务管理器查看）
            string windowTitle = "PowerToys.CropAndLock"; // 示例：记事本程序
            // 2. 目标透明度（0-255）
            byte transparency = 180; // 半透明效果

            // 执行透明度设置
            bool success = SetWindowTransparency(windowTitle, transparency);
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
        public static bool SetWindowTransparency(string windowTitle, byte alpha)
        {
            // 1. 获取目标窗口句柄（根据窗口标题）
            IntPtr hWnd = GetProcessID(windowTitle);
            if (hWnd == IntPtr.Zero)
                return false;

            // 2. 检查窗口是否已设置为分层窗口，若未设置则添加样式
            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            if ((exStyle & WS_EX_LAYERED) == 0)
            {
                // 添加分层窗口样式
                SetWindowLong(hWnd, GWL_EXSTYLE, exStyle | WS_EX_LAYERED);
            }

            // 3. 设置透明度（alpha值）
            return SetLayeredWindowAttributes(hWnd, 0, alpha, LWA_ALPHA);
        }
    }
}