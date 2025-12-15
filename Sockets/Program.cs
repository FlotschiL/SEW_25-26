using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpServer
{
    static void Main()
    {
        int port = 5000;
        TcpListener listener = new TcpListener(IPAddress.Any, port);

        listener.Start();
        Console.WriteLine($"Server listening on port {port}...");

        using TcpClient client = listener.AcceptTcpClient();
        Console.WriteLine("Client connected.");

        using NetworkStream stream = client.GetStream();

        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {received}");
        }

        Console.WriteLine("Client disconnected.");
        listener.Stop();
    }
}