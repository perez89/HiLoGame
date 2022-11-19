namespace GameLogic;

public class Engine : IEngine
{
    private readonly IEngineList _engineList;
    private readonly IIterationsList _iterationsList;

    public Engine(IEngineList engineList, IIterationsList iterationsList)
    {
        _engineList = engineList;
        _iterationsList = iterationsList;
    }

    public (bool, string) CheckGuess(string gameId, string playerId, int guess) {
        var mysteryNumber = _engineList.GetValueFromPlayerGame(playerId);

        _iterationsList.Add(gameId, playerId, guess, mysteryNumber);

        if (guess > mysteryNumber) {

            return (false, $"The mystery number is less than your guess({guess}), player {playerId}.");
        }else if(guess < mysteryNumber)
        {

            return (false, $"The mystery number is greater than your guess({guess}), player {playerId}.");
        }

        int iterations = _iterationsList.Count(gameId, playerId);
        _engineList.ResetPlayerGame(playerId);
        return (true, $"You have successfully found the mystery number({mysteryNumber}). You needed {iterations} iterations.");
    }

    public (string, string) StartGame(string playerId) {
        var gameId = Guid.NewGuid().ToString();

        _engineList.StartPlayerGame(playerId);

        return (gameId, $"You can start your guess's player {playerId}.");
    }

    public void ResetPlayerGame(string playerId)
    {
        _engineList.ResetPlayerGame(playerId);
    }
}