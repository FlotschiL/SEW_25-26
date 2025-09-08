using System;
using System.Diagnostics;
using System.IO;
namespace Multithreading_Example;

class Program
{
    public static void Main()
    {
        //Diagramm.Start();
        //ThreadClass.Start4Threads();
        //ThreadClass.StartDynamicThreads(1, 100_000_000, 4);
        for (var i = 0; i < 1; i++)
        {
            int count = MachosThreadExample.Start();
            if (count != 100)
            {
                Console.WriteLine(count);
            }

        }
        /*
        for (int i = 1; i < 10; i++)
        {
            Console.WriteLine($"ThreadsAnzahl: {i}");
            ThreadClass.StartDynamicThreads(100_000_000, i);
            //KillianThread.Start(i);
        }
*/
    }
}