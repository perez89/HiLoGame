namespace GameLogic.Models;

public record Iteration
{
    public Iteration(string gameId, string playerId, int guess, int mysteryNumber)
    {
        GameId = gameId;
        PlayerId = playerId;
        Guess = guess;
        MysteryNumber = mysteryNumber;
        Date = DateTime.Now;
    }

    public string GameId { get; }
    public string PlayerId { get; }
    public int MysteryNumber { get; }
    public int Guess { get; }
    public DateTime Date { get; }
}
