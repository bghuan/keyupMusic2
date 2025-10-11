using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public static class TransparencyHelper
{
    // 常量定义
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_LAYERED = 0x80000;
    private const int WS_EX_TRANSPARENT = 0x20;
    private const int LWA_ALPHA = 0x2;
    private const int LWA_COLORKEY = 0x1;  // 颜色键模式
    private const int HWND_TOPMOST = -1;
    private const int HWND_NOTOPMOST = -2;
    private const int SWP_NOMOVE = 0x2;
    private const int SWP_NOSIZE = 0x1;
    private const int SWP_NOACTIVATE = 0x10;

    // 黑色的RGB值（要保留的颜色）
    private const uint BLACK_COLOR = 0x000000;  // RGB(0,0,0)

    // 外部DLL函数声明
    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, int dwFlags);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    // 全局状态
    private static byte is_tran_alpha = 255;
    private static bool is_tran_powertoy2 = false;
    private static string is_tran_process = "";
    private static string ProcessName => GetProcessNameFromWindow(GetForegroundWindow());

    /// <summary>
    /// 设置Edge浏览器窗口仅保留黑色部分，其余区域透明
    /// </summary>
    /// <param name="enable">是否启用黑色保留透明效果</param>
    public static void SetEdgeBlackTransparency(bool enable)
    {
        IntPtr hwnd = 0;
        Process[] objProcesses = Process.GetProcessesByName("msedge");
        if (objProcesses.Length > 0)
        {
            hwnd = objProcesses[0].MainWindowHandle;
        }
        // 获取当前激活的Edge窗口句柄
        if (hwnd == IntPtr.Zero) return;


        // 验证是否为Edge浏览器窗口
        //if (!ProcessName.Equals("msedge", StringComparison.OrdinalIgnoreCase))
        //{
        //    return; // 仅对Edge生效
        //}

        // 获取当前窗口样式
        int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        exStyle |= WS_EX_LAYERED;  // 必须启用分层窗口样式

        if (enable)
        {
            // 启用：黑色保留，其他颜色透明
            //exStyle |= WS_EX_TRANSPARENT;  // 鼠标穿透
            SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE); // 置顶
            // 设置颜色键：将非黑色区域设为透明（保留黑色）
            const uint WHITE_COLOR = 0xFFFFFF; // RGB(255,255,255)

            SetLayeredWindowAttributes(hwnd, BLACK_COLOR, 255, LWA_COLORKEY);
            is_tran_process = ProcessName;
        }
        else
        {
            // 禁用：恢复正常窗口
            //exStyle &= ~WS_EX_TRANSPARENT;  // 移除穿透
            SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE); // 取消置顶
            // 恢复为正常不透明
            SetLayeredWindowAttributes(hwnd, 0, 255, LWA_ALPHA);
            is_tran_process = "";
        }

        // 应用窗口样式修改
        SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);
    }

    /// <summary>
    /// 从窗口句柄获取进程名称
    /// </summary>
    private static string GetProcessNameFromWindow(IntPtr hWnd)
    {
        try
        {
            GetWindowThreadProcessId(hWnd, out int processId);
            using (Process process = Process.GetProcessById(processId))
            {
                return process.ProcessName;
            }
        }
        catch
        {
            return "";
        }
    }
    static void Main(string[] args)
    {
        SetEdgeBlackTransparency(true);

        Console.WriteLine("所有下载任务已完成");
        Console.WriteLine("按任意键退出...");
        Console.ReadKey();
    }
}
