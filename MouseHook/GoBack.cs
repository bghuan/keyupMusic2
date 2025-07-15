using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;
using static keyupMusic2.Native;
using static keyupMusic2.Simulate;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace keyupMusic2
{
    public partial class biu
    {
        public static List<string> list_go_back = new List<string> { explorer, VSCode, msedge, chrome, devenv, androidstudio, ApplicationFrameHost, Common.cs2, steam, Common.Glass, Glass2, Glass3, vlc, Common.PowerToysCropAndLock, KingdomRush1, lz_image_download, Honeyview, PotPlayerMini64, _哔哩哔哩,  };
        public static HashSet<MouseMsg> go_back_keys = new HashSet<MouseMsg>() {
            MouseMsg.go, MouseMsg.go_up, MouseMsg.back, MouseMsg.back_up
        };


        public static List<ReplaceKey2> replace2 = new List<ReplaceKey2> {
           new ReplaceKey2(Honeyview,       MouseMsg.go,        Keys.Oem6),
           new ReplaceKey2(Honeyview,       MouseMsg.back,      Keys.Oem4),
           new ReplaceKey2(chrome,          MouseMsg.go,        Keys.F),
           new ReplaceKey2(Common.cs2,      MouseMsg.go,        Keys.Escape),
           //new ReplaceKey2(msedge,          MouseMsg.go,        Keys.Home,       ()=>{ }),
           //new ReplaceKey(msedge,          MouseMsg.go_up,   Keys.Home,       ()=>{ }),
           //new ReplaceKey(string.Empty,    MouseMsg.go,        Keys.MediaNextTrack),
           //new ReplaceKey(string.Empty,    MouseMsg.go,        Keys.MediaPreviousTrack),
        };
        private void GoBack2(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;
            if (!go_back_keys.Contains(e.Msg)) return;
            if (!ReplaceKey2.proName.Contains(ProcessName)) return;

            var replace = replace2;
            for (int i = 0; i < replace.Count; i++)
            {
                if (e.Msg == replace[i].before && ProcessName == replace[i].process)
                {
                    if (replace[i].action != null)
                    {
                        if (!e.Msg.IsUpEvent())
                            replace[i].action.Invoke();
                        return;
                    }
                    if (!e.Msg.IsUpEvent()) down_press(replace[i].after, replace[i].raw);
                    else up_press(replace[i].after, replace[i].raw);
                    return;
                }
            }
        }
        private void GoBack(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;
            var go = e.Msg == MouseMsg.go_up;
            var back = e.Msg == MouseMsg.back_up;
            if (!(go || back)) return;

            if (ProcessName == msedge)
                Msedge(go, back);
            else if (ProcessName == chrome)
                chromeasd(go, back);
            else if (ProcessName == Common.cs2)
                cs2(go, back);
            else if (ProcessName == steam && (go))
                press("808,651;close", 1);
            else if (ProcessName == StartMenuExperienceHost)
                _StartMenuExperienceHost(go, back);
            else if (ProcessName == SearchHost)
                _StartMenuExperienceHost(go, back);
            else if (ProcessName == vlc)
                raw(go, back);
            else if (ProcessName.Contains(KingdomRush))
                _KingdomRush(go, back);
            else if (ProcessName == (lz_image_download))
                _lz_image_download(go, back);

            if (!IsDesktopFocused() && list_go_back.Contains(ProcessName))
                return;

            if (go)
                press(Keys.MediaNextTrack);
            else if (back)
                press(Keys.MediaPreviousTrack);
        }

        private void _Honeyview(bool go, bool back)
        {

        }

        private void _lz_image_download(bool go, bool back)
        {
            //if (back)
            //    CloseProcess();
        }

        private void _KingdomRush(bool go, bool back)
        {
            if (go)
                press(Keys.D2);
            else if (back)
                press(Keys.D1);
        }

        private void raw(bool go, bool back)
        {
            if (go)
                press_raw(Keys.MediaNextTrack);
            else if (back)
                press_raw(Keys.MediaPreviousTrack);
        }

        private void _StartMenuExperienceHost(bool go, bool back)
        {
            press(Keys.LWin);
        }
        private void chromeasd(bool go, bool back)
        {
            //if (back)
            //    if ((judge_color(3876, 95, Color.FromArgb(104, 107, 101)))
            //        && judge_color(6437, 147, Color.FromArgb(55, 61, 53)))
            //        press([Keys.LControlKey, Keys.W]);
            if (back)
                if ((judge_color(27, 95, Color.FromArgb(120, 123, 117))
                    && judge_color(2496, 121, Color.FromArgb(55, 61, 53))))
                    press([Keys.LControlKey, Keys.W]);
            //else if (is_lizhi)
            //{
            //    press([Keys.LControlKey, Keys.W]);
            //}
            //if (go) press(Keys.F);

        }
        private void Msedge(bool go, bool back)
        {
            if (back)
                if (judge_color(33, 80, Color.FromArgb(204, 204, 204), 0))
                    //if (judge_color(92, 73, Color.FromArgb(0, 0, 0), null, 0))
                    press([Keys.LControlKey, Keys.W]);
                else if (ProcessTitle.Contains("首发起点中文网"))
                    press([Keys.LControlKey, Keys.W]);
            if (go) press(Keys.Right);
            //if (back)
            //{

            //    IntPtr hwnd = GetForegroundWindow();
            //    Bitmap before = CaptureWindow(hwnd);
            //    Thread.Sleep(200);
            //    Bitmap after = CaptureWindow(hwnd);
            //    bool changed = !BitmapsAreSimilar(before, after);
            //    before.Dispose();
            //    after.Dispose();

            //    if (!changed)
            //        press_raw([LControlKey, W]);
            //}
        }
        private void cs2(bool go, bool back)
        {
            if (is_down(Keys.LButton)) return;
            if (back)
            {
                if (is_down(Keys.D1)) press("B;1243,699;1483,429;1483,568;1483,828;1483,696;1483,969;B;D3;");
                else if (is_down(Keys.D5)) press("B;985,969;1483,429;1483,568;1483,696;1483,969;B;D3;");
                else press("B;985,699;1483,429;1483,568;1483,696;1483,828;1483,969;B;D3;");
            }
            //if (go) press("Escape;", 100);
        }
    }
}
