using Sockets;

class TcpServer
{
    public static void Main(string[] args)
    {
        //FirstSocket s = new FirstSocket();
        //valid browser HTTP
        //SimpleHttpServer server = new SimpleHttpServer();
        //server.Start();
        //HTTPFileServer httpServer = new HTTPFileServer();
        //httpServer.Start();
        SimpleImageServer server = new SimpleImageServer();
        server.Start();
        
    }
}