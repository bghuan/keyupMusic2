using System.Diagnostics;
using static keyupMusic4.Common;
using static keyupMusic4.KeyboardMouseHook;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace keyupMusic4
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


        private int _block1(KeyboardMouseHook.MouseEventArgs e)
        {
            if (is_lizhi) return 1;
            return 0;
        }
        private int _block2(KeyboardMouseHook.MouseEventArgs e)
        {
            if (is_lizhi) return 2;
            return 0;
        }
        private int _block3(KeyboardMouseHook.MouseEventArgs e)
        {
            if (is_lizhi) return 3;
            return 0;
    }

    }
}
