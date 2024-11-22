namespace keyupMusic2
{
    public class Log
    {
        private static readonly object _lockObject = new object();
        public static void log(string message)
        {
            lock (_lockObject)
            {
                try
                {
                    File.AppendAllText("log\\log.txt", "\r" + DateTime.Now.ToString("") + " " + message);
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }
                finally
                {
                    string fff = "ffs";
                }
            }
        }
    }
}
