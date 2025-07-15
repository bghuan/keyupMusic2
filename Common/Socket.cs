using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static keyupMusic2.Common;

namespace keyupMusic2
{
    public class Socket
    {
        private const string Hostname = "127.0.0.1";
        private const int pport = 13000;

        public static TcpListener listener;
        public static TcpClient client;
        public static NetworkStream stream;
        public static bool socket_run = true;
        public Socket()
        {
            if (!socket_run) return;
            StartServer();
        }
        public static void StartServer(int port = pport)
        {
            Task.Run(() =>
            {
                try
                {
                    using (listener = new TcpListener(IPAddress.Any, port))
                    {
                        listener.Start();
                        while (true)
                        {
                            client = listener.AcceptTcpClient();
                            stream = client.GetStream();

                            byte[] buffer = new byte[1024];
                            int i = stream.Read(buffer, 0, buffer.Length);
                            string request = Encoding.UTF8.GetString(buffer, 0, i);
                            resp(request);

                            // 解析请求并获取查询参数
                            var queryParams = ParseQueryParameters(request);
                            if (!string.IsNullOrEmpty(queryParams))
                                request = queryParams;
                            else
                            {
                                int idx = request.IndexOf("\r\n\r\n");
                                if (idx >= 0) request = request.Substring(idx + 4);
                            }

                            handle(request);
                            client.Dispose();
                            stream.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }
        private static string ParseQueryParameters(string request)
        {
            var parameters = "";

            // 提取请求行（第一行）
            int endOfFirstLine = request.IndexOf("\r\n");
            if (endOfFirstLine <= 0)
                return parameters;

            string firstLine = request.Substring(0, endOfFirstLine);
            // 第一行格式: "GET /?a=123 HTTP/1.1"
            string[] parts = firstLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                return parameters;

            // 获取URL部分（如 "/?a=123"）
            string url = parts[1];

            // 提取查询字符串（?后面的部分）
            int queryStart = url.IndexOf('?');
            if (queryStart == -1)
                return parameters;

            string queryString = url.Substring(queryStart + 1);

            return queryString;
        }
        private static void resp(string request)
        {
            string[] lines = request.Split(new[] { "\r\n" }, StringSplitOptions.None);
            string methodLine = lines[0];
            string method = methodLine.Split(' ')[0];

            string responseContent = "";
            string httpResponse = "";

            if (method == "OPTIONS")
            {
                // 预检请求响应
                httpResponse =
                    "HTTP/1.1 204 No Content\r\n" +
                    "Access-Control-Allow-Origin: *\r\n" +
                    "Access-Control-Allow-Methods: POST, GET, OPTIONS\r\n" +
                    "Access-Control-Allow-Headers: Content-Type\r\n" +
                    "Access-Control-Max-Age: 86400\r\n" +
                    "\r\n";
            }
            else
            {
                // 实际请求处理
                responseContent = "yo";

                httpResponse =
                    "HTTP/1.1 200 OK\r\n" +
                    "Access-Control-Allow-Origin: *\r\n" +
                    "Content-Type: text/plain\r\n" +
                    $"Content-Length: {Encoding.UTF8.GetByteCount(responseContent)}\r\n" +
                    "\r\n" +
                    responseContent;
            }

            byte[] responseBuffer = Encoding.UTF8.GetBytes(httpResponse);
            stream.Write(responseBuffer, 0, responseBuffer.Length);
        }


        public static void handle(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                Task.Run(() =>
                {
                    try
                    {
                        if (msg.Contains(Huan.start_check_str) || msg.Contains(Huan.start_check_str2))
                            huan.start_catch(msg);
                        else if (msg.StartsWith(Huan.start_next))
                            huan.next_catch(msg);
                        else if (msg.StartsWith(Huan.huan_invoke))
                            huan.Invoke(() => { huan.label1.Text = msg.Substring(11); });
                        else if (msg.StartsWith(download_image_prix))
                            download_image(msg);
                        //else if (msg.Contains(OpencvReceive.opencvstr))
                        //    huan.Opencv.deal(msg);
                        else if (Band.band_handle(msg))
                            Band.Button1(msg);
                        //huan.Invoke(() => { huan.label1.Text = msg; });
                    }
                    catch (Exception ex)
                    {
                        log("Socket handle error: " + ex.Message);
                    }
                });
            }
        }
        public static void socket_write(string msg)
        {
            using (var client = new TcpClient(Hostname, pport))
            using (var stream = client.GetStream())
            {
                byte[] data = Encoding.UTF8.GetBytes(msg);
                stream.Write(data, 0, data.Length);
            }
        }
    }

}