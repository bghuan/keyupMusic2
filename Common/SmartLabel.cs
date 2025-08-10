using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Forms;
using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;
using static keyupMusic2.Native;
using static System.Net.Mime.MediaTypeNames;

namespace keyupMusic2
{
    public class SmartLabel : Label
    {
        private string _lastText = "";
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
                        //await Task.Delay(LabelTick * 1000);
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
    }
}