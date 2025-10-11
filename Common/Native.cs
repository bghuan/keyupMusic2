using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace keyupMusic2
{
    public static class Native
    {
        public delegate IntPtr LowLevelMouseHookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate IntPtr LowLevelkeyboardHookProc(int code, int wParam, IntPtr lParam);
        public const int WH_MOUSE_LL = 14;
        public const int WH_KEYBOARD_LL = 13;

        public static IntPtr SetMouseHook(LowLevelMouseHookProc proc, string ModuleName)
        {
            return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(ModuleName), 0);
        }

        public static IntPtr SetKeyboardHook(LowLevelkeyboardHookProc proc, string ModuleName)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(ModuleName), 0);
        }

        public struct keyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point Point);


        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);
        public static int VK_LBUTTON = 0x01;
        public static int VK_RBUTTON = 0x02;
        public static int VK_XBUTTON1 = 0x05;
        public static int VK_XBUTTON2 = 0x06;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, SW nCmdShow);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseHookProc lpfn,
            IntPtr hMod,
            uint dwThreadId);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelkeyboardHookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetWindowText(IntPtr hWnd, string text);
        public enum SW
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,     // 等价于 SW_NORMAL (已弃用)
            SW_SHOWMINIMIZED = 2,  // 显示并最小化
            SW_SHOWMAXIMIZED = 3,  // 显示并最大化
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,// 最小化但不激活
            SW_SHOWNA = 8,
            SW_RESTORE = 9,        // 等价于 SW_SHOWNORMAL
            SW_SHOWDEFAULT = 10,   // 根据启动参数显示窗口
            SW_FORCEMINIMIZE = 11  // 强制最小化（不激活）
        }
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        public const int WM_APPCOMMAND = 0x319;
        public const uint MAX_VOLUME = 0xFFFF;
        public const int APPCOMMAND_VOLUME_UNMUTE = 0x80000;
        public const uint WM_KEYDOWN = 0x0100;
        public const uint WM_KEYUP = 0x0101;

        [DllImport("winmm.dll")]
        public static extern uint waveOutSetVolume(IntPtr hwo, uint dwVolume);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        // Structure to hold window rectangle  
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // Constants for GetSystemMetrics  
        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        // 导入user32.dll中的GetWindowText函数
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public static int MOUSEEVENTF_MOVE = 0x0001;
        public static int MOUSEEVENTF_LEFTDOWN = 0x0002;
        public static int MOUSEEVENTF_LEFTUP = 0x0004;
        public static int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public static int MOUSEEVENTF_RIGHTUP = 0x0010;
        public static int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public static int MOUSEEVENTF_MIDDLEUP = 0x0040;
        public static int MOUSEEVENTF_ABSOLUTE = 0x8000;
        // 定义 MOUSEEVENTF_WHEEL 标志  
        public const int MOUSEEVENTF_WHEEL = 0x0800;
        public const int WHEEL_DELTA = 120;
        // 鼠标事件标志
        public const int MOUSEEVENTF_XDOWN = 0x0080;
        public const int MOUSEEVENTF_XUP = 0x0100;
        public const int XBUTTON1 = 0x0001; // 鼠标后退按钮
        public const int XBUTTON2 = 0x0002; // 鼠标前进按钮



        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //// 声明 mouse_event 函数  
        //[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);


        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(HandleRef hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr WindowHandle);
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
        public const int WM_CLOSE = 0x0010;
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public const int GWL_STYLE = -16;
        public const int WS_CAPTION = 0x00C00000;
        public const int WS_BORDER = 0x00800000;
        public const int WS_THICKFRAME = 0x00040000;
        public const uint SWP_FRAMECHANGED = 0x0020;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOACTIVATE = 0x0010;
        // 窗口消息常量
        public const uint WM_NCLBUTTONDOWN = 0x00A1;
        public const int HTCAPTION = 2;
        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint); [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("Kernel32.dll", EntryPoint = "GetTickCount", CharSet = CharSet.Auto)]
        internal static extern int GetTickCount();
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(UInt32 uCode, UInt32 uMapType);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern uint GetDpiForWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();
        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);    // === Window枚举和标题 ===
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        // 常量定义
        public const int GWL_EXSTYLE = -20;               // 扩展窗口样式索引
        public const int WS_EX_LAYERED = 0x80000;         // 分层窗口样式
        public const int LWA_ALPHA = 0x2;                 // 透明度调节标志
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern IntPtr LocalAlloc(int uFlags, int uBytes);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LocalFree(IntPtr hMem);
        // 窗口位置参数常量
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);    // 置顶
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);  // 取消置顶
        public const uint SWP_SHOWWINDOW = 0x0040;  // 确保窗口可见
        public const int WS_EX_TRANSPARENT = 0x20;

    }
}
