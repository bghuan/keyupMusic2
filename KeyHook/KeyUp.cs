using static keyupMusic2.Common;
using static keyupMusic2.MouseKeyboardHook;

namespace keyupMusic2
{
    public class KeyUp
    {
        public static void yo(KeyboardHookEventArgs e)
        {
            switch (e.key)
            {
                case Keys.Home:
                case Keys.End:
                    bmpScreenshot.Dispose();
                    break;
            }

            switch (ProcessName)
            {
                case cs2:
                    switch (e.key)
                    {
                        //case Keys.A:
                        //    press_dump_task(Keys.D, 80);
                        //    break;
                        //case Keys.D:
                        //    press_dump_task(Keys.A, 80);
                        //    break;
                        //case Keys.W:
                        //    press_dump_task(Keys.S, 80);
                        //    break;
                        //case Keys.S:
                        //    press_dump_task(Keys.W, 80);
                        //    break;
                    }
                    break;
            }
        }
    }
}
