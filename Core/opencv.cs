using System.Drawing;
using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace keyupMusic4
{
    public class OpencvReceive
    {
        public static string opencvstr = "opencv";

        public void deal(string msg)
        {
            var x = int.Parse(msg.Substring(msg.IndexOf("---") + 3).Split(",")[0]);
            var y = int.Parse(msg.Substring(msg.IndexOf("---") + 3).Split(",")[1]);
            var x2 = x;
            x = y;
            y = 400 - x2;
            x = x * screenHeight / 475 ;
            y = y * screenHeight / 400 ;
            mouse_move(x,y);
        }
    }
}