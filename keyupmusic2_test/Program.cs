using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // 调用异步方法并等待完成
        DownloadEpisodesAsync().Wait();

        Console.WriteLine("所有下载任务已完成");
        Console.WriteLine("按任意键退出...");
        Console.ReadKey();
    }

    static async Task DownloadEpisodesAsync()
    {
        // 定义下载范围：从第04集到第40集
        for (int episode = 4; episode <= 40; episode++)
        {
            try
            {
                // 格式化集数为两位数（如04, 05, ..., 40）
                string index = episode.ToString("D2");

                // m3u8文件地址
                string m3u8Url = "https://c1.rrcdnbf1.com/video/baoqingtianzhibixuedanxin/%E7%AC%AC" + index + "%E9%9B%86/index.m3u8";

                // 输出文件路径
                string outputPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    $"第{index}集.mp4"
                );

                Console.WriteLine($"\n===== 开始下载第{index}集 =====");
                Console.WriteLine($"视频地址: {m3u8Url}");
                Console.WriteLine($"保存路径: {outputPath}");

                M3u8Downloader downloader = new M3u8Downloader();
                // 设置并发线程数
                downloader.MaxDegreeOfParallelism = 10;

                // 下载当前集
                await downloader.DownloadAsync(m3u8Url, outputPath, progress =>
                {
                    Console.Write($"\r下载进度: {progress:F2}%");
                });

                Console.WriteLine($"\n第{index}集已成功保存到: {outputPath}");
                Console.WriteLine($"===== 第{index}集下载完成 =====\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n第{episode.ToString("D2")}集下载发生错误: {ex.Message}");
                Console.WriteLine("是否继续下载下一集？(y/n)");
                string answer = Console.ReadLine();
                if (answer?.Trim().ToLower() != "y")
                {
                    Console.WriteLine("用户选择终止下载");
                    break;
                }
            }
        }
    }
}
