namespace HiLoClient;

public class GameSession : IGameSession
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GameSession> _logger;
    private readonly IGameInitialization _gameInitialization;
    public GameSession(ILogger<GameSession> logger, IHttpClientFactory httpClientFactory, IGameInitialization gameInitialization) {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _gameInitialization = gameInitialization;
    }

    public async Task Start()
    {
        //https://localhost:7143/Session
        //var httpRequestMessage = new HttpRequestMessage(
        //    HttpMethod.Post,
        //    )
        //{
        //    //Headers =
        //    //{
        //    //    { HeaderNames.Accept, "application/vnd.github.v3+json" },
        //    //    { HeaderNames.UserAgent, "HttpRequestsSample" }
        //    //}
        //};
        var playerId = ConsoleCommands.GetPlayerName();

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
