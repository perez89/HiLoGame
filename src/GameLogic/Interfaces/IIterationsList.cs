namespace GameLogic.Interfaces;

public interface IIterationsList
{
    void Add(string gameId, string playerId, int guess, int mysteryNumber);

    int Count(string gameId, string playerId);
}