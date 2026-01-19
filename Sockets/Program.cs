using Sockets;

class TcpServer
{
    public static void Main(string[] args)
    {
        //FirstSocket s = new FirstSocket();
        //valid browser HTTP
        SimpleHttpServer server = new SimpleHttpServer();
        server.Start();
    }
}