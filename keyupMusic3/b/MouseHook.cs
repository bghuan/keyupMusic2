﻿//using System;
//using System.Diagnostics;
//using System.Drawing;
//using System.Threading;
//using WGestures.Common.OsSpecific.Windows;
//using WGestures.Common;
//using System.Windows.Forms;
//using System.ComponentModel;
//using Microsoft.VisualBasic.ApplicationServices;
//using Win32;

//namespace WGestures.Core.Impl.Windows
//{
//    public class MouseKeyboardHook : IDisposable
//    {
//        //public static Keys stop_key = Keys.F13;
//        public static List<Keys> stop_keys = new List<Keys>();
//        public static bool mouse_downing = false;
//        public static bool handling = false;
//        protected virtual int KeyboardHookProc(int code, int wParam, ref Native.keyboardHookStruct lParam)
//        {
//            var key = (Keys)lParam.vkCode;
//            {
//                KeyboardEventType type;

//                if ((wParam == (int)User32.WM.WM_KEYDOWN || wParam == (int)User32.WM.WM_SYSKEYDOWN))
//                {
//                    type = KeyboardEventType.KeyDown;
//                }
//                else if ((wParam == (int)User32.WM.WM_KEYUP || wParam == (int)User32.WM.WM_SYSKEYUP))
//                {
//                    type = KeyboardEventType.KeyUp;
//                }
//                else return Native.CallNextHookEx(_hookId, code, wParam, ref lParam);

//                var args = new KeyboardHookEventArgs(type, key, wParam, lParam);
//                //if (key != stop_key)
//                if (stop_keys.Count == 0 || !stop_keys.Contains(key) || type == KeyboardEventType.KeyUp || key == Keys.VolumeDown || key == Keys.VolumeUp)
//                    KeyboardHookEvent(args);

//                if (args.Handled)
//                    return 1;
//            }
//            //if (key == stop_key)
//            //{
//            //    stop_key = Keys.F13;
//            //    return 1;
//            //}

//            return Native.CallNextHookEx(_hookId, code, wParam, ref lParam);
//        }
//        const int WM_HOOK_TIMEOUT = (int)User32.WM.WM_USER + 1;

//        public bool IsDisposed { get; private set; }
//        public bool is_install = false;
//        private IntPtr _hookId;
//        private IntPtr _kbdHookId;
//        private uint _hookThreadNativeId;
//        private Thread _hookThread;

//        private Native.LowLevelMouseHookProc _mouseHookProc;
//        private Native.LowLevelkeyboardHookProc _kbdHookProc;

//        public class MouseHookEventArgs : EventArgs
//        {
//            public bool downing;
//            public MouseMsg Msg { get; private set; }
//            public int X { get; private set; }
//            public int Y { get; private set; }

//            public Point Pos => new Point() { X = X, Y = Y };

//            public IntPtr wParam;
//            public IntPtr lParam;

//            public bool Handled { get; set; }

//            public MouseHookEventArgs(MouseMsg msg, int x, int y, IntPtr wParam, IntPtr lParam)
//            {
//                Msg = msg;
//                X = x;
//                Y = y;

//                this.wParam = wParam;
//                this.lParam = lParam;
//            }
//        }

//        public class KeyboardHookEventArgs : EventArgs
//        {
//            public KeyboardEventType Type;
//            public int wParam;
//            public Native.keyboardHookStruct lParam;
//            public Keys key;
//            public bool Handled;
//            public bool Handling;

//            public KeyboardHookEventArgs(KeyboardEventType type, Keys key, int wParam, Native.keyboardHookStruct lParam)
//            {
//                Type = type;
//                this.wParam = wParam;
//                this.lParam = lParam;
//                this.key = key;
//            }
//        }

//        public delegate void MouseHookEventHandler(MouseHookEventArgs e);
//        public delegate void KeyboardHookEventHandler(KeyboardHookEventArgs e);

//        public event MouseHookEventHandler MouseHookEvent;
//        public event KeyboardHookEventHandler KeyboardHookEvent;
//        public event Func<Native.MSG, bool> GotMessage;


//        public MouseKeyboardHook()
//        {
//            _mouseHookProc = MouseHookProc;
//            _kbdHookProc = KeyboardHookProc;
//        }

//        private void _install()
//        {
//            if (MouseHookEvent != null)
//                _hookId = Native.SetMouseHook(_mouseHookProc);
//            if (KeyboardHookEvent != null)
//                _kbdHookId = Native.SetKeyboardHook(_kbdHookProc);

//            is_install = true;
//        }

//        private void _uinstall()
//        {
//            var hookId = _hookId;
//            var kbdHookId = _kbdHookId;
//            _hookId = IntPtr.Zero;
//            _kbdHookId = IntPtr.Zero;
//            is_install = false;


//            if (Native.UnhookWindowsHookEx(hookId) && Native.UnhookWindowsHookEx(kbdHookId))
//            {
//                Debug.WriteLine("钩子已卸载");
//            }
//        }

//        public void Install()
//        {
//            if (_hookThread != null) throw new InvalidOperationException("钩子已经安装了");

//            _hookThread = new Thread(() =>
//            {
//                _install();
//                Debug.WriteLine("钩子安装成功");

//                _hookThreadNativeId = Native.GetCurrentThreadId();

//                try
//                {
//                    var @continue = true;
//                    do
//                    {
//                        Native.MSG msg;
//                        if (Native.GetMessage(out msg, IntPtr.Zero, 0, 0) <= 0) break;

//                        switch (msg.message)
//                        {
//                            case WM_HOOK_TIMEOUT:
//                                Debug.WriteLine("Reinstalling Mouse Hook");
//                                try
//                                {
//                                    _uinstall();
//                                }
//                                catch (Win32Exception e)
//                                {
//                                    Debug.WriteLine(e); //ignore
//                                }
//                                _install();
//                                break;

//                            case (uint)User32.WM.WM_CLOSE:
//                                @continue = false;
//                                _uinstall();
//                                _hookThreadNativeId = 0;
//                                break;
//                        }

//                        if (GotMessage != null)
//                        {
//                            @continue = GotMessage(msg);
//                        }
//                        else @continue = true;


//                    } while (@continue);

//                }
//                finally
//                {
//                    if (_hookId != IntPtr.Zero) Native.UnhookWindowsHookEx(_hookId);
//                    if (_kbdHookId != IntPtr.Zero) Native.UnhookWindowsHookEx(_kbdHookId);
//                }

//                Debug.WriteLine("钩子线程结束");

//                //GC.KeepAlive(hookProc);

//            }, maxStackSize: 1)
//            {
//                IsBackground = true,
//                Priority = ThreadPriority.Highest,
//                Name = "MouseHook钩子线程"
//            };

//            _hookThread.Start();
//        }

//        public void Uninstall()
//        {
//            if (_hookId == IntPtr.Zero || _kbdHookId == IntPtr.Zero || _hookThreadNativeId == 0) return;
//            //发送一个消息给钩子线程,使其GetMessage退出
//            if (_hookThread != null && _hookThread.IsAlive)
//            {
//                _uinstall();
//            }
//        }
//        public void Uninstall2()
//        {
//            if (_hookId == IntPtr.Zero || _kbdHookId == IntPtr.Zero || _hookThreadNativeId == 0) return;
//            //发送一个消息给钩子线程,使其GetMessage退出
//            if (_hookThread != null && _hookThread.IsAlive)
//            {
//                Native.PostThreadMessage(_hookThreadNativeId, (uint)User32.WM.WM_CLOSE, UIntPtr.Zero, IntPtr.Zero);

//                if (!_hookThread.Join(1000 * 3))
//                {
//                    throw new TimeoutException("等待钩子线程结束超时");
//                }

//                _hookThread = null;
//            }
//        }

//        protected virtual IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
//        {
//            if (nCode < 0)
//            {
//                Debug.WriteLine("nCode < 0 ??");
//                return Native.CallNextHookEx(_hookId, nCode, wParam, lParam);
//            }

//            //注意：用这个API来过的鼠标位置，不会出现在迅雷上坐标值变为一半的问题。
//            Native.POINT curPos;
//            Native.GetCursorPos(out curPos);
//            //Debug.WriteLine(wParam);
//            var args = new MouseHookEventArgs((MouseMsg)wParam, curPos.x, curPos.y, wParam, lParam);

//            try
//            {
//                if (MouseHookEvent != null)
//                {
//                    //Task.Run(() => MouseHookEvent(args));
//                    MouseHookEvent(args);
//                }
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine("MouseHookEvent中发生了未处理的异常，并且冒泡到了MouseHookProc。这是不应该出现的。" + e);
//            }

//            return args.Handled ? new IntPtr(-1) : Native.CallNextHookEx(_hookId, nCode, wParam, lParam);
//        }


//        #region dispose
//        //If the method is invoked from the finalizer (disposing is false), 
//        //other objects should not be accessed. 
//        //The reason is that objects are finalized in an unpredictable order and so they,
//        //or any of their dependencies, might already have been finalized.
//        protected virtual void Dispose(bool disposing)
//        {
//            if (IsDisposed) return;

//            if (disposing)
//            {
//                Uninstall();
//            }
//            else
//            {
//                Uninstall();
//            }

//            IsDisposed = true;
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        ~MouseKeyboardHook()
//        {
//            Dispose(false);
//        }
//        #endregion
//    }

//    public enum MouseMsg
//    {
//        WM_LBUTTONDOWN = 0x0201,
//        WM_LBUTTONUP = 0x0202,
//        WM_MOUSEMOVE = 0x0200,

//        WM_MOUSEWHEEL = 0x020A,
//        WM_MBUTTONDOWN = 0x0207,
//        WM_MBUTTONUP = 0X0208,

//        WM_RBUTTONDOWN = 0x0204,
//        WM_RBUTTONUP = 0x0205,

//        WM_XBUTTONDOWN = 0x020B,
//        WM_XBUTTONUP = 0x020C
//    }

//    public enum KeyboardEventType
//    {
//        KeyDown, KeyUp
//    }

//    public enum XButtonNumber
//    {
//        One = 1, Two = 2
//    }

//}
