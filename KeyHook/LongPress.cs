using System.Drawing.Imaging;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class LongPressClass
    {
        public static int long_press_tick = 300;
        public static bool long_press_lbutton = false;
        public LongPressClass()
        {
        }
        public void deal(Keys keys)
        {
            //return;
            huan.Invoke2(() => { huan.label1.Text = ProcessName + " longpress " + keys; });

            if (keys == Keys.F9)
            {
                huan.Invoke2(() =>
                {
                    huan.timer_stop();
                    huan.system_sleep(true);
                });
            }
            else if (keys == Keys.Home || keys == Keys.End)
            {
                Common.bmpScreenshot?.Save(Common.bmpScreenshot_path, ImageFormat.Png);
                play_sound_di();
                //bmpScreenshot.Dispose();
            }
            else if (keys == Keys.LButton)
            {
                long_press_lbutton = true;
            }
            else if (keys == Keys.Escape)
            {
                HideProcess(chrome);
                SetDesktopToBlack();
            }
        }

    }
}
