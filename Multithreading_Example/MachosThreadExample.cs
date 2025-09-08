using System.Diagnostics;

namespace Multithreading_Example;

public class MachosThreadExample
{
    public static int i = 0;
    public static int Start(int threadCount = 100)
    {
        Thread[] threads = new Thread[threadCount];
        for (int t = 0; t < threadCount; t++)
        {
            threads[t] = new Thread(() =>
            {
                Thread.Sleep(1000);
            });
            threads[t].Start();


        }

        // Warten bis alle fertig sind
        foreach (var t in threads)
            t.Join();
        int hlp = i;
        i = 0;
        return hlp;

    }
}