using KeyEvent = keyupMusic2.KeyboardMouseHook.KeyEventArgs;

namespace keyupMusic2
{
    public class Default
    {
        public static Keys handling_keys;
        public static bool catched = false;
        public static int handling_times = 0;
        List<Keys> keys = new List<Keys> { };
        public string ClassName()
        {
            return this.GetType().Name;
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
        }
        public void fin_handling(KeyEvent e)
        {
        }
    }
}
