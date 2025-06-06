using System.Runtime.InteropServices;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    partial class biuCL
    {

        static int far = 400;
        static int _fa = screenWidth - far;
        static int gao = screenHeight1;
        static int cha = screenWidth1;
        static int ga2 = screen2Height1;
        static int ch2 = screen2Width;
        int chrome_x_min = -50;

        public static RECTT line1 = new RECTT(new RECT(far, 0, _fa, 0),
                                new RECT(0, 0, cha, far));
        public static RECTT line2 = new RECTT(new RECT(far, gao, _fa - 200, gao),
                                new RECT(0, gao - far + 200, cha, gao));
        public static RECTT line3 = new RECTT(new RECT(cha, far, cha, gao - far),
                                new RECT(cha - far, 0, cha, gao));
        public static RECTT line5 = new RECTT(new RECT(ch2 + far, 0, -far, 0),
                                new RECT(ch2, 0, 0, far));
        public static RECTT line6 = new RECTT(new RECT(ch2 + far, ga2, -far, ga2),
                                new RECT(ch2, ga2 - far, 0, ga2));
        public static RECTT line7 = new RECTT(new RECT(ch2, far, ch2, ga2 - far),
                                new RECT(ch2, 0, ch2 + far, ga2));

        public void ScreenLine(MouseHookEventArgs e)
        {
            if (e.Msg != MouseMsg.move) return;
            if (ProcessName.Equals(err)) return;
            int line = 0;
            di = false;
            if (line1.target(e.Pos)) { line = 1; }
            else if (line2.target(e.Pos)) { line = 2; }
            else if (line3.target(e.Pos)) { line = 3; }
            else if (line5.target(e.Pos)) { line = 5; }
            else if (line6.target(e.Pos)) { line = 6; }
            else if (line7.target(e.Pos)) { line = 7; }

            if (line != 0 && is_down(LButton) || is_down(RButton))
            {
                RECTT.release();
                return;
            }

            if (line == 1)
            {
                if (ProcessName.Equals(chrome))
                    if (ProcessPosition(chrome).X >= chrome_x_min)
                        press(Keys.F);
            }
            else if (line == 2)
            {
                if (is_douyin() && IsFullScreen()) return;
                if (ProcessName == Common.chrome && IsFullScreen())
                {
                    if (chrome_red()) press(Keys.F);
                    if (ProcessPosition(chrome).X >= chrome_x_min)
                    {
                        if (!judge_color(1840, 51, Color.FromArgb(162, 37, 45)))
                            press(Keys.F);
                        SS().MouseWhell(-120 * 12);
                        return;
                    }
                }
                if (IsFullScreen()) return;
                mouse_click2(0);
            }
            else if (line == 3)
            {
                if (is_douyin()) return;
                if (ProcessTitle.Contains(bilibili)) return;
                if (ProcessTitle.Contains(Ghostrunner2)) return;
                if (ProcessTitle.Contains(ItTakesTwo)) return;
                if (ProcessName == Common.chrome)
                    if (chrome_red()) press(Keys.F);
                if (IsDiffProcess())
                    FocusPointProcess();
            }
            else if (line == 5)
            {
                if (IsDiffProcess())
                    mouse_click2(0);
                press(Keys.F);
                SS().MouseWhell(120 * 12);
            }
            else if (line == 6)
            {
                if (IsDiffProcess())
                    mouse_click2(0);
                if (!chrome_red())
                    press(Keys.F, 20);
                SS().MouseWhell(-120 * 12);
            }
            else if (line == 7)
            {
                if (IsDiffProcess())
                    FocusPointProcess();
            }

            line1.ignore(e.Pos);
            line2.ignore(e.Pos);
            line3.ignore(e.Pos);
            line5.ignore(e.Pos);
            line6.ignore(e.Pos);
            line7.ignore(e.Pos);
            //if (line != 0) RECTT.release();
        }

        private static bool chrome_red()
        {
            return (judge_color(-1783, 51, Color.FromArgb(162, 37, 45))
                       && judge_color(-645, 45, Color.FromArgb(162, 37, 45)));
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
            readonly RECT a;
            readonly RECT b;
            public static List<RECTT> All = new List<RECTT>();
            public RECTT(RECT a, RECT b)
            {
                this.a = a;
                this.b = b;
                All.Add(this);
            }
            ~RECTT()
            {
                All.Remove(this);
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
            public void ignore(Point testPoint)
            {
                if (can == true) return;

                var aaa = !(testPoint.X >= b.Left && testPoint.X <= b.Right &&
                       testPoint.Y >= b.Top && testPoint.Y <= b.Bottom);

                if (aaa) can = true;
                return;
            }
            public static void release()
            {
                foreach (var item in All)
                {
                    item.can = false;
                }
            }
        }

    }
}
