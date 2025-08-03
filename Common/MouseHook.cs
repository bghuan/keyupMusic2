using System.Diagnostics;
using System.Runtime.InteropServices;

namespace keyupMusic2
{
    public class KeyboardMouseHook
    {
        protected virtual IntPtr KeyboardHookProc(int code, int wParam, ref Native.keyboardHookStruct lParam)
        {
            var args = new KeyEventArgs(0, (Keys)lParam.vkCode, wParam, lParam);
            if (!args.isVir)
                KeyEvent(args);
            if (args.Handled)
                return new IntPtr(-1);

            return Native.CallNextHookEx(_key_hookId, code, wParam, ref lParam);
        }

        protected virtual IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var args = new MouseEventArgs((MouseMsg)wParam, Cursor.Position.X, Cursor.Position.Y, wParam, lParam);
            Param_Data(lParam, args);

            if (!args.isVir)
                MouseEvent(args);
            if (args.Handled)
                return new IntPtr(-1);

            return Native.CallNextHookEx(_mouse_hookId, nCode, wParam, lParam);
        }
        private static void Param_Data(nint lParam, MouseEventArgs args)
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

        private Native.LowLevelMouseHookProc _mouseHookProc;
        private Native.LowLevelkeyboardHookProc _kbdHookProc;

        public class MouseEventArgs : EventArgs
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

            public MouseEventArgs(MouseMsg msg, int x, int y, IntPtr wParam, IntPtr lParam)
            {
                Msg = msg;
                X = x;
                Y = y;

                this.wParam = wParam;
                this.lParam = lParam;
            }
        }
        public class KeyEventArgs : EventArgs
        {
            public KeyType Type;
            public int wParam;
            public Native.keyboardHookStruct lParam;
            public Keys key;
            public bool Handled;
            public bool Handling;
            public int X;
            public int Y;
            public bool isVir;
            public Point Pos => new Point() { X = X, Y = Y };

            public KeyEventArgs(KeyType type, Keys key, int wParam, Native.keyboardHookStruct lParam)
            {
                Type = (wParam == 0x0101 || wParam == 0x0105) ? KeyType.Up : KeyType.Down;
                this.wParam = wParam;
                this.lParam = lParam;
                this.key = key;
                this.isVir = lParam.dwExtraInfo == Common.isVirConst;

                this.X = Cursor.Position.X;
                this.Y = Cursor.Position.Y;
            }
        }
        public event Action<MouseEventArgs> MouseEvent;
        public event Action<KeyEventArgs> KeyEvent;
        public string ModuleName => Process.GetCurrentProcess().MainModule.ModuleName;
        public KeyboardMouseHook()
        {
            _kbdHookProc = KeyboardHookProc;
            _mouseHookProc = MouseHookProc;
        }
        public void Install()
        {
            if (_key_hookId == IntPtr.Zero && KeyEvent != null)
                _key_hookId = Native.SetKeyboardHook(_kbdHookProc, ModuleName);
            if (_mouse_hookId == IntPtr.Zero && MouseEvent != null)
                _mouse_hookId = Native.SetMouseHook(_mouseHookProc, ModuleName);
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
            if (_mouse_hookId == IntPtr.Zero && MouseEvent != null)
            {
                _mouse_hookId = Native.SetMouseHook(_mouseHookProc, ModuleName);
            }
            else if (_mouse_hookId != IntPtr.Zero)
            {
                Native.UnhookWindowsHookEx(_mouse_hookId);
                _mouse_hookId = IntPtr.Zero;
            }
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

    public enum KeyType
    {
        Down, Up
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
