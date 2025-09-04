namespace Multithreading_Example;
using System;
using System.Diagnostics;
using System.IO;
public class Diagramm
{
    public static void Start()
    {
        string csvPath = "/home/flo/Documents/School/Year4/SEW/primzahlen.csv";

        // CSV-Header schreiben
        using (var writer = new StreamWriter(csvPath))
        {
            writer.WriteLine("Limit,AnzahlPrimzahlen,Zeit_ms");

            for (int limit = 0; limit <= 1_000_000_000; limit += 1_000_000_000)
            {
                long avg = 0;
                int avgtries = 1;
                int count = 0;
                for (int i = 0; i < avgtries; i++)
                {
                    Stopwatch sw = Stopwatch.StartNew();

                    count = CountPrimes(limit);

                    sw.Stop();
                    avg += sw.ElapsedMilliseconds;
                    
                }


                //writer.WriteLine($"{limit},{count},{(float)avg/avgtries}");
                Console.WriteLine($"Limit {limit}: {count} Primzahlen, {(float)avg/avgtries} ms");
            }
        }

        Console.WriteLine($"Ergebnisse gespeichert in {Path.GetFullPath("primzahlen.csv")}");
    }

    static int CountPrimes(int limit)
    {
        if (limit < 2) return 0;

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
        for (int i = 2; i <= limit; i++)
        {
            if (isPrime[i]) count++;
        }

        return count;
    }
}
