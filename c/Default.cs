using static WGestures.Core.Impl.Windows.MouseKeyboardHook;
using static keyupMusic2.Common;
using KeyEvent = WGestures.Core.Impl.Windows.MouseKeyboardHook.KeyboardHookEventArgs;

namespace keyupMusic2
{
    public class Default
    {
        public static Keys handling_keys;
        public static bool handling = true;
        public static bool catched = false;
        public static int handling_times = 0;
        List<Keys> keys = new List<Keys> { };
        public string ClassName()
        {
            return this.GetType().Name;
        }
        public void raw_press()
        {
            if (handling_times < 20000)
            {
                handling_times++;
                handling = false;
                Common.press(handling_keys);
                keys.Add(handling_keys);
                Thread.Sleep(10);
            }
            else
            {
                throw new Exception("handling_times>1000" + handling_times + handling_keys.ToString());
            }
        }
        public void raw_press2()
        {
            if (handling_times < 20000)
            {
                handling_times++;
                handling = false;
                //KeyboardInput.SendString("x");
                Common.press_hold(handling_keys,300);
                Common.press_hold(handling_keys,300);
                keys.Add(handling_keys);
                Thread.Sleep(10);
            }
            else
            {
                throw new Exception("handling_times>1000" + handling_times + handling_keys.ToString());
            }
        }
        public virtual bool judge_handled(KeyEvent e)
        {
            return false;
        }
        public void handlingggg(KeyEvent e)
        {
            pre_handling(e);
            do_handling(e);
            fin_handling(e);
        }
        public virtual void do_handling(KeyEvent e)
        {
            throw new NotImplementedException();
        }

        public void pre_handling(KeyEvent e)
        {
            Common.hooked = true;
            handling_keys = e.key;
            catched = false;
        }
        public void fin_handling(KeyEvent e)
        {
            Common.hooked = false;
            if (!handling) handling = true;
            catched = true;
        }
    }
}
