using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class M3u8Downloader
{
    //static async Task Main(string[] args)
    //{
    //    // 调用异步方法并等待完成
    //    DownloadEpisodesAsync().Wait();

    //    Console.WriteLine("所有下载任务已完成");
    //    Console.WriteLine("按任意键退出...");
    //    Console.ReadKey();
    //}

    //static async Task DownloadEpisodesAsync()
    //{
    //    // 定义下载范围：从第04集到第40集
    //    for (int episode = 4; episode <= 40; episode++)
    //    {
    //        try
    //        {
    //            // 格式化集数为两位数（如04, 05, ..., 40）
    //            string index = episode.ToString("D2");

    //            // m3u8文件地址
    //            string m3u8Url = "https://c1.rrcdex.m3u8";

    //            // 输出文件路径
    //            string outputPath = Path.Combine(
    //                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
    //                $"第{index}集.mp4"
    //            );

    //            Console.WriteLine($"\n===== 开始下载第{index}集 =====");
    //            Console.WriteLine($"视频地址: {m3u8Url}");
    //            Console.WriteLine($"保存路径: {outputPath}");

    //            M3u8Downloader downloader = new M3u8Downloader();
    //            // 设置并发线程数
    //            downloader.MaxDegreeOfParallelism = 10;

    //            // 下载当前集
    //            await downloader.DownloadAsync(m3u8Url, outputPath, progress =>
    //            {
    //                Console.Write($"\r下载进度: {progress:F2}%");
    //            });

    //            Console.WriteLine($"\n第{index}集已成功保存到: {outputPath}");
    //            Console.WriteLine($"===== 第{index}集下载完成 =====\n");
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"\n第{episode.ToString("D2")}集下载发生错误: {ex.Message}");
    //            Console.WriteLine("是否继续下载下一集？(y/n)");
    //            string answer = Console.ReadLine();
    //            if (answer?.Trim().ToLower() != "y")
    //            {
    //                Console.WriteLine("用户选择终止下载");
    //                break;
    //            }
    //        }
    //    }

    private readonly HttpClient _httpClient;
    private int _totalSegments;
    private int _completedSegments;
    private string _tempDirectory;

    // 最大并发线程数，可根据实际情况调整
    public int MaxDegreeOfParallelism { get; set; } = 8;

    public M3u8Downloader()
    {
        _httpClient = new HttpClient();
        // 设置超时时间（10分钟）
        _httpClient.Timeout = TimeSpan.FromMinutes(10);
    }

    public async Task DownloadAsync(string m3u8Url, string outputPath, Action<double> progressCallback = null)
    {
        // 创建临时目录保存TS片段
        _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDirectory);

        try
        {
            // 下载并解析m3u8文件
            string m3u8Content = await _httpClient.GetStringAsync(m3u8Url);
            List<string> tsUrls = ParseM3u8(m3u8Content, m3u8Url);

            if (tsUrls.Count == 0)
            {
                throw new Exception("未找到TS片段");
            }

            _totalSegments = tsUrls.Count;
            _completedSegments = 0;

            Console.WriteLine($"发现 {_totalSegments} 个TS片段，开始多线程下载...");

            // 使用并行任务下载所有TS片段
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism
            };

            await Task.Run(() =>
            {
                Parallel.ForEach(tsUrls, options, (tsUrl, state, index) =>
                {
                    try
                    {
                        DownloadTsSegment(tsUrl, (int)index).Wait();

                        // 更新进度
                        int completed = Interlocked.Increment(ref _completedSegments);
                        double progress = (double)completed / _totalSegments * 100;
                        progressCallback?.Invoke(progress);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"下载片段 {index} 失败: {ex.Message}");
                        state.Break(); // 发生错误时停止所有任务
                        throw;
                    }
                });
            });

            // 合并所有TS片段
            Console.WriteLine("所有片段下载完成，开始合并...");
            await MergeTsFiles(tsUrls.Count, outputPath);

            Console.WriteLine("视频合并完成");
        }
        finally
        {
            // 清理临时文件
            if (Directory.Exists(_tempDirectory))
            {
                try
                {
                    Directory.Delete(_tempDirectory, recursive: true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"清理临时文件失败: {ex.Message}");
                }
            }
        }
    }

    private async Task DownloadTsSegment(string tsUrl, int index)
    {
        try
        {
            // 为TS片段生成有序的文件名，确保合并时顺序正确
            string tsFileName = $"{index:D8}.ts"; // 使用8位数字，确保排序正确
            string tsFilePath = Path.Combine(_tempDirectory, tsFileName);

            // 下载TS片段
            var response = await _httpClient.GetAsync(tsUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var fileStream = new FileStream(tsFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"下载TS片段失败: {tsUrl}，错误: {ex.Message}");
        }
    }

    private async Task MergeTsFiles(int totalSegments, string outputPath)
    {
        // 确保输出目录存在
        string outputDirectory = Path.GetDirectoryName(outputPath);
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        // 按文件名排序（确保顺序正确）
        string[] tsFiles = Directory.GetFiles(_tempDirectory, "*.ts")
                                    .OrderBy(f => f)
                                    .ToArray();

        if (tsFiles.Length != totalSegments)
        {
            throw new Exception($"TS片段不完整，预期 {totalSegments} 个，实际 {tsFiles.Length} 个");
        }

        // 合并所有TS文件
        using (var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            foreach (string tsFile in tsFiles)
            {
                using (var tsStream = new FileStream(tsFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    await tsStream.CopyToAsync(outputStream);
                }
            }
        }
    }

    private List<string> ParseM3u8(string m3u8Content, string baseUrl)
    {
        var tsUrls = new List<string>();
        string[] lines = m3u8Content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        Uri baseUri = new Uri(baseUrl);

        foreach (string line in lines)
        {
            // 跳过注释和EXT标签
            if (line.StartsWith("#"))
                continue;

            // 处理相对路径
            if (Uri.TryCreate(line, UriKind.Absolute, out Uri absoluteUri))
            {
                tsUrls.Add(line);
            }
            else if (Uri.TryCreate(baseUri, line, out Uri combinedUri))
            {
                tsUrls.Add(combinedUri.AbsoluteUri);
            }
        }

        return tsUrls;
    }
}
