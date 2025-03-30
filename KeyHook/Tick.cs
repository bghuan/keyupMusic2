using System.Timers;
using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.Native;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace keyupMusic2
{
    public class Tick
    {
        public Tick()
        {
            StartKeyPressLoop();
        }

        public static System.Timers.Timer timer;
        public static void StartKeyPressLoop()
        {
            timer = new System.Timers.Timer(5000);
            timer.Elapsed += (sender, e) =>
            {
                if (ProcessName2 == cs2)
                {
                    if (!is_ctrl() && !is_down(Keys.LWin))
                        press(Keys.F1);
                }
                else if (ExistProcess(cs2) && Position == PositionMiddle)
                {
                    press_middle_bottom();
                }
                SetWindowTitle(Common.devenv, "");
                SetWindowTitle(Common.chrome, "");
                SetWindowTitle(Common.PowerToysCropAndLock, "");
            };
            timer.AutoReset = true;
            timer.Start();
        }
    }
}
