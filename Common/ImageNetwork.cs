using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace keyupMusic2
{
    public partial class Common
    {
        public static string download_image_prix = "#/picture/";
        public static void download_image(List<string> mmm)
        {
            //foreach(var msg in mmm)
            //{
            //    string id = msg.Replace(".webp","").Replace(download_image_prix,"");
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
        static int SetDesktopWallpaperAli_count = 0;
        static bool SetDesktopWallpaperAli_flag = false;
        public static void SetDesktopWallpaperAli(string filePath, bool di = true)
        {
            if (SetDesktopWallpaperAli_flag)
            {
                Task.Run(() =>
                {
                    if (di)
                        play_sound_di();
                    for (int i = 0; i < 120; i++)
                    {
                        Sleep(1000);
                        if (!SetDesktopWallpaperAli_flag)
                        {
                            SetDesktopWallpaperAli(filePath, di);
                            break;
                        }
                    }

                });

                return;
            }
            SetDesktopWallpaperAli_flag = true;
            {
                var currentPath = GetWallpaperFromRegistry();
                if (!currentPath.Contains(Common.keyupMusic))
                {
                    SetDesktopWallpaperAli_flag = false;
                    return;
                }
            }

            if (di)
                play_sound_di();
            {
                var fileName = Path.GetFileName(filePath).Replace("webp", "jpg");
                var bigfilePath = Path.Combine(Directory.GetCurrentDirectory(), "image", "downloaded_images", "1", fileName);
                if (File.Exists(bigfilePath))
                {
                    File.Copy(filePath, _wallpapersPath_current, true);
                    SetDesktopWallpaper(bigfilePath);
                    SetDesktopWallpaperAli_flag = false;
                    return;
                }
            }

            SetDesktopWallpaperAli_count = 0;
            AliConvertIamgeSize(filePath, di);
            if (Common.taskID == "")
            {
                //SetDesktopWallpaper(filePath);
                SetDesktopWallpaperAli_flag = false;
                return;
            }

            int i = 0;
            System.Timers.Timer timer = new() { Interval = 3000 };

            timer.Elapsed += (s, e) =>
            {
                i++;
                if (i < 4) { if (di) play_sound_di(); return; }
                if (GetTaskResultDownload(filePath, di))
                {
                    var fileName = Path.GetFileName(filePath).Replace("webp", "jpg");
                    var newpic = Path.Combine(Directory.GetCurrentDirectory(), "image", "downloaded_images", "1", fileName);
                    //ConvertAndResize(GetCurrentWallpaperPath(), _wallpapersPath_output);
                    SetDesktopWallpaper(newpic);
                    SetDesktopWallpaperAli_flag = false;
                    timer.Stop();
                    timer.Dispose();
                }
                if (i > 8)
                {
                    timer.Stop();
                    timer.Dispose();
                    SetDesktopWallpaperAli_flag = false;
                }
            };
            timer.AutoReset = true;
            timer.Start();
        }
        public static DateTime ali_image_success_time = DateTime.MinValue;
        public static bool GetTaskResultDownload(string filePath, bool di = true)
        {
            SetDesktopWallpaperAli_count++;
            if (SetDesktopWallpaperAli_count > 30)
            {
                log("AliConvertIamgeSize count > 30, return GetTaskResultDownload");
                return true;
            }
            var res = GetTaskResult(Common.taskID);
            var inlogtxt = (GetWindowTitle().Contains("log"));
            if (!inlogtxt && (res.Contains("SUCCEEDED") || res.Contains("RUNNING") || res.Contains("PENDING"))) { }
            else
                log(filePath + res);
            try
            {
                JObject jsonObject = JObject.Parse(res);
                JToken outputToken = jsonObject["output"];
                string taskId = outputToken["task_id"]?.ToString();
                string output_image_url = outputToken["output_image_url"]?.ToString();
                string state = outputToken["task_status"]?.ToString();

                if (taskId == null || taskId == "")
                {
                    //log("taskId == null || taskId == \"\"");
                    Common.taskID = "";
                    //TaskRun(() => { SetDesktopWallpaperAli(GetNextWallpaper(), WallpaperStyle.Fit); }, 1000);
                    if (di)
                        play_sound_di2();
                    UploadImage("https://bghuan.cn/api/saveimage", _wallpapersPath_temp);
                    return true;
                }
                if (state == "FAILED")
                {
                    //log("state == \"FAILED\"");
                    Common.taskID = "";
                    //TaskRun(() => { SetDesktopWallpaperAli(GetNextWallpaper(), WallpaperStyle.Fit); }, 1000);
                    if (di)
                        play_sound_di2();
                    UploadImage("https://bghuan.cn/api/saveimage", _wallpapersPath_temp);
                    return true;
                }

                var fileName = Path.GetFileName(filePath);
                fileName = fileName.Replace("webp", "jpg");
                var sdas = Path.Combine(Directory.GetCurrentDirectory(), "image", "downloaded_images", "1", fileName);

                DownloadImage(output_image_url, sdas);
                ali_image_success_time = DateTime.Now;
                //ConvertAndResize(sdas, sdas.Replace("jpg", "webp"));
                UploadImage("https://bghuan.cn/api/saveimage", _wallpapersPath_temp);
                if (di)
                    play_sound_bongocat(1);
                return true;
            }
            catch (Exception ex)
            {
                //UploadImage("https://bghuan.cn/api/saveimage", _wallpapersPath_temp);
            }
            if (di)
                play_sound_di();
            return false;
        }
        public static void AliConvertIamgeSize(string filePath, bool di)
        {
            SetDesktopWallpaperAli_count++;
            if (SetDesktopWallpaperAli_count > 30)
            {
                log("AliConvertIamgeSize count > 30, return AliConvertIamgeSize");
                return;
            }
            // 要上传的图片文件路径
            string imageFilePath = GetCurrentWallpaperPath();
            if (RemoveWebpWhiteBorder(GetCurrentWallpaperPath(), _wallpapersPath_no_border))
                imageFilePath = _wallpapersPath_no_border;

            // 获取屏幕尺寸（假设为主屏幕）
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;   // 例如：2560
            int screenHeight = Screen.PrimaryScreen.Bounds.Height; // 例如：1440

            // 获取原图尺寸
            var imageSize = GetWebpDimensions(imageFilePath);
            int imageWidth = imageSize.width;
            int imageHeight = imageSize.height;

            // 计算总扩展量（若原图大于屏幕，扩展量为0）
            int totalWidthToAdd = Math.Max(0, screenWidth - imageWidth);
            int totalHeightToAdd = Math.Max(0, screenHeight - imageHeight);

            // 分配扩展量到左右、上下（平均分配）
            int leftOffset = totalWidthToAdd / 2;       // 左扩展
            int rightOffset = totalWidthToAdd - leftOffset; // 右扩展（处理奇数情况）

            int topOffset = totalHeightToAdd / 2;       // 上扩展
            int bottomOffset = totalHeightToAdd - topOffset; // 下扩展（处理奇数情况）
            if (imageSize.width != 0 && (imageSize.width < 512 || imageSize.height < 512))
            {
                Common.taskID = "";
                TaskRun(() => { SetDesktopWallpaperAli(GetNextWallpaper(), di); }, 1000);
                if (di)
                    play_sound_di2();
                return;
            }

            string host = "https://bghuan.cn";
            string apiUrl = host + "/api/saveimage";
            var response = UploadImage(apiUrl, imageFilePath);

            if (response.Success)
            {
                var imageUrl = host + response.FilePath.Replace("..", "");

                ImageOutPaintingClient();
                string res = OutPaintImage(imageUrl, topOffset, bottomOffset, leftOffset, rightOffset);
                var inlogtxt = (GetWindowTitle().Contains("log"));
                if (!inlogtxt && (res.Contains("SUCCEEDED") || res.Contains("RUNNING") || res.Contains("PENDING"))) { }
                else
                    log(imageFilePath + res);

                JObject jsonObject = JObject.Parse(res);
                JToken outputToken = jsonObject["output"];
                if (outputToken == null)
                {
                    Common.taskID = "";
                    //TaskRun(() => { SetDesktopWallpaperAli(GetNextWallpaper(), WallpaperStyle.Fit); }, 1000);
                    if (di)
                        play_sound_di2();
                    UploadImage("https://bghuan.cn/api/saveimage", _wallpapersPath_temp);
                    return;
                }
                string state = outputToken["task_status"]?.ToString();
                if (state == "FAILED")
                {
                    Common.taskID = "";
                    //TaskRun(() => { SetDesktopWallpaperAli(GetNextWallpaper(), WallpaperStyle.Fit); }, 1000);
                    if (di)
                        play_sound_di2();
                    UploadImage("https://bghuan.cn/api/saveimage", _wallpapersPath_temp);
                    return;
                }

                string taskId = outputToken["task_id"]?.ToString();
                Common.taskID = taskId;

            }
            //TaskRun(() =>
            //{   
            //    var response2 = UploadImage("https://bghuan.cn/api/saveimage", _wallpapersPath_temp);
            //}, 20_000);
        }

        public static string taskID = "";

        private static HttpClient _httpClient;
        private static string _apiKey;
        private static string _apiUrl = "https://dashscope.aliyuncs.com/api/v1/services/aigc/image2image/out-painting";

        //public static string apiKey = "sk-6d*************58";
        public static void ImageOutPaintingClient()
        {
            _apiKey = ConfigValue(ConfigApiKey);
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _httpClient.DefaultRequestHeaders.Add("X-DashScope-Async", "enable");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        /// <summary>
        /// 查询异步任务结果（同步版本）
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>任务结果</returns>
        public static string GetTaskResult(string taskId)
        {
            HttpClient _httpClient = new HttpClient();
            var apiKey = ConfigValue(ConfigApiKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            string resultUrl = $"https://dashscope.aliyuncs.com/api/v1/tasks/{taskId}";
            HttpResponseMessage response = _httpClient.GetAsync(resultUrl).Result;

            //response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        } /// <summary>
          /// 下载网络图片并保存到本地
          /// </summary>
          /// <param name="imageUrl">图片URL</param>
          /// <param name="localPath">本地保存路径</param>
        public static void DownloadImage(string imageUrl, string localPath)
        {
            try
            {
                HttpClient _httpClient = new HttpClient();
                // 发送HTTP请求获取图片内容
                using (HttpResponseMessage response = _httpClient.GetAsync(imageUrl).Result)
                {
                    // 检查响应状态
                    response.EnsureSuccessStatusCode();

                    // 获取图片内容流
                    using (Stream contentStream = response.Content.ReadAsStreamAsync().Result)
                    using (FileStream fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write))
                    {
                        // 复制流到文件
                        contentStream.CopyTo(fileStream);
                    }
                }

                Console.WriteLine($"图片已成功保存到: {localPath}");
            }
            catch (AggregateException ex)
            {
                // 处理HttpClient异步操作的异常
                Console.WriteLine($"下载图片时出错: {ex.InnerException?.Message ?? ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生未知错误: {ex.Message}");
                throw;
            }
        }
        public static string OutPaintImage(string imageUrl, int q, int w, int e, int r)
        {
            try
            {
                // 构建请求内容
                var requestBody = new
                {
                    model = "image-out-painting",
                    input = new
                    {
                        image_url = imageUrl
                    },
                    parameters = new
                    {
                        //top_offset = q,
                        //bottom_offset = w,
                        //left_offset = e,
                        //right_offset = r,
                        output_ratio = "16:9",
                        add_watermark = false,
                    }
                };

                // 序列化为JSON
                string jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // 发送请求
                HttpResponseMessage response = _httpClient.PostAsync(_apiUrl, content).Result;

                // 检查响应状态
                //response.EnsureSuccessStatusCode();

                // 返回响应内容
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"调用API时出错: {ex.Message}");
                if (ex is AggregateException aggregateEx && aggregateEx.InnerException != null)
                {
                    Console.WriteLine($"内部异常: {aggregateEx.InnerException.Message}");
                    if (aggregateEx.InnerException is HttpRequestException httpEx)
                    {
                        Console.WriteLine($"HTTP状态码: {httpEx.StatusCode}");
                    }
                }
                throw;
            }
        }
        public static ApiResponse UploadImage(string apiUrl, string imageFilePath)
        {
            // 生成随机边界字符串
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            // 创建HTTP请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            // 获取文件信息
            FileInfo fileInfo = new FileInfo(imageFilePath);
            string fileName = fileInfo.Name;
            string contentType = "image/jpeg"; // 可以根据扩展名自动判断

            // 设置请求内容长度
            long contentLength = 0;
            contentLength += boundaryBytes.Length;

            // 文件内容部分
            string fileHeader =
                "Content-Disposition: form-data; name=\"image\"; filename=\"" + fileName + "\"\r\n" +
                "Content-Type: " + contentType + "\r\n\r\n";
            byte[] fileHeaderBytes = Encoding.UTF8.GetBytes(fileHeader);
            contentLength += fileHeaderBytes.Length;
            contentLength += fileInfo.Length;
            contentLength += endBoundaryBytes.Length;

            request.ContentLength = contentLength;

            // 写入请求内容
            using (Stream requestStream = request.GetRequestStream())
            {
                // 写入边界
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);

                // 写入文件头部
                requestStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);

                // 写入文件内容
                using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                    }
                }

                // 写入结束边界
                requestStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            }

            // 获取响应
            using (WebResponse response = request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string jsonResponse = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);
            }
        }
        public class ApiResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public string FilePath { get; set; }
        }
    }
}
