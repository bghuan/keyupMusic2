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
                            byte[] bytes = new byte[256];
                            int i;
                            while ((i = stream.Read(bytes, 0, bytes.Length)) > 0)
                            {
                                string dataReceived = Encoding.UTF8.GetString(bytes, 0, i);
                                //Log.log(dataReceived);
                                Invoke(dataReceived);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                }
            });
        }

        public static void Invoke(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                if (msg.Contains(Huan.start_check_str) || msg.Contains(Huan.start_check_str2))
                    huan.start_catch(msg);
                else if (msg.StartsWith(Huan.huan_invoke))
                    huan.Invoke(() => { huan.label1.Text = msg.Substring(11); });
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