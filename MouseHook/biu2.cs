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
        private void easy_read()
        {
            if (e.Msg == MouseMsg.move)
            {
                IntPtr hwnd = Native.WindowFromPoint(Position);
                string asd = GetWindowName(hwnd) + " " + GetWindowText(hwnd);
                if (huan.label1.Text != asd)
                {
                    huan.Invoke2(() =>
                    {
                        huan.label1.Text = asd;
                    }, 10);
                }
                return;
            }

            string msg = e.Msg.ToString();
            huan.Invoke2(() =>
            {
                IEnumerable<Keys> pressedKeys = GetPressedKeys();
                msg = GetPointName() + " " + msg + "    " + string.Join(", ", pressedKeys);
                huan.label1.Text = msg;
            }, 10);
        }
        public void BottomLine()
        {
            if (e.Msg == MouseMsg.click_r_up)
            {
                if (e.Y == screenHeight1 && !IsFullScreen())
                {
                    Sleep(222);
                    mouse_move_to(12, 1308 - screenHeight);
                    mouse_click();
                }
            }
        }
        public static MouseMsg catch_key;
        public static bool catch_state;
        public static bool catch_ed;
        public static void catch_on(MouseMsg msg)
        {
            catch_ed = false;
            catch_state = true;
            catch_key = msg;
        }
        public static void catch_off()
        {
            catch_ed = false;
            catch_state = false;
            catch_key = MouseMsg.none;
        }

    }
}
