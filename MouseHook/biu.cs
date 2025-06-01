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
        Point r_down_x = Point.Empty;
        bool r_chrome_menu = false;

        public bool judge_handled(MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.move) return false;
            if (e.Msg == MouseMsg.middle) return true;
            if (e.Msg == MouseMsg.middle_up) return true;
            if (e.Msg == MouseMsg.wheel && e.Y == 0) return true;

            if (e.Msg == MouseMsg.back || e.Msg == MouseMsg.go || e.Msg == MouseMsg.back_up || e.Msg == MouseMsg.go_up)
            {
                if (!list_go_back.Contains(ProcessName)) return true;
                if (is_douyin()) return true;
            }
            if (ProcessName == Common.chrome)
            {
                if (is_down(Keys.RButton)) return false;
                if (e.Msg == MouseMsg.click_r)
                    return true;
                if (e.Msg == MouseMsg.click_r_up)
                    return true;
            }
            if (ProcessName == Common.devenv)
            {
                //if (e.Y != 0) return false;

                if (is_down(Keys.RButton)) return false;
                if (e.Msg == MouseMsg.click_r)
                    return true;
                if (e.Msg == MouseMsg.click_r_up)
                    return true;
            }
            return false;
        }

        public void MouseHookProc(MouseHookEventArgs e)
        {
            //if (handing3) return;
            if (handing) return;
            handing = true;
            handing3 = true;
            this.e = e;

            if (judge_handled(e)) e.Handled = true;

            if (e.Msg != MouseMsg.move) FreshProcessName();

            easy_read();

            Task.Run(() =>
            {
                Cornor();
                ScreenLine();
                BottomLine();
                Other();

                Glass();
                Kingdom();

                GoBack();
                Gestures();
                All();
            });

            handing = false;
        }

        private void Gestures()
        {
            if (e.Msg == MouseMsg.click_r) r_down_x = e.Pos;
            if (e.Msg == MouseMsg.click) { r_chrome_menu = false; return; }
            if (e.Msg == MouseMsg.move) return;
            if (e.Msg != MouseMsg.click_r_up) return;
            if (r_down_x == Point.Empty) return;

            int arraw = 0;
            int arraw_long = 110;
            bool catched = false;
            //log(e.X + " " + e.Y + " " + r_down_x.X + " " + r_down_x.Y);
            if (e.Y - r_down_x.Y > arraw_long) arraw = 3;
            else if (e.Y - r_down_x.Y < -arraw_long) arraw = 4;
            else if (e.X - r_down_x.X > arraw_long) arraw = 1;
            else if (e.X - r_down_x.X < -arraw_long) arraw = 2;

            var is_chrome = ProcessName == chrome && !r_chrome_menu;

            switch (arraw)
            {
                case 0:
                    if (ProcessName == chrome)
                    {
                        r_chrome_menu = true;
                        mouse_click_right();
                    }
                    break;
                
                case 1:
                    if (is_chrome) quick_left_right(arraw); break;// 右
                case 2:
                    if (is_chrome) quick_left_right(arraw); break;// 左
                case 3:
                    if (is_chrome) SS().KeyPress(Keys.M); break;//  下
                case 4:
                    if (is_chrome) SS().KeyPress(Keys.F); break;//  上
            }
            if (catched)
            {
                if (is_down(Keys.LButton)) mouse_click();
                if (is_down(Keys.RButton)) mouse_click_right();
                r_down_x = Point.Empty;
                play_sound_di();
            }
        }

        private void All()
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
            if (e.Msg == MouseMsg.wheel && e.X == screenWidth1 && ProcessName == chrome)
            {
                Keys keys = Keys.Right;
                if (e.data > 0) keys = Keys.Left;
                press(keys);
            }
        }

        private void easy_read()
        {
            if (e.Msg == MouseMsg.move) return;

            string msg = e.Msg.ToString();
            huan.Invoke2(() =>
            {
                IEnumerable<Keys> pressedKeys = GetPressedKeys();
                msg = msg + "    " + string.Join(", ", pressedKeys);
                huan.label1.Text = msg;
            }, 10);
        }
        private void GoBack()
        {
            if (e.Msg == MouseMsg.move) return;

            if (e.Msg == MouseMsg.go)
            {
                if (ProcessName == msedge) press(Keys.Right);
                if (ProcessName == chrome) press(Keys.F);
                if (ProcessName == cs2) press_raw(Keys.MediaNextTrack);
            }
            if (e.Msg == MouseMsg.back)
            {
                if (ProcessName == cs2) press_raw(Keys.MediaPreviousTrack);
            }

            if (list_go_back.Contains(ProcessName)) return;

            if (e.Msg == MouseMsg.go)
                press(Keys.MediaNextTrack);
            else if (e.Msg == MouseMsg.back)
                press(Keys.MediaPreviousTrack);
        }

        public void BottomLine()
        {
            if (e.Msg == MouseMsg.click_r_up)
            {
                if (e.Y == screenHeight1 && !IsFullScreen())
                {
                    Sleep(322);
                    mouse_move_to(0, 1325 - screenHeight);
                    mouse_click();
                }
            }
        }
        int ffff = 0;

    }
}
