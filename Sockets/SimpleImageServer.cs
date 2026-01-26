using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class SimpleImageServer
{
    public void Start()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 2025);
        listener.Start();
        Console.WriteLine("Server started. Navigate to /image.jpg or /a.txt");

        while (true)
        {
            using Socket socket = listener.AcceptSocket();
            using NetworkStream stream = new NetworkStream(socket);
            using StreamReader sr = new StreamReader(stream);

            // 1. Get the requested file name
            string requestLine = sr.ReadLine();
            if (string.IsNullOrEmpty(requestLine)) continue;
            string fileName = requestLine.Split(' ')[1].TrimStart('/');

            if (File.Exists(fileName))
            {
                // 2. Read the file as raw bytes (works for images AND text)
                byte[] fileBytes = File.ReadAllBytes(fileName);
                string extension = Path.GetExtension(fileName).ToLower();

                // 3. Determine the "MIME Type" so the browser knows what it is
                string contentType = extension switch {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    _ => "text/plain"
                };

                // 4. Send Header as text
                StreamWriter sw = new StreamWriter(stream);
                sw.Write("HTTP/1.1 200 OK\r\n");
                sw.Write($"Content-Type: {contentType}\r\n");
                sw.Write($"Content-Length: {fileBytes.Length}\r\n");
                sw.Write("\r\n"); 
                sw.Flush(); // Push headers out before sending binary data

                // 5. Send Body as raw bytes
                stream.Write(fileBytes, 0, fileBytes.Length);
            }
            
            socket.Close();
        }
    }
}