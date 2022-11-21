namespace HiLoClient.ConsoleCmds;

public class ConsoleGuess : IConsoleGuess
{
    public int GetResponse()
    {
        Console.WriteLine("Write your guess: ");

        var input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input) && int.TryParse(input.Trim(), out int guess) && guess > 0)
        {
            return guess;
        }

        Console.WriteLine("");
        Console.WriteLine("Please, write a guess bigger than 0.");

        return GetResponse();
    }
}
