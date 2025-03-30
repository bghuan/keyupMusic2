using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public class KeyUp
    {
        public static int w = 0;
        public static void yo(KeyboardHookEventArgs e)
        {
            switch (ProcessName)
            {
                case cs2:
                    switch (e.key)
                    {
                        //case Keys.A:
                        //    w = 0;
                        //    break;
                        //case Keys.D:
                        //    if (w == 1) break;
                        //    w = 1;
                        //    //TaskRun(() => { press(Keys.A, 2000); },1000);
                        //    press_dump(Keys.A, 100);
                        //    //SS(2000).KeyDown(Keys.A);
                        //    //SS(10).KeyUp(Keys.A);
                        //    //press("A");
                        //    if (w == 1) break;
                        //    w = 0;
                        //    break;
                    }
                    break;
            }
        }
    }
}
