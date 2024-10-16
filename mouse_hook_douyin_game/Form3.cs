using System.Runtime.InteropServices;
using static mouse_hook_douyin_game.MouseHook;
using static mouse_hook_douyin_game.MouseHook.MouseMsg;

namespace mouse_hook_douyin_game
{
    public partial class Form3 : Form
    {
        int douyin_input_X = 2220;
        int douyin_input_Y = 1385;
        public Form3()
        {
            InitializeComponent();
            MouseHook _mouseKbdHook = new MouseHook();
            _mouseKbdHook.MouseHookEvent += MouseHookProcDouyin;
        }

        Point point_start = new Point();
        Point point_end = new Point();
        double RX;
        double RY;
        string area_start = "";
        string area_end = "";

        public void MouseHookProcDouyin(MouseHookEventArgs e)
        {
            //if (ProcessName != douyin && ProcessName != ApplicationFrameHost) return;
            int x = e.X - point_start.X;
            int y = -(e.Y - point_start.Y);

            if (e.Msg == WM_MOUSEMOVE) { set_clip_txt(get_area_number(x, y), "", "", true); }
            if (e.Msg == WM_LBUTTONDOWN)
            {
                area_start = get_area_number(x, y);
            }
            else if (e.Msg == WM_LBUTTONUP)
            {
                area_end = get_area_number(x, y);
                string vs = "-";
                if (is_ctrl()) vs = "可不可以" + vs;
                set_clip_txt(area_start, vs, area_end);
            }
            else if (e.Msg == WM_RBUTTONDOWN)
            {
                if (is_ctrl()) point_start = e.Pos;
                else area_start = get_area_number(x, y);
            }
            else if (e.Msg == WM_RBUTTONUP)
            {
                if (is_ctrl()) init_area(e, x, y);
                else
                {
                    area_end = get_area_number(x, y);
                    string vs = "/";
                    if (is_ctrl()) vs = "可不可以" + vs;
                    set_clip_txt(area_start, vs, area_end);
                }
            }
        }
        private void set_clip_txt(string a, string b, string c, bool flag = false)
        {
            //if (flag && label1.Text != a) { Invoke(() => { label1.Text = a; }); return; }
            if (a == c || string.IsNullOrEmpty(a) || string.IsNullOrEmpty(c)) return;
            string cmd = a + b + c;
            if (string.IsNullOrEmpty(cmd) || cmd.Length < 3 || cmd.Length > 6) { return; }
            Invoke(() =>
            {
                //label1.Text = cmd;
                Clipboard.SetText(cmd);
            });

            var old_pos = Cursor.Position;
            mouse_move(douyin_input_X, douyin_input_Y);
            Thread.Sleep(10);
            mouse_click();
            press([Keys.LControlKey, Keys.V]);
            press([Keys.Enter]);
            mouse_move(old_pos.X, old_pos.Y);
        }

        private void init_area(MouseHookEventArgs e, int x, int y)
        {
            point_end = e.Pos;

            RX = Math.Abs(point_end.X - point_start.X) / 2 / 2;
            RY = Math.Abs(point_end.Y - point_start.Y) / 4 / 2;
        }

        int[] arr_area = new int[] { 5, 6, 7, 8, 9, 8, 7, 6, 5 };
        double[] arr_start_x = new double[] { -2, -2.5, -3, -3.5, -4, -3.5, -3, -2.5, -2 };
        private string get_area_number(int x, int y)
        {
            string number_area = x + " " + y;

            int number = 1;
            for (int i = 0; i < arr_area.Length; i++)
            {
                double current_y = -4 + i;
                double current_x = arr_start_x[i];
                double current_point_y = (current_y * RY * 2);

                for (int j = 0; j < arr_area[i]; j++)
                {
                    double current_point_x = ((current_x + j) * RX * 2);
                    var current_point = new Point((int)current_point_x, (int)current_point_y);
                    var diff_x = x - current_point.X;
                    var diff_y = y - current_point.Y;

                    number_area += "," + current_point.X + " " + current_point.Y;
                    if (diff_x > -RX && diff_x < RX && diff_y > -RX && diff_y < RX)
                    {
                        return number.ToString();
                    }
                    number++;
                }
            }
            return "";
        }
    }
    public class MouseHook
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);
        public static bool is_ctrl()
        {
            return GetAsyncKeyState(Keys.ControlKey) < 0;
        }

        public const int WH_MOUSE_LL = 14;
        private static int hMouseHook = 0;
        private HookProc mouseHookProcedure;
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        public delegate void MouseHookEventHandler(MouseHookEventArgs e);
        public event MouseHookEventHandler MouseHookEvent;

        public MouseHook()
        {
            mouseHookProcedure = MouseHookProc;
            hMouseHook = SetWindowsHookEx(WH_MOUSE_LL, mouseHookProcedure, IntPtr.Zero, 0);
        }
        ~MouseHook()
        {
            UnhookWindowsHookEx(hMouseHook);
        }

        public int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0 && MouseHookEvent != null)
            {
                var args = new MouseHookEventArgs((MouseMsg)wParam, Cursor.Position.X, Cursor.Position.Y);
                Task.Run(() => { MouseHookEvent(args); });
            }
            return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }
        public enum MouseMsg
        {
            WM_MOUSEMOVE = 0x0200,
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
        }
        public class MouseHookEventArgs : EventArgs
        {
            public MouseMsg Msg { get; private set; }
            public int X { get; private set; }
            public int Y { get; private set; }
            public Point Pos => new Point() { X = X, Y = Y };
            public MouseHookEventArgs(MouseMsg msg, int x, int y)
            {
                Msg = msg;
                X = x;
                Y = y;
            }
        }

        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        const int MOUSEEVENTF_MOVE = 0x0001;
        public static int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        public static int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        public static void mouse_click()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        public static void mouse_move(int x, int y)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x * 65536 / screenWidth, y * 65536 / screenHeight, 0, 0);
        }
        public static void _press(Keys keys)
        {
            keybd_event((byte)keys, 0, 0, 0);
            keybd_event((byte)keys, 0, 2, 0);
        }
        public static void press(Keys[] keys, int tick = 10)
        {
            if (keys == null || keys.Length == 0 || keys.Length > 100)
                return;
            if (keys.Length == 1)
                _press(keys[0]);
            else if (keys.Length > 1 && keys[0] == keys[1])
                foreach (var key in keys)
                    _press(key);
            else
            {
                foreach (var item in keys)
                    keybd_event((byte)item, 0, 0, 0);
                Array.Reverse(keys);
                foreach (var item in keys)
                    keybd_event((byte)item, 0, 2, 0);
            }
            Thread.Sleep(tick);
        }
    }
}
