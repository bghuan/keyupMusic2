namespace keyupMusic2
{
    public class MouseKeyboardHook : IDisposable
    {
        public static Dictionary<Keys, string> stop_keys = new Dictionary<Keys, string>();
        public static bool mouse_downing = false;
        public static bool handling = false;
        protected virtual int KeyboardHookProc(int code, int wParam, ref Native.keyboardHookStruct lParam)
        {
            var key = (Keys)lParam.vkCode;
            {
                KeyboardEventType type;

                if ((wParam == 0x0100 || wParam == 0x0104))
                {
                    type = KeyboardEventType.KeyDown;
                }
                else if ((wParam == 0x0101 || wParam == 0x0105))
                {
                    type = KeyboardEventType.KeyUp;
                }
                else
                    return Native.CallNextHookEx(_key_hookId, code, wParam, ref lParam);

                //if (key == Keys.F22) return 1;
                if (key == (Keys.LButton | Keys.XButton2)) return 1;
                var args = new KeyboardHookEventArgs(type, key, wParam, lParam);
                if (args.isVir) return Native.CallNextHookEx(_key_hookId, code, wParam, ref lParam);
                if (stop_keys.Count == 0 || !stop_keys.ContainsKey(key) || type == KeyboardEventType.KeyUp || key == Keys.VolumeDown || key == Keys.VolumeUp)
                    KeyboardHookEvent(args);

                if (args.Handled) return 1;
            }

            return Native.CallNextHookEx(_key_hookId, code, wParam, ref lParam);
        }
        protected virtual IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Point curPos;
            Native.GetCursorPos(out curPos);
            var args = new MouseHookEventArgs((MouseMsg)wParam, curPos.X, curPos.Y, wParam, lParam);

            if (MouseHookEvent != null)
                MouseHookEvent(args);

            return args.Handled ? new IntPtr(-1) : Native.CallNextHookEx(_mouse_hookId, nCode, wParam, lParam);
        }

        const int WM_HOOK_TIMEOUT = 0x0400 + 1;

        private IntPtr _key_hookId = IntPtr.Zero;
        public IntPtr _mouse_hookId = IntPtr.Zero;
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
            public bool isVir;
            public Point Pos => new Point() { X = X, Y = Y };

            public KeyboardHookEventArgs(KeyboardEventType type, Keys key, int wParam, Native.keyboardHookStruct lParam)
            {
                Type = type;
                this.wParam = wParam;
                this.lParam = lParam;
                this.key = key;
                this.isVir = lParam.dwExtraInfo == Common.isVirConst;

                this.X = Cursor.Position.X;
                this.Y = Cursor.Position.Y;
            }
        }

        public delegate void MouseHookEventHandler(MouseHookEventArgs e);
        public delegate void KeyboardHookEventHandler(KeyboardHookEventArgs e);

        public event MouseHookEventHandler MouseHookEvent;
        public event KeyboardHookEventHandler KeyboardHookEvent;
        public MouseKeyboardHook()
        {
            _kbdHookProc = KeyboardHookProc;
            _mouseHookProc = MouseHookProc;
        }
        public void Install()
        {
            if (_key_hookId == IntPtr.Zero && KeyboardHookEvent != null)
                _key_hookId = Native.SetKeyboardHook(_kbdHookProc);
            if (_mouse_hookId == IntPtr.Zero && MouseHookEvent != null)
                _mouse_hookId = Native.SetMouseHook(_mouseHookProc);
        }

        public void Uninstall()
        {
            if (_key_hookId != IntPtr.Zero)
                Native.UnhookWindowsHookEx(_key_hookId);
            if (_mouse_hookId != IntPtr.Zero)
                Native.UnhookWindowsHookEx(_mouse_hookId);
            _key_hookId = IntPtr.Zero;
            _mouse_hookId = IntPtr.Zero;
        }
        public void ChangeMouseHooks()
        {
            if (_mouse_hookId == IntPtr.Zero && MouseHookEvent != null)
            {
                _mouse_hookId = Native.SetMouseHook(_mouseHookProc);
            }
            else if (_mouse_hookId != IntPtr.Zero)
            {
                Native.UnhookWindowsHookEx(_mouse_hookId);
                _mouse_hookId = IntPtr.Zero;
            }
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
