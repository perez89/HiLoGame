namespace GameLogic.Interfaces;

public interface IEngine
{
    (bool, string) CheckGuess(string gameId, string playerId, int guess);
    (string, string) StartGame(string playerId);
    void ResetPlayerGame(string playerId);
}