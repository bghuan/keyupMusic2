using System.Drawing;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace keyupMusic2
{
    public class OpencvReceive
    {
        public static Huan huan;
        public static string opencvstr = "opencv";

        public OpencvReceive(Form parentForm)
        {
            huan = (Huan)parentForm;
        }
        public void deal(string msg)
        {
            var x = int.Parse(msg.Substring(msg.IndexOf("---") + 3).Split(",")[0]);
            var y = int.Parse(msg.Substring(msg.IndexOf("---") + 3).Split(",")[1]);
            var x2 = x;
            x = y;
            y = 400 - x2;
            x = x * screenHeight / 475 ;
            y = y * screenHeight / 400 ;
            //huan.Invoke(() =>
            //{
            //    huan.label1.Text = "" + x + "," + y;
            //});
            mouse_move(x,y);
        }
    }
}