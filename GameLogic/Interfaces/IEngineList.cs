namespace GameLogic.Interfaces;
public interface IEngineList
{
    int GetValueFromPlayerGame(string playerId);
    void ResetPlayerGame(string playerId);
    void StartPlayerGame(string playerId);
}