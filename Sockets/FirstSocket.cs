using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Socket_Sender;

public class FirstSocket
{
    public TcpListener listener;
    public FirstSocket()
    {
        int port = 2025;
        listener = new TcpListener(IPAddress.Loopback, port);

        listener.Start();
        Console.WriteLine($"Server listening on port {port}...");
        int count = 5;
        for (int i = 0; i < count; i++)
        {
            new Thread(() => Service()).Start();
        }

    }

    public void Service()//echo socket
    {
        while (true)
        {
            Socket socket = listener.AcceptSocket();
            Console.WriteLine($"Connected: {socket.RemoteEndPoint}");
            Stream stream = new NetworkStream(socket);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream);
            sw.AutoFlush = true;
            string input = sr.ReadLine();
            Console.WriteLine($"Client requested: {input}");
            sw.WriteLine(input.ToUpper());
            socket.Close();
        }
    }
}