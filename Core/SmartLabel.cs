using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Forms;
using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;
using static keyupMusic4.Native;
using static System.Net.Mime.MediaTypeNames;

namespace keyupMusic4
{
    public class SmartLabel : Label
    {
        private DateTime lastLabel1SetTime = DateTime.MinValue;

        public new string Text
        {
            get => base.Text;
            set
            {
                if (LabelTicking && Common.LabelTick > 0)
                {
                    base.Text = value;
                    Task.Run(() =>
                    {
                        Thread.Sleep(Common.LabelTick * 1000);
                        Common.LabelTick = 0;
                    });
                }
                else if (Common.LabelTick > 0)
                {
                }
                else
                {
                    base.Text = value;
                }
                LabelTicking = false;
            }
        }

        public new string Text2
        {
            get => base.Text;
            set
            {
                base.Text = value;
                Task.Run(() =>
                {
                    Thread.Sleep(1000);
                });
            }
        }
    }
}