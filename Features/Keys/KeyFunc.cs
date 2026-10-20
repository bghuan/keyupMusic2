using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Reflection.Metadata;
using System.Windows.Forms;
using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;
using static keyupMusic4.Native;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace keyupMusic4
{
    public class KeyFunc2
    {
        public void init()
        {
            new KeyFunc(Windblown, Keys.W, Keys.S);
            new KeyFunc(Windblown, Keys.S, Keys.W);
            new KeyFunc(cs2, Keys.Capital, Keys.Tab);
            new KeyFunc(cs2, Keys.Oem3, Keys.Tab);
            new KeyFunc(ShapeofDreams, Keys.B, Keys.LButton);

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




            new KeyFunc(Keys.LMenu, () =>
            {
                if (isctrl()) return;
                if (is_shift()) return;     
                if (is_down(LWin)) return;
                press(Tab, 10);
            })
            { longPressAction = () => { }, handled = false };

            new KeyFunc(Keys.Home, copy_screen, Upp);
            new KeyFunc(Keys.End, copy_secoed_screen, Upp);

            new KeyFunc(Keys.F1, SuperClass.get_point_color) { handledNot = isctrl };
            new KeyFunc(Keys.F2, AllClass.quick_scale) { type = KeyType.Up, handled = false };
            new KeyFunc(Keys.F10, () => { ToggleTopMost(); }) { type = KeyType.Up };
            new KeyFunc(Keys.F11, AllClass.quick_visiualstudio) { type = KeyType.Up };

            new KeyFunc(Keys.F12, AllClass.quick_wechat_or_notify) { type = KeyType.Up };
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
                // 鼠标键替换：down_press/up_press 是键盘模拟，对鼠标键（LButton/RButton/MButton）无效，改用鼠标事件
                if (replaceKey == Keys.LButton || replaceKey == Keys.RButton || replaceKey == Keys.MButton)
                {
                    if (replaceKey == Keys.LButton)
                    {
                        if (e.Type == KeyType.Down) down_mouse();
                        else up_mouse();
                    }
                    else if (replaceKey == Keys.RButton)
                    {
                        if (e.Type == KeyType.Down) Common.mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                        else Common.mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                    }
                    else if (e.Type == KeyType.Down)
                        mouse_middle();
                    return true;
                }
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
