using System.Text;
using System.Windows.Forms;
using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;
using static keyupMusic4.Native;
using static keyupMusic4.Simulate;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace keyupMusic4
{
    public partial class biu
    {
        public biu()
        {
            ReplaceKey2.init();
            init();
        }
        IntPtr last_hwnd = IntPtr.Zero;
        DateTime last_read_time = DateTime.MinValue;
        Point click_r_point = new Point();
        // 鼠标移动时刷新窗口信息（限频 200ms + hwnd 缓存，避免高频进程查询）
        private void refresh_point_info(KeyboardMouseHook.MouseEventArgs e)
        {
            if (isctrl())
            {
                Show(e.X + " " + e.Y);
                return;
            }

            IntPtr hwnd = Native.WindowFromPoint(Position);
            // 限频 + hwnd 缓存：指针未离开当前窗口且距上次读取不足 200ms 时跳过，避免每次移动都做进程查询
            if (hwnd == last_hwnd && (DateTime.Now - last_read_time).TotalMilliseconds < 200) return;
            last_hwnd = hwnd;
            last_read_time = DateTime.Now;
            string name = GetWindowName(hwnd);
            string title = GetWindowTitle(hwnd);
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
                refresh_point_info(e);
                return;
            }
            if (ProcessName == VSCode) return;
            if (ProcessName == chrome && e.Msg == MouseMsg.click) return;


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
                if (ScreenPrimary.Width < 3000 && e.Y == screenHeight1 && !IsFullScreen())
                {
                    if (ProcessName == Common.mhtab) return;
                    TaskRun(() =>
                    {
                        press(Keys.Escape);
                        Sleep(100);
                        CloseProcess();
                    }, 100);
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
;