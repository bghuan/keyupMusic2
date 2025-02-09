using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace keyupMusic2
{
    public class TcpServer
    {
        private const string Hostname = "127.0.0.1";
        private const int pport = 13000;
        public static Huan huan;

        public static TcpListener listener;
        public static TcpClient client;
        public static NetworkStream stream;
        static int restart_times = 0;
        public TcpServer(Form parentForm)
        {
            huan = (Huan)parentForm;
            StartServer();
        }
        private static int retryCount = 0;
        private const int maxRetries = 5; // 最大重试次数

        public static void StartServer(int port = pport)
        {
            Task.Run(() =>
            {
                try
                {
                    using (listener = new TcpListener(IPAddress.Any, port))
                    {
                        listener.Start();
                        retryCount = 0; // 重置重试次数

                        while (true)
                        {
                            client = listener.AcceptTcpClient();
                            stream = client.GetStream();
                            _StartServer(port);
                        }
                    }
                }
                catch (Exception ex)
                {
                    retryCount++;
                    if (retryCount <= maxRetries)
                    {
                        //huan.Invoke(() => { huan.label1.Text = $"服务器启动错误：{ex.Message}，重试中 ({retryCount}/{maxRetries})"; });
                        Console.WriteLine($"服务器启动错误：{ex.Message}，重试中 ({retryCount}/{maxRetries})");

                        Thread.Sleep(1000);
                        StartServer(port); // 递归调用以重试
                    }
                    else
                    {
                        huan.Invoke(() => { huan.label1.Text = "服务器启动失败，已达到最大重试次数。"; });
                        Console.WriteLine("服务器启动失败，已达到最大重试次数。");
                    }
                }
            });
        }

        public static void _StartServer(int port)
        {
            while (true)
            {
                try
                {
                    if (stream == null || !stream.CanRead)
                    {
                        break;
                    }

                    byte[] bytes = new byte[256];
                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) > 0)
                    {
                        string dataReceived = Encoding.UTF8.GetString(bytes, 0, i);
                        Invoke(dataReceived);
                    }
                }
                catch (Exception ex)
                {
                    huan.Invoke(() => { huan.label1.Text = "读取数据时发生错误：" + ex.Message; });
                    Console.WriteLine("读取数据时发生错误：" + ex.Message);
                    if (ex.Message.Contains("远程主机强迫关闭了一个现有的连接。"))
                    {
                        // 尝试重新建立连接
                        client = null;
                        stream = null;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                    }
                    if (client != null)
                    {
                        client.Dispose();
                    }
                }
                Thread.Sleep(10);
            }
        }
        public static void Invoke(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                if (Band.band_handle(msg)) { Band.Button1(msg); return; }
                huan.Invoke(() => { huan.label1.Text = msg; });
            }
        }
        private static DateTime lastSendTime = DateTime.MinValue;
        public static string lastMessageToSend;
        public static void socket_write(string msg)
        {
            Task.Run(() =>
            {
                if (restart_times > 10)
                    return;
                //if (string.IsNullOrEmpty(msg))
                //    return;
                try
                {
                    if (client == null || stream == null || client.Connected == false)
                    {
                        restart_times++;
                        client = new TcpClient(Hostname, pport);
                        stream = client.GetStream();
                        Thread.Sleep(1000);
                    }
                    lastMessageToSend = msg;

                    var now = DateTime.Now;
                    if (now.Subtract(lastSendTime).TotalMilliseconds < 100)
                        return;

                    byte[] data = Encoding.UTF8.GetBytes(msg);
                    stream.Write(data, 0, data.Length);
                    lastSendTime = now;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("发送数据时发生错误：" + ex.Message);
                }
            });
        }
        public static void CheckAndSendPendingMessage(string txt)
        {
            var now = DateTime.Now;
            if ((now.Subtract(lastSendTime).TotalMilliseconds > 100 || lastSendTime == DateTime.MinValue) && txt != lastMessageToSend)
            {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(lastMessageToSend);
                    stream.Write(data, 0, data.Length);
                    lastSendTime = now;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("发送数据时发生错误：" + ex.Message);
                }
            }
            //else if (stream != null)
            //{
            //    try
            //    {
            //        string asd = "+";
            //        if (last_) asd = "_";
            //        last_ = !last_;
            //        byte[] data = Encoding.UTF8.GetBytes(asd);
            //        stream.Write(data, 0, data.Length);
            //        lastSendTime = now;
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("发送数据时发生错误：" + ex.Message);
            //    }

            //}
        }
        static bool last_ = false;
    }
}