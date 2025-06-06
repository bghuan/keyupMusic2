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
        public biu(Form parentForm)
        {
            huan = (Huan)parentForm;
            biusc = new biuCL();
        }
        public Huan huan;
        biuCL biusc;
        public static Point r_down_x = Point.Empty;
        bool menu_opened = false;

        public bool judge_handled(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return false;
            if (e.Msg == MouseMsg.middle) return true;
            if (e.Msg == MouseMsg.middle_up) return true;
            if (e.Msg == MouseMsg.wheel && e.Y == 0) return true;

            if (e.Msg == MouseMsg.back || e.Msg == MouseMsg.back_up
             || e.Msg == MouseMsg.go || e.Msg == MouseMsg.go_up)
            {
                if (is_down(Keys.XButton1) || is_down(Keys.XButton2)) { return false; }
                if (!list_go_back.Contains(ProcessName)) return true;
                if (Common.cs2.Equals(ProcessName)) return true;
                if (is_douyin()) return true;
            }
            if (e.Msg == MouseMsg.click_r || e.Msg == MouseMsg.click_r_up)
            {
                var list = new[] { chrome, devenv, Glass2, Glass3 };
                if (list.Contains(Common.ProcessName))
                {
                    if (is_down(Keys.RButton)) return false;
                    if (GetPointName() == explorer) return false;
                    return true;
                }
            }
            if (e.Msg == MouseMsg.wheel && e.X == screenWidth1)
            {
                var list = new[] { chrome };
                if (list.Contains(Common.ProcessName))
                    return true;
            }
            if (e.Msg == MouseMsg.wheel)
            {
                var list = new[] { Glass2 };
                if (list.Contains(Common.ProcessName))
                    return true;
            }
            return false;
        }

        public void MouseHookProc(MouseHookEventArgs e)
        {
            if (e.Msg != MouseMsg.move) FreshProcessName();
            if (judge_handled(e))
            {
                e.Handled = true;
                VirKeyState(e.Msg);
                if (IsDiffProcess()) FocusPointProcess();
            }

            Task.Run(() =>
            {
                easy_read(e);

                biusc.Cornor(e);
                biusc.ScreenLine(e);
                BottomLine(e);
                Other(e);

                Glass(e);

                GoBack(e);
                Gestures(e);
                All(e);
            });
        }

        private void All(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return;

            if (e.Msg == MouseMsg.middle)
                press(Keys.MediaPlayPause);
            if (e.Msg == MouseMsg.wheel && e.Y == 0)
            {
                Keys keys = Keys.F7;
                if (e.data > 0) keys = Keys.F8;
                press(keys);
            }
            if (catch_state && catch_key == e.Msg) catch_ed = true;
        }

    }
}
