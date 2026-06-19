using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Reflection.Metadata;
using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
using static keyupMusic2.Native;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace keyupMusic2
{
    public class KeyFunc2
    {
        bool downWin;
        public void init()
        {
            new KeyFunc(Windblown, Keys.W, Keys.S);
            new KeyFunc(Windblown, Keys.S, Keys.W);
            new KeyFunc(cs2, Keys.Capital, Keys.Tab);
            new KeyFunc(cs2, Keys.Oem3, Keys.Tab);

            new KeyFunc(Keys.RMenu, () =>
            {
                play_sound_di();
                if (isctrl())
                {
                    if (ProcessName == cloudmusic)
                        HideProcess(cloudmusic);
                    else
                        FocusProcess(cloudmusic);
                }
                else
                    press(MediaPlayPause);
            })
            { longPressAction = () => { down_press(Keys.RMenu); } };


            //new KeyFunc(Keys.LWin, () =>
            //{
            //    if (downWin)
            //    {
            //        up_press(LControlKey);
            //        downWin = false;
            //    }
            //    else if (Position.X == 0 && Position.Y == 0)
            //    {     
            //        down_press(LControlKey); downWin = true;
            //    }
            //})
            //{ longPressAction = () => { }, handled = true, type = KeyType.Yo };

            //new KeyFunc(Keys.V, () =>
            //{
            //    if (downWin && is_down_vir(A)) up_press(A);
            //    if (Position.X == 0 && Position.Y == 0)
            //    {
            //        down_press(A); downWin = true;
            //    }
            //})
            //{ longPressAction = () => { }, handled = true };

            new KeyFunc(Keys.LMenu, () =>
            {
                //if (ProcessName == WeChat) { }                                       
                //if (is_lbutton()) return;
                if (isctrl()) return;
                if (is_shift()) return;     
                if (is_down(LWin)) return;
                //if (Position.Y == 0) return;
                //if (Position.X == 0) return;
                //if (Position.X == screenWidth1 && Position.Y != screenHeight1) return;
                //if (start_catch_time.AddSeconds(1) < DateTime.Now)
                //        press(Tab, 0);                            
                press(Tab, 10);
            })
            { longPressAction = () => { }, handled = false };

            new KeyFunc(Keys.Home, copy_screen, Upp);
            new KeyFunc(Keys.End, copy_secoed_screen, Upp);

            new KeyFunc(Keys.F1, SuperClass.get_point_color) { handledNot = isctrl };
            new KeyFunc(Keys.F2, AllClass.quick_scale) { type = KeyType.Up, handled = false };
            //new KeyFunc(Keys.F10, () => { HideProcess(isctrl()); }) { type = KeyType.Up };
            //new KeyFunc(Keys.F10, () => { SetTransparency(); }) { type = KeyType.Up };
            new KeyFunc(Keys.F10, () => { ToggleTopMost(); }) { type = KeyType.Up };
            new KeyFunc(Keys.F11, AllClass.quick_visiualstudio) { type = KeyType.Up };

            //new KeyFunc(Keys.Up, () => { /*if (is_tran_process == ProcessName)*/ { SetTransparency(2); } }) { type = KeyType.Down, handled = false, repeat = true, resume = true };
            //new KeyFunc(Keys.Down, () => { /*if (is_tran_process == ProcessName)*/ { SetTransparency(-8); } }) { type = KeyType.Down, handled = false, repeat = true, resume = true };
            //new KeyFunc(Keys.CapsLock, () =>
            //{
            //    //if (capslock_vloumn)
            //    //{
            //    //    capslock_vloumn = false;
            //    //    //press(CapsLock);
            new KeyFunc(Keys.F12, AllClass.quick_wechat_or_notify) { type = KeyType.Up };
            //    //}                                           
            //})
            //{ type = KeyType.Up, handled = false };
            //new KeyFunc(Keys.D5, () => { press(MediaPlayPause); }, msedge);
            //{
            //    var processName = SearchHost;
            //    var action = () => { press(LWin); };
            //    new KeyFunc { key = Keys.MediaNextTrack, action = action, processName = processName };
            //    new KeyFunc { key = Keys.MediaPreviousTrack, action = action, processName = processName };
            //}
            //{
            //    var processName = steam;
            //    var action = () =>
            //    {
            //        press("808,651;", 1);
            //        CloseProcess(steam);
            //    };
            //    var action2 = () =>
            //    {
            //        MessageBox.Show("11");
            //        press("36,70", 0);
            //    };
            //    new KeyFunc { key = Keys.F5, action = action, processName = processName };
            //    //new KeyFunc { msg = MouseMsg.go, action = action, processName = processName };
            //    //new KeyFunc { msg = MouseMsg.back, action = action2, processName = processName };
            //}
        }
    }
    public class KeyFunc
    {
        public Keys key;
        public MouseMsg msg;
        public Keys replaceKey;
        public KeyType type = KeyType.Down;
        public string processName = "";
        public string processTitle = "";
        public Action action;
        public bool hold;
        public bool repeat;
        public bool resume;
        public Action longPressAction;
        public bool handled = true;
        public Func<bool> handledNot;
        public bool longPressRelease = true;
        public KeyFunc()
        {
            All.Add(this);
            AllKeys.Add(key);
        }
        public KeyFunc(Keys key, Action action, KeyType type = KeyType.Down)
        {
            this.key = key;
            this.action = action;
            All.Add(this);
            AllKeys.Add(key);
        }
        public KeyFunc(string processName, Keys key, Keys replaceKey)
        {
            this.processName = processName;
            this.key = key;
            this.replaceKey = replaceKey;
            All.Add(this);
            AllKeys.Add(key);
        }
        public KeyFunc(Func<bool> aaa, Keys key, Keys replaceKey)
        {
            //this.processName = processName;
            if (aaa())
            {
                this.key = key;
                this.replaceKey = replaceKey;
            }
            else
            {
                this.key = key;
                this.replaceKey = key;
            }
            All.Add(this);
            AllKeys.Add(key);
        }
        public bool aaa() { return true; }
        internal static bool LongPressFlag(Keys keys)
        {
            for (int i = 0; i < KeyFunc.All.Count; i++)
            {
                if (KeyFunc.All[i].key == keys && KeyFunc.All[i].action != null && (KeyFunc.All[i].processName == "" || KeyFunc.All[i].processName == ProcessName))
                {
                    if (KeyFunc.All[i].longPressAction == null) press(keys);
                    else KeyFunc.All[i].longPressAction();
                    return true;
                }
            }
            return false;
        }
        public bool target(KeyboardMouseHook.KeyEventArgs e)
        {
            if (key == 0) return false;
            if (key != e.key) return false;
            if (processName != "" && processName != ProcessName) return false;
            if (processTitle != "" && !ProcessTitle.Contains(processTitle)) return false;

            if (replaceKey != None)
            {
                if (e.Type == KeyType.Down) down_press(replaceKey);
                else up_press(replaceKey);
                return true;
            }
            if (action != null)
            {
                if (e.Type == KeyType.Down && hold && !repeat) return false;
                hold = e.Type == KeyType.Down;
                if (e.Type == KeyType.Up && is_down(e.key)) up_press(e.key);
                if (type != e.Type) return false;
                //if (type != e.Type && e.Type != KeyType.Yo) return false;
                if (LongPressKey == e.key && (longPressRelease && !repeat)) return false;
                action();
                if (resume) return false;
                return true;
            }

            return false;
        }
        public static List<KeyFunc> All = new List<KeyFunc>();
        public static HashSet<Keys> AllKeys = new HashSet<Keys>();

        //JOB ADD mouse accept
        internal static bool judge(KeyboardMouseHook.KeyEventArgs e)
        {
            if (!AllKeys.Contains(e.key)) return false;
            var keyfunc = KeyFunc.All.Where(a => a.handled || a.handledNot != null).ToArray();
            for (int i = 0; i < keyfunc.Count(); i++)
            {
                if (keyfunc[i].key != e.key || (keyfunc[i].processName != "" && keyfunc[i].processName != ProcessName))
                {
                    continue;
                }
                if (keyfunc[i].replaceKey != None)
                {
                    return true;
                }
                if (keyfunc[i].handledNot == null || !keyfunc[i].handledNot())
                {
                    return true;
                }
                if (keyfunc[i].action != null)
                {
                    return true;
                }
            }
            return false;
        }
        internal static bool HookEvent(KeyboardMouseHook.KeyEventArgs e)
        {
            for (int i = 0; i < All.Count; i++)
            {
                if (All[i].target(e)) return true;
            }
            return false;
        }
    }
}
