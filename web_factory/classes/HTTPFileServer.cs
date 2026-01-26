namespace web_factory.classes;

using System.Net;
using System.Net.Sockets;
using web_factory.classes;
using web_factory.Factories;

public class HTTPFileServer
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
            using StreamReader sr = new StreamReader(stream);

            string requestLine = sr.ReadLine();
            if (string.IsNullOrEmpty(requestLine)) continue;

            // Extract path: GET /file.pdf HTTP/1.1 -> file.pdf
            string[] parts = requestLine.Split(' ');
            string requestedFile = "wwwroot/" + parts[1].TrimStart('/');
            Console.WriteLine(File.Exists(requestedFile));
            Console.WriteLine(requestedFile);
            // Select the appropriate factory
            ResponseFactory factory = GetFactory(requestedFile);
            
            // Create the response and send it
            HttpResponse response = factory.CreateResponse(requestedFile);
            byte[] fullData = response.GetFullResponseBytes();
            
            stream.Write(fullData, 0, fullData.Length);
            socket.Close();
        }
    }

    private ResponseFactory GetFactory(string filePath)
    {
        if (!File.Exists(filePath)) 
            return new NotFoundResponseFactory();

        string extension = Path.GetExtension(filePath).ToLower();

        return extension switch
        {
            ".txt" => new TextResponseFactory(),
            ".pdf" => new PdfResponseFactory(),
            _ => new TextResponseFactory() // Default
        };
    }
}