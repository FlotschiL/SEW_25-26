using System.Text;

namespace TrainSim;


    using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class GPT_Railway
{
    class Program
    {
        static bool _running = true;
        static object _consoleLock = new object();

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();

            int sectionCount = 5;
            int trackLength = 60;
            int trainCount = 2;

            // Build railway with N sections
            var railway = new Railway(trackLength, sectionCount);

            // Start trains
            List<Task> trains = new List<Task>();
            for (int i = 0; i < trainCount; i++)
            {
                int row = 3 + i * 2;
                trains.Add(Task.Run(() => railway.RunTrain("🚆", row, 150 + i * 50)));
            }

            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.WriteLine("Press Q to quit...");

            while (_running)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    _running = false;
                }
                Thread.Sleep(50);
            }

            Task.WaitAll(trains.ToArray());
            Console.CursorVisible = true;
        }

        class Railway
        {
            private readonly int _length;
            private readonly int _sectionCount;
            private readonly int _sectionWidth;
            private readonly SemaphoreSlim[] _semaphores;

            public Railway(int length, int sectionCount)
            {
                _length = length;
                _sectionCount = sectionCount;
                _sectionWidth = length / sectionCount;
                _semaphores = new SemaphoreSlim[sectionCount];
                for (int i = 0; i < sectionCount; i++)
                    _semaphores[i] = new SemaphoreSlim(1, 1);

                DrawTrack();
            }

            public async Task RunTrain(string train, int row, int speedMs)
            {
                int pos = -train.Length;

                while (_running)
                {
                    int sectionIndex = GetSectionIndex(pos + train.Length / 2);

                    // enter section (wait until it's free)
                    if (sectionIndex >= 0 && sectionIndex < _sectionCount)
                        await _semaphores[sectionIndex].WaitAsync();

                    // draw
                    lock (_consoleLock)
                    {
                        Erase(pos - 1, row, train.Length);
                        Draw(pos, row, train);
                    }

                    await Task.Delay(speedMs);
                    pos++;

                    // leave section
                    int prevSection = GetSectionIndex(pos - 1 - train.Length / 2);
                    if (prevSection != sectionIndex && prevSection >= 0)
                        _semaphores[prevSection].Release();

                    // wrap around
                    if (pos > _length) pos = -train.Length;
                }
            }

            private int GetSectionIndex(int position)
            {
                if (position < 0 || position >= _length) return -1;
                return position / _sectionWidth;
            }

            private void DrawTrack()
            {
                StringBuilder sb = new StringBuilder(new string('=', _length));
                for (int i = 1; i < _sectionCount; i++)
                    sb[i * _sectionWidth] = '|'; // mark section boundaries

                Console.WriteLine(sb.ToString());
            }

            private void Draw(int pos, int row, string text)
            {
                if (pos < 0) return;
                Console.SetCursorPosition(pos, row);
                Console.Write(text.Substring(0, Math.Min(text.Length, _length - pos)));
            }

            private void Erase(int pos, int row, int length)
            {
                if (pos < 0) return;
                Console.SetCursorPosition(pos, row);
                Console.Write(new string('=', Math.Min(length, _length - pos)));
            }
        }
    }
}
