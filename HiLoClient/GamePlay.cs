namespace HiLoClient;

public class GamePlay : IGamePlay
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GameSession> _logger;
    public GamePlay(ILogger<GameSession> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task Start(string gameId, string playerId, string token) {
        Console.WriteLine("");

        int guess = ConsoleCommands.GetGuess();         

        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://localhost:7143/game/guess?gameId={gameId}&playerId={playerId}&guess={guess}")
        {
            //Headers =
            //{
            //    { HeaderNames.Accept, "application/vnd.github.v3+json" },
            //    { HeaderNames.UserAgent, "HttpRequestsSample" }
            //}
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
        else {
            _logger.LogWarning("Something went wrong while requesting the response from getting the guess.");
        }

        return;                  
    }
}
