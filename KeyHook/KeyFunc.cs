using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
using static keyupMusic2.Native;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace keyupMusic2
{
    public class KeyFunc2
    {
        public void init()
        {
            new KeyFunc(Keys.F1, SuperClass.get_point_color) { type = KeyType.Down };
            new KeyFunc(Keys.F2, AllClass.quick_scale) { type = KeyType.Up ,handled = false};
            new KeyFunc(Keys.F10, HideProcess) { type = KeyType.Up };
            new KeyFunc(Keys.F11, AllClass.quick_visiualstudio) { type = KeyType.Up };
            new KeyFunc(Keys.F12, AllClass.quick_wechat_or_notify) { type = KeyType.Up };
            //new KeyFunc(Keys.D5, () => { press(MediaPlayPause); }, msedge);
            {
                var processName = SearchHost;
                var action = () => { press(LWin); };
                new KeyFunc { key = Keys.MediaNextTrack, action = action, processName = processName };
                new KeyFunc { key = Keys.MediaPreviousTrack, action = action, processName = processName };
            }
            {
                var processName = steam;
                var action = () =>
                {
                    press("808,651;", 1);
                    CloseProcess(steam);
                };
                var action2 = () =>
                {
                    MessageBox.Show("11");
                    press("36,70", 0);
                };
                new KeyFunc { key = Keys.F5, action = action, processName = processName };
                new KeyFunc { msg = MouseMsg.go, action = action, processName = processName };
                new KeyFunc { msg = MouseMsg.back, action = action2, processName = processName };
            }
        }
    }
    public class KeyFunc
    {
        public Keys key = Keys.None;
        public MouseMsg msg;
        public Keys replaceKey;
        public KeyType type = KeyType.Down;
        public string processName = "";
        public string processTitle = "";
        public Action action;
        public Action longPressAction;
        public bool handled = true;
        public bool longPressRelease = true;
        public KeyFunc()
        {
            All.Add(this);
        }
        public KeyFunc(Keys key, Action action)
        {
            this.key = key;
            this.action = action;
            All.Add(this);
        }
        public bool target(KeyboardMouseHook.KeyEventArgs e)
        {
            if (key == Keys.None) return false;
            if (action == null) return false;
            if (key != e.key) return false;
            if (type != e.Type) return false;
            if (LongPressKey == e.key && longPressRelease) return false;
            if (processName != "" && processName != ProcessName) return false;
            if (processTitle != "" && !ProcessTitle.Contains(processTitle)) return false;
            action();
            return true;
        }
        public static List<KeyFunc> All = new List<KeyFunc>();

        internal static void hook_KeyDown_ddzzq(KeyboardMouseHook.KeyEventArgs e)
        {
            for (int i = 0; i < All.Count; i++)
            {
                if (All[i].target(e)) break;
            }
        }
    }
}
