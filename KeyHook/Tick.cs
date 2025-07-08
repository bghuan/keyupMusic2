using System.Collections.Concurrent;
using System.Windows.Forms;
using static keyupMusic2.Common;
using Timer = System.Timers.Timer;

namespace keyupMusic2
{
    public class Tick
    {
        private readonly Timer timer100msKeyPress = new() { Interval = 100 , AutoReset =true};
        private readonly Timer timer8000ms = new() { Interval = 8000, AutoReset = true };
        private readonly Timer timer60000ms = new() { Interval = 60_000 , AutoReset =true};

        public Tick()
        {
            timer100msKeyPress.Elapsed += (s, e) => Every100msHandler();
            timer100msKeyPress.Start();

            timer8000ms.Elapsed += (s, e) => Every8000ms();
            timer8000ms.Start();

            timer60000ms.Elapsed += (s, e) => Every60000msHandler();
            timer60000ms.Start();
        }

        private void Every60000msHandler()
        {
            if (DateTime.Now.Minute == 0)
            {
                if (ProcessName == wemeetapp) return;
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
            //    SetDesktopWallpaperAli(GetNextWallpaper(), WallpaperStyle.Fit);
            //}
        }

        private void Every100msHandler()
        {
            if (Huan.handling_keys.Count == 0) return;

            var handling_keys = Huan.handling_keys;
            //if (is_steam_game()) return;
            foreach (var key in handling_keys)
            {
                if (DateTime.Now - key.Value > TimeSpan.FromMilliseconds(LongPressClass.long_press_tick))
                {
                    huan.LongPress.deal(key.Key); // 执行长按方法
                    handling_keys[key.Key] = DateTime.Now.AddDays(1);
                }
            }
        }

        private void Every8000ms()
        {
            FreshProcessName();
            if (ProcessName == cs2)
            {
                if (ExistProcess(wemeetapp, true)) return;
                if (!is_ctrl() && !is_down(Keys.LWin) && PositionMiddle == Position)
                    press(Keys.F1);
                if (Position != PositionMiddle)
                    PositionMiddle = Position;
                //VirtualKeyboardForm.Instance?.SetInitClean();
            }
            else if (system_sleep_count > 0)
            {
                int slow = 5;
                int end = 5 + slow;
                if (system_sleep_count >= slow)
                    for (int i = end - system_sleep_count; i > 0; i--)
                    {
                        play_sound_di();
                        Sleep(100);
                    }
                if (system_sleep_count >= end)
                    system_hard_sleep();
                system_sleep_count++;
            }
            else if (gcc_restart)
            {
                for (int i = 0; i < 30; i++)
                    if (ExistProcess(gcc))
                    {
                        CloseProcessFoce(gcc);
                        Sleep(100);
                    }
                ProcessRun(gccexe);
                //Sleep(5000);
                //HideProcess(gccexe);
                //Sleep(1100);
                //CloseProcess(gccexe);

                gcc_restart = false;
            }
            else if (ExistProcess(cs2) && Position == PositionMiddle && IsFullScreen())
            {
                press_middle_bottom();
            }
            //else if (Position.X == 0)
            //{
            //    SetDesktopWallpaper(GetNextWallpaper(), WallpaperStyle.Fit);
            //}
            bland_title();
        }

        // 存储按键及其首次按下时间
        private static ConcurrentDictionary<Keys, DateTime> _keyPressTimes = new ConcurrentDictionary<Keys, DateTime>();
        private static ConcurrentDictionary<Keys, int> _longPressedKeys = new ConcurrentDictionary<Keys, int>();
    }
}
