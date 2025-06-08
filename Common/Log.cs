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

        private static string cache = "";
        private const int MAX_CACHE_LENGTH = 2000; // 最大长
        public static void logcache(string message)
        {
            string newLog = $"\r{DateTime.Now:HH:mm:ss} {message}";

            int newLength = cache.Length + newLog.Length;
            if (newLength > MAX_CACHE_LENGTH)
            {
                int keepLength = MAX_CACHE_LENGTH - newLog.Length;
                cache = cache.Substring(Math.Max(0, keepLength)); // 避免负数索引
            }

            cache += newLog;
        }
        public static void logcachesave()
        {
            cache = "\r" + DateTime.Now.ToString("") + "logcachesave start"
                    + "\r" + cache;
            cache += "\r" + "logcachesave end";
            File.AppendAllText("log\\log.txt", "\r" + DateTime.Now.ToString("") + " " + cache);
            cache = "";
        }
    }
}
