using System.Diagnostics;
using System.Runtime.InteropServices;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    partial class biuCL
    {
        private void _line2()
        {
            if (ProcessName == Common.chrome)
            {
                if (chrome_red()) press(Keys.F);
                if (ProcessPosition(chrome).X >= chrome_x_min)
                {
                    if (!judge_color(1840, 51, Color.FromArgb(162, 37, 45)))
                        press(Keys.F);
                    SS().MouseWhell(-120 * 12);
                }
            }
            if (IsFullScreen()) return;
            mouse_click2(0);
        }

        private void _line3()
        {
            if (is_douyin()) return;
            if (ProcessTitle.Contains(bilibili)) return;
            if (ProcessTitle.Contains(Ghostrunner2)) return;
            if (ProcessTitle.Contains(ItTakesTwo)) return;
            if (ProcessName == Common.chrome)
                if (chrome_red()) press(Keys.F);
            if (IsDiffProcess())
                mouse_click2(0);
            //FocusPointProcess();
        }
        private void _line6()
        {
            if (IsDiffProcess())
                mouse_click2(0);
            if (!chrome_red())
                press(Keys.F, 20);
            SS().MouseWhell(-120 * 12);
        }

        private void _line7()
        {
            if (IsDiffProcess())
                mouse_click2(0);
            press(Keys.F);
            SS().MouseWhell(120 * 12);
        }


        public void Cornor(MouseHookEventArgs e)
        {
            if (e.Msg != MouseMsg.move) { RECTT.release(); return; }
            if (e.X < screen2Width || e.Y < 0 || e.Y > screen2Height1) return;

            RECTT rect = RECTT.get(e.Pos);

            if (rect == null) { RECTT.ignoreAll(e.Pos); return; }

            FreshProcessName();
            di = false;
            //log(rect.name + " " + e.X + " " + e.Y);

            if (is_down(LButton) || is_down(RButton)) { RECTT.release(); return; }
            Show(rect.name);

            switch (rect.name)
            {
                case nameof(corner1): _corner1(); break;
                case nameof(corner2): _corner2(); break;
                case nameof(corner5): _corner5(); break;
                case nameof(corner6): _corner6(); break;
                case nameof(line2): _line2(); break;
                case nameof(line3): _line3(); break;
                case nameof(line6): _line6(); break;
                case nameof(line7): _line7(); break;
            }

            if (rect != null) RECTT.release();
        }


        static int far = 300;
        static int _fa = screenWidth - far;
        static int gao = screenHeight1;
        static int cha = screenWidth1;
        static int ga2 = screen2Height1;
        static int ch2 = screen2Width;
        private static string[] special = [Common.keyupMusic2, LosslessScaling, msedgewebview2];
        int chrome_x_min = -50;

        //public static RECTT line1 = new RECTT(new RECT(far, 0, _fa, 0),
        //                        new RECT(0, 0, cha, far));
        public static RECTT line2 = new RECTT(nameof(line2), 
                                new RECT(0, gao, _fa - 200, gao),
                                new RECT(0, gao - far + 100, cha, gao));
        public static RECTT line3 = new RECTT(nameof(line3),
                                new RECT(cha, far, cha, gao - far),
                                new RECT(cha - far, 0, cha, gao));

        //public static RECTT line5 = new RECTT(new RECT(ch2 + far, 0, -far, 0),
        //new RECT(ch2 - 1, 0 - 1, 0, far));
        public static RECTT line6 = new RECTT(nameof(line6),
                                new RECT(ch2, ga2, 0, ga2),
                                new RECT(ch2, ga2 - far, 0, ga2));
        public static RECTT line7 = new RECTT(nameof(line7),
                                new RECT(ch2, far, ch2, ga2),
                                new RECT(ch2, 0, ch2 + far, ga2));



        RECTT corner1 = new RECTT(nameof(corner1),
                                new RECT(0, 0, 0, 0), new RECT(0, 0, far, far));//
        RECTT corner2 = new RECTT(nameof(corner2),
                                new RECT(cha, 0, cha, 0), new RECT(cha - far, 0, cha, far));//
        //RECTT corner3 = new RECTT(new RECT(cha, gao, cha, gao), new RECT(cha - far, gao - far, cha, gao));
        //RECTT corner4 = new RECTT(new RECT(0, gao, 0, gao), new RECT(0, gao - far, far, gao));

        RECTT corner5 = new RECTT(nameof(corner5),
                                new RECT(ch2, 0, ch2, 0),
                                new RECT(ch2, 0, ch2 + far, far));//
        RECTT corner6 = new RECTT(nameof(corner6),
                                new RECT(-1, 0, -1, 0),
                                new RECT(-1 - far, 0, -1, far));
        //RECTT corner7 = new RECTT(new RECT(-1, ga2, -1, ga2), new RECT(-1 - far, ga2 - far, -1, ga2));
        //RECTT corner8 = new RECTT(new RECT(ch2, ga2, ch2, ga2), new RECT(ch2, ga2 - far, ch2 + far, ga2));//

        bool di = false;
        DateTime di_time = DateTime.MinValue;


        public void ScreenLine(MouseHookEventArgs e)
        {
            if (e.Msg != MouseMsg.move) return;
            if (ProcessName.Equals(err)) return;
            if (e.X < screen2Width || e.Y < 0 || e.Y > screen2Height1) return;
            int line = 0;
            di = false;
            //if (line1.target(e.Pos)) { line = 1; }
            if (line2.target(e.Pos)) { line = 2; }
            else if (line3.target(e.Pos)) { line = 3; }
            //else if (line5.target(e.Pos)) { line = 5; }
            else if (line6.target(e.Pos)) { line = 6; }
            else if (line7.target(e.Pos)) { line = 7; }

            //if (corner1.target(e.Pos)) { line = 1; }
            //else if (corner2.target(e.Pos)) { line = 2; }
            ////else if (corner3.target(e.Pos)) { line = 3; }
            ////else if (corner4.target(e.Pos)) { line = 4; }
            //else if (corner5.target(e.Pos)) { line = 5; }
            //else if (corner6.target(e.Pos)) { line = 6; }
            ////else if (corner7.target(e.Pos)) { line = 7; }
            ////else if (corner8.target(e.Pos)) { line = 8; }

            //else if (line2.target(e.Pos)) { line = 2; }
            //else if (line3.target(e.Pos)) { line = 3; }
            ////else if (line5.target(e.Pos)) { line = 5; }
            //else if (line6.target(e.Pos)) { line = 6; }
            //else if (line7.target(e.Pos)) { line = 7; }

            if (line != 0 && is_down(LButton) || is_down(RButton))
            {
                RECTT.release();
                return;
            }

            //if (line == 1)
            //{
            //    if (ProcessName.Equals(chrome))
            //        if (ProcessPosition(chrome).X >= chrome_x_min)
            //            press(Keys.F);
            //}
            if (line == 2)
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
            //else if (line == 5)
            //{
            //    if (IsDiffProcess())
            //        mouse_click2(0);
            //    press(Keys.F);
            //    SS().MouseWhell(120 * 12);
            //}
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
                    mouse_click2(0);
                press(Keys.F);
                SS().MouseWhell(120 * 12);
            }

            //line1.ignore(e.Pos);
            line2.ignore(e.Pos);
            line3.ignore(e.Pos);
            //line5.ignore(e.Pos);
            line6.ignore(e.Pos);
            line7.ignore(e.Pos);
            //if (line != 0) RECTT.release();
        }

        public static bool chrome_red()
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
            public string name;
            public RECTT(string name, RECT a, RECT b)
            {
                this.name = name;
                this.a = a;
                this.b = b;
                All.Add(this);
            }
            ~RECTT()
            {
                All.Remove(this);
            }
            public override string ToString()
            {
                return base.ToString();
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
            public static void ignoreAll(Point testPoint)
            {
                foreach (var item in All)
                {
                    if (item.can == true) continue;

                    var aaa = !(testPoint.X >= item.b.Left && testPoint.X <= item.b.Right &&
                           testPoint.Y >= item.b.Top && testPoint.Y <= item.b.Bottom);

                    if (aaa) item.can = true;
                }
            }
            public static void release()
            {
                foreach (var item in All)
                {
                    item.can = false;
                }
            }
            public static RECTT get(Point testPoint)
            {
                foreach (var item in All)
                {
                    if (item.target(testPoint)) return item;
                }
                return null;
            }
        }

        public void press(string str, int tick = 100, bool force = false)
        {
            dii();
            Common.press(str, tick, force);
        }

        public void press(Keys keys, int tick = 0)
        {
            dii();
            Common.press(keys, tick);
        }

        public void press_close()
        {
            dii();
            Common.CloseProcess();
        }
        public void mouse_click(int tick = 10)
        {
            dii();
            Common.mouse_click(tick);
        }
        public void press(Keys[] keys, int tick = 10)
        {
            dii();
            Common.press(keys, tick);
        }
        public void HideProcess()
        {
            dii();
            Common.HideProcess();
        }
        public void mouse_click(int x, int y)
        {
            dii();
            Common.mouse_click(x, y);
        }
        public void mouse_click2(int tick = 0)
        {
            dii();
            Common.mouse_click2(tick);
        }
        public void dii()
        {
            if (!di && DateTime.Now - di_time > TimeSpan.FromMilliseconds(200)) play_sound_di();
            di = true;
            di_time = DateTime.Now;
            //if (di_time.AddMilliseconds(100) > DateTime.Now) play_sound_di();
            //di_time = DateTime.Now;
        }
        public void FocusPointProcess()
        {
            dii();
            Common.FocusPointProcess();
        }
    }
}
