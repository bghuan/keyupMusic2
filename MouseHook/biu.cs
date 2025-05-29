using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;
using static keyupMusic2.Simulate;

namespace keyupMusic2
{
    public partial class biu
    {
        public biu(Form parentForm)
        {
            huan = (Huan)parentForm;
        }
        public Huan huan;
        bool listen_move = false;
        bool downing = false;
        bool downing2 = false;
        bool handing = false;
        bool handing2 = false;
        bool handing3 = false;
        bool left_side_click = false;
        bool left_down_click = false;
        bool left_up_click = false;
        bool right_up_click = false;
        bool right_up_f = false;
        bool right_down_click = false;
        bool right_side_click = false;
        private static readonly object _lockObject_handing2 = new object();
        MouseKeyboardHook.MouseHookEventArgs e = null;
        private Point start = Point.Empty;
        private int threshold = 10;
        bool r_button_downing = false;
        bool x_button_dowing = false;

        public void MouseHookProc(MouseHookEventArgs e)
        {
            if (hooked_mouse) return;
            if (handing4) return;
            if (handing3) return;
            if (handing) return;
            hooked_mouse = true;
            handing = true;
            handing3 = true;
            this.e = e;
            if (e.Msg != MouseMsg.WM_MOUSEMOVE) FreshProcessName();
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN && e.X < screenHeight && e.X > screenHeight - 200 && e.Y < 100) TaskRun(() => { FreshProcessName(); }, 500);

            if (judge_handled(e)) e.Handled = true;
            quick_go_back(e);

            Douyin(e);
            //Task.Run(ACPhoenix);
            Task.Run(Devenv);
            Task.Run(Cornor);
            Task.Run(ScreenLine);
            Task.Run(UnderLine);
            //Task.Run(QQMusic);
            Task.Run(Other);
            Task.Run(Glass);
            Task.Run(Kingdom);

            handing = false;
            hooked_mouse = false;

        }
        public bool judge_handled(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.WM_GO_XBUTTONUP || e.Msg == MouseMsg.WM_XBUTTONUP)
            {

                string dsadasd = "ddddddddd";
            }
            if (e.Msg == MouseMsg.WM_MOUSEMOVE) return false;
            if (e.Msg == MouseMsg.WM_XBUTTONDOWN || e.Msg == MouseMsg.WM_GO_XBUTTONDOWN || e.Msg == MouseMsg.WM_XBUTTONUP || e.Msg == MouseMsg.WM_GO_XBUTTONUP)
            {
                if (!list_go_back.Contains(ProcessName)) return true;
            }
            if (ProcessName == Common.chrome)
            {
                if (ExistProcess(PowerToysCropAndLock, true))
                {
                    if (e.Msg == MouseMsg.WM_RBUTTONDOWN) return true;
                    if (e.Msg == MouseMsg.WM_RBUTTONUP) return true;
                }
            }
            if (is_douyin())
            {
                if (e.Msg == MouseMsg.WM_XBUTTONDOWN) return true;
            }
            return false;
        }

        private static void quick_go_back(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.WM_MOUSEMOVE) return;

            if (e.Msg == MouseMsg.WM_GO_XBUTTONDOWN && ProcessName == msedge)
                press(Keys.Right);
            if (e.Msg == MouseMsg.WM_GO_XBUTTONDOWN && ProcessName == chrome)
                press(Keys.F);

            if (list_go_back.Contains(ProcessName)) return;

            if (e.Msg == MouseMsg.WM_GO_XBUTTONDOWN)
                press(Keys.MediaNextTrack);
            else if (e.Msg == MouseMsg.WM_XBUTTONDOWN)
                press(Keys.MediaPreviousTrack);
        }
        public void Douyin(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (!is_douyin()) return;

            if (e.Msg == MouseMsg.WM_XBUTTONDOWN)
            {
                SS().KeyPress(Keys.X);
            }
            else if (e.Msg == MouseMsg.WM_LBUTTONUP)
            {
                if (e.Y == screenHeight1 && e.X < screenWidth2)
                    SS().KeyPress(Keys.PageUp);
                else if (e.Y == screenHeight1 && e.X < screenWidth1)
                    SS().KeyPress(Keys.PageDown);
            }
        }

        public void Devenv()
        {
            if (ProcessName != keyupMusic2.Common.devenv) return;

            if (e.Msg == MouseMsg.WM_RBUTTONDOWN)
            {
                if ((e.Y != 0)) return;
                if (Deven_runing())
                    press([Keys.RControlKey, Keys.RShiftKey, Keys.F5]);
                //Task.Run(() => Sim.KeyPress([Keys.RControlKey, Keys.RShiftKey, Keys.F5]));
                //press("115, 69",101);
                else
                    press([Keys.F5]);
            }
        }
        public void UnderLine()
        {
            if (e.Msg == MouseMsg.WM_RBUTTONUP)
            {
                if (e.Y == screenHeight1 && !IsFullScreen())
                {
                    Sleep(322);
                    mouse_move_to(0, 1325 - screenHeight);
                    mouse_click();
                }
                //if (e.Y > screenHeight - 20 && !IsFullScreen())
                //{
                //    Sleep(322);
                //    mouse_move_to(0, 1325 - screenHeight);
                //    mouse_click();
                //}
            }
        }
        int ffff = 0;

    }
}
