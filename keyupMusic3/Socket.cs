using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace keyupMusic3
{
    public class TcpServer
    {
        static TcpClient client;
        static NetworkStream stream;
        public static void StartServer()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 13000);
                stream = client.GetStream();
            }
            catch (Exception) { }
        }
        public static void socket_write(string msg)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(msg);

                stream.Write(data, 0, data.Length);
            }
            catch (Exception) { }
        }
    }
}