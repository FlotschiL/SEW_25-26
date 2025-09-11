
object locker = new object();  
new Thread(new ThreadStart(() => Spieler1())).Start();
void Spieler1()
{
    while (true)
    {
        lock (locker)
        {
            Console.WriteLine("Ping");
        }
    }
}
void Spieler2()
{
    while (true)
    {
        lock (locker)
        {
            Console.WriteLine("Pong");
        }
    }
}