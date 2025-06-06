using System.Windows.Forms;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class Tick
    {
        public Tick(Form parentForm)
        {
            huan = (Huan)parentForm;
            StartKeyPressLoop();
            EveryMinite();
            Every100ms();
            Every1000ms();
        }
        public static Huan huan;
        public static void Every1000ms()
        {
            System.Timers.Timer timer = new(1000);
            timer.Elapsed += (sender, e) =>
            {
                //var currentKeys = GetVirPressedKeys();
                //log(string.Join(", ", currentKeys) + ProcessName);
            };
            timer.AutoReset = true;
            timer.Start();
        }
        public static void EveryMinite()
        {
            System.Timers.Timer timer = new(1000 * 60);
            timer.Elapsed += (sender, e) =>
            {
                if (lock_err) return;
                if (DateTime.Now.Minute == 0)
                {
                    press(Keys.MediaStop);
                    play_sound(DateTime.Now.Hour % 10);
                }
            };
            timer.AutoReset = true;
            timer.Start();
        }
        public static void StartKeyPressLoop()
        {
            System.Timers.Timer timer = new System.Timers.Timer(8000);
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
            else if (!KeyTime.ContainsKey(system_sleep_string))
            {
                KeyTime[system_sleep_string] = DateTime.MinValue;
            }
            else if (lock_err && KeyTime.ContainsKey(system_sleep_string) && KeyTime[system_sleep_string].AddMinutes(5) > DateTime.Now)
            {
                play_sound(Keys.D1, true);
                if (system_sleep_count++ > 4)
                    system_hard_sleep();
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
                Sleep(5000);
                HideProcess(gccexe);
                //Sleep(1100);
                //CloseProcess(gccexe);

                gcc_restart = false;
            }
            else if (ExistProcess(cs2) && Position == PositionMiddle)
            {
                press_middle_bottom();
            }
            bland_title();
        }

        // 存储按键及其首次按下时间
        private static Dictionary<Keys, DateTime> _keyPressTimes = new Dictionary<Keys, DateTime>();
        private static HashSet<Keys> _longPressedKeys = new HashSet<Keys>();
        public static void Every100ms()
        {
            System.Timers.Timer timer = new(100);
            timer.Elapsed += (sender, e) =>
            {
                var currentKeys = GetVirPressedKeys();

                // 处理新按下的按键
                foreach (var key in currentKeys)
                {
                    if (!_keyPressTimes.ContainsKey(key.Key))
                    {
                        _keyPressTimes[key.Key] = DateTime.Now;
                        _longPressedKeys.Remove(key.Key); // 重置长按状态
                    }

                    // 检查是否长按（超过1秒）
                    if ((DateTime.Now - _keyPressTimes[key.Key]).TotalSeconds >= 1 &&
                        !_longPressedKeys.Contains(key.Key)
                        && (key.Value == null || key.Value == ProcessName))
                    {
                        huan.LongPress.deal(key.Key); // 执行长按方法
                        _longPressedKeys.Add(key.Key); // 标记为已触发
                    }
                }

                // 移除已释放的按键
                var keysToRemove = _keyPressTimes.Keys
                    .Where(k => !currentKeys.ContainsKey(k))
                    .ToList();

                foreach (var key in keysToRemove)
                {
                    _keyPressTimes.Remove(key);
                    _longPressedKeys.Remove(key);
                }
            };
            timer.AutoReset = true;
            timer.Start();
        }


    }
}
