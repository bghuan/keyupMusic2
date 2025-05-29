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
            //log(ProcessName2+ lock_err+ KeyTime[system_sleep_string]+ KeyTime[system_sleep_string].AddMinutes(5)+ DateTime.Now+ system_sleep_count+ gcc_restart);
            if (ProcessName2 == cs2)
            {
                if (!is_ctrl() && !is_down(Keys.LWin))
                    press(Keys.F1);
                if (Position != PositionMiddle)
                    PositionMiddle = Position;
            }
            else if (!KeyTime.ContainsKey(system_sleep_string))
            {
                KeyTime[system_sleep_string] = DateTime.MinValue;
            }
            else if (lock_err && KeyTime.ContainsKey(system_sleep_string) && KeyTime[system_sleep_string].AddMinutes(5) > DateTime.Now)
            {
                play_sound(Keys.D1);
                if (system_sleep_count++ > 4)
                //if (system_sleep_count> 0)
                {
                    system_hard_sleep();
                }
            }
            else if (gcc_restart)
            {
                for (int i = 0; i < 30; i++)
                {
                    if (ExistProcess(gcc))
                    {
                        CloseProcessFoce(gcc);
                        Sleep(100);
                    }
                }
                ProcessRun(gccexe);
                //Sleep(1100);
                //CloseProcess(gccexe);

                gcc_restart = false;
            }
            else if (ExistProcess(cs2) && Position == PositionMiddle)
            {
                press_middle_bottom();
            }
            else if (system_sleep_count != 0)
            {
                system_sleep_count = 0;
            }
            bland_title();
        }

    }
}
