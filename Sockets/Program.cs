using Sockets;

class TcpServer
{
    public static void Main(string[] args)
    {
        //FirstSocket s = new FirstSocket();
        SimpleHttpServer server = new SimpleHttpServer();
        server.Start();
    }
}