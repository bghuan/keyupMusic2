using System.Runtime.InteropServices;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    partial class biu
    {

        static int f150 = 400;
        static int f_150 = screenWidth - f150;
        static int gao = screenHeight1;
        static int chang = screenWidth1;
        static int gao2 = screen2Height1;
        static int chang2 = screen2Width;
        int chrome_x_min = -50;

        RECTT line1 = new RECTT(new RECT(f150, 0, f_150, 0),
                                new RECT(0, 0, chang, f150));
        RECTT line2 = new RECTT(new RECT(f150, gao, f_150 - 200, gao),
                                new RECT(0, gao - f150, chang, gao));
        RECTT line3 = new RECTT(new RECT(chang, f150, chang, gao - f150),
                                new RECT(chang - f150, 0, chang, gao));
        RECTT line5 = new RECTT(new RECT(chang2 + f150, 0, -f150, 0),
                                new RECT(chang2, 0, 0, f150));
        RECTT line6 = new RECTT(new RECT(chang2 + f150, gao2, -f150, gao2),
                                new RECT(chang2, gao2 - f150, 0, gao2));
        RECTT line7 = new RECTT(new RECT(chang2, f150, chang2, gao2 - f150),
                                new RECT(chang2, 0, chang2 + f150, gao2));

        public void ScreenLine()
        {
            if (e.Msg != MouseMsg.move) return;
            int line = 0;
            if (line1.target(e.Pos)) { play_sound_di(); line = 1; }
            else if (line2.target(e.Pos)) { play_sound_di(); line = 2; }
            else if (line3.target(e.Pos)) { play_sound_di(); line = 3; }
            else if (line5.target(e.Pos)) { play_sound_di(); line = 5; }
            else if (line6.target(e.Pos)) { play_sound_di(); line = 6; }
            else if (line7.target(e.Pos)) { play_sound_di(); line = 7; }

            if (line == 1)
            {
                if (ProcessName.Equals(err)) return;
                if (ProcessName.Equals(chrome))
                    if (ProcessPosition(chrome).X >= chrome_x_min)
                        SS().KeyPress(Keys.F);
            }
            else if (line == 2)
            {
                var chrome_red = (judge_color(-1783, 51, Color.FromArgb(162, 37, 45))
                               && judge_color(-645, 45, Color.FromArgb(162, 37, 45)));
                var right_up_f = ProcessName == Common.chrome && chrome_red;
                left_down_click = false;
                if (ProcessName.Equals(err)) return;
                if (is_douyin() && IsFullScreen()) return;
                if (judge_color(Color.FromArgb(210, 27, 70))) { return; }
                if (right_up_f && ProcessName == Common.chrome) SS().KeyPress(Keys.F);
                mouse_click2(0);
                if (ProcessName2 == Common.chrome && ProcessPosition(chrome).X >= chrome_x_min)
                {
                    if (!judge_color(1840, 51, Color.FromArgb(162, 37, 45)))
                        SS().KeyPress(Keys.F);
                    SS().MouseWhell(-120 * 12);
                    return;
                }
            }
            else if (line == 3)
            {
                var aaa = ProcessName == Common.chrome && !(!judge_color(-1783, 51, Color.FromArgb(162, 37, 45)) || !judge_color(-645, 45, Color.FromArgb(162, 37, 45)));
                if (is_douyin()) return;
                if (ProcessTitle.Contains(bilibili)) return;
                if (ProcessName.Equals(err)) return;
                if (ProcessTitle.Contains(Ghostrunner2)) return;
                if (ProcessTitle.Contains(ItTakesTwo)) return;
                if (aaa && ProcessName == Common.chrome) SS().KeyPress(Keys.F);
                left_side_click = false;
                mouse_click2(0);
                //if (ProcessName2 == msedge && is_douyin()) mouse_click2(0);
            }
            else if (line == 5)
            {
                right_up_click = false;
                mouse_click2(0);
                SS().KeyPress(Keys.F)
                    .MouseWhell(120 * 12);
            }
            else if (line == 6)
            {
                right_down_click = false;
                if (ProcessName2 != Common.chrome)
                {
                    mouse_click2(10);
                }
                //mouse_click2(0);
                var aaa = ProcessName == Common.chrome && !(judge_color(-1783, 51, Color.FromArgb(162, 37, 45)) && judge_color(-645, 45, Color.FromArgb(162, 37, 45)));
                if (aaa)
                    SS().KeyPress(Keys.F);
                SS().MouseWhell(-120 * 12);
            }
            else if (line == 7)
            {
                right_side_click = false;
                mouse_click2(0);
            }

            line1.ignore(e.Pos);
            line2.ignore(e.Pos);
            line3.ignore(e.Pos);
            line5.ignore(e.Pos);
            line6.ignore(e.Pos);
            line7.ignore(e.Pos);
            if (line != 0)
            {
                corner1.can = false;
                corner2.can = false;
                corner3.can = false;
                corner4.can = false;
                corner5.can = false;
                corner6.can = false;
                corner7.can = false;
                corner8.can = false;
            }
        }

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public RECT(int Left, int Top, int Right, int Bottom)
            {
                this.Left = Left;
                this.Top = Top;
                this.Right = Right;
                this.Bottom = Bottom;
            }
        }
        public class RECTT
        {
            public bool can = true;
            public RECT a;
            public RECT b;
            public RECTT(RECT a, RECT b)
            {
                this.a = a;
                this.b = b;
            }
            public bool target(Point testPoint)
            {
                if (can == false) return false;
                RECT a = this.a;

                var aaa = (testPoint.X >= a.Left && testPoint.X <= a.Right &&
                       testPoint.Y >= a.Top && testPoint.Y <= a.Bottom);

                if (aaa) can = false;
                return aaa;
            }
            public bool ignore(Point testPoint)
            {
                if (can == true) return true;
                RECT a = this.b;

                var aaa = !(testPoint.X >= a.Left && testPoint.X <= a.Right &&
                       testPoint.Y >= a.Top && testPoint.Y <= a.Bottom);

                if (aaa) can = true;
                return aaa;
            }
        }

    }
}
