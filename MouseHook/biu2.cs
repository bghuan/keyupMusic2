using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
using static keyupMusic2.Native;
using static keyupMusic2.Simulate;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace keyupMusic2
{
    public partial class biu
    {
        public biu()
        {
            ReplaceKey2.init();
            init();
        }
        IntPtr last_hwnd = IntPtr.Zero;
        Point click_r_point = new Point();
        private void easy_redad(KeyboardMouseHook.MouseEventArgs e)
        {
            if (isctrl())
            {
                Show(e.X + " " + e.Y);
                return;
            }

            IntPtr hwnd = Native.WindowFromPoint(Position);
            //if (hwnd == last_hwnd) return;
            string name = GetWindowName(hwnd);
            string title = GetWindowTitle(hwnd);
            last_hwnd = hwnd;
            string message = ProcessName + "+" + name + "+" + title + "+" + processWrapper?.classname;
            if (huan.label1.Text != message)
            {
                if (name != ProcessName && name != explorer)
                    CleanMouseState();
                huan.Invoke2(() =>
                {
                    huan.label1.Text = message;
                }, 10);
            }
            return;
        }
        private void easy_read(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.move)
            {
                //RateLimiter2.Execute(easy_redad, e);
                easy_redad(e);
                return;
            }
            if (ProcessName == VSCode) return;
            if (ProcessName == chrome && e.Msg == MouseMsg.click) return;

            //if (e.Msg != MouseMsg.wheel)
            //    Log.logcache(e.Msg.ToString());
            //process_and_log(e.Msg.ToString());

            string msg = e.Msg.ToString();
            huan.Invoke2(() =>
            {
                IEnumerable<Keys> pressedKeys = GetPressedKeys();
                msg = ProcessName + "-" + msg + string.Join(",", pressedKeys) + " " + DateTimeNow2();
                huan.label1.Text = msg;
            }, 10);
        }
        public void BottomLine(KeyboardMouseHook.MouseEventArgs e)
        {
            if (e.Msg == MouseMsg.click_r_up)
            {
                if (e.Y == screenHeight1 && !IsFullScreen())
                {
                    Task.Run(() =>
                    {
                        Sleep(150);
                        mouse_move_to(12, 1308 - screenHeight);
                        DelayRun(
                            () => GetPointName() == ShellExperienceHost,
                            () => mouse_click(),
                            400, 10);
                    });
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
