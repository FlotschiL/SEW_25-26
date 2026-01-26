using System.Text;

namespace web_factory.classes;

public class HttpResponse
{
    public string StatusCode { get; set; } = "200 OK";
    public string ContentType { get; set; } = "text/plain";
    public byte[] Body { get; set; } = Array.Empty<byte>();

    public byte[] GetFullResponseBytes()
    {
        string header = $"HTTP/1.1 {StatusCode}\r\n" +
                        $"Content-Type: {ContentType}\r\n" +
                        $"Content-Length: {Body.Length}\r\n" +
                        "\r\n";

        byte[] headerBytes = Encoding.UTF8.GetBytes(header);
        byte[] combined = new byte[headerBytes.Length + Body.Length];
        
        Buffer.BlockCopy(headerBytes, 0, combined, 0, headerBytes.Length);
        Buffer.BlockCopy(Body, 0, combined, headerBytes.Length, Body.Length);
        
        return combined;
    }
}