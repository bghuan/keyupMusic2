﻿using System;
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

                            int idx = request.IndexOf("\r\n\r\n");
                            if (idx >= 0) request = request.Substring(idx + 4);

                            Invoke(request);
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

        private static void resp(string request)
        {
            // 处理请求并生成响应
            string responseContent = "yo";

            // 构建HTTP响应
            string httpResponse = $"HTTP/1.1 200 OK\r\n" +
                                  $"Access-Control-Allow-Origin: *\r\n" +
                                  $"Content-Type: text/plain\r\n" +
                                  $"Content-Length: {responseContent.Length}\r\n" +
                                  $"\r\n" +
                                  $"{responseContent}";

            // 发送响应
            byte[] responseBuffer = Encoding.UTF8.GetBytes(httpResponse);
            stream.Write(responseBuffer, 0, responseBuffer.Length);
        }

        public static void Invoke(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                if (msg.Contains(Huan.start_check_str) || msg.Contains(Huan.start_check_str2))
                    huan.start_catch(msg);
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