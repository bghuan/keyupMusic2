using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace keyupMusic2
{
    public static class Native
    {
        public delegate IntPtr LowLevelMouseHookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate int LowLevelkeyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);
        public const int WH_MOUSE_LL = 14;
        public const int WH_KEYBOARD_LL = 13;

        public static IntPtr SetMouseHook(LowLevelMouseHookProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public static IntPtr SetKeyboardHook(LowLevelkeyboardHookProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
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
        public static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);
        public static int VK_LBUTTON = 0x01;
        public static int VK_RBUTTON = 0x02;

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
        // 引入FindWindow函数
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // 引入SetWindowText函数
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetWindowText(IntPtr hWnd, string text);
        public enum SW
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWNA = 8,
            SW_SHOWMAXIMIZED = 11,
            SW_MAXIMIZE = 12,
            SW_RESTORE = 13
        }
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        public const int WM_APPCOMMAND = 0x319;
        public const uint MAX_VOLUME = 0xFFFF;
        public const int APPCOMMAND_VOLUME_UNMUTE = 0x80000;

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

        // 导入user32.dll中的GetForegroundWindow函数
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
        public const int SW_RESTORE = 9;


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
    }
}
