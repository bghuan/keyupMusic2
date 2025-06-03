using System.Runtime.InteropServices;

namespace keyupMusic2
{
    public class MouseKeyboardHook : IDisposable
    {
        public static Dictionary<Keys, string> handling_keys = new Dictionary<Keys, string>();
        public static bool handling = false;
        protected virtual int KeyboardHookProc(int code, int wParam, ref Native.keyboardHookStruct lParam)
        {
            var args = new KeyboardHookEventArgs(0, (Keys)lParam.vkCode, wParam, lParam);
            if (!args.isVir)
                KeyboardHookEvent(args);
            if (args.Handled)
                return 1;

            return Native.CallNextHookEx(_key_hookId, code, wParam, ref lParam);
        }

        protected virtual IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Point curPos;
            Native.GetCursorPos(out curPos);
            var args = new MouseHookEventArgs((MouseMsg)wParam, curPos.X, curPos.Y, wParam, lParam);

            Param_Data(lParam, args);
            if (args.isVir) return Native.CallNextHookEx(_mouse_hookId, nCode, wParam, lParam);

            if (MouseHookEvent != null)
                MouseHookEvent(args);

            return args.Handled ? new IntPtr(-1) : Native.CallNextHookEx(_mouse_hookId, nCode, wParam, lParam);
        }
        private static void Param_Data(nint lParam, MouseHookEventArgs args)
        {
            if (args.Msg == MouseMsg.move) return;

            MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)lParam;

            args.dwExtraInfo = (int)hookStruct.dwExtraInfo;
            args.data = (int)hookStruct.mouseData;
            short buttonData = (short)((hookStruct.mouseData >> 16 & 0xFFFF));

            if (buttonData == 2 && (args.Msg == MouseMsg.back || args.Msg == MouseMsg.back_up))
            {
                args.Msg = args.Msg == MouseMsg.back
                           ? MouseMsg.go
                           : MouseMsg.go_up;
            }
            if (args.Msg == MouseMsg.wheel)
            {
                var scrollAmount = buttonData / 120;
                args.data = scrollAmount;
            }
        }

        private IntPtr _key_hookId = IntPtr.Zero;
        public IntPtr _mouse_hookId = IntPtr.Zero;
        private uint _hookThreadNativeId;
        private Thread _hookThread;

        private Native.LowLevelMouseHookProc _mouseHookProc;
        private Native.LowLevelkeyboardHookProc _kbdHookProc;

        public class MouseHookEventArgs : EventArgs
        {
            public MouseMsg Msg { get; set; }
            public int X { get; private set; }
            public int Y { get; private set; }
            public bool Handled { get; set; }

            public IntPtr wParam;
            public IntPtr lParam;
            public Point Pos => new Point(X, Y);
            public int data;
            public int dwExtraInfo;
            public bool isVir => dwExtraInfo == Common.isVirConst;

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
            public KeyboardType Type;
            public int wParam;
            public Native.keyboardHookStruct lParam;
            public Keys key;
            public bool Handled;
            public bool Handling;
            public int X;
            public int Y;
            public bool isVir;
            public Point Pos => new Point() { X = X, Y = Y };

            public KeyboardHookEventArgs(KeyboardType type, Keys key, int wParam, Native.keyboardHookStruct lParam)
            {
                Type = (wParam == 0x0101 || wParam == 0x0105) ? KeyboardType.KeyUp : KeyboardType.KeyDown;
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
        click = 0x0201,
        click_up = 0x0202,
        move = 0x0200,

        wheel = 0x020A,
        middle = 0x0207,
        middle_up = 0X0208,

        click_r = 0x0204,
        click_r_up = 0x0205,

        back = 0x020B,
        back_up = 0x020C,

        go = 0x920B,
        go_up = 0x920C,

        none = 0x920D,
    }

    public enum KeyboardType
    {
        KeyDown, KeyUp
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHOOKSTRUCT
    {
        public Point pt;
        public uint mouseData;  // 高16位包含XButton信息
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;

        public static explicit operator MSLLHOOKSTRUCT(nint v)
        {
            return (MSLLHOOKSTRUCT)Marshal.PtrToStructure(v, typeof(MSLLHOOKSTRUCT));
        }
    }

}
