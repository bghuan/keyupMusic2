using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace keyupMusic2;

public static class pagedown_edge
{
    public static string yo()
    {
        IntPtr hwnd = GetForegroundWindow(); // 获取当前活动窗口的句柄

        string windowTitle = GetWindowText(hwnd);
        Console.WriteLine("当前活动窗口名称: " + windowTitle);

        var filePath = "a.txt";
        var fildsadsePath = "err";
        var module_name = "err";
        var module_nasme = "err";

        try
        {
            uint processId;
            GetWindowThreadProcessId(hwnd, out processId);
            using (Process process = Process.GetProcessById((int)processId))
            {
                fildsadsePath = process.MainModule.FileName;
                module_name = process.MainModule.ModuleName;
                module_nasme = process.ProcessName;
            }
        }
        catch (System.Exception ex)
        {
            fildsadsePath = ex.Message;
        }

        log(DateTime.Now.ToString("") + " " + windowTitle + " " + fildsadsePath + module_nasme + "\n");
        return module_nasme;
    }


    [DllImport("user32.dll", SetLastError = true)]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    // 导入user32.dll中的GetForegroundWindow函数
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    // 导入user32.dll中的GetWindowText函数
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    // 获取窗口标题的辅助方法
    private static string GetWindowText(IntPtr hWnd)
    {
        const int nChars = 256;
        StringBuilder Buff = new StringBuilder(nChars);
        if (GetWindowText(hWnd, Buff, nChars) > 0)
        {
            return Buff.ToString();
        }
        return null;
    }
    private static readonly object _lockObject = new object();
    public static void log(string message)
    {
        //log(GetWindowText(GetForegroundWindow()));
        //File.AppendAllText("log.txt", "\r" + DateTime.Now.ToString("") + " " + message);
        // 使用using语句确保StreamWriter被正确关闭和释放  
        lock (_lockObject)
        {
            try
            {
                File.AppendAllText("log.txt", "\r" + DateTime.Now.ToString("") + " " + message);
            }
            catch (Exception)
            {
            }
        }
    }
}