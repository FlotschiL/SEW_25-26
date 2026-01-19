using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

public class HTTPFileServer
{
    public void Start()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 2025);
        listener.Start();
        Console.WriteLine("Server started. Listening for /a.txt or /b.txt...");

        while (true)
        {
            using Socket socket = listener.AcceptSocket();
            using NetworkStream stream = new NetworkStream(socket);
            using StreamWriter sw = new StreamWriter(stream) { AutoFlush = true };
            using StreamReader sr = new StreamReader(stream);

            // 1. Read the first line: "GET /a.txt HTTP/1.1"
            string requestLine = sr.ReadLine();
            if (string.IsNullOrEmpty(requestLine)) continue;

            // 2. Extract the path (the middle part of the string)
            string[] parts = requestLine.Split(' ');
            string requestedFile = parts[1].TrimStart('/'); // Removes the leading "/"

            string responseHeader;
            string responseBody;

            // 3. Logic: Only allow a.txt or b.txt
            if ((requestedFile == "a.txt" || requestedFile == "b.txt") && File.Exists(requestedFile))
            {
                responseBody = File.ReadAllText(requestedFile);
                responseHeader = "HTTP/1.1 200 OK\r\n";
            }
            else
            {
                responseBody = "File Not Found";
                responseHeader = "HTTP/1.1 404 Not Found\r\n";
            }

            // 4. Send the HTTP Response
            sw.Write(responseHeader);
            sw.Write("Content-Type: text/plain\r\n");
            sw.Write($"Content-Length: {Encoding.UTF8.GetByteCount(responseBody)}\r\n");
            sw.Write("\r\n"); // The critical empty line
            sw.Write(responseBody);

            socket.Close();
        }
    }
}