using System.Text;

namespace TrainSim;
public class Railway
{
    Semaphore[] _semaphores = new Semaphore[1];
    private int _semaphoreCount;
    private int _trainCount;
    private readonly int Length = 50;
    public Railway()
    {
        
    }

    public void StartSim()
    {

        Console.WriteLine("Enter the number of Sections(default: 10)");
        string number = Console.ReadLine();
        _semaphoreCount = number == ""  ? 10 : int.Parse(number);
        Console.WriteLine("Enter the number of Trains(default:1)");
        number = Console.ReadLine();
        _trainCount = number == ""  ? 1 : int.Parse(number);
        Console.Clear();
        Console.WriteLine(this);
        int i = 0;
        do
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Q) 
                    break; // Exit loop when 'q' key is pressed
                // Handle other keys if needed
            }
            EraseTrain(i-1, 3, 3);
            DrawTrainAt(i++, 3, "///");
            
            Thread.Sleep(500);
        } while (true);
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('=', Length);
        return "\n\n\n" + sb.ToString();
    }
    void DrawTrainAt(int pos, int row, string train)
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

    void EraseTrain(int pos, int row, int length)
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