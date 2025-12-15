namespace Socket_Sender;
using System;
using System.Net.Sockets;
using System.Text;

public class FirstSender
{
    public FirstSender(string[] args)
    {
        string serverIp = "127.0.0.1";
        int port = 2025;

        using TcpClient client = new TcpClient();
        client.Connect(serverIp, port);
        Console.WriteLine(client.Client.LocalEndPoint);
        using NetworkStream stream = client.GetStream();


        StreamReader sr = new StreamReader(stream);
        StreamWriter sw = new StreamWriter(stream);
        sw.AutoFlush = true;
        sw.WriteLine(args[0]);
        string response = sr.ReadLine();
        
        Console.WriteLine("Message: " + response);
        
        stream.Close();
        client.Close();
    }
}