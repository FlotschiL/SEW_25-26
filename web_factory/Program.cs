using web_factory.classes;

class Program
{
    static void Main(string[] args)
    {
        HTTPFileServer server = new HTTPFileServer();
        server.Start();
    }
}