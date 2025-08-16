using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class Rawinput
    {
        // err 开始handled后,切换设备按下被特殊键盘监听的键,device name 不改变,handled了proc接受不到了
        // Map friendly device keys -> identifying substrings
        public static readonly Dictionary<string, string> devices = new()
        {
            { Common.acer,       Common.acerdevice },
            { Common.coocaa,     Common.coocaadevice },
            { Common.logi,       Common.logidevice },
            { Common.airkeyboard,Common.airkeyboarddevice },
            { Common.airmouse,   Common.airmousedevice },
        };

        // Cache: raw device handle -> friendly device key
        public static readonly Dictionary<IntPtr, string> devicesMap = new();

        private static string NormalizeDeviceKey(string deviceName)
        {
            if (string.IsNullOrWhiteSpace(deviceName)) return string.Empty;

            // Defensive: any null values in devices
            foreach (var kv in devices.OrderByDescending(i => i.Value.Length))
            {
                if (!string.IsNullOrEmpty(kv.Value) &&
                    deviceName.IndexOf(kv.Value, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return kv.Key;
                }
            }

            return string.Empty;
        }

        public static KeyboardMouseHook.KeyEventArgs ProcessRawInput2(IntPtr hRawInput)
        {
            uint dwSize = 0;
            Console.Write("[RawInput]");
            // 第一次调用获取数据大小
            if (GetRawInputData(hRawInput, RID_INPUT, IntPtr.Zero, ref dwSize,
                (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) != 0)
            {
                WriteLine(" null");
                return null;
            }

            if (dwSize == 0)
            {
                WriteLine(" 数据大小为0");
                return null;
            }

            // 分配缓冲区并读取原始输入数据
            IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
            try
            {
                if (GetRawInputData(hRawInput, RID_INPUT, buffer, ref dwSize,
                    (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) != dwSize)
                {
                    WriteLine(" 读取失败");
                    return null;
                }
                RAWINPUT raw = Marshal.PtrToStructure<RAWINPUT>(buffer);

                string deviceName = GetOrCacheDeviceName(raw.header.hDevice);
                var e = new KeyboardMouseHook.KeyEventArgs(0, 0, 0, deviceName);

                if (raw.header.dwType == RIM_TYPEMOUSE)
                {
                    e.dwExtraInfo2 = $"Proc, Mouse, {deviceName}";
                }
                else if (raw.header.dwType == RIM_TYPEKEYBOARD)
                {
                    ushort vKey = raw.keyboard.VKey;
                    bool isE0 = (raw.keyboard.Flags & RI_KEY_E0) != 0;
                    bool isBreak = (raw.keyboard.Flags & RI_KEY_BREAK) != 0;
                    string keyName = Enum.IsDefined(typeof(Keys), (int)vKey) ? ((Keys)vKey).ToString() : $"VK_{vKey}";

                    if (raw.keyboard.ExtraInformation == Common.isVirConst)
                    {
                        WriteLine(" " + Common.isVirConst);
                        return null;
                    }
                    if (raw.keyboard.ExtraInformation == Common.isVirConst + 1)
                    {
                        WriteLine(" " + Common.isVirConst + 1);
                        return null;
                    }

                    e = new KeyboardMouseHook.KeyEventArgs(isBreak ? KeyType.Up : KeyType.Down, (Keys)vKey, (int)raw.keyboard.ExtraInformation, deviceName);
                    e.dwExtraInfo2 = $"Proc, {keyName}, {deviceName}, {raw.header.hDevice}, {(!isBreak ? "Down" : "Up")}";
                }
                else if (raw.header.dwType == RIM_TYPEHID)
                {
                    IntPtr hidPtr = IntPtr.Add(buffer, Marshal.SizeOf(typeof(RAWINPUTHEADER)));
                    RAWINPUTHID hid = Marshal.PtrToStructure<RAWINPUTHID>(hidPtr);

                    int hidDataOffset = Marshal.SizeOf(typeof(RAWINPUTHEADER)) + Marshal.SizeOf(typeof(RAWINPUTHID));
                    int hidDataSize = (int)hid.dwSizeHid * (int)hid.dwCount;
                    byte[] hidData = new byte[hidDataSize];
                    Marshal.Copy(IntPtr.Add(buffer, hidDataOffset), hidData, 0, hidDataSize);

                    foreach (var b in hidData)
                    {
                        if (b == 0xE9)
                            WriteLine("Volume Up");
                        else if (b == 0xEA)
                            WriteLine("Volume Down");
                        else if (b == 0xCD)
                        {
                            WriteLine("Play/Pause");
                            e.key = Keys.MediaPlayPause;
                        }
                        else if (b == 0xB3)
                            WriteLine("Mute");
                        else if (b == 234)
                            WriteLine("Mute");
                        else if (b == 226)
                            WriteLine("Mute");
                        // 可扩展更多 usage
                    }
                }
                WriteLine(" done");
                return e;
            }
            catch (Exception e)
            {
                WriteLine(" e:" + e.Message);
                return null;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static string GetOrCacheDeviceName(IntPtr deviceHandle)
        {
            if (deviceHandle == IntPtr.Zero) return string.Empty;

            //if (devicesMap.TryGetValue(deviceHandle, out var cached) && cached != null)
            //    return cached;

            string raw = GetDeviceName(deviceHandle) ?? string.Empty;
            string normalized = NormalizeDeviceKey(raw);
            //devicesMap[deviceHandle] = normalized;
            return normalized;
        }

        public static void RegisterInputDevices(IntPtr hwnd)
        {
            // Register for keyboard in INPUTSINK mode (receive when not focused)
            var rid = new RAWINPUTDEVICE[]
  {
    new RAWINPUTDEVICE { usUsagePage = 0x01, usUsage = 0x06, dwFlags = RIDEV_INPUTSINK, hwndTarget = hwnd },
    new RAWINPUTDEVICE { usUsagePage = 0x01, usUsage = 0x02, dwFlags = RIDEV_INPUTSINK, hwndTarget = hwnd },
    new RAWINPUTDEVICE { usUsagePage = 0x0C, usUsage = 0x01, dwFlags = RIDEV_INPUTSINK, hwndTarget = hwnd }
  };


            if (!RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICE))))
            {
                // Ensure Windows Forms is referenced for MessageBox
                MessageBox.Show("Failed to register for raw input.");
            }
        }

        public static string GetDeviceName(IntPtr deviceHandle)
        {
            uint size = 0;
            // First call to get required buffer size (in characters for Unicode)
            GetRawInputDeviceInfo(deviceHandle, RIDI_DEVICENAME, IntPtr.Zero, ref size);
            if (size == 0) return null;

            IntPtr dataPtr = Marshal.AllocHGlobal((int)size * sizeof(char));
            try
            {
                uint result = GetRawInputDeviceInfo(deviceHandle, RIDI_DEVICENAME, dataPtr, ref size);
                if (result > 0)
                {
                    // Unicode string
                    string? s = Marshal.PtrToStringUni(dataPtr, (int)result);
                    return s;
                }
            }
            finally
            {
                Marshal.FreeHGlobal(dataPtr);
            }

            return null;
        }

        // ==== Structs & P/Invoke ====
        private const uint RIM_TYPEMOUSE = 0;
        private const uint RIM_TYPEKEYBOARD = 1;
        private const uint RIM_TYPEHID = 2;

        private const ushort HID_USAGE_PAGE_GENERIC = 0x01;
        private const ushort HID_USAGE_GENERIC_MOUSE = 0x02;
        private const ushort HID_USAGE_GENERIC_KEYBOARD = 0x06;

        private const uint RIDEV_INPUTSINK = 0x00000100;

        public const int WM_INPUT = 0x00FF;
        public const int WM_INPUT_YO = WM_INPUT + 123;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int RID_INPUT = 0x10000003;
        public const uint RIDI_DEVICENAME = 0x20000007;

        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTDEVICE
        {
            public ushort usUsagePage;
            public ushort usUsage;
            public uint dwFlags;
            public IntPtr hwndTarget;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTHEADER
        {
            public uint dwType;
            public uint dwSize;
            public IntPtr hDevice;
            public IntPtr wParam; // WPARAM
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RAWKEYBOARD
        {
            public ushort MakeCode;
            public ushort Flags;
            public ushort Reserved;
            public ushort VKey;
            public uint Message;
            public uint ExtraInformation;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTHID
        {
            public uint dwSizeHid;
            public uint dwCount;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct RAWINPUT
        {
            public RAWINPUTHEADER header;
            public RAWKEYBOARD keyboard;
        }

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevices, uint uiNumDevices, uint cbSize);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint GetRawInputDeviceInfo(IntPtr hDevice, uint uiCommand, IntPtr pData, ref uint pcbSize);

        const ushort RI_KEY_E0 = 0x02;
        const ushort RI_KEY_BREAK = 0x01;





    }
}
