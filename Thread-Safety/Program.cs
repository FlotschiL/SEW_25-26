new Thread(new ThreadStart(() => Spieler1())).Start();
new Thread(new ThreadStart(() => Spieler2())).Start();
object ping = new object();
object pong = new object();
void Spieler1()
{
    while (true)
    {
        Monitor.Enter(ping);
        {
            Console.WriteLine("Ping");
        }
        Monitor.Exit(ping);
    }
}
void Spieler2()
{
    while (true)
    {
        Monitor.Enter(ping);
        {
            Console.WriteLine("Pong");
        }
        Monitor.Exit(ping);
    }
}