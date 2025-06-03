using System.Runtime.InteropServices;

namespace keyupMusic2
{
    public class Simulate
    {
        public Simulate(int tick = 100)
        {
            this.tick = tick;
        }
        public int tick = 0;

        //public Simulate KeyPress(params object[] inputs)
        //    => inputs.Aggregate(this, (sim, input)
        //    => input is Keys key ? sim.KeyPress(key) :
        //       input is Keys[] keys ? sim.KeyPress(keys) :
        //       input is string str ? sim.KeyPress(str) :
        //       sim);

        public Simulate KeyPress(string text)
        {
            if (text == null || text.Length == 0) return this;
            var inputs = new KeyboardInput.INPUT[text.Length * 2];

            for (int i = 0; i < text.Length; i++)
            {
                inputs[i * 2] = KeyboardInput.INPUT.CreateStringDown(text[i]);
                inputs[i * 2 + 1] = KeyboardInput.INPUT.CreateStringUp(text[i]);
            }

            KeyboardInput.SendInput((uint)inputs.Length, ref inputs[0], KeyboardInput.INPUT.Size);

            if (tick > 0) Thread.Sleep(tick);
            return this;
        }
        public Simulate KeyPress(Keys key, bool Extend = false)
        {
            var inputs = new KeyboardInput.INPUT[2];

            inputs[0] = KeyboardInput.INPUT.CreateKeyDown((ushort)key, Extend);
            inputs[1] = KeyboardInput.INPUT.CreateKeyUp((ushort)key, Extend);

            KeyboardInput.SendInput((uint)inputs.Length, ref inputs[0], KeyboardInput.INPUT.Size);

            if (tick > 0) Thread.Sleep(tick);
            return this;
        }
        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern uint SendInput(UInt32 numberOfInputs, KeyboardInput.INPUT[] inputs, Int32 sizeOfInputStructure);
        public Simulate KeyPress(Keys[] key)
        {
            var inputs = new KeyboardInput.INPUT[key.Length * 2];

            for (int i = 0; i < key.Length; i++)
            {
                inputs[i] = KeyboardInput.INPUT.CreateKeyDown((ushort)key[i]);
            }
            for (int i = 0; i < key.Length; i++)
            {
                inputs[i + key.Length] = KeyboardInput.INPUT.CreateKeyUp((ushort)key[key.Length - 1 - i]);
            }

            KeyboardInput.SendInput((uint)inputs.Length, ref inputs[0], KeyboardInput.INPUT.Size);

            if (tick > 0) Thread.Sleep(tick);
            return this;
        }
        public Simulate KeyDown(Keys key)
        {
            var inputs = new KeyboardInput.INPUT[1];
            inputs[0] = KeyboardInput.INPUT.CreateKeyDown((ushort)key);

            KeyboardInput.SendInput((uint)inputs.Length, ref inputs[0], KeyboardInput.INPUT.Size);

            if (tick > 0) Thread.Sleep(tick);
            return this;
        }
        public Simulate KeyUp(Keys key)
        {
            var inputs = new KeyboardInput.INPUT[1];
            inputs[0] = KeyboardInput.INPUT.CreateKeyUp((ushort)key);

            KeyboardInput.SendInput((uint)inputs.Length, ref inputs[0], KeyboardInput.INPUT.Size);

            if (tick > 0) Thread.Sleep(tick);
            return this;
        }

        public Simulate MouseWhell(int delta)
        {
            var input = new KeyboardInput.INPUT[1];
            input[0].Type = KeyboardInput.INPUT.INPUTTYPE.Mouse;

            input[0].Data.Mouse.mouseData = (uint)delta;
            input[0].Data.Mouse.Flags = Native.MOUSEEVENTF_WHEEL;
            input[0].Data.Mouse.time = (uint)Native.GetTickCount();
            input[0].Data.Mouse.dwExtraInfo = (nint)Common.isVir;

            KeyboardInput.SendInput((uint)input.Length, ref input[0], Marshal.SizeOf(input[0]));

            if (tick > 0) Thread.Sleep(tick);
            return this;
        }
        public Simulate Wait(int tick)
        {
            Thread.Sleep(tick);
            return this;
        }
        public Simulate Sleep(int tick)
        {
            return Wait(tick);
        }
    }
    public class KeyboardInput
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public INPUTTYPE Type;
            public INPUTUNION Data;
            public static int Size => Marshal.SizeOf(typeof(INPUT));

            [StructLayout(LayoutKind.Explicit)]
            public struct INPUTUNION
            {
                [FieldOffset(0)] public MOUSEINPUT Mouse;
                [FieldOffset(0)] public KEYBDINPUT Keyboard;

                public struct MOUSEINPUT
                {
                    public int dx;
                    public int dy;
                    public uint mouseData;
                    public uint Flags;
                    public uint time;
                    public IntPtr dwExtraInfo;
                }

                public struct KEYBDINPUT
                {
                    public ushort wVk;
                    public ushort ScanCode;
                    public KeyboardFlag Flags;
                    public uint time;
                    public IntPtr dwExtraInfo;
                }
            }
            public static INPUT CreateKeyDown(ushort virtualKey, bool Extend = false)
            {
                return new INPUT
                {
                    Type = INPUTTYPE.Keyboard,
                    Data = new INPUTUNION
                    {
                        Keyboard = new INPUTUNION.KEYBDINPUT
                        {
                            wVk = virtualKey,
                            ScanCode = (ushort)(Native.MapVirtualKey(virtualKey, 0) & 0xFFU),
                            Flags = Extend ? KeyboardFlag.KeyDown | KeyboardFlag.ExtendedKey : KeyboardFlag.KeyDown,
                            time = 0,
                            dwExtraInfo = (nint)Common.isVir
                        }
                    }
                };
            }
            public static INPUT CreateKeyUp(ushort virtualKey, bool Extend = false)
            {
                return new INPUT
                {
                    Type = INPUTTYPE.Keyboard,
                    Data = new INPUTUNION
                    {
                        Keyboard = new INPUTUNION.KEYBDINPUT
                        {
                            wVk = virtualKey,
                            ScanCode = (ushort)(Native.MapVirtualKey(virtualKey, 0) & 0xFFU),
                            Flags = Extend ? KeyboardFlag.KeyUp | KeyboardFlag.ExtendedKey : KeyboardFlag.KeyUp,
                            time = 0,
                            dwExtraInfo = (nint)Common.isVir
                        }
                    }
                };
            }
            public static INPUT CreateStringDown(ushort virtualKey)
            {
                return new INPUT
                {
                    Type = INPUTTYPE.Keyboard,
                    Data = new INPUTUNION
                    {
                        Keyboard = new INPUTUNION.KEYBDINPUT
                        {
                            wVk = 0,
                            ScanCode = virtualKey,
                            Flags = KeyboardFlag.Unicode,
                            time = 0,
                            dwExtraInfo = (nint)Common.isVir
                        }
                    }
                };
            }
            public static INPUT CreateStringUp(ushort virtualKey)
            {
                return new INPUT
                {
                    Type = INPUTTYPE.Keyboard,
                    Data = new INPUTUNION
                    {
                        Keyboard = new INPUTUNION.KEYBDINPUT
                        {
                            wVk = 0,
                            ScanCode = virtualKey,
                            Flags = KeyboardFlag.Unicode | KeyboardFlag.KeyUp,
                            time = 0,
                            dwExtraInfo = (nint)Common.isVir
                        }
                    }
                };
            }
            public enum INPUTTYPE : uint
            {
                Mouse = 0,
                Keyboard = 1,
            }
            [Flags]
            public enum KeyboardFlag : uint
            {
                None = 0x0000,
                KeyDown = 0x000,
                ExtendedKey = 0x0001,
                KeyUp = 0x0002,
                Unicode = 0x0004,
                ScanCode = 0x0008,
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);
    }
}
