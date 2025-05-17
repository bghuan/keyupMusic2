using System.Diagnostics;
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
            timer = new System.Timers.Timer(8000);
            timer.Elapsed += (sender, e) =>
            {
                NewMethod();
            };
            timer.AutoReset = true;
            timer.Start();
        }

        private static void NewMethod()
        {
            if (ProcessName2 == cs2)
            {
                if (!is_ctrl() && !is_down(Keys.LWin))
                    press(Keys.F1);
                if (Position != PositionMiddle)
                    PositionMiddle = Position;
            }
            else if (lock_err && KeyTime[system_sleep_string].AddMinutes(5) > DateTime.Now)
            {
                if (system_sleep_count++ > 3)
                {
                    Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,1");
                    KeyTime[system_sleep_string] = DateTime.Now;
                }
            }
            else if (ExistProcess(cs2) && Position == PositionMiddle)
            {
                press_middle_bottom();
            }
            else
            {
                system_sleep_count = 0;
            }
            SetWindowTitle(Common.devenv, "");
            SetWindowTitle(Common.chrome, "");
            SetWindowTitle(Common.PowerToysCropAndLock, "");
            SetWindowTitle(Common.wemeetapp, "");
        }
    }
}
