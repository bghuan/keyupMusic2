using keyupMusic2;
using System.Windows.Forms;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class Band
    {
        public static bool band_handle(string msg)
        {
            return true;
        }
        public static void Button1(string msg)
        {
            Keys key = Keys.None;
            if (msg.Contains("up"))
                key = Keys.Up;
            else if (msg.Contains("down"))
                key = Keys.Down;
            else if (msg.Contains("left"))
                key = Keys.Left;
            else if (msg.Contains("right"))
                key = Keys.Right;
            else if (msg.Contains("\"msg\":1"))
                key = Keys.PageDown;
            else if (msg.Contains("\"msg\":2"))
                key = Keys.F3;
            else
            {
                log("band listening msg not right,msg: " + msg);
            }

            if (is_douyin())
            {
                if (key == Keys.PageDown)
                    key = Keys.Down;
                else if (key == Keys.Left)
                    key = Keys.PageUp;
                else if (key == Keys.Right)
                    key = Keys.PageDown;
                else if (key == Keys.F3)
                    key = Keys.X;
            }
            else if (ProcessName2 == Common.msedge)
            {
                if (key == Keys.Up)
                    key = Keys.PageUp;
                if (ProcessTitle.Contains("502"))
                    if (key == Keys.PageDown)
                        key = Keys.F5;
                    else if (key == Keys.Up)
                        key = Keys.F11;
            }
            else if (ProcessName2 == Common.steam)
            {
                if (key == Keys.PageDown)
                    key = Keys.Down;
                else if (key == Keys.Right)
                {
                    key = Keys.None;
                    mouse_click();
                }
                else if (key == Keys.Left)
                {
                    key = Keys.MediaPreviousTrack;
                }
            }
            else if (ProcessName2 == Common.vlc)
            {
                if (key == Keys.PageDown)
                    key = Keys.D2;
                else if (key == Keys.Up)
                    key = Keys.D1;
                else if (key == Keys.Left)
                    key = Keys.Space;
            }

            if (key == Keys.None) return;

            press_raw(key);
            //log(msg + "-" + ProcessName2 + "-" + key);
        }
    }
}




//            else if (ProcessName == Common.ACPhoenix)
//{
//    if (key == Keys.PageDown)
//        key = Keys.Space;
//    else if (key == Keys.Left)
//        key = Keys.Tab;
//    else if (key == Keys.Right)
//        key = Keys.Tab;
//    else if (key == Keys.Up)
//        key = Keys.E;
//    else if (key == Keys.Down)
//        key = Keys.E;
//}