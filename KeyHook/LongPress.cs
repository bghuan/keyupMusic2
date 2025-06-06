using System.Drawing.Imaging;
using static keyupMusic2.Common;

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
        public void deal(Keys keys)
        {
            //return;
            huan.Invoke2(() => { huan.label1.Text = ProcessName + " longpress " + keys; });

            if (keys == Keys.F3 || keys == Keys.F9)
            {
                huan.Invoke2(() =>
                {
                    huan.timer_stop();
                    huan.system_sleep(true);
                });
            }
        }

    }
}
