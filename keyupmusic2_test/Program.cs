using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    private const string Hostname = "127.0.0.1";

    static TcpListener listener;
    static TcpClient client;
    static NetworkStream stream;
    static void Main()
    {
        socket_write("AA");
    }
    public static void socket_write(string msg)
    {
        if (string.IsNullOrEmpty(msg))
            return;
        if (client == null || stream == null || client.Connected == false)
        {
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