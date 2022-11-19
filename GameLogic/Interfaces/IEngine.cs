namespace GameLogic.Interfaces;

public interface IEngine
{
    string CheckGuess(string gameId, string playerId, int guess);
    Tuple<string, string> StartGame(string playerId);
    void ResetPlayerGame(string playerId);
}