using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

public class SimpleHttpServer
{
    public void Start()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 2025);
        listener.Start();
        Console.WriteLine("Server started on port 2025...");

        while (true)
        {
            using Socket socket = listener.AcceptSocket();
            using NetworkStream stream = new NetworkStream(socket);
            using StreamWriter sw = new StreamWriter(stream) { AutoFlush = true };
            using StreamReader sr = new StreamReader(stream);

            // 1. Read the request (e.g., "GET / HTTP/1.1")
            string request = sr.ReadLine();
            Console.WriteLine($"Request: {request}");

            // 2. Prepare the content
            string body = "<html><body><h1>Hello from C#</h1></body></html>";

            // 3. Construct and send the HTTP Response string
            // The format MUST be: Status line + Headers + Double Newline + Body
            sw.Write("HTTP/1.1 200 OK\r\n");
            sw.Write("Content-Type: text/html\r\n");
            sw.Write($"Content-Length: {Encoding.UTF8.GetByteCount(body)}\r\n");
            sw.Write("Connection: close\r\n");
            sw.Write("\r\n"); // The critical empty line
            sw.Write(body);   // The actual data

            socket.Close();
        }
    }
}