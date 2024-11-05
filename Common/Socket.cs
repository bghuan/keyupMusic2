using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace keyupMusic2
{
    public class TcpServer
    {
        private const string Hostname = "127.0.0.1";
        public static Huan huan;

        static TcpListener listener;
        static TcpClient client;
        static NetworkStream stream;
        static int restart_times = 0;
        public TcpServer(Form parentForm)
        {
            huan = (Huan)parentForm;
            StartServer();
        }
        public static void StartServer(int port = 13000)
        {
            Task.Run(() =>
            {
                try
                {
                    listener = new TcpListener(IPAddress.Any, port);
                    listener.Start();

                    while (true)
                    {
                        client = listener.AcceptTcpClient();
                        stream = client.GetStream();
                        _StartServer(port);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("服务器启动错误：" + ex.Message);
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
                    while ((i = stream.Read(bytes, 0, bytes.Length)) > 0||15>4)
                    {
                        string dataReceived = Encoding.UTF8.GetString(bytes, 0, i);
                        Invoke(dataReceived);
                    }
                }
                catch (Exception ex)
                {
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
                Thread.Sleep(100);
            }
        }
        public static void Invoke(string msg)
        {
            huan.Invoke(() => { huan.label2.Text = msg; });
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
                        client = new TcpClient(Hostname, 13000);
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
            if ((now.Subtract(lastSendTime).TotalMilliseconds > 100 || lastSendTime == DateTime.MinValue)&& txt!= lastMessageToSend)
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