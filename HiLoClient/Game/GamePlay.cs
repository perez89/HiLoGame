namespace HiLoClient.Game;

public class GamePlay : IGamePlay
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GameSession> _logger;
    private readonly IConsoleGuess _consoleGuess;
    public GamePlay(ILogger<GameSession> logger, IHttpClientFactory httpClientFactory, IConsoleGuess consoleGuess)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _consoleGuess = consoleGuess;
    }

    public async Task Start(string gameId, string playerId, string token)
    {
        Console.WriteLine("");

        int guess = _consoleGuess.GetResponse();

        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://localhost:7143/game/guess?gameId={gameId}&playerId={playerId}&guess={guess}")
            {
                Headers =
                    {
                        { HeaderNames.Authorization, $"Bearer {token}" }
                    }
            };

        var httpClient = _httpClientFactory.CreateClient();

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var contentString =
                await httpResponseMessage.Content.ReadAsStringAsync();

            var playDto = JsonConvert.DeserializeObject<PlayDto>(contentString);
            Console.WriteLine($"");
            Console.WriteLine(playDto.Response);

            var gameHasFinished = playDto.HasFinish;

            if (!gameHasFinished)
                await Start(gameId, playerId, token);
        }
        else
        {
            _logger.LogWarning("Something went wrong while requesting the response from getting the guess.");
        }

        return;
    }
}
