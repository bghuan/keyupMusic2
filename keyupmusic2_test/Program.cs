using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace ProcessMetrics
{
    public class KeyboardMouseHook
    {
        protected virtual IntPtr KeyboardHookProc(int code, int wParam, ref KEYBOARDHOOKStruct lParam)
        {
            var args = new KeyEventArgs((wParam == 0x0101 || wParam == 0x0105) ? KeyboardType.KeyUp : KeyboardType.KeyDown, (Keys)lParam.vkCode, wParam, lParam);
            KeyEvent(args);
            if (args.Handled) return new IntPtr(-1);
            return CallNextHookEx(_key_hookId, code, wParam, ref lParam);
        }
        protected virtual IntPtr MouseHookProc(int nCode, int wParam, ref MOUSEHOOKSTRUCT lParam)
        {
            var args = new MouseEventArgs((MouseType)wParam, wParam, ref lParam);
            MouseEvent(args);
            if (args.Handled) return new IntPtr(-1);
            return CallNextHookEx(_mouse_hookId, nCode, wParam, ref lParam);
        }
        public class KeyEventArgs : EventArgs
        {
            public KeyboardType Type;
            public Keys key;
            public int wParam;
            public KEYBOARDHOOKStruct lParam;
            public bool Handled;

            public KeyEventArgs(KeyboardType type, Keys key, int wParam, KEYBOARDHOOKStruct lParam)
            {
                this.Type = type;
                this.wParam = wParam;
                this.lParam = lParam;
                this.key = key;
            }
        }
        public class MouseEventArgs : EventArgs
        {
            public MouseType Type;
            public int X;
            public int Y;
            public int wParam;
            public MOUSEHOOKSTRUCT lParam;
            public bool Handled;
            public int dwExtraInfo;
            public int data;
            public Point Pos => new Point(X, Y);

            public MouseEventArgs(MouseType type, int wParam, ref MOUSEHOOKSTRUCT lParam)
            {
                this.Type = type;
                this.X = lParam.pt.X;
                this.Y = lParam.pt.Y;
                this.wParam = wParam;
                this.lParam = lParam;

                if (type != MouseType.back && type != MouseType.back_up && type != MouseType.wheel) return;
                short buttonData = (short)((lParam.mouseData >> 16 & 0xFFFF));
                if (buttonData == 2 && (type == MouseType.back || type == MouseType.back_up))
                    this.Type = type == MouseType.back ? MouseType.go : MouseType.go_up;
                this.dwExtraInfo = lParam.dwExtraInfo;
                this.data = buttonData; //滚轮滚动数据
            }
        }
        public KeyboardMouseHook()
        {
            _keyboardHookProc = KeyboardHookProc;
            _mouseHookProc = MouseHookProc;
        }
        private IntPtr _key_hookId = IntPtr.Zero;
        public IntPtr _mouse_hookId = IntPtr.Zero;

        public event MouseHookEventHandler MouseEvent;
        public event KeyboardHookEventHandler KeyEvent;
        public delegate void MouseHookEventHandler(MouseEventArgs e);
        public delegate void KeyboardHookEventHandler(KeyEventArgs e);

        private LowLevelkeyboardHookProc _keyboardHookProc;
        private LowLevelMouseHookProc _mouseHookProc;
        public delegate IntPtr LowLevelkeyboardHookProc(int code, int wParam, ref KEYBOARDHOOKStruct lParam);
        public delegate IntPtr LowLevelMouseHookProc(int nCode, int wParam, ref MOUSEHOOKSTRUCT lParam);
        public void Install()
        {
            if (_key_hookId == IntPtr.Zero && KeyEvent != null)
                _key_hookId = SetKeyboardHook(_keyboardHookProc);
            if (_mouse_hookId == IntPtr.Zero && MouseEvent != null)
                _mouse_hookId = SetMouseHook(_mouseHookProc);
        }
        public void Uninstall()
        {
            if (_key_hookId != IntPtr.Zero) UnhookWindowsHookEx(_key_hookId);
            if (_mouse_hookId != IntPtr.Zero) UnhookWindowsHookEx(_mouse_hookId);
            _key_hookId = IntPtr.Zero;
            _mouse_hookId = IntPtr.Zero;
        }
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

        public const int WH_MOUSE_LL = 14;
        public const int WH_KEYBOARD_LL = 13;
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseHookProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelkeyboardHookProc callback, IntPtr hInstance, uint threadId);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KEYBOARDHOOKStruct lParam);
        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref MOUSEHOOKSTRUCT lParam);
        public struct KEYBOARDHOOKStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        public struct MOUSEHOOKSTRUCT
        {
            public Point pt;
            public int mouseData;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        public enum MouseType
        {
            move = 0x0200,
            click = 0x0201,
            click_up = 0x0202,
            click_r = 0x0204,
            click_r_up = 0x0205,
            middle = 0x0207,
            middle_up = 0X0208,
            wheel = 0x020A,
            back = 0x020B,
            back_up = 0x020C,
            go = 0x920B, //自定义
            go_up = 0x920C, //自定义
            none = 0x920D, //自定义
        }
        public enum KeyboardType
        {
            KeyDown, KeyUp
        }
    }
    class Program
    {
        static KeyboardMouseHook _KeyboardMouseHook = new KeyboardMouseHook();
      
        private static void KeyBoardHookProc(KeyboardMouseHook.KeyEventArgs e)
        {
            var str = e.key.ToString() + e.Type;
            File.WriteAllText("a.txt", str);
            //Invoke(() => { label1.Text = str; });
        }
        private static void MouseHookProc(KeyboardMouseHook.MouseEventArgs e)
        {
            var str = e.Pos.ToString() + e.Type.ToString() + (e.data == 0 ? "" : e.data.ToString());
            //Invoke(() => { label1.Text = str; });
        }
        static void Main(string[] args)
        {

            //File.WriteAllText("a.txt", "aaa");
            //_KeyboardMouseHook.KeyEvent += KeyBoardHookProc;
            ////_KeyboardMouseHook.MouseEvent += MouseHookProc;
            //_KeyboardMouseHook.Install();
            //HideProcess();
            Console.ReadLine();
        }
        public static void HideProcess()
        {
            IntPtr hwnd = GetForegroundWindow(); // 获取当前活动窗口的句柄
            ShowWindow(hwnd, 0);
        }
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
    }
}
