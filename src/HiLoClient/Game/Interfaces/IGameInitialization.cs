namespace HiLoClient.Game.Interfaces;

public interface IGameInitialization
{
    Task StartGame(string playerId, string token);
}