using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices;

namespace keyupMusic2
{
    public class KeyboardInput3
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Input
        {
            public uint type;
            public InputUnion U;
            public static int Size => Marshal.SizeOf(typeof(Input));
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)] public MouseInput mi;
            [FieldOffset(0)] public KeyboardInput ki;
            [FieldOffset(0)] public HardwareInput hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr hExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardInput
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr hExtraInfo;
            public uint Unicode; // Only used if dwFlags contains KEYEVENTF_UNICODE  
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HardwareInput
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        private const uint INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const uint KEYEVENTF_UNICODE = 0x0004;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, ref Input pInputs, int cbSize);

        public static void SendString(string text)
        {
            var inputs = new Input[text.Length * 2]; // 每个字符两个事件：按下和释放  
            int index = 0;

            foreach (char c in text)
            {
                var kiDown = new KeyboardInput
                {
                    wVk = 0,
                    wScan = 0,
                    dwFlags = KEYEVENTF_UNICODE | 0, // 0 for press  
                    time = 0,
                    hExtraInfo = IntPtr.Zero,
                    Unicode = c
                };

                var kiUp = kiDown;
                kiUp.dwFlags |= KEYEVENTF_KEYUP;

                inputs[index++] = new Input { type = INPUT_KEYBOARD, U = new InputUnion { ki = kiDown } };
                inputs[index++] = new Input { type = INPUT_KEYBOARD, U = new InputUnion { ki = kiUp } };
            }

            SendInput((uint)inputs.Length, ref inputs[0], Input.Size);
        }
    }
}
