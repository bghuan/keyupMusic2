using System.Collections.Concurrent;
using System.Windows.Forms;
using static keyupMusic2.Common;
using Timer = System.Timers.Timer;

namespace keyupMusic2
{
    public class Tick
    {
        private readonly Timer timer100msKeyPress = new() { Interval = 100, AutoReset = true };
        private readonly Timer timer8000ms = new() { Interval = 8000, AutoReset = true };
        private readonly Timer timer60000ms = new() { Interval = 60_000, AutoReset = true };

        public Tick()
        {
            timer100msKeyPress.Elapsed += (s, e) => LongPressClass.Every100msHandler();
            timer100msKeyPress.Start();

            timer8000ms.Elapsed += (s, e) => Every8000ms();
            timer8000ms.Start();

            timer60000ms.Elapsed += (s, e) => Every60000msHandler();
            timer60000ms.Start();
        }

        private void Every60000msHandler()
        {
            if (NotityTime != DateTime.MinValue && DateTime.Now >= NotityTime.AddMinutes(-11) && DateTime.Now <= NotityTime.AddMinutes(-10))
            {
                SuperClass.notify("定时提醒", $"距离目标时间还有 10~11 分钟（目标时间：{NotityTime:yyyy-MM-dd HH:mm:ss}）");
                NotityTime = DateTime.MinValue;
            }
            if (DateTime.Now.Minute == 0)
            {
                if (ProcessName == wemeetapp) return;
                if (ProcessName == cs2) return;
                if (ExistProcess(wemeetapp, true)) return;
                float vol = 0.3f;

                var is_music = IsAnyAudioPlaying();
                if (is_music) press(Keys.MediaPlayPause);
                float volume = GetSystemVolume();
                if (volume > vol) { SetSystemVolume(vol - 0.02f); press(VolumeUp); }
                int num_music = DateTime.Now.Hour % 10;
                if (DateTime.Now.Hour > 12) num_music = (DateTime.Now.Hour - 12) % 10;
                play_sound(num_music);

                Task.Run(() =>
                {
                    Sleep(player_time);
                    float volume2 = GetSystemVolume();
                    if (volume > vol && volume2 == vol)
                    {
                        SetSystemVolume(volume - 0.02f);
                        press(VolumeUp);
                    }
                    if (is_music) press(Keys.MediaPlayPause);
                });
            }
            //if (DateTime.Now.Minute % 10 == 0)
            //{
            //    IEnumerable<Keys> pressedKeys = GetPressedKeys();
            //    if (!pressedKeys.Any())
            //    {
            //        press([LShiftKey, F1]);
            //    }
            //    //SetDesktopWallpaperFull();
            //    //SetDesktopWallpaperAli(GetNextWallpaper(), false);
            //}
        }

        bool exp = false;
        private void Every8000ms()
        {
            //FreshProcessName();
            //IEnumerable<Keys> pressedKeys = GetPressedKeys().Where(x => x != Menu && x != ShiftKey && x != ControlKey);
            //if (!exp && pressedKeys.Count() != Huan.handling_keys.Count)
            //{
            //    exp = true;
            //    throw new Exception("exp " + string.Join(",", pressedKeys) + " " + string.Join(",", Huan.handling_keys));
            //}
            //else
            //{
            //    exp = false;
            //}

            if (ProcessName == cs2)
            {
                if (ExistProcess(wemeetapp, true)) return;
                if (GetPointName() != cs2) return;
                if (!is_ctrl() && !is_down(Keys.LWin) && PositionMiddle == Position)
                    press(Keys.F1);
                if (Position != PositionMiddle)
                    PositionMiddle = Position;
                //if (VirtualKeyboardForm.Instance.Visible)
                //    VirtualKeyboardForm.Instance.Hide();
            }
            else if (system_sleep_count > 0)
            {
                int slow = 1;
                int end = 3 + slow;
                if (system_sleep_count > slow)
                    for (int i = end - system_sleep_count; i > 0; i--)
                    {
                        play_sound_di();
                        Sleep(100);
                    }
                if (system_sleep_count > end)
                {
                    log("tick system_hard_sleep");
                    system_hard_sleep();
                }
                system_sleep_count++;
            }
            else if (gcc_restart)
            {
                CloseProcessFoce(gcc);
                ProcessRun(gccexe);
                gcc_restart = false;
            }
            else if (ExistProcess(cs2) && Position == PositionMiddle && IsFullScreen())
            {
                press_middle_bottom();
            }
            else if (ExistProcess(OpenWith))
            {
                CloseProcess(OpenWith);
            }
            //else if (!VirtualKeyboardForm.Instance.Visible)
            //{
            //    VirtualKeyboardForm.Instance.Show();
            //}
            //VirtualKeyboardForm.Instance?.TriggerKey(Tab, true);
            bland_title();
            Console2.WriteLine($"Tick");
        }
    }
}
