namespace HiLoClient.Game;

public class GameSession : IGameSession
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GameSession> _logger;
    private readonly IGameInitialization _gameInitialization;
    private readonly IConsolePlayerName _consolePlayerName;

    public GameSession(ILogger<GameSession> logger, IHttpClientFactory httpClientFactory, IGameInitialization gameInitialization, IConsolePlayerName consolePlayerName)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _gameInitialization = gameInitialization;
        _consolePlayerName = consolePlayerName;
    }

    public async Task Start()
    {

        var playerId = _consolePlayerName.GetResponse();

        var token = await GetTokenAsync(playerId);

        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("Something went wrong with session result");
            return;
        }

        await _gameInitialization.StartGame(playerId, token);

        ExitGame(playerId);
    }

    private async Task<string> GetTokenAsync(string playerId)
    {
        var json = JsonConvert.SerializeObject(playerId);
        var data = new StringContent($"{json}", Encoding.UTF8, "application/json");

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.PostAsync("https://localhost:7143/Session", data);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        return string.Empty;
    }

    private void ExitGame(string playerId)
    {
        Console.WriteLine($"");
        Console.WriteLine($"Player {playerId}, you have exit the game, press any key to close the Client.");
        Console.WriteLine($"Thank you.");

        Console.ReadKey();
        Environment.Exit(0);
    }
}
