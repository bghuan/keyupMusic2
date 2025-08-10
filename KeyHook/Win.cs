using System.Diagnostics;
using System.Diagnostics.Metrics;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;

namespace keyupMusic2
{
    public class WinClass
    {
        public static readonly HashSet<Keys> keys = new() {
            Q
            , Left
            , Right
            , W
            , L
            , Keys.Enter
            , Keys.D2
            , Keys.F
            , Keys.G
        };
        public static bool handling = false;
        public static bool judge_handled(KeyboardMouseHook.KeyEventArgs e)
        {
            if (!is_down(Keys.LWin) && !is_down(Keys.RWin))
                return false;
            if (keys.Contains(e.key))
                return true;
            return false;
        }

        //public class KeyEvent
        //{
        //    Keys key;
        //    Action action;

        //    public KeyEvent(Keys key, Action action)
        //    {
        //        this.key = key;
        //        this.action = action;
        //    }
        //}
        //static KeyEvent a = new KeyEvent(D2, () => press(100, LWin, "d2", "en", Enter));
        public void hook_KeyDown_ddzzq(KeyboardMouseHook.KeyEventArgs e)
        {
            if (!is_down(Keys.LWin) && !is_down(Keys.RWin)) return;
            // 发送无效 Win + Key 事件,使当前 Win 键无效
            if (keys.Contains(e.key)) press(Keys.LControlKey);
            bool catched = true;

            switch (e.key)
            {
                case Keys.Q:
                    run_chrome();
                    break;
                case Keys.Right:
                case Keys.Left:
                    int arraw = e.key == Keys.Left ? 1 : 2;
                    quick_left_right(arraw);
                    break;
                case Keys.L:
                    Native.LockWorkStation();
                    break;
                case Keys.Enter:
                    press([RMenu, Enter]);
                    break;
                case Keys.D2:
                    string edgePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge_proxy.exe";
                    string arguments = "--profile-directory=Default " +
                                      "--app-id=nddflfcbcnndpkncdlkndocendhgndbc " +
                                      "--app-url=https://www.douyin.com/?recommend=1";
                    ProcessRun(edgePath, arguments);
                    //press("500;525,417;H", 1500);

                    var judge = () =>
                    {
                        //FreshProcessName();
                        return
                        judge_color(247, 229, Color.FromArgb(22, 24, 35), 0)
                        && !judge_color(525, 417, Color.FromArgb(22, 24, 35), 0)
                        && !judge_color(525, 417, Color.FromArgb(255, 255, 255), 0);
                    };
                    var run = () =>
                    {
                        press("200;525.417;525,418;500;H;100;", 300);
                        if (!IsFullScreen())
                            press("100;525.417;525,418;500;H", 200);
                        int x = 2218;
                        x -= 50;
                        press($"100;{x},1404;{x},1171;1703.1439", 200);

                        if (!IsFullScreen()) return;

                        LossScale();
                    };
                    var action2 = () => DelayRun(judge, run, 3222, 100);
                    action2();

                    break;
                default: catched = false; break;
            }
            FreshProcessName();
        }
        private static void run_chrome()
        {
            press(100, LWin, "chrome", "en", Enter);
        }
    }
}
