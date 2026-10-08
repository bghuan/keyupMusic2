using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace keyupMusic4
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


        public void HookEvent(KeyboardMouseHook.KeyEventArgs e)
        {
            if (!is_down(Keys.LWin) && !is_down(Keys.RWin)) return;
            // 发送无效 Win + Key 事件,使当前 Win 键无效
            if (keys.Contains(e.key)) press(Keys.LControlKey);

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
                    edgePath = @"C:\Program Files\Google\Chrome\Application\chrome_proxy.exe";
                    string arguments = "--profile-directory=Default " +
                                      "--app-id=nddflfcbcnndpkncdlkndocendhgndbc " +
                                      "--app-url=https://www.douyin.com/?recommend=1";
                    ProcessRun(edgePath, arguments);

                    var judge = () =>
                    {
                        return
                        judge_color(159, 818, Color.FromArgb(22, 24, 35), 0)
                        && !judge_color(525, 417, Color.FromArgb(22, 24, 35), 0)
                        && !judge_color(525, 417, Color.FromArgb(255, 255, 255), 0);
                    };
                    var run = () =>
                    {
                        press("200;H;100;", 300);
                        if (!IsFullScreen())
                            press("200;H;100;", 300);
                        int x = 2218;
                        x -= 50;


                        LossScale();
                    };
                    var action2 = () => DelayRun(judge, run, 3222, 100);
                    action2();

                    break;
                default: break;
            }
            FreshProcessName2();
        }
        private static void run_chrome()
        {
            press(100, LWin, "chrome", "en", Enter);
        }
    }
}
