using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    partial class biu
    {
        public void init()
        {
            line2.aMouseHookEvent += _line2;
            line3.aMouseHookEvent += _line3;
            line6.aMouseHookEvent += _line6;
            line7.aMouseHookEvent += _line7;

            corner1.aMouseHookEvent += _corner1;
            corner2.aMouseHookEvent += _corner2;
            corner4.aMouseHookEvent += _corner4;
            corner5.aMouseHookEvent += _corner5;
            corner6.aMouseHookEvent += _corner6;

            //block1.aMouseHookEvent += _block1;
            //block2.aMouseHookEvent += _block2;
            //block3.aMouseHookEvent += _block3;
        }

        static int far = 300;
        static int _fa = screenWidth - far;
        static int gao = screenHeight1;
        static int cha = screenWidth1;

        static int ga2 = screen2Height1;
        static int ch2 = screen2Width;
        static int ch0 = screen2X;
        static int ga0 = screen2Y;
        int chrome_x_min = -50;


        public static RECTT line2 = new RECTT(nameof(line2),
                                new JU(0, gao, _fa - 200, gao),
                                new JU(0, gao - far + 100, cha, gao));
        public static RECTT line3 = new RECTT(nameof(line3),
                                new JU(0, far, 0, gao),
                                new JU(0, 0, far, gao));
        public static RECTT line6 = new RECTT(nameof(line6),
                                new JU(screen2X, ga2, ch2, ga2),
                                new JU(screen2X, ga2 - far + 100, ch2, ga2));
        public static RECTT line7 = new RECTT(nameof(line7),
                                new JU(ch2, far, ch2, ga2),
                                new JU(ch2 - far, 0, ch2, ga2));

        RECTT corner1 = new RECTT(nameof(corner1),
                                new JU(0, 0, 0, 0), new JU(0, 0, far, far));
        RECTT corner2 = new RECTT(nameof(corner2),
                                new JU(cha, 0, cha, 0), new JU(cha - far, 0, cha, far));
        RECTT corner4 = new RECTT(nameof(corner4),
                                new JU(cha, gao, cha, gao), new JU(cha - far, gao - far, cha, gao));
        RECTT corner5 = new RECTT(nameof(corner5),
                                new JU(ch0, ga0, ch0, ga0),
                                new JU(ch0, ga0, ch0 + far, far + ga0));
        RECTT corner6 = new RECTT(nameof(corner6),
                                new JU(ch2, ga0, ch2, ga0),
                                new JU(ch2 - far, ga0, ch2, far + ga0));

        public void Cornor(MouseHookEventArgs e)
        {
            if (e.Msg != MouseMsg.move) { RECTT.release(); return; }
            //if (e.X > screen2Width || e.Y < 0 || e.Y > screenHeightMax) return;

            var rect = RECTT.get(e.Pos);
            if (rect == null)
            {
                var rect2 = RECTT.ignoreAll(e.Pos);
                rect?.doo2(e);
                return;
            }
            if (is_down(LButton) || is_down(RButton)) { RECTT.release(); return; }

            //log(rect.name + " " + e.X + " " + e.Y);

            rect.doo(e);// Main

            RECTT.release();
        }

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
            public JU(Point a, Point b)
            {
                this.Left = a.X; this.Top = a.Y; this.Right = b.X; this.Bottom = b.Y;
            }
        }
        public class RECTT
        {
            public bool can = true;
            public readonly JU a, b;
            public static List<RECTT> All = new List<RECTT>();
            public string name;
            public Task aTask;

            public delegate int aEventHandler(MouseHookEventArgs e);
            public event aEventHandler aMouseHookEvent;

            public delegate void bEventHandler(MouseHookEventArgs e);
            public event bEventHandler bMouseHookEvent;
            public void doo(MouseHookEventArgs e)
            {
                int music = di_tune(e);
                e.data = music;

                Show(name, 2);

                if (aMouseHookEvent != null)
                {
                    FreshProcessName();
                    int result = aMouseHookEvent.Invoke(e);
                    FreshProcessName();

                    if (result == 0) return;
                    play_sound_bongocat(result);
                }
            }

            private int di_tune(MouseHookEventArgs e)
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

                return music;
            }

            public void doo2(MouseHookEventArgs e)
            {
                bMouseHookEvent?.Invoke(e);
            }
            public RECTT(string name, JU a, JU b)
            {
                this.name = name; this.a = a; this.b = b;

                if (ScreenSecond == Rectangle.Empty && (name.Contains("5") || name.Contains("6") || name.Contains("7") || name.Contains("8"))) { return; }
                All.Add(this);
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