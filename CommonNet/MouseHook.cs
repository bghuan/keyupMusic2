using System.ComponentModel;
using System.Diagnostics;
using WGestures.Common.OsSpecific.Windows;
using Win32;

namespace WGestures.Core.Impl.Windows
{
    public class MouseKeyboardHook : IDisposable
    {
        public static List<Keys> stop_keys = new List<Keys>();
        public static bool mouse_downing = false;
        public static bool handling = false;
        protected virtual int KeyboardHookProc(int code, int wParam, ref Native.keyboardHookStruct lParam)
        {
            var key = (Keys)lParam.vkCode;
            {
                KeyboardEventType type;

                if ((wParam == (int)User32.WM.WM_KEYDOWN || wParam == (int)User32.WM.WM_SYSKEYDOWN))
                {
                    type = KeyboardEventType.KeyDown;
                }
                else if ((wParam == (int)User32.WM.WM_KEYUP || wParam == (int)User32.WM.WM_SYSKEYUP))
                {
                    type = KeyboardEventType.KeyUp;
                }
                else return Native.CallNextHookEx(_hookId, code, wParam, ref lParam);

                var args = new KeyboardHookEventArgs(type, key, wParam, lParam);
                if (stop_keys.Count == 0 || !stop_keys.Contains(key) || type == KeyboardEventType.KeyUp || key == Keys.VolumeDown || key == Keys.VolumeUp)
                    KeyboardHookEvent(args);

                if (args.Handled)
                    return 1;
            }

            return Native.CallNextHookEx(_hookId, code, wParam, ref lParam);
        }
        const int WM_HOOK_TIMEOUT = (int)User32.WM.WM_USER + 1;

        private IntPtr _hookId;
        private IntPtr _kbdHookId;
        private uint _hookThreadNativeId;
        private Thread _hookThread;

        private Native.LowLevelMouseHookProc _mouseHookProc;
        private Native.LowLevelkeyboardHookProc _kbdHookProc;

        public class MouseHookEventArgs : EventArgs
        {
            public MouseMsg Msg { get; private set; }
            public int X { get; private set; }
            public int Y { get; private set; }

            public Point Pos => new Point() { X = X, Y = Y };

            public IntPtr wParam;
            public IntPtr lParam;

            public bool Handled { get; set; }

            public MouseHookEventArgs(MouseMsg msg, int x, int y, IntPtr wParam, IntPtr lParam)
            {
                Msg = msg;
                X = x;
                Y = y;

                this.wParam = wParam;
                this.lParam = lParam;
            }
        }
        public class KeyboardHookEventArgs : EventArgs
        {
            public KeyboardEventType Type;
            public int wParam;
            public Native.keyboardHookStruct lParam;
            public Keys key;
            public bool Handled;
            public bool Handling;
            public int X;
            public int Y;

            public KeyboardHookEventArgs(KeyboardEventType type, Keys key, int wParam, Native.keyboardHookStruct lParam)
            {
                Type = type;
                this.wParam = wParam;
                this.lParam = lParam;
                this.key = key;

                this.X = Cursor.Position.X;
                this.Y = Cursor.Position.Y;
            }
        }

        public delegate void MouseHookEventHandler(MouseHookEventArgs e);
        public delegate void KeyboardHookEventHandler(KeyboardHookEventArgs e);

        public event MouseHookEventHandler MouseHookEvent;
        public event KeyboardHookEventHandler KeyboardHookEvent;
        public event Func<Native.MSG, bool> GotMessage;
        public MouseKeyboardHook()
        {
            _mouseHookProc = MouseHookProc;
            _kbdHookProc = KeyboardHookProc;
        }
        public void Install()
        {
            if (_hookThread != null) throw new InvalidOperationException("钩子已经安装了");

            if (MouseHookEvent != null)
                _hookId = Native.SetMouseHook(_mouseHookProc);
            if (KeyboardHookEvent != null)
                _kbdHookId = Native.SetKeyboardHook(_kbdHookProc);
        }

        public void Uninstall()
        {
            if (_hookId != IntPtr.Zero)
                Native.UnhookWindowsHookEx(_hookId);
            if (_kbdHookId != IntPtr.Zero)
                Native.UnhookWindowsHookEx(_kbdHookId);
            _hookId = IntPtr.Zero;
            _kbdHookId = IntPtr.Zero;
        }
        protected virtual IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Native.POINT curPos;
            Native.GetCursorPos(out curPos);
            var args = new MouseHookEventArgs((MouseMsg)wParam, curPos.x, curPos.y, wParam, lParam);

            if (MouseHookEvent != null)
                MouseHookEvent(args);

            return args.Handled ? new IntPtr(-1) : Native.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        protected virtual void Dispose(bool disposing)
        {
            Uninstall();
        }

        public void Dispose()
        {
            Dispose(true);
        }
        ~MouseKeyboardHook()
        {
            Dispose(false);
        }
    }

    public enum MouseMsg
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,

        WM_MOUSEWHEEL = 0x020A,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0X0208,

        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,

        WM_XBUTTONDOWN = 0x020B,
        WM_XBUTTONUP = 0x020C
    }

    public enum KeyboardEventType
    {
        KeyDown, KeyUp
    }

}
