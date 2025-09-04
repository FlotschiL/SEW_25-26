using System;
using System.Diagnostics;
using System.Threading;
namespace Multithreading_Example;
class ThreadClass
{
    public static void Start()
    {
        Thread t1 = new Thread(() =>
        {
            Stopwatch sw = Stopwatch.StartNew();
            int count = CountPrimes(1, 500_000_000);
            sw.Stop();
            Console.WriteLine($"[Thread 1] Primzahlen bis 500 Mio: {count}, Zeit: {sw.Elapsed}");
        });

        Thread t2 = new Thread(() =>
        {
            Stopwatch sw = Stopwatch.StartNew();
            int count = CountPrimes(500_000_001, 1_000_000_000);
            sw.Stop();
            Console.WriteLine($"[Thread 2] Primzahlen von 500 Mio bis 1 Mrd: {count}, Zeit: {sw.Elapsed}");
        });

        // Threads starten
        t1.Start();
        t2.Start();

        // Warten, bis beide fertig sind
        t1.Join();
        t2.Join();

        Console.WriteLine("Beide Threads fertig!");
    }
    static int CountPrimes(int start, int limit)
    {
        if (limit < 2 || start > limit) return 0;
        if (start < 2) start = 2;

        bool[] isPrime = new bool[limit + 1];
        Array.Fill(isPrime, true);
        isPrime[0] = isPrime[1] = false;

        for (int i = 2; i * i <= limit; i++)
        {
            if (isPrime[i])
            {
                for (int j = i * i; j <= limit; j += i)
                {
                    isPrime[j] = false;
                }
            }
        }

        int count = 0;
        for (int i = start; i <= limit; i++)
        {
            if (isPrime[i]) count++;
        }

        return count;
    }

}