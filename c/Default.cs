namespace keyupMusic2
{
    public class Default
    {
        public static Keys handling_keys;
        public static bool handling = true;
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
    }
}
