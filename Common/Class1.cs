using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static Win32.User32;

namespace keyupMusic2
{
    public class KeyboardInput
    {
        public static void SimulateMouseWheel(uint msg)
        {
            KeyboardInput.INPUT.SimulateMouseWheel(msg);
        }

        // 声明Windows API函数  
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        //[DllImport("user32.dll", SetLastError = true)]
        //public unsafe static extern UInt32 SendInput(UInt32 numberOfInputs, INPUT* inputs, Int32 sizeOfInputStructure);

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public uint type;
            public InputUnion U;
            public static int Size => Marshal.SizeOf(typeof(INPUT));

            [StructLayout(LayoutKind.Explicit)]
            public struct InputUnion
            {
                [FieldOffset(0)] public MOUSEINPUT mi;
                [FieldOffset(0)] public KEYBDINPUT ki;
                [FieldOffset(0)] public HARDWAREINPUT hi;

                public struct MOUSEINPUT
                {
                    public int dx;
                    public int dy;
                    public uint mouseData;
                    public uint dwFlags;
                    public uint time;
                    public IntPtr dwExtraInfo;
                }

                public struct KEYBDINPUT
                {
                    public ushort wVk;
                    public ushort wScan;
                    public uint dwFlags;
                    public uint time;
                    public IntPtr dwExtraInfo;
                }

                public struct HARDWAREINPUT
                {
                    public uint uMsg;
                    public ushort wParamL;
                    public ushort wParamH;
                }
            }
            public const uint INPUT_MOUSE = 1;
            public const uint MOUSEEVENTF_WHEEL = 0x0800;


            public static void SimulateMouseWheel(uint delta)
            {
                INPUT inputDown = new INPUT
                {
                    type = INPUT_MOUSE,
                    U = new InputUnion
                    {
                        mi = new InputUnion.MOUSEINPUT
                        {
                            dx = 0,
                            dy = 0,
                            mouseData = delta, // 滚轮滚动的量，正值向上滚动，负值向下滚动  
                            dwFlags = MOUSEEVENTF_WHEEL,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };

                SendInput(1, ref inputDown, Marshal.SizeOf(typeof(INPUT)));
            }
            public static INPUT CreateKeyDown(ushort virtualKey)
            {
                return new INPUT
                {
                    type = 1, // INPUT_KEYBOARD  
                    U = new InputUnion
                    {
                        ki = new InputUnion.KEYBDINPUT
                        {
                            wVk = virtualKey,
                            wScan = (ushort)(NativeMethods.MapVirtualKey(virtualKey, 0) & 0xFFU),
                            dwFlags = (0x0004), // 0 for key press  
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };
            }

            public static INPUT CreateKeyUpString(ushort virtualKey)
            {
                return new INPUT
                {
                    type = 1, // INPUT_KEYBOARD  
                    U = new InputUnion
                    {
                        ki = new InputUnion.KEYBDINPUT
                        {
                            wVk = 0,
                            wScan = virtualKey,
                            dwFlags = 0x0004, // KEYEVENTF_KEYUP  
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };
            }
            public static INPUT CreateKeyUpString2(ushort virtualKey)
            {
                return new INPUT
                {
                    type = 1, // INPUT_KEYBOARD  
                    U = new InputUnion
                    {
                        ki = new InputUnion.KEYBDINPUT
                        {
                            wVk = 0,
                            wScan = virtualKey,
                            dwFlags = 0x0004 | 0x0002, // KEYEVENTF_KEYUP  
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };
            }
            public static INPUT CreateKeyUp(ushort virtualKey)
            {
                return new INPUT
                {
                    type = 1, // INPUT_KEYBOARD  
                    U = new InputUnion
                    {
                        ki = new InputUnion.KEYBDINPUT
                        {
                            wVk = virtualKey,
                            wScan = (ushort)(NativeMethods.MapVirtualKey(virtualKey, 0) & 0xFFU),
                            dwFlags = 2, // KEYEVENTF_KEYUP  
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };
            }
            public static INPUT CreateKey(ushort virtualKey, int dwFlags)
            {
                return new INPUT
                {
                    type = 1, // INPUT_KEYBOARD  
                    U = new InputUnion
                    {
                        ki = new InputUnion.KEYBDINPUT
                        {
                            wVk = virtualKey,
                            wScan = (ushort)(NativeMethods.MapVirtualKey(virtualKey, 0) & 0xFFU),
                            dwFlags = 2, // KEYEVENTF_KEYUP  
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };
            }
        }

        //使用SendInput发送字符串
        public static void PressKey(Keys key)
        {
            var inputs = new INPUT[2];

            inputs[0] = KeyboardInput.INPUT.CreateKeyDown((ushort)key);
            inputs[1] = KeyboardInput.INPUT.CreateKeyUp((ushort)key);

            SendInput((uint)inputs.Length, ref inputs[0], KeyboardInput.INPUT.Size);
        }

        //使用SendInput发送字符串
        public static void SendString(string text)
        {
            var inputs = new INPUT[text.Length * 2];

            for (int i = 0; i < text.Length; i++)
            {
                inputs[i * 2] = KeyboardInput.INPUT.CreateKeyUpString(text[i]);
                inputs[i * 2 + 1] = KeyboardInput.INPUT.CreateKeyUpString2(text[i]);
            }

            SendInput((uint)inputs.Length, ref inputs[0], KeyboardInput.INPUT.Size);
        }
        //public unsafe static void SendString2(string text)
        //{
        //    var inputs = new INPUT[text.Length * 2];
        //    for (int i = 0; i < text.Length; i++)
        //    {
        //        ushort vk = VirtualKeyFromChar(text[i]);
        //        inputs[i * 2] = KeyboardInput.INPUT.CreateKeyDown(vk);
        //        inputs[i * 2 + 1] = KeyboardInput.INPUT.CreateKeyUp(vk);
        //    }

        //    var input = stackalloc INPUT[1];
        //    input[0] = inputs[0];

        //    var successful = SendInput((uint)inputs.Length, input, Marshal.SizeOf(typeof(INPUT)));
        //}


        // 辅助函数：从字符到虚拟键码的映射（这里仅处理ASCII字符）  
        public static ushort VirtualKeyFromChar(char c)
        {
            if (c >= 'a' && c <= 'z') return (ushort)(c - 'a' + 0x41);
            if (c >= 'A' && c <= 'Z') return (ushort)(c - 'A' + 0x41);
            if (c >= '0' && c <= '9') return (ushort)(c - '0' + 0x30);
            // 这里可以根据需要添加更多的字符映射  
            return 0; // 如果字符不支持，则返回0  
        }
    }

    public class KeyboardInput2
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public uint type;
            public InputUnion U;
            public static int Size => Marshal.SizeOf(typeof(INPUT));

            [StructLayout(LayoutKind.Explicit)]
            public struct InputUnion
            {
                [FieldOffset(0)] public MOUSEINPUT mi;
                [FieldOffset(0)] public KEYBDINPUT ki;
                [FieldOffset(0)] public HARDWAREINPUT hi;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MOUSEINPUT
            {
                public int dx;
                public int dy;
                public uint mouseData;
                public uint dwFlags;
                public uint time;
                IntPtr dwExtraInfo;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct KEYBDINPUT
            {
                public ushort wVk;
                public ushort wScan;
                public uint dwFlags;
                public uint time;
                public IntPtr dwExtraInfo;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct HARDWAREINPUT
            {
                public uint uMsg;
                ushort wParamL;
                ushort wParamH;
            }

            const uint INPUT_MOUSE = 1;
            const uint INPUT_KEYBOARD = 1;
            const uint INPUT_HARDWARE = 2;

            public static void SendString(string text)
            {
                foreach (char c in text)
                {
                    // Press the key  
                    INPUT ip = new INPUT
                    {
                        type = INPUT_KEYBOARD,
                        U = new INPUT.InputUnion
                        {
                            ki = new INPUT.KEYBDINPUT
                            {
                                wVk = 0, // 0 means use wScan value  
                                wScan = (ushort)Char.ToUpperInvariant(c),
                                dwFlags = 0, // 0 for key press  
                                time = 0,
                                dwExtraInfo = IntPtr.Zero
                            }
                        }
                    };
                    SendInput(1, ref ip, Marshal.SizeOf(typeof(INPUT)));

                    // Release the key  
                    ip.U.ki.dwFlags = 2; // KEYEVENTF_KEYUP  
                    SendInput(1, ref ip, Marshal.SizeOf(typeof(INPUT)));
                }
            }

            [DllImport("user32.dll", SetLastError = true)]
            private static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);
        }
        public static void SendString(string msg)
        {
            KeyboardInput2.INPUT.SendString(msg);
        }
    }
}
