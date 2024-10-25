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
        }
        public static void StartServer(int port = 13000)
        {

            Task.Run(() =>
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                client = listener.AcceptTcpClient();
                stream = client.GetStream();

                _StartServer(port);
            });
        }

        public static void _StartServer(int port)
        {
            while (true)
            {
                try
                {
                    byte[] bytes = new byte[256];
                    int i = stream.Read(bytes, 0, bytes.Length);
                    if (i > 0)
                    {
                        string dataReceived = Encoding.ASCII.GetString(bytes, 0, i);
                        Invoke(dataReceived);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("读取数据时发生错误：" + ex.Message);
                    break;
                }
                Thread.Sleep(500);
            }
        }
        public static void Invoke(string msg)
        {
            huan.Invoke(() => { huan.label1.Text = msg; });
        }
        public static void socket_write(string msg)
        {
            if (restart_times > 10)
                return;
            if (string.IsNullOrEmpty(msg))
                return;
            if (client == null || stream == null || client.Connected == false)
            {
                restart_times++;
                client = new TcpClient(Hostname, 13000);
                stream = client.GetStream();
            }
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(msg);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送数据时发生错误：" + ex.Message);
            }
        }
    }
}