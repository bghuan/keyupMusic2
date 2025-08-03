using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

class HighPerformanceKeyboardHook
{
    // 低级别键盘钩子ID
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;

    // 用于存储待处理的键盘事件
    private static readonly ConcurrentQueue<ConsoleKey> _keyQueue = new ConcurrentQueue<ConsoleKey>();
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;
    private static Thread _processingThread;
    private static bool _isRunning;

    public static void Start()
    {
        _hookID = SetHook(_proc);
        _isRunning = true;

        // 启动独立的事件处理线程
        _processingThread = new Thread(ProcessKeyEvents)
        {
            IsBackground = true,
            Priority = ThreadPriority.BelowNormal // 低于正常优先级，不影响系统响应
        };
        _processingThread.Start();
    }

    public static void Stop()
    {
        _isRunning = false;
        UnhookWindowsHookEx(_hookID);
        _processingThread?.Join();
    }

    // 钩子回调函数 - 仅负责收集事件，不做任何处理
    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            // 只在按键按下时处理（避免重复处理）
            if (wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                // 只做最基本的转换，然后放入队列
                _keyQueue.Enqueue((ConsoleKey)vkCode);
            }
        }
        // 立即传递给下一个钩子，不阻塞
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    // 独立线程处理键盘事件
    private static void ProcessKeyEvents()
    {
        while (_isRunning)
        {
            // 从队列中获取事件并处理
            if (_keyQueue.TryDequeue(out ConsoleKey key))
            {
                // 这里可以处理复杂逻辑，但避免长时间阻塞
                Console.WriteLine($"Key pressed: {key}");
            }
            else
            {
                // 队列空时短暂休眠，减少CPU占用
                Thread.Sleep(1);
            }
        }
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    #region Windows API 导入
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    #endregion
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("高性能键盘钩子演示 - 按ESC键退出");
        HighPerformanceKeyboardHook.Start();

        // 等待用户按下ESC键退出
        while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }

        HighPerformanceKeyboardHook.Stop();
        Console.WriteLine("\n程序已退出");
    }
}
