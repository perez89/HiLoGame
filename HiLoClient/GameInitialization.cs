namespace HiLoClient;

public class GameInitialization : IGameInitialization
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GameSession> _logger;
    private readonly IGamePlay _gamePlay;
    private readonly IConsoleKeepPlaying _consoleKeepPlaying;
    public GameInitialization(ILogger<GameSession> logger, IHttpClientFactory httpClientFactory, IGamePlay gamePlay, IConsoleKeepPlaying consoleKeepPlaying)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _gamePlay= gamePlay;
        _consoleKeepPlaying = consoleKeepPlaying;
    }

    public async Task StartGame(string playerId, string token) {
     
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://localhost:7143/game/start?playerId={playerId}")
        {
            //Headers =
            //{
            //    { HeaderNames.Accept, "application/vnd.github.v3+json" },
            //    { HeaderNames.UserAgent, "HttpRequestsSample" }
            //}
        };
        var httpClient = _httpClientFactory.CreateClient();

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        StartDto startDto = null;
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var contentString =
                await httpResponseMessage.Content.ReadAsStringAsync();

            startDto = JsonConvert.DeserializeObject<StartDto>(contentString);
        }

        Console.WriteLine(startDto.Response);


        await _gamePlay.Start(startDto.GameId, playerId, token);        

        var stopPlaying = _consoleKeepPlaying.GetResponse();       

        if (!stopPlaying)
            await StartGame(playerId, token);
    }
}
