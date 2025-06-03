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
        private void GoBack()
        {
            if (e.Msg == MouseMsg.move) return;
            var go = e.Msg == MouseMsg.go;
            var back = e.Msg == MouseMsg.back;
            if (!(go || back)) return;

            if (ProcessName == msedge)
                if (go) press(Keys.Right);
            if (ProcessName == chrome)
                if (go) press(Keys.F);
            if (ProcessName == Common.cs2)
                cs2(go, back);
            if (ProcessName == steam)
                if (go) press("808,651;close", 1);

            if (list_go_back.Contains(ProcessName)) return;

            if (go)
                press(Keys.MediaNextTrack);
            else if (back)
                press(Keys.MediaPreviousTrack);
        }

        private void cs2(bool go, bool back)
        {
            if (is_down(Keys.LButton)) return;
            if (go)
            {
                if (is_down(Keys.D1)) press("B;1243,699;1483,429;1483,568;1483,828;1483,696;1483,969;B;D3;");
                else if (is_down(Keys.D5)) press("B;985,969;1483,429;1483,568;1483,696;1483,969;B;D3;");
                else press("B;985,699;1483,429;1483,568;1483,828;1483,696;1483,969;B;D3;");
            }
            if (back) press("Escape;");
        }
    }
}
