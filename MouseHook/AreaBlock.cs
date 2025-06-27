using System.Diagnostics;
using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace keyupMusic2
{
    partial class biu
    {
        public static int block_far = 200;
        public static Point _左上 = new Point(0, 0);
        public static Point _右上 = new Point(ScreenPrimary.Width, 0);
        public static Point _左下 = new Point(0, ScreenPrimary.Height);
        public static Point _右下 = new Point(ScreenPrimary.Width, ScreenPrimary.Height);
        public static Point _2上 = new Point(ScreenPrimary.Width / 3, 0);
        public static Point _2下 = new Point(ScreenPrimary.Width / 3, ScreenPrimary.Height);
        public static Point _3上 = new Point(ScreenPrimary.Width / 3 * 2, 0);
        public static Point _3下 = new Point(ScreenPrimary.Width / 3 * 2, ScreenPrimary.Height);

        public static Point _2上_1 = new Point(ScreenPrimary.Width / 3 - block_far, 0);
        public static Point _2上_2 = new Point(ScreenPrimary.Width / 3 + block_far, 0);
        public static Point _2下_1 = new Point(ScreenPrimary.Width / 3, 0 - block_far);
        public static Point _2下_2 = new Point(ScreenPrimary.Width / 3, 0 + block_far);
        public static Point _3上_1 = new Point(ScreenPrimary.Width / 3 * 2 - block_far, 0);
        public static Point _3上_2 = new Point(ScreenPrimary.Width / 3 * 2 + block_far, 0);
        public static Point _3下_1 = new Point(ScreenPrimary.Width / 3 * 2, 0 - block_far);
        public static Point _3下_2 = new Point(ScreenPrimary.Width / 3 * 2, 0 + block_far);

        //public static RECTT block2 = new RECTT(nameof(line2),
        //                        new JU(_左上, _2下),
        //                        new JU(_左上, _2下_2));
        //public static RECTT block3 = new RECTT(nameof(line3),
        //                        new JU(_2上, _3下),
        //                        new JU(_2上_1, _3下_2));
        //public static RECTT block6 = new RECTT(nameof(line6),
        //                        new JU(_3上, _右下),
        //                        new JU(_3上_1, _右下));

        //public static RECTT block1 = new RECTT(nameof(block1),
        //                        new JU(_左上, _2下),
        //                        new JU(_左上, _2下));
        //public static RECTT block2 = new RECTT(nameof(block2),
        //                        new JU(_2上, _3下),
        //                        new JU(_2上, _3下));
        //public static RECTT block3 = new RECTT(nameof(block3),
        //                        new JU(_3上, _右下),
        //                        new JU(_3上, _右下));
        private int _block1(MouseHookEventArgs e)
        {
            if (is_lizhi) return 1;
            return 0;
        }
        private int _block2(MouseHookEventArgs e)
        {
            if (is_lizhi) return 2;
            return 0;
        }
        private int _block3(MouseHookEventArgs e)
        {
            if (is_lizhi) return 3;
            return 0;
    }

    }
}
