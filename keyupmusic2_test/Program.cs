using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace P2PCommunication
{
    class Program
    {
        public static TcpListener listener;
        public static TcpClient client2;
        public static NetworkStream stream;
        private const string Hostname = "127.0.0.1";
        private const int pport = 13000;
        private const string WebSocketUrl = "wss://bghuan.cn/ws";
        // 中继服务器配置
        private const string RelayServerUrl = "wss://bghuan.cn/ws3"; // 替换为实际中继服务器地址
        private const int TcpPort = 5000; // 点对点通信使用的 TCP 端口
        ScreenVideoSharingServer server;

        static void Main(string[] args)
        {
            new Program().StartTargetHost();
        }


        // 目标主机逻辑（接收消息）
        void StartTargetHost()
        {
            string serverId = "Server1"; 

             server = new ScreenVideoSharingServer() {};

            Console.WriteLine($"目标主机 {serverId} 已连接到中继服务器");

            StartTcpListener();

            Console.WriteLine("按 Enter 退出...");
            Console.ReadLine();
        }

        //// 向中继服务器注册
        //void RegisterWithRelay(string clientId)
        //{
        //    string publicIp = GetPublicIp(); // 获取公网 IP
        //    string message = $"{{\"type\":\"register\",\"id\":\"{clientId}\",\"address\":\"{publicIp}:{TcpPort}\"}}";
        //    //ws.Send(message);
        //    server.socket_write(message);
        //    Console.WriteLine($"已向中继服务器注册: {message}");
        //}

        // 处理从中继服务器收到的消息
        void HandleRelayMessage(string message, string clientId, string targetId)
        {
            try
            {
                dynamic msg = Newtonsoft.Json.JsonConvert.DeserializeObject(message);

                if (msg.type == "success")
                {
                    Console.WriteLine($"注册成功: {msg.message}");

                    // 如果是客户端，发送请求获取目标主机地址
                    if (targetId != null)
                    {
                        string request = $"{{\"type\":\"request\",\"targetId\":\"{targetId}\"}}";
                        server.socket_write(request);
                        Console.WriteLine($"已请求目标主机 {targetId} 的地址");
                    }
                }
                else if (msg.type == "targetInfo")
                {
                    string targetAddress = msg.address;
                    Console.WriteLine($"获取到目标主机地址: {targetAddress}");

                    // 尝试建立点对点连接
                    TryConnectToTarget(targetAddress);
                }
                else if (msg.type == "connectionRequest")
                {
                    string fromId = msg.from;
                    Console.WriteLine($"收到来自 {fromId} 的连接请求");
                }
                else if (msg.type == "forwarded")
                {
                    string fromId = msg.from;
                    string data = msg.data;
                    Console.WriteLine($"收到来自 {fromId} 的转发消息: {data}");
                }
                else if (msg.type == "error")
                {
                    Console.WriteLine($"中继服务器错误: {msg.message}");
                }
                else
                {
                    Console.WriteLine($"未知消息类型: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理消息时出错: {ex.Message}");
            }
        }

        // 尝试连接到目标主机
        void TryConnectToTarget(string targetAddress)
        {
            try
            {
                string[] parts = targetAddress.Split(':');
                string ip = parts[0];
                int port = int.Parse(parts[1]);

                using var tcpClient = new TcpClient();
                Console.WriteLine($"尝试连接到目标主机: {targetAddress}");
                tcpClient.Connect(IPAddress.Parse(ip), port);
                Console.WriteLine("点对点连接成功！");

                // 发送消息
                using var stream = tcpClient.GetStream();
                byte[] data = Encoding.UTF8.GetBytes("hello");
                stream.Write(data, 0, data.Length);
                Console.WriteLine("已发送消息: hello");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接目标主机失败: {ex.Message}");

                // 这里可以添加重试逻辑或回退到中继转发模式
            }
        }

        // 启动 TCP 监听器
        void StartTcpListener()
        {
            Task.Run(() =>
            {
                try
                {
                    using var listener = new TcpListener(IPAddress.Any, TcpPort);
                    listener.Start();
                    Console.WriteLine($"TCP 监听器已启动，端口: {TcpPort}");

                    while (true)
                    {
                        using var client = listener.AcceptTcpClient();
                        Console.WriteLine("收到新的点对点连接");

                        using var stream = client.GetStream();
                        byte[] buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"收到消息: {message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"TCP 监听错误: {ex.Message}");
                }
            });
        }

        // 获取公网 IP
        string GetPublicIp()
        {
            try
            {
                using var client = new WebClient();
                return client.DownloadString("http://icanhazip.com").Trim();
            }
            catch (Exception)
            {
                // 如果无法获取公网 IP，使用本地 IP
                return Dns.GetHostEntry(Dns.GetHostName())
                    .AddressList
                    .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)?
                    .ToString() ?? "127.0.0.1";
            }
        }
    }
}