using System.Diagnostics;
using System.Drawing.Imaging;
using System.Net;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.Native;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace keyupMusic2
{
    public class LongPressClass
    {
        public LongPressClass(Form parentForm)
        {
            huan = (Huan)parentForm;
        }
        public LongPressClass()
        {
        }
        public static Huan huan;
        public void asd(Keys keys)
        {
            huan.Invoke2(() => { huan.label1.Text = "longpress " + keys; });

            if (keys == Keys.F3 || keys == Keys.F9)
            {
                huan.Invoke2(() =>
                {
                    huan.timer_stop();
                    huan.system_sleep(true);
                });
            }
            if (keys == Keys.Home || keys == Keys.End)
            {
                if (bmpScreenshot == null) return;
                Common.bmpScreenshot.Save(Common.path, ImageFormat.Png);
                play_sound_di();
                bmpScreenshot.Dispose();
            }
        }

    }
}
