namespace HiLoClient;

public static class ConsoleCommands
{
    public static string GetPlayerName() {
        Console.WriteLine("Write your nickname: ");
      
        var input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input) && input.Trim().Length > 2)
        {
            return input.Trim();
        };

        Console.WriteLine("");
        Console.WriteLine("Please, write a nickname with at least 3 characters");

        return GetPlayerName();
    }

    public static int GetGuess() {
        Console.WriteLine("Write your guess: ");    
        
        var input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input) && int.TryParse(input.Trim(), out int guess) && guess > 0)
        {
            return guess;
        }

        Console.WriteLine("");
        Console.WriteLine("Please, write a guess bigger than 0.");

        return GetGuess();        
    }

    public static bool KeepPlaying() {
      
        Console.WriteLine("");
        Console.WriteLine("Do you still want to play another game? 'yes/y' to play 'no/n' to stop");
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

        return KeepPlaying();      
    }
}
