using System.Text;
using web_factory.classes;

namespace web_factory.Factories;

public abstract class ResponseFactory
{
    // The "Factory Method"
    public abstract HttpResponse CreateResponse(string filePath);
}

// Factory for Text files
public class TextResponseFactory : ResponseFactory
{
    public override HttpResponse CreateResponse(string filePath)
    {
        return new HttpResponse
        {
            ContentType = "text/plain",
            Body = File.ReadAllBytes(filePath)
        };
    }
}

// Factory for PDF files
public class PdfResponseFactory : ResponseFactory
{
    public override HttpResponse CreateResponse(string filePath)
    {
        return new HttpResponse
        {
            ContentType = "application/pdf",
            Body = File.ReadAllBytes(filePath)
        };
    }
}

// Factory for 404 Errors
public class NotFoundResponseFactory : ResponseFactory
{
    public override HttpResponse CreateResponse(string filePath)
    {
        return new HttpResponse
        {
            StatusCode = "404 Not Found",
            ContentType = "text/plain",
            Body = Encoding.UTF8.GetBytes("File Not Found")
        };
    }
}