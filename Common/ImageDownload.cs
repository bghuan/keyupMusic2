using Microsoft.Win32;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using static keyupMusic2.Native;
using static keyupMusic2.winBinWallpaper;

namespace keyupMusic2
{
    public partial class Common
    {
        public static string download_image_prix = "#/picture/";
        public static void download_image(List<string> mmm)
        {
            //foreach(var msg in mmm)
            //{
            //    string id = msg.Replace(".png","").Replace(download_image_prix,"");
            //    ProcessRun(lz_image_downloadexe, id,true);
            //    Sleep(2000);
            //}
            ProcessRun(lz_image_downloadexe, string.Join(" ", mmm), true);
        }
        public static void download_image(string msg)
        {
            play_sound_di();
            if (msg.StartsWith(download_image_prix))
            {
                string id = msg.Substring(download_image_prix.Length);
                _download_image(id);
            }
            play_sound_di();
            //press([Keys.LControlKey, Keys.W]);
            //mousego();
        }
        public static void _download_image(string id)
        {
            //if (ProcessName != chrome) return;
            //if (!ProcessTitle.Contains("详情")) return;
            ProcessRun(lz_image_downloadexe, id);
        }

        // 壁纸样式常量
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDCHANGE = 0x02;

        // 请求状态管理
        private static WallpaperRequest currentRequest = null;
        private static CancellationTokenSource cancellationToken = null;
        private static System.Threading.Timer debounceTimer = null;
        private static readonly object requestLock = new object();
        private static int requestCounter = 0;

        private class WallpaperRequest
        {
            public string FilePath { get; set; }
            public WallpaperStyle Style { get; set; }
            public Action<bool, string> Callback { get; set; }
            public CancellationToken Token { get; set; }
            public int RequestId { get; set; }
        }
        public static bool can_set_wallpaper = false; // 是否可以设置壁纸
        // 优化后的壁纸设置方法（最后触发的请求会使之前的无效）
        public static void SetDesktopWallpaper(string filePath, WallpaperStyle style = WallpaperStyle.Stretched, Action<bool, string> callback = null)
        {
            if (!can_set_wallpaper)
                return;
            // 检查文件是否存在
            if (!File.Exists(filePath))
            {
                Console.WriteLine("错误: 文件不存在 - " + filePath);
                callback?.Invoke(false, "文件不存在");
                return;
            }

            lock (requestLock)
            {
                // 取消当前正在执行的请求
                cancellationToken?.Cancel();
                cancellationToken?.Dispose();
                cancellationToken = new CancellationTokenSource();

                // 创建新请求
                var newRequest = new WallpaperRequest
                {
                    FilePath = filePath,
                    Style = style,
                    Callback = callback,
                    Token = cancellationToken.Token,
                    RequestId = Interlocked.Increment(ref requestCounter)
                };

                // 保存当前请求
                currentRequest = newRequest;

                // 重置防抖计时器
                ResetDebounceTimer(() =>
                {
                    // 检查请求是否仍然有效
                    if (currentRequest == newRequest && !newRequest.Token.IsCancellationRequested)
                    {
                        ProcessRequest(newRequest);
                    }
                });
            }
        }
        // 重置防抖计时器
        private static void ResetDebounceTimer(Action action)
        {
            // 取消现有计时器
            debounceTimer?.Dispose();

            // 创建新计时器
            debounceTimer = new System.Threading.Timer(
                _ =>
                {
                    debounceTimer.Dispose();
                    debounceTimer = null;
                    action();
                },
                null,
                150, // 150ms 防抖时间
                Timeout.Infinite
            );
        }
        // 处理壁纸请求
        private static void ProcessRequest(WallpaperRequest request)
        {
            try
            {
                // 检查是否已取消
                if (request.Token.IsCancellationRequested)
                {
                    Console.WriteLine($"请求 {request.RequestId} 已取消");
                    request.Callback?.Invoke(false, "请求已取消");
                    return;
                }

                Console.WriteLine($"开始处理请求 {request.RequestId}: {request.FilePath}");

                // 设置壁纸样式和平铺选项
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
                {
                    if (key == null)
                    {
                        throw new Exception("无法访问注册表路径");
                    }

                    SetWallpaperStyle(request.Style, key);
                }

                // 检查是否已取消
                if (request.Token.IsCancellationRequested)
                {
                    Console.WriteLine($"请求 {request.RequestId} 在设置注册表后取消");
                    request.Callback?.Invoke(false, "请求已取消");
                    return;
                }

                // 应用壁纸
                int result = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, request.FilePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);

                if (result == 0)
                {
                    throw new Exception("设置壁纸失败");
                }

                ishide_DesktopWallpaper = 2;
                Console.WriteLine($"成功: 请求 {request.RequestId} 壁纸已设置为 {request.FilePath}");

                // 执行回调（成功）
                request.Callback?.Invoke(true, "壁纸设置成功");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"请求 {request.RequestId} 被取消");
                request.Callback?.Invoke(false, "请求被取消");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"请求 {request.RequestId} 异常: {ex.Message}");

                // 执行回调（失败）
                request.Callback?.Invoke(false, ex.Message);
            }
        }

        private static void SetWallpaperStyle(WallpaperStyle style, RegistryKey key)
        {
            switch (style)
            {
                case WallpaperStyle.Centered:
                    key.SetValue("WallpaperStyle", "1");  // 居中
                    key.SetValue("TileWallpaper", "0"); // 不平铺
                    break;
                case WallpaperStyle.Tiled:
                    key.SetValue("WallpaperStyle", "1");  // 居中（但会平铺）
                    key.SetValue("TileWallpaper", "1"); // 平铺
                    break;
                case WallpaperStyle.Stretched:
                    key.SetValue("WallpaperStyle", "2");  // 拉伸
                    key.SetValue("TileWallpaper", "0"); // 不平铺
                    break;
                case WallpaperStyle.Fit:
                    key.SetValue("WallpaperStyle", "6");  // 适应（保持比例）
                    key.SetValue("TileWallpaper", "0"); // 不平铺
                    break;
                case WallpaperStyle.Fill:
                    key.SetValue("WallpaperStyle", "10"); // 填充（保持比例，可能裁剪）
                    key.SetValue("TileWallpaper", "0"); // 不平铺
                    break;
                case WallpaperStyle.Original:
                    key.SetValue("WallpaperStyle", "0");  // 原始大小
                    key.SetValue("TileWallpaper", "0"); // 不平铺
                    break;
            }
        }
    }

    // 壁纸样式枚举
    public enum WallpaperStyle
    {
        Centered,  // 居中显示
        Tiled,     // 平铺显示
        Stretched, // 拉伸填充
        Fit,       // 适应屏幕（保持比例）
        Fill,      // 填充屏幕（保持比例，可能裁剪）
        Original   // 原始大小（不拉伸，可能只显示部分）
    }


}
