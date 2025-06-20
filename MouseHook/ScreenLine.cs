using System.Diagnostics;
using System.Runtime.InteropServices;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    partial class biuCL
    {
        private void _line2(MouseHookEventArgs e)
        {
            if (ProcessName == Common.chrome)
            {
                if (chrome_red()) press(Keys.F);
                var pos = ProcessPosition(chrome).X;
                if (pos < screenWidth2 && IsFullScreen())
                {
                    if (!judge_color(1840, 51, Color.FromArgb(162, 37, 45)))
                        press(Keys.F, 51);
                    SS().MouseWhell(-1440);
                }
            }
            if (IsFullScreen()) return;
            mouse_click2(0);
        }

        private void _line3(MouseHookEventArgs e)
        {
            if (is_douyin())
                return;
            if (ProcessName == Common.chrome)
            {
                if (!ProcessTitle.Contains("chat")) return;
                if (chrome_red()) press(Keys.F);
                var pos = ProcessPosition(chrome).X;
                if (pos < screenWidth2 && IsFullScreen())
                {
                    press(Keys.F, 51);
                    SS().MouseWhell(1440);
                }
            }
            if (ProcessName == Common.vlc)
            {
                press(Keys.Space, 11);
            }
            if (IsDiffProcess())
                mouse_click2(0);
        }

        private void _line6(MouseHookEventArgs e)
        {
            if (IsDiffProcess())
                mouse_click2(0);
            if (!chrome_red())
                press(Keys.F, 50);
            SS().MouseWhell(-1440);
        }

        private void _line7(MouseHookEventArgs e)
        {
            if (IsDiffProcess())
                mouse_click2(0);
            press(Keys.F, 50);
            SS().MouseWhell(1440);
        }

        public biuCL()
        {
            line2.aMouseHookEvent += _line2;
            line3.aMouseHookEvent += _line3;
            line6.aMouseHookEvent += _line6;
            line7.aMouseHookEvent += _line7;

            corner1.aMouseHookEvent += _corner1;
            corner2.aMouseHookEvent += _corner2;
            corner5.aMouseHookEvent += _corner5;
            corner6.aMouseHookEvent += _corner6;
        }
        public void Cornor(MouseHookEventArgs e)
        {
            if (e.Msg != MouseMsg.move) { RECTT.release(); return; }
            if (e.X > screen2Width || e.Y < 0 || e.Y > screen2Height1) return;

            var rect = RECTT.get(e.Pos);
            if (rect == null)
            {
                var rect2 = RECTT.ignoreAll(e.Pos);
                rect?.doo2(e);
                return;
            }

            FreshProcessName();
            di = false;
            //log(rect.name + " " + e.X + " " + e.Y);

            if (is_down(LButton) || is_down(RButton)) { RECTT.release(); return; }
            Show(rect.name,2);

            rect.doo(e);
            FreshProcessName();

            RECTT.release();
        }

        static int far = 300;
        static int _fa = screenWidth - far;
        static int gao = screenHeight1;
        static int cha = screenWidth1;

        static int ga2 = screen2Height1;
        static int ch2 = screen2Width;
        static int ch0 = screen2Width0;
        int chrome_x_min = -50;

        //public static RECTT line2 = new RECTT(nameof(line2),
        //                        new RECT(0, gao - 60, cha, gao - 25),
        //                        new RECT(0, 0, cha, gao - 5));
        public static RECTT line2 = new RECTT(nameof(line2),
                                new JU(0, gao, _fa - 200, gao),
                                new JU(0, gao - far + 100, cha, gao));
        public static RECTT line3 = new RECTT(nameof(line3),
                                new JU(0, far, 0, gao),
                                new JU(0, 0, far, gao));
        public static RECTT line6 = new RECTT(nameof(line6),
                                new JU(screen2Width0, ga2, ch2, ga2),
                                new JU(screen2Width0, ga2 - far + 100, ch2, ga2));
        public static RECTT line7 = new RECTT(nameof(line7),
                                new JU(ch2, far, ch2, ga2),
                                new JU(ch2 - far, 0, ch2, ga2));

        RECTT corner1 = new RECTT(nameof(corner1),
                                new JU(0, 0, 0, 0), new JU(0, 0, far, far));
        RECTT corner2 = new RECTT(nameof(corner2),
                                new JU(cha, 0, cha, 0), new JU(cha - far, 0, cha, far));
        RECTT corner5 = new RECTT(nameof(corner5),
                                new JU(ch0, 0, ch0, 0),
                                new JU(ch0, 0, ch0 + far, far));
        RECTT corner6 = new RECTT(nameof(corner6),
                                new JU(ch2, 0, ch2, 0),
                                new JU(ch2 - far, 0, ch2, far));

        bool di = false;
        DateTime di_time = DateTime.MinValue;


        public static bool chrome_red()
        {
            return judge_color(6095, 56, Color.FromArgb(162, 37, 45))
                       && judge_color(4646, 56, Color.FromArgb(162, 37, 45));
            //return (judge_color(-1783, 51, Color.FromArgb(162, 37, 45))
            //           && judge_color(-645, 45, Color.FromArgb(162, 37, 45)));
        }

        public struct JU
        {
            public int Left, Top, Right, Bottom;
            public JU(int Left, int Top, int Right, int Bottom)
            {
                this.Left = Left; this.Top = Top; this.Right = Right; this.Bottom = Bottom;
            }
        }
        public class RECTT
        {
            public bool can = true;
            public readonly JU a, b;
            public static List<RECTT> All = new List<RECTT>();
            public string name;
            public Task aTask;

            public delegate void aEventHandler(MouseHookEventArgs e);
            public event aEventHandler aMouseHookEvent;

            public delegate void bEventHandler(MouseHookEventArgs e);
            public event bEventHandler bMouseHookEvent;
            public void doo(MouseHookEventArgs e)
            {
                int music = 0;
                if (a.Left == a.Right)
                {
                    int total = Math.Abs(a.Bottom - a.Top);
                    if (total > 0)
                    {
                        int yOffset = Math.Abs(e.Y - Math.Min(a.Top, a.Bottom));
                        // 1~9, 最上为1，最下为0
                        music = Math.Min(yOffset * 10 / total, 10);
                        if (music < 1) music = 1;
                        if (music > 9) music = 0;
                    }
                }
                else if (a.Top == a.Bottom)
                {
                    int total = Math.Abs(a.Right - a.Left);
                    if (total > 0)
                    {
                        int xOffset = Math.Abs(e.X - Math.Min(a.Left, a.Right));
                        // 1~9, 最左为1，最右为0
                        music = Math.Min(xOffset * 10 / total, 10);
                        if (music < 1) music = 1;
                        if (music > 9) music = 0;
                    }
                }
                play_sound_bongocat(music);
                aMouseHookEvent?.Invoke(e);
            }
            public void doo2(MouseHookEventArgs e)
            {
                bMouseHookEvent?.Invoke(e);
            }
            public RECTT(string name, JU a, JU b)
            {
                this.name = name; this.a = a; this.b = b; All.Add(this);
            }
            ~RECTT() { All.Remove(this); }
            public override string ToString() => base.ToString();
            public bool target(Point testPoint)
            {
                if (!can) return false;
                var hit = (testPoint.X >= a.Left && testPoint.X <= a.Right &&
                           testPoint.Y >= a.Top && testPoint.Y <= a.Bottom);
                if (hit) can = false;
                return hit;
            }
            public bool ignore(Point testPoint)
            {
                if (can) return false;
                var outB = !(testPoint.X >= b.Left && testPoint.X <= b.Right &&
                             testPoint.Y >= b.Top && testPoint.Y <= b.Bottom);
                if (outB) can = true;
                return outB;
            }
            public static RECTT ignoreAll(Point testPoint)
            {
                //foreach (var item in All)
                //{
                //    if (item.can) continue;
                //    var outB = !(testPoint.X >= item.b.Left && testPoint.X <= item.b.Right &&
                //                 testPoint.Y >= item.b.Top && testPoint.Y <= item.b.Bottom);
                //    if (outB) item.can = true;
                //}
                foreach (var item in All)
                    if (item.ignore(testPoint)) return item;
                return null;
            }
            public static void release()
            {
                foreach (var item in All) item.can = false;
            }
            public static int[] special_int = [0, screenWidth1, screenHeight1, screenHeight1 - 1, screen2Width, screen2Height1];
            public static RECTT get(Point testPoint)
            {
                //if (!(special_int.Contains(testPoint.X) || special_int.Contains(testPoint.Y))) return null;
                foreach (var item in All)
                    if (item.target(testPoint)) return item;
                return null;
            }
        }
    }
}
