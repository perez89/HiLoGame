namespace HiLoClient
{
    public interface IGameInitialization
    {
        Task StartGame(string playerId, string token);
    }
}