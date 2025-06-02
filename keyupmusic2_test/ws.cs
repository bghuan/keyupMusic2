using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ScreenVideoSharingServer 
{
    public static TcpListener listener;
    public static TcpClient client2;
    public static NetworkStream stream;

    private const string Hostname = "127.0.0.1";
    private const int pport = 13000;
    private const string WebSocketUrl = "wss://bghuan.cn/ws3";

    private ClientWebSocket client;
    private CancellationTokenSource cts;
    private Task receiveTask;

    public ScreenVideoSharingServer()
    {
        client = new ClientWebSocket();
        cts = new CancellationTokenSource();

        try
        {
            // 异步连接
            client.ConnectAsync(new Uri(WebSocketUrl), cts.Token).Wait();
            Console.WriteLine("WebSocket 连接成功");

            // 启动异步接收任务
            receiveTask = ReceiveMessagesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"连接错误: {ex.Message}");
            throw;
        }
    }

    public void socket_write(string msg)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(msg);
        SendAsync(buffer, WebSocketMessageType.Text).Wait();
    }

    public void socket_write(byte[] data)
    {
        SendAsync(data, WebSocketMessageType.Binary).Wait();
    }

    private async Task SendAsync(byte[] data, WebSocketMessageType messageType)
    {
        try
        {
            if (client.State == WebSocketState.Open)
            {
                await client.SendAsync(
                    new ArraySegment<byte>(data),
                    messageType,
                    true,
                    cts.Token
                );
            }
            else
            {
                Console.WriteLine($"WebSocket 状态异常: {client.State}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发送消息失败: {ex.Message}");
        }
    }

    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[4096];

        try
        {
            while (client.State == WebSocketState.Open && !cts.IsCancellationRequested)
            {
                var result = await client.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    cts.Token
                );

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("收到关闭消息");
                    await client.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        string.Empty,
                        cts.Token
                    );
                    break;
                }
                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"收到文本消息: {message}");
                    OnMessageReceived(message);
                }
                else if (result.MessageType == WebSocketMessageType.Binary)
                {
                    var binaryData = new byte[result.Count];
                    Array.Copy(buffer, binaryData, result.Count);
                    Console.WriteLine($"收到二进制消息，长度: {result.Count}");
                    OnBinaryMessageReceived(binaryData);
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("接收任务被取消");
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine($"WebSocket 错误: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"接收消息时发生意外错误: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("接收任务已停止");
        }
    }

    // 文本消息处理（可重写）
    protected virtual void OnMessageReceived(string message)
    {
        Console.WriteLine(message);
    }

    // 二进制消息处理（可重写）
    protected virtual void OnBinaryMessageReceived(byte[] data)
    {
        // 默认实现，可由子类重写
    }
}