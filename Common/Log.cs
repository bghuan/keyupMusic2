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
                    // 添加时间格式
                    string logEntry = $"\r{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {message}";

                    // 获取堆栈信息，参数1表示跳过当前方法
                    System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(2);
                    string stackInfo = stackTrace.ToString();

                    // 处理堆栈信息：分割成行，过滤不需要的行
                    string[] stackLines = stackInfo.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    if (stackLines.Length > 0)
                    {
                        // 从第二行开始筛选，排除包含"System"的行
                        var filteredLines = stackLines
                                                     .Where(line => !line.Contains("System."))
                                                     .Where(line => !line.Contains("KeyBoardHookProc"))
                                                     .Select(ProcessStackLine)// 处理每行只保留方法名
                                                     .Reverse()
                                                     .ToList();

                        if (filteredLines.Any())
                        {
                            logEntry += $" {string.Join("/", filteredLines)}";
                        }
                    }

                    File.AppendAllText("log\\log.txt", logEntry);
                }
                catch (Exception e)
                {
                    // 记录异常信息，包括堆栈跟踪
                    string errorMsg = $"\r{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} 日志记录失败: {e.Message}\r异常堆栈:\r{e.StackTrace}";
                    Console.WriteLine(errorMsg);
                }
            }
        }

        // 处理堆栈行，只保留方法名
        private static string ProcessStackLine(string line)
        {
            // 去除行首的空白和"at "前缀
            string processed = line.TrimStart().Replace("at ", "", StringComparison.OrdinalIgnoreCase);

            // 分割出方法名部分（处理普通方法和匿名方法两种情况）
            int lastDotIndex = processed.LastIndexOf('.');
            if (lastDotIndex > -1)
            {
                processed = processed.Substring(lastDotIndex + 1);
            }

            // 去除参数部分（从'('开始的内容）
            int paramStartIndex = processed.IndexOf('(');
            if (paramStartIndex > -1)
            {
                processed = processed.Substring(0, paramStartIndex);
            }

            // 处理匿名方法的特殊命名（如<>c__DisplayClass4_0.<KeyBoardHookProc>b__0 -> KeyBoardHookProc）
            if (processed.Contains("<") && processed.Contains(">"))
            {
                int start = processed.IndexOf('<') + 1;
                int end = processed.IndexOf('>', start);
                if (start < end)
                {
                    processed = processed.Substring(start, end - start);
                }
            }

            return processed;
        }
    }
}
