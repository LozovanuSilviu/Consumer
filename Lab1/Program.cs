using Lab1;

public class Program
{
    public static void Main(string[] args)
    {
        var server = new ConsumerServer();
        server.Start();
        Console.WriteLine("Press a key to quit.");
        Console.ReadKey();
        server.Stop();
        
    }
}