using SixLabors.ImageSharp.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace keyupMusic2
{
    public class Rawinput
    {
        public static string acer = Common.acerdevice;
        public static string coocaa = Common.coocaadevice;

        public static string SetDeviceName(string deviceName)
        {
            if (string.IsNullOrEmpty(deviceName)) return "";
            if (deviceName.Contains(acer)) return Common.acer;
            else if (deviceName.Contains(coocaa)) return Common.coocaa;
            return "";
        }

        public static KeyboardMouseHook.KeyEventArgs ProcessRawInput2(IntPtr hRawInput)
        {
            uint dwSize = 0;
            GetRawInputData(hRawInput, RID_INPUT, IntPtr.Zero, ref dwSize, (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER)));

            IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
            try
            {
                if (GetRawInputData(hRawInput, RID_INPUT, buffer, ref dwSize, (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) != dwSize)
                    return null;

                // 先解析 RAWINPUTHEADER
                RAWINPUTHEADER header = Marshal.PtrToStructure<RAWINPUTHEADER>(buffer);

                if (header.dwType == 1) // Keyboard
                {
                    IntPtr keyboardPtr = IntPtr.Add(buffer, Marshal.SizeOf(typeof(RAWINPUTHEADER)));
                    RAWKEYBOARD keyboard = Marshal.PtrToStructure<RAWKEYBOARD>(keyboardPtr);

                    ushort vkey = keyboard.VKey;
                    IntPtr deviceHandle = header.hDevice;
                    uint msg = keyboard.Message;

                    string deviceName = GetDeviceName(deviceHandle);
                    string keyName = ((Keys)vkey).ToString();

                    bool isKeyDown = msg == 0x0100 || msg == 0x0104; // WM_KEYDOWN/WM_SYSKEYDOWN
                    bool isKeyUp = msg == 0x0101 || msg == 0x0105;   // WM_KEYUP/WM_SYSKEYUP

                    deviceName = SetDeviceName(deviceName);

                    if (keyboard.ExtraInformation == Common.isVirConst) return null;
                    if (keyboard.ExtraInformation == Common.isVirConst + 1) return null;

                    var aa = new KeyboardMouseHook.KeyEventArgs(
                        isKeyDown ? KeyType.Down : KeyType.Up,
                        (Keys)vkey,
                        (int)keyboard.ExtraInformation
                        , deviceName);
                    //if (isKeyDown)
                        Console.WriteLine($"Proc, {keyName}, {deviceName}, {deviceHandle}, {(isKeyDown ? "Down" : isKeyUp ? "Up" : "Other")}");
                    //Console.WriteLine($"Key: {keyName} ({vkey}), Device Handle: {deviceHandle}, Device: {deviceName}, {(isKeyDown ? "Down" : isKeyUp ? "Up" : "Other")}");

                    if (isKeyDown || isKeyUp)
                        return aa;
                }
                else if (header.dwType == 2) // HID 消费控制
                {
                    IntPtr hidPtr = IntPtr.Add(buffer, Marshal.SizeOf(typeof(RAWINPUTHEADER)));
                    RAWINPUTHID hid = Marshal.PtrToStructure<RAWINPUTHID>(hidPtr);
                    RAWKEYBOARD keyboard = Marshal.PtrToStructure<RAWKEYBOARD>(hidPtr);

                    int hidDataOffset = Marshal.SizeOf(typeof(RAWINPUTHEADER)) + Marshal.SizeOf(typeof(RAWINPUTHID));
                    int hidDataSize = (int)hid.dwSizeHid * (int)hid.dwCount;
                    byte[] hidData = new byte[hidDataSize];
                    Marshal.Copy(IntPtr.Add(buffer, hidDataOffset), hidData, 0, hidDataSize);

                    foreach (var b in hidData)
                    {
                        if (b == 0xE9) Console.WriteLine("Volume Up");
                        else if (b == 0xEA) Console.WriteLine("Volume Down");
                        else if (b == 0xCD) Console.WriteLine("Play/Pause");
                        else if (b == 0xB3) Console.WriteLine("Mute");
                        // 可扩展更多 usage
                    }
                }
                else
                {
                    IntPtr keyboardPtr = IntPtr.Add(buffer, Marshal.SizeOf(typeof(RAWINPUTHEADER)));
                    RAWKEYBOARD keyboard = Marshal.PtrToStructure<RAWKEYBOARD>(keyboardPtr);
                    RAWINPUTHID hid = Marshal.PtrToStructure<RAWINPUTHID>(keyboardPtr);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
            return null;
        }

        public static void RegisterInputDevices(IntPtr hwnd)
        {
            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[1];

            rid[0].usUsagePage = 0x01; // Generic desktop controls
            rid[0].usUsage = 0x06;     // Keyboard
            rid[0].dwFlags = 0x100;
            rid[0].hwndTarget = hwnd;

            //// 消费控制（多媒体键、遥控器等）
            //rid[1].usUsagePage = 0x0C;
            //rid[1].usUsage = 0x01;
            //rid[1].dwFlags = 0x100; // RIDEV_INPUTSINK
            //rid[1].hwndTarget = hwnd;

            //// 消费控制（多媒体键、遥控器等）
            //rid[2].usUsagePage = 0x01;
            //rid[2].usUsage = 0x02;
            //rid[2].dwFlags = 0x100; // RIDEV_INPUTSINK
            //rid[2].hwndTarget = hwnd;


            if (!RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICE))))
            {
                MessageBox.Show("Failed to register for raw input.");
            }
        }

        public static string GetDeviceName(IntPtr deviceHandle)
        {
            uint size = 0;
            GetRawInputDeviceInfo(deviceHandle, RIDI_DEVICENAME, IntPtr.Zero, ref size);
            if (size <= 0) return null;

            IntPtr dataPtr = Marshal.AllocHGlobal((int)size);
            try
            {
                if (GetRawInputDeviceInfo(deviceHandle, RIDI_DEVICENAME, dataPtr, ref size) > 0)
                {
                    return Marshal.PtrToStringAnsi(dataPtr);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(dataPtr);
            }

            return null;
        }
        // Structs and Imports
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
            public IntPtr wParam;
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
        struct RAWINPUTHID
        {
            public uint dwSizeHid;
            public uint dwCount;
        }


        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevices, uint uiNumDevices, uint cbSize);

        [DllImport("User32.dll")]
        public static extern uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern uint GetRawInputDeviceInfo(IntPtr hDevice, uint uiCommand, IntPtr pData, ref uint pcbSize);

        public const int WM_INPUT = 0x00FF;
        public const int WM_INPUT_YO = WM_INPUT + 123;
        public const int RID_INPUT = 0x10000003;
        public const uint RIDI_DEVICENAME = 0x20000007;
    }
}
