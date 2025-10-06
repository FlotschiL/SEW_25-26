using System.Globalization;
using System.Text;

namespace TrainSim;
public class Railway
{
    private static SemaphoreSlim[] _semaphores;
    private static int _semaphoreCount;
    private static int _trainCount;
    private static readonly int Length = 50;
    private static List<int> SectionPositions;
    private static List<bool> SectionLocks;
    private static int spaceBetweenSections;
    private static void LockSection(int sectionIndex)
    {
        Console.SetCursorPosition(SectionPositions[sectionIndex-1]-1, 0);
        SectionLocks[sectionIndex-1] = !SectionLocks[sectionIndex-1];
        Console.Write(SectionLocks[sectionIndex-1] ? "🔒" : "🔓");
        
    }
    public static void StartSim()
    {

        Console.WriteLine("Enter the number of Sections(default: 5)");
        string number = Console.ReadLine();
        _semaphoreCount = number == ""  ? 5: int.Parse(number);
        /*Console.WriteLine("Enter the number of Trains(default:1)");
        number = Console.ReadLine();
        _trainCount = number == ""  ? 1 : int.Parse(number);*/
        _trainCount = 2;
        Console.Clear();
        Initialize();
        List<Thread> threads = new List<Thread>();
        for (int j = 0; j < _trainCount; j++)
        {
            threads.Add(new Thread(() =>
            {
                Thread.Sleep(new Random().Next(100, 5000));
                //Console.Write("Starting");
                int k = 0;
                int currentSection = 0;
                while (k < Length)
                {
                    if (SectionPositions.Contains(k))
                    {
                        _semaphores[currentSection].Release();
                        _semaphores[++currentSection].Wait();
                    }
                    EraseTrain(k-1, 3, 3);
                    DrawTrainAt(k++, 3, "///");
                    Thread.Sleep(500);
                    
                }
            }));
        }
        threads.ForEach(t => t.Start());
        int i = 0;
        do
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Q) 
                    break; // Exit loop when 'q' key is pressed
                switch (key)
                {
                    case ConsoleKey.D1:
                        LockSection(1);
                        break;
                    case ConsoleKey.D2:
                        LockSection(2);
                        break;
                    case ConsoleKey.D3:
                        LockSection(3);
                        break;
                    case ConsoleKey.D4:
                        LockSection(4);
                        break;
                    case ConsoleKey.D5:
                        LockSection(5);
                        break;
                }
            }
        } while (true);
        threads.ForEach(t => t.Join());
    }
    public static void Initialize()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('=', Length);
        //Sections
        string Sections = " ";
        string SectionNames = " ";
        spaceBetweenSections = Length/(_semaphoreCount + 1)+1;
        SectionPositions = new List<int>();
        SectionLocks = new List<bool>();
        _semaphores = new SemaphoreSlim[_semaphoreCount];
        for (int i = 1; i < Length; i++)
        {
            if (i % spaceBetweenSections == 0)
            {
                Sections = Sections.Substring(0, i - 1);
                Sections += "🔒";
                SectionPositions.Add(i);
                SectionLocks.Add(true);
                _semaphores[i/spaceBetweenSections-1] = (new SemaphoreSlim(0, _trainCount));
                SectionNames = SectionNames.Substring(0, i - 2);
                SectionNames += "I(" + (i/spaceBetweenSections).ToString() + ")";
            }
            else
            {
                Sections += " ";
                SectionNames += " ";
            }
        }

        Console.Write(Sections + "\n" + SectionNames + "\n\n" + sb.ToString());
    }
    static void DrawTrainAt(int pos, int row, string train)
    {
        int start = Math.Max(0, pos);
        int visible = Math.Min(train.Length, Length - start);

        if (visible <= 0) return;

        try
        {
            Console.SetCursorPosition(start, row);
            Console.Write(train.Substring(0, visible));
        }
        catch { }
    }

    static void EraseTrain(int pos, int row, int length)
    {
        int start = Math.Max(0, pos);
        int visible = Math.Min(length, Length - start);
        if (visible <= 0) return;

        try
        {
            Console.SetCursorPosition(start, row);
            Console.Write(new string('=', visible));
        }
        catch {
            Console.WriteLine("ERROR");
        }
    }
}