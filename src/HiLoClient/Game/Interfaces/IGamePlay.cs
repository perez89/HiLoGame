namespace HiLoClient.Game.Interfaces;

public interface IGamePlay
{
    Task Start(string gameId, string playerId, string token);
}