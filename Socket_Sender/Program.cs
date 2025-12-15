using System;
using System.Net.Sockets;
using System.Text;

class TcpClientApp
{
    static void Main(string[] args)
    {
        string serverIp = "127.0.0.1";
        int port = 5000;

        using TcpClient client = new TcpClient();
        client.Connect(serverIp, port);

        using NetworkStream stream = client.GetStream();

        string message = args[0];
        byte[] data = Encoding.UTF8.GetBytes(message);

        stream.Write(data, 0, data.Length);
        Console.WriteLine("Message sent.");

        stream.Close();
        client.Close();
    }
}