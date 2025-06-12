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

            if (list_go_back.Contains(ProcessName)) return;

            if (go)
                press(Keys.MediaNextTrack);
            else if (back)
                press(Keys.MediaPreviousTrack);
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
            if (back)
                if ((judge_color(3876, 95, Color.FromArgb(104, 107, 101)))
                    && judge_color(6437, 147, Color.FromArgb(55, 61, 53)))
                    press([Keys.LControlKey, Keys.W]);
            if (go) press(Keys.F);
        }
        private void Msedge(bool go, bool back)
        {
            if (back)
                if (judge_color(33, 80, Color.FromArgb(204, 204, 204), 0))
                    //if (judge_color(92, 73, Color.FromArgb(0, 0, 0), null, 0))
                    press([Keys.LControlKey, Keys.W]);
            if (go) press(Keys.Right);
        }
        private void cs2(bool go, bool back)
        {
            if (is_down(Keys.LButton)) return;
            if (back)
            {
                if (is_down(Keys.D1)) press("B;1243,699;1483,429;1483,568;1483,828;1483,696;1483,969;B;D3;");
                else if (is_down(Keys.D5)) press("B;985,969;1483,429;1483,568;1483,696;1483,969;B;D3;");
                else press("B;985,699;1483,429;1483,568;1483,828;1483,696;1483,969;B;D3;");
            }
            if (go) press("Escape;", 100);
        }
    }
}
