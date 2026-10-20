namespace keyupMusic4
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 全局异常日志：未处理异常写 log/crash.log，避免静默退出难以排查
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                try { File.WriteAllText("log\\crash.log", $"[{DateTime.Now}] UnhandledException {e.ExceptionObject}"); } catch { }
            };
            Application.ThreadException += (s, e) =>
            {
                try { File.WriteAllText("log\\crash.log", $"[{DateTime.Now}] ThreadException {e.Exception}"); } catch { }
            };
            try
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                Application.Run(new Huan());
            }
            catch (Exception ex)
            {
                try { File.WriteAllText("log\\crash.log", $"[{DateTime.Now}] Main {ex}"); } catch { }
                throw;
            }
        }
    }
}