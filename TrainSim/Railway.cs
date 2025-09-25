using System.Globalization;
using System.Text;

namespace TrainSim;
public class Railway
{
    private static Semaphore[] _semaphores;
    private static int _semaphoreCount;
    private static int _trainCount;
    private static readonly int Length = 50;
    private static List<int> StationPositions;
    private static void LockStation(int StationIndex)
    {
        Console.SetCursorPosition(StationPositions[StationIndex], 0);
        Console.Write("+");
    }
    public static void StartSim()
    {

        Console.WriteLine("Enter the number of Sections(default: 1)");
        string number = Console.ReadLine();
        _semaphoreCount = number == ""  ? 1 : int.Parse(number);
        /*Console.WriteLine("Enter the number of Trains(default:1)");
        number = Console.ReadLine();
        _trainCount = number == ""  ? 1 : int.Parse(number);*/
        Console.Clear();
        Initialize();
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
                        LockStation(1);
                        break;
                    case ConsoleKey.D2:
                        LockStation(2);
                        break;
                    case ConsoleKey.D3:
                        LockStation(3);
                        break;
                    case ConsoleKey.D4:
                        LockStation(4);
                        break;
                    case ConsoleKey.D5:
                        LockStation(5);
                        break;
                }
            }
            EraseTrain(i-1, 3, 3);
            DrawTrainAt(i++, 3, "///");
            
            Thread.Sleep(500);
        } while (true);
    }
    public static void Initialize()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('=', Length);
        //Stations
        string Stations = " ";
        string StationNames = " ";
        int spaceBetweenStations = Length/(_semaphoreCount + 1)+1;
        StationPositions = new List<int>();
        for (int i = 1; i < Length; i++)
        {
            if (i % spaceBetweenStations == 0)
            {
                Stations = Stations.Substring(0, i - 1);
                Stations += "-";
                StationPositions.Add(i);
                StationNames = StationNames.Substring(0, i - 2);
                StationNames += "I(" + (i/spaceBetweenStations).ToString() + ")";
            }
            else
            {
                Stations += " ";
                StationNames += " ";
            }
        }

        Console.Write(Stations + "\n" + StationNames + "\n\n" + sb.ToString());
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