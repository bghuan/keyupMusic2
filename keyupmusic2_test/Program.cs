using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChromeUrlReader
{
    class Program
    {
        // Chrome可执行文件路径
        private const string ChromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

        // 远程调试端口
        private const int DebugPort = 9222;

        // 临时数据目录（避免干扰正常Chrome会话）
        private const string TempDataDir = @"C:\Temp\ChromeDebugSession";

        static async Task Main(string[] args)
        {
            try
            {
                // 启动Chrome并启用远程调试
                Process chromeProcess = StartChromeWithDebugging();

                // 等待Chrome启动
                await Task.Delay(2000);

                // 获取当前URL
                string currentUrl = await GetCurrentChromeUrl();

                Console.WriteLine($"当前Chrome URL: {currentUrl}");

                // 清理：关闭Chrome
                // chromeProcess.Kill();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
            }

            Console.ReadLine();
        }

        static Process StartChromeWithDebugging()
        {
            // 创建临时数据目录
            Directory.CreateDirectory(TempDataDir);

            // 启动Chrome并启用远程调试
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ChromePath,
                Arguments = $"--remote-debugging-port={DebugPort} --user-data-dir=\"{TempDataDir}\"",
                UseShellExecute = true
            };

            return Process.Start(startInfo);
        }

        static async Task<string> GetCurrentChromeUrl()
        {
            using var httpClient = new HttpClient();

            // 获取调试端点列表
            var response = await httpClient.GetStringAsync($"http://localhost:{DebugPort}/json");
            var tabs = JsonSerializer.Deserialize<List<ChromeTab>>(response);

            // 查找活跃的标签页
            foreach (var tab in tabs)
            {
                if (tab.Type == "page" && !string.IsNullOrEmpty(tab.WebSocketDebuggerUrl))
                {
                    // 获取标签页的当前URL
                    return tab.Url;

                    // 或者，如果你想通过WebSocket实时监控URL变化，可以连接到调试器URL
                    // await MonitorUrlChanges(tab.WebSocketDebuggerUrl);
                }
            }

            return "未找到活跃标签页";
        }

        static async Task MonitorUrlChanges(string webSocketUrl)
        {
            using var clientWebSocket = new ClientWebSocket();
            await clientWebSocket.ConnectAsync(new Uri(webSocketUrl), CancellationToken.None);

            // 发送命令获取当前URL
            string command = JsonSerializer.Serialize(new
            {
                id = 1,
                method = "Page.getNavigationHistory"
            });

            var commandBytes = Encoding.UTF8.GetBytes(command);
            await clientWebSocket.SendAsync(new ArraySegment<byte>(commandBytes),
                WebSocketMessageType.Text, true, CancellationToken.None);

            // 接收响应
            var buffer = new byte[4096];
            var result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string response = Encoding.UTF8.GetString(buffer, 0, result.Count);

            Console.WriteLine($"URL变化: {response}");
        }
    }

    class ChromeTab
    {
        public string Url { get; set; }
        public string Type { get; set; }
        public string WebSocketDebuggerUrl { get; set; }
    }
}