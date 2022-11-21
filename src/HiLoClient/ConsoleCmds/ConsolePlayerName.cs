namespace HiLoClient.ConsoleCmds;

public class ConsolePlayerName : IConsolePlayerName
{
    public string GetResponse()
    {
        Console.WriteLine("Write your nickname: ");

        var input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input) && input.Trim().Length > 2)
        {
            return input.Trim();
        };

        Console.WriteLine("");
        Console.WriteLine("Please, write a nickname with at least 3 characters");

        return GetResponse();
    }
}
