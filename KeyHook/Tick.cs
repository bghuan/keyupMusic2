using System.Collections.Concurrent;
using System.Windows.Forms;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class Tick
    {
        private readonly System.Windows.Forms.Timer timer100msKeyPress = new() { Interval = 100 };
        private readonly System.Windows.Forms.Timer timer8000ms = new() { Interval = 8000 };
        private readonly System.Windows.Forms.Timer timer60000ms = new() { Interval = 60_000 };

        public Tick()
        {
            timer100msKeyPress.Tick += (s, e) => Every100msHandler();
            timer100msKeyPress.Start();

            timer8000ms.Tick += (s, e) => Every8000ms();
            timer8000ms.Start();

            timer60000ms.Tick += (s, e) => Every60000msHandler();
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
            if (DateTime.Now.Minute % 9 == 0)
            {
                if (ishide_DesktopWallpaper == 2)
                    SetDesktopWallpaper(GetNextWallpaper(), WallpaperStyle.Fit);
            }
            if (DateTime.Now.Minute % 10 == 0)
            {
                if (ishide_DesktopWallpaper == 2)
                    SetDesktopWallpaper(GetNextWallpaper(), WallpaperStyle.Fit);
            }
        }

        private void Every100msHandler()
        {
            if (is_steam_game()) return;
            var currentKeys = GetVirPressedKeys();

            // 处理新按下的按键
            foreach (var key in currentKeys)
            {
                if (!_keyPressTimes.ContainsKey(key.Key))
                    _keyPressTimes[key.Key] = DateTime.Now;

                if ((DateTime.Now - _keyPressTimes[key.Key]).Milliseconds >= LongPressClass.long_press_tick &&
                    !_longPressedKeys.ContainsKey(key.Key)
                    && (key.Value == null || key.Value == ProcessName))
                {
                    //if (ReplaceKey2.proName.Contains(ProcessName)) { }
                    //if (ReplaceKey2.proNameMap.ContainsKey(ProcessName)) { }
                    //mousekeymap.ContainsValue

                    huan.LongPress.deal(key.Key); // 执行长按方法
                    _longPressedKeys.TryAdd(key.Key, 1); // 标记为已触发
                }
            }

            var keysToRemove = _keyPressTimes.Keys
                .Where(k => !currentKeys.ContainsKey(k))
                .ToList();

            foreach (var key in keysToRemove)
            {
                _keyPressTimes.TryRemove(key, out _);
                _longPressedKeys.TryRemove(key, out _);
            }
        }

        private void Every8000ms()
        {
            FreshProcessName();
            if (ProcessName == cs2)
            {
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
            else if (ExistProcess(cs2) && Position == PositionMiddle)
            {
                press_middle_bottom();
            }
            else if (Position.X == 0 && IsDesktopFocused())
            {
                if (ishide_DesktopWallpaper == 2)
                    SetDesktopWallpaper(GetNextWallpaper(), WallpaperStyle.Fit);
            }
            bland_title();
        }

        // 存储按键及其首次按下时间
        private static ConcurrentDictionary<Keys, DateTime> _keyPressTimes = new ConcurrentDictionary<Keys, DateTime>();
        private static ConcurrentDictionary<Keys, int> _longPressedKeys = new ConcurrentDictionary<Keys, int>();
    }
}
