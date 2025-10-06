using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TrainSim;

    public static class Railway2
    {
        // Simulation parameters (adjustable in StartSim)
        private static int _semaphoreCount = 5;
        private static int _trainCount = 2;
        private static readonly int Length = 50;

        // concurrency / state
        private static SemaphoreSlim[] _semaphores;
        private static List<int> SectionPositions;
        private static List<bool> SectionLocks;
        private static int spaceBetweenSections;

        // console sync
        private static readonly object _consoleLock = new();

        // randomized delay per thread (thread-safe)
        private static readonly ThreadLocal<Random> _rnd = new(() => {
            // seed with fast-changing value per thread
            return new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId));
        });

        public static void StartSim()
        {
            Console.OutputEncoding = Encoding.UTF8; // for lock/unlock emojis

            Console.WriteLine("Enter the number of Sections (default: 5):");
            string number = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(number) && int.TryParse(number, out int parsedSections) && parsedSections >= 1)
                _semaphoreCount = parsedSections;
            else
                _semaphoreCount = 5;

            Console.WriteLine("Enter the number of Trains (default: 2):");
            number = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(number) && int.TryParse(number, out int parsedTrains) && parsedTrains >= 1)
                _trainCount = parsedTrains;
            else
                _trainCount = 2;

            Console.Clear();
            Initialize();

            // create threads (trains)
            var threads = new List<Thread>();
            for (int t = 0; t < _trainCount; t++)
            {
                var threadIndex = t; // capture
                var thread = new Thread(() =>
                {
                    var rnd = _rnd.Value!;
                    // random start delay so trains don't perfectly sync
                    Thread.Sleep(rnd.Next(100, 1200));

                    int k = 0; // position
                    int currentSection = -1; // index of section currently occupied (-1 none)

                    while (k < Length)
                    {
                        // If we reached a section boundary, try to move into that section
                        if (SectionPositions.Contains(k))
                        {
                            int nextSection = SectionPositions.IndexOf(k);

                            // Wait until the section is unlocked (manual lock)
                            while (SectionLocks[nextSection])
                            {
                                Thread.Sleep(150); // polling while locked; cheap for this sim
                            }

                            // Acquire the next section semaphore (wait until available)
                            _semaphores[nextSection].Wait();

                            // Successfully entered the next section -> release previous section (if any)
                            if (currentSection >= 0)
                            {
                                _semaphores[currentSection].Release();
                            }

                            currentSection = nextSection;
                        }

                        // Render movement: erase old pos, draw at new pos
                        EraseTrain(k - 1, 3, 3);
                        DrawTrainAt(k, 3, "///");

                        k++;
                        Thread.Sleep(200 + rnd.Next(0, 300));
                    }

                    // train finished: release last section if holding one
                    if (currentSection >= 0)
                    {
                        _semaphores[currentSection].Release();
                    }
                })
                {
                    IsBackground = true,
                    Name = $"Train-{threadIndex+1}"
                };

                threads.Add(thread);
            }

            // start trains
            threads.ForEach(t => t.Start());

            // user input loop for locking/unlocking sections
            ShowInstructions();
            do
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Q)
                    {
                        break;
                    }

                    // handle numeric keys 1..9 and numpad
                    int sectionNumber = -1;
                    if (keyInfo.Key >= ConsoleKey.D0 && keyInfo.Key <= ConsoleKey.D9)
                    {
                        sectionNumber = keyInfo.Key - ConsoleKey.D0;
                    }
                    else if (keyInfo.Key >= ConsoleKey.NumPad0 && keyInfo.Key <= ConsoleKey.NumPad9)
                    {
                        sectionNumber = keyInfo.Key - ConsoleKey.NumPad0;
                    }

                    if (sectionNumber >= 1 && sectionNumber <= _semaphoreCount)
                    {
                        ToggleSectionLock(sectionNumber - 1);
                    }
                }
                else
                {
                    Thread.Sleep(50); // small idle to avoid busy loop
                }
            } while (true);

            // wait for trains to finish before exit (optional)
            foreach (var t in threads)
            {
                if (t.IsAlive)
                    t.Join();
            }

            SafeWrite(() =>
            {
                Console.SetCursorPosition(0, 10 + 2);
                Console.WriteLine("\nSimulation ended. Press any key to exit.");
            });
            Console.ReadKey(true);
        }

        private static void ShowInstructions()
        {
            SafeWrite(() =>
            {
                Console.SetCursorPosition(0, 6);
                Console.WriteLine("Controls:");
                Console.WriteLine($"Press 1..{_semaphoreCount} to toggle lock/unlock a section.");
                Console.WriteLine("Press Q to quit (waits for trains to finish).");
            });
        }

        public static void Initialize()
        {
            // Prepare section lists and semaphores
            SectionPositions = new List<int>();
            SectionLocks = new List<bool>();
            _semaphores = new SemaphoreSlim[_semaphoreCount];

            // space sections evenly: avoid hitting ends
            spaceBetweenSections = Length / (_semaphoreCount + 1);
            if (spaceBetweenSections < 1) spaceBetweenSections = 1;

            for (int i = 0; i < _semaphoreCount; i++)
            {
                int pos = (i + 1) * spaceBetweenSections;
                if (pos >= Length) pos = Length - 1;
                SectionPositions.Add(pos);
                SectionLocks.Add(false); // unlocked by default

                // semaphore initial count 1 (only one train in a section)
                _semaphores[i] = new SemaphoreSlim(1, 1);
            }

            // Draw track + section markers + labels
            var sb = new StringBuilder();
            for (int i = 0; i < Length; i++) sb.Append('=');

            var track = new char[Length];
            for (int i = 0; i < Length; i++) track[i] = ' ';

            var labels = new char[Length];
            for (int i = 0; i < Length; i++) labels[i] = ' ';

            for (int i = 0; i < SectionPositions.Count; i++)
            {
                int p = SectionPositions[i];
                track[p] = "🔒".ToCharArray()[0]; // initially show as locked? we'll show unlocked state based on SectionLocks below
                var labelText = $"I{(i + 1)}";
                for (int j = 0; j < labelText.Length && p + 1 + j < Length; j++)
                    labels[p + 1 + j] = labelText[j];
            }

            SafeWrite(() =>
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(new string(track));
                Console.WriteLine(new string(labels));
                Console.WriteLine();
                Console.WriteLine(sb.ToString());
                // initial update of lock icons based on SectionLocks
                for (int i = 0; i < SectionPositions.Count; i++)
                    RedrawSectionLockIcon(i);
            });
        }

        private static void ToggleSectionLock(int sectionIndex)
        {
            // flip lock flag
            SectionLocks[sectionIndex] = !SectionLocks[sectionIndex];

            // redraw lock icon
            RedrawSectionLockIcon(sectionIndex);
        }

        private static void RedrawSectionLockIcon(int sectionIndex)
        {
            int pos = SectionPositions[sectionIndex];
            SafeWrite(() =>
            {
                try
                {
                    Console.SetCursorPosition(Math.Max(0, pos), 0);
                    Console.Write(SectionLocks[sectionIndex] ? "🔒" : "🔓");
                }
                catch
                {
                    // ignore any cursor errors (console small)
                }
            });
        }

        static void DrawTrainAt(int pos, int row, string train)
        {
            int start = Math.Max(0, pos);
            int visible = Math.Min(train.Length, Length - start);

            if (visible <= 0) return;

            SafeWrite(() =>
            {
                try
                {
                    Console.SetCursorPosition(start, row);
                    Console.Write(train.Substring(0, visible));
                }
                catch { }
            });
        }

        static void EraseTrain(int pos, int row, int length)
        {
            int start = Math.Max(0, pos);
            int visible = Math.Min(length, Length - start);
            if (visible <= 0) return;

            SafeWrite(() =>
            {
                try
                {
                    Console.SetCursorPosition(start, row);
                    // draw background track chars to "erase"
                    Console.Write(new string('=', visible));
                }
                catch
                {
                    // ignore
                }
            });
        }

        private static void SafeWrite(Action action)
        {
            lock (_consoleLock)
            {
                action();
            }
        }
    }
