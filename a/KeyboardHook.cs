using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;
using WGestures.Common.OsSpecific.Windows;

namespace KeyboardHooksd____
{
    public class KeyboardHook
    {
        public event KeyEventHandler KeyDownEvent;
        public event KeyPressEventHandler KeyPressEvent;
        public event KeyEventHandler KeyUpEvent;

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        static int hKeyboardHook = 0;
        public const int WH_KEYBOARD_LL = 13;
        HookProc KeyboardHookProcedure;
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);
        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        public void Start()
        {
            if (hKeyboardHook == 0)
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
                if (hKeyboardHook == 0)
                {
                    Stop();
                    throw new Exception("安装键盘钩子失败");
                }
            }
        }
        public void Stop()
        {
            bool retKeyboard = true;


            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }

            if (!(retKeyboard)) throw new Exception("卸载钩子失败！");
        }
        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey,
                                         int uScanCode,
                                         byte[] lpbKeyState,
                                         byte[] lpwTransKey,
                                         int fuState);

        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            if ((nCode >= 0) && (KeyDownEvent != null || KeyUpEvent != null || KeyPressEvent != null))
            {
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                if (KeyDownEvent != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyDownEvent(this, e);
                    Console.WriteLine("KeyDownEvent" + e.KeyValue);
                }
                if (KeyPressEvent != null && wParam == WM_KEYDOWN)
                {
                    byte[] keyState = new byte[256];
                    GetKeyboardState(keyState);

                    byte[] inBuffer = new byte[2];
                    if (ToAscii(MyKeyboardHookStruct.vkCode, MyKeyboardHookStruct.scanCode, keyState, inBuffer, MyKeyboardHookStruct.flags) == 1)
                    {
                        KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
                        KeyPressEvent(this, e);
                    }
                }
                if (KeyUpEvent != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyUpEvent(this, e);
                    Console.WriteLine("KeyUpEvent" + e.KeyValue);
                }

            }
            return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }
        ~KeyboardHook()
        {
            Stop();
        }


        //private IntPtr _hookId;
        //private IntPtr _kbdHookId;
        //public event MouseEventHandler MouseHookEvent;
        //public MouseEventArgs MouseEventArgs;
        //protected virtual IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        //{
        //    if (nCode < 0)
        //    {
        //        Debug.WriteLine("nCode < 0 ??");
        //        return Native.CallNextHookEx(_hookId, nCode, wParam, lParam);
        //    }

        //    //注意：用这个API来过的鼠标位置，不会出现在迅雷上坐标值变为一半的问题。
        //    Native.POINT curPos;
        //    Native.GetCursorPos(out curPos);
        //    //Debug.WriteLine(wParam);
        //    var args = new MouseEventArgs(null, curPos.x, curPos.y);

        //    try
        //    {
        //        if (MouseHookEvent != null)
        //        {
        //            MouseHookEvent(this, args);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("MouseHookEvent中发生了未处理的异常，并且冒泡到了MouseHookProc。这是不应该出现的。" + e);
        //    }

        //    return Native.CallNextHookEx(_hookId, nCode, wParam, lParam);
        //}

    }
}