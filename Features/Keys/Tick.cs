using System.Collections.Concurrent;
using System.Windows.Forms;
using static keyupMusic4.Common;
using Timer = System.Timers.Timer;

namespace keyupMusic4
{
    public class Tick
    {
        private readonly Timer timer100ms = new() { Interval = 0_100, AutoReset = true };
        private readonly Timer timer1000ms = new() { Interval = 0_200, AutoReset = true };
        private readonly Timer timer8000ms = new() { Interval = 8_000, AutoReset = true };
        private readonly Timer timer60000ms = new() { Interval = 60_000, AutoReset = true };

        public Tick()
        {
            timer100ms.Elapsed += (s, e) => LongPressClass.Every100msHandler();
            timer100ms.Start();

            timer1000ms.Elapsed += (s, e) => Every1000ms();
            timer1000ms.Start();

            timer8000ms.Elapsed += (s, e) => Every8000ms();
            timer8000ms.Start();

        }

        private void Every1000ms()
        {
            if (ProcessName == ShapeofDreams)
            {
                // 鼠标静止 1 秒不动 → 自动点击一下（持续静止则每 1 秒点一次；移动后重新计时）
                if (DateTime.Now - biu.ShapeLastMove >= TimeSpan.FromSeconds(1)
                    && DateTime.Now - biu.ShapeLastClick >= TimeSpan.FromSeconds(1))
                {
                    biu.ShapeLastClick = DateTime.Now;
                    mouse_click(1);
                }
                if ((judge_color(1238, 747, Color.FromArgb(226, 226, 225), 5)
                    //|| (judge_color(1238, 747, Color.FromArgb(226, 226, 225), 5))
                    )
                    && judge_color(1414, 742, Color.FromArgb(13, 22, 28), 1))
                {
                    press(1234, 748);
                    mouse_move(2225, 827);
                }
                //SS(200).KeyPress(Keys.F, true);
                //down_press(F);
                //up_press(F);
                if (WallpaperEnabled)
                    press_dump(F, 100);
            }
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
                float vol = 0.2f;

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
        }

        private void Every8000ms()
        {

            if (ProcessName == cs2)
            {
                if (ExistProcess(wemeetapp, true)) return;
                if (ExistProcess(LosslessScaling)) CloseProcess(LosslessScaling);
                if (GetPointName() != cs2) return;
                if (!is_ctrl() && !is_down(Keys.LWin) && PositionMiddle == Position)
                    press(Keys.F1);
                if (Position != PositionMiddle)
                    PositionMiddle = Position;
            }
            else if (gcc_restart)
            {
                CloseProcessFoce(gcc);
                ProcessRun(gccexe);
                gcc_restart = false;
            }
            bland_title();
            Console2.WriteLine($"Tick");
        }
    }
}
