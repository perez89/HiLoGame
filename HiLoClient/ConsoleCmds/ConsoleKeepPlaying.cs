namespace HiLoClient.ConsoleCmds;

public class ConsoleKeepPlaying : IConsoleKeepPlaying
{
    public bool GetResponse()
    {
        Console.WriteLine("");
        Console.WriteLine("Do you want to play another game? 'yes/y' to play and 'no/n' to stop");

        var input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            if (input.Equals("yes", StringComparison.OrdinalIgnoreCase) || input.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if (input.Equals("no", StringComparison.OrdinalIgnoreCase) || input.Equals("n", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return GetResponse();
    }
}
