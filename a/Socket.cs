using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace keyupMusic2
{
    public class TcpServer
    {
        public Huan huan;
        public TcpServer(Form parentForm)
        {
            huan = (Huan)parentForm;
        }
        public void StartServer(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Server started on port " + port);

            while (true)
            {
                // 等待客户端连接  
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected!");

                // 获取流对象  
                NetworkStream stream = client.GetStream();

                // 读取客户端发送的数据  
                byte[] bytes = new byte[256];
                int i = stream.Read(bytes, 0, bytes.Length);
                string dataReceived = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", dataReceived);

                Invoke(() => { huan.label1.Text = dataReceived; });
            }
        }
        public void Invoke(Action action)
        {
            huan.Invoke(action);
        }
    }
}