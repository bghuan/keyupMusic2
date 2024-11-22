using static keyupMusic2.Common;
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
        bool left_left_click = false;
        bool left_down_click = false;
        bool right_up_click = false;
        bool right_up_f = false;
        bool right_down_click = false;
        private static readonly object _lockObject_handing2 = new object();
        MouseKeyboardHook.MouseHookEventArgs e = null;
        private Point start = Point.Empty;
        private int threshold = 10;
        bool r_button_downing = false;
        bool x_button_dowing = false;

        public void MouseHookProc(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (hooked_mouse) return;
            if (handing3) return;
            if (handing) return;
            hooked_mouse = true;
            handing = true;
            handing3 = true;
            this.e = e;
            if (e.Msg != MouseMsg.WM_MOUSEMOVE) Task.Run(() => { FreshProcessName(); });
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN && e.X < screenHeight && e.X > screenHeight - 200 && e.Y < 100) TaskRun(() => { FreshProcessName(); }, 500);

            //Douyin(e);
            Douyin(e, Common.msedge);
            Task.Run(ACPhoenix);
            Task.Run(Devenv);
            Task.Run(Cornor);
            Task.Run(ScreenLine);
            Task.Run(UnderLine);
            Task.Run(QQMusic);
            Task.Run(Other);

            handing = false;
            hooked_mouse = false;

        }
        public void Douyin(MouseKeyboardHook.MouseHookEventArgs e, string allow = Common.douyin)
        {
            if (ProcessName != allow && !is_douyin()) return;
            if (e.Msg == MouseMsg.WM_MOUSEMOVE && !downing) { return; }

            if (e.Msg == MouseMsg.WM_RBUTTONDOWN)
            {
                if (e.Y > 1370) return;
                if (ProcessName == msedge && ProcessTitle.Contains("抖音"))
                {
                    e.Handled = true;
                    r_button_downing = true;
                }
                if (e.X != 0) return;
                if (ProcessName2 != allow) return;
                e.Handled = true;
                start = Position;
                downing = true;
            }
            else if (e.Msg == MouseMsg.WM_RBUTTONUP)
            {
                if (e.Y > 1370) return;
                if (r_button_downing && ProcessName == msedge && ProcessTitle.Contains("抖音"))
                {
                    e.Handled = true;
                    r_button_downing = false;
                }
                if (ProcessName2 != allow) return;
                if (!downing) return;
                e.Handled = true;
                downing = false;
            }
            else if (e.Msg == MouseMsg.WM_MOUSEMOVE && downing == true)
            {
                int y = e.Y - start.Y;
                if (Math.Abs(y) > threshold)
                {
                    if (y > 0)
                        press(Keys.VolumeDown);
                    if (y < 0)
                        press(Keys.VolumeUp);
                    start = Position;
                }
            }
            else if (e.Msg == MouseMsg.WM_XBUTTONDOWN && is_douyin())
            {
                string dasad = Common.ProcessTitle;
                string dasasssd = Common.ProcessName;

                e.Handled = true;
                x_button_dowing = true;
                Task.Run(() =>
                {
                    if (e.X < screenWidth1 && e.Y == 0)
                        SS().KeyPress(Keys.H);
                    else if (e.X == screenWidth1 && e.Y == 0)
                        press("2222,1410;100;2222,1120", 1);
                    else if (e.X == screenWidth1 && e.Y == screenHeight1)
                    { SS().KeyPress(Keys.R); mouse_move(2384, 1237); }
                    else
                        SS().KeyPress(Keys.X);
                });
            }
            else if (x_button_dowing && e.Msg == MouseMsg.WM_XBUTTONUP && is_douyin())
            {
                e.Handled = true;
                x_button_dowing = false;
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
                if (e.Y + 1 == screenHeight && !IsFullScreen())
                {
                    Sleep(322);
                    mouse_move_to(0, 1325 - screenHeight);
                    mouse_click();
                }
            }
        }
        int ffff = 0;


        private void QQMusic()
        {
            if (ProcessName != keyupMusic2.Common.QQMusic) return;
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            {
                ctrl_shift(true);
            }
        }
        public void Other()
        {
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            {
                if (ProcessName == keyupMusic2.Common.msedge && (e.Y == (screenHeight - 1)))
                    press(Keys.PageDown, 0);
            }
            //else if (e.Msg == MouseMsg.WM_LBUTTONUP)
            //{
            //    if (e.X == 6719 || e.Y == 1619)
            //    {
            //        HideProcess(keyupMusic2.Common.chrome); return;
            //    };
            //}
        }
    }
}
