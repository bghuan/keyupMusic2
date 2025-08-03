using System.Drawing.Imaging;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class LongPressClass
    {
        public static int long_press_tick = 300;
        public static bool long_press_lbutton = false;
        public static bool long_press_rbutton = false;
        public LongPressClass()
        {
        }
        public void deal(Keys keys)
        {
            //play_sound_di();
            huan.Invoke2(() => { huan.label1.Text = ProcessName + " longpress " + keys; });

            if (keys == Keys.F3)
            {
                press(keys);
            }
            else if (keys == Keys.F9)
            {
                huan.Invoke2(() =>
                {
                    //huan.timer_stop();
                    huan.system_sleep(true);
                });
            }
            else if (keys == Keys.Home || keys == Keys.End)
            {
                try { Common.bmpScreenshot?.Save(Common.bmpScreenshot_path, ImageFormat.Png); }
                catch (Exception ex) { }
                play_sound_di();
                //bmpScreenshot.Dispose();
            }
            else if (keys == Keys.LButton)
            {
                long_press_lbutton = true;
            }
            else if (keys == Keys.RButton)
            {
                long_press_rbutton = true;
            }
            else if (keys == Keys.Escape)
            {
                play_sound_di();
                HideSomething();
            }
            for (int i = 0; i < KeyFunc.All.Count; i++)
            {
                if (KeyFunc.All[i].key == keys && KeyFunc.All[i].action != null && (KeyFunc.All[i].processName == "" || KeyFunc.All[i].processName == ProcessName))
                {
                    if (KeyFunc.All[i].longPressAction == null) press(keys);
                    else KeyFunc.All[i].longPressAction();
                    break;
                }
            }
            if (IsDesktopFocused())
            {
                play_sound_di();
            }
            LongPressKey = keys;
            VirtualKeyboardForm.Instance?.TriggerKey(keys, true);
        }

    }
}
