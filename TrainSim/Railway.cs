namespace TrainSim;
public class Railway
{
    Semaphore[] _semaphores = new Semaphore[1];
    private int _semaphoreCount;
    private int _trainCount;

    public Railway()
    {
        
    }

    public void StartSim()
    {
        Console.WriteLine("Enter the number of Sections(default: 10)");
        string number = Console.ReadLine();
        _semaphoreCount = number == ""  ? 10 : int.Parse(number);
        Console.WriteLine("Enter the number of Trains(default:2)");
        number = Console.ReadLine();
        _trainCount = number == ""  ? 2 : int.Parse(number);
        do 
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Q) 
                    break; // Exit loop when 'q' key is pressed
                // Handle other keys if needed
            }
            Console.WriteLine("Miau");
        } while (true);
    }
    public override string ToString()
    {
        return base.ToString();
    }
}