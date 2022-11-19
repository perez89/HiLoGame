# nullable disable

var host = CreateHostBuilder(args).Build();
Game app = host.Services.GetRequiredService<Game>();
static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            (_, services) => services
                .AddHttpClient()
                .AddSingleton<Game, Game>()
                .AddSingleton<IAppConfig, AppConfig>());
}

await host.RunAsync();

interface IAppConfig
{
    string Setting { get; }
}
class AppConfig : IAppConfig
{
    public string Setting { get; }
    public AppConfig()
    {
        Console.WriteLine("AppConfig constructed");
    }
}
class Game
{
    readonly IAppConfig config;
    private readonly IHttpClientFactory _httpClientFactory;
    public Game(IAppConfig config, ILogger<Game> logger, IHttpClientFactory httpClientFactory)
    {
        this.config = config;
        _httpClientFactory = httpClientFactory;
        logger.Log(LogLevel.Information, "Application constructed");


        StartGame();
    }

    public async void StartGame()
    {
        try {
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
            var playerId = string.Empty;
            Console.WriteLine("Write your nickname: ");

            while (true)
            {
                var input = Console.ReadLine();


                if (!string.IsNullOrEmpty(input) && input.Trim().Length > 2)
                {
                    playerId = input.Trim();
                    break;
                };

                Console.WriteLine("Please, write a nickname with at least 3 characters");
            };

            //
            //httpRequestMessage.Content = new StringContent($"{playerId}", Encoding.UTF8, "application/json");

            var json = JsonConvert.SerializeObject(playerId);
            var data = new StringContent($"{json}", Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.PostAsync("https://localhost:7143/Session", data);

            string token = null;
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                token =
                    await httpResponseMessage.Content.ReadAsStringAsync();
               
            }

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Something went wrong with session result");
                return;
            }


            while (true)
            {
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

                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                StartDto startDto = null;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentString =
                        await httpResponseMessage.Content.ReadAsStringAsync();

                    startDto = JsonConvert.DeserializeObject<StartDto>(contentString);

                }

                Console.WriteLine(startDto.Response);


                var gameHasFinished = false;
                while (!gameHasFinished) {
                    int guess;
                    Console.WriteLine("");


                    while (true)
                    {
                        Console.WriteLine("Write your guess: ");
                        var input = Console.ReadLine();
                        Console.WriteLine("");

                        if (!string.IsNullOrEmpty(input) && int.TryParse(input.Trim(), out guess) && guess > 0)
                        {
                            break;
                        };

                        Console.WriteLine("Please, write a guess bigger than 0.");
                    }

                    //'https://localhost:7143/Game?gameId=sfsdsfd&playerId=323r23&guess=65'
                    httpRequestMessage = new HttpRequestMessage(
                        HttpMethod.Get,
                        $"https://localhost:7143/game/guess?gameId={startDto.GameId}&playerId={startDto.PlayerId}&guess={guess}")
                    {
                        //Headers =
                        //{
                        //    { HeaderNames.Accept, "application/vnd.github.v3+json" },
                        //    { HeaderNames.UserAgent, "HttpRequestsSample" }
                        //}
                    };

                    httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                    PlayDto playDto = null;
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        var contentString =
                            await httpResponseMessage.Content.ReadAsStringAsync();

                        playDto = JsonConvert.DeserializeObject<PlayDto>(contentString);
                        Console.WriteLine(playDto.Response);

                        gameHasFinished = playDto.HasFinish;
                    }

                    guess = 0;

                    if (gameHasFinished)
                        break;
                }

                var stopPlaying = false;
               
                while (true)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Do you still want to play another game? 'yes/y' to play 'no/n' to stop");
                    var input = Console.ReadLine();

                    if (!string.IsNullOrEmpty(input))
                    {
                        if (input.Equals("yes", StringComparison.OrdinalIgnoreCase) || input.Equals("y", StringComparison.OrdinalIgnoreCase))
                        {

                            stopPlaying = false;
                            break;
                        }
                        else if (input.Equals("no", StringComparison.OrdinalIgnoreCase) || input.Equals("n", StringComparison.OrdinalIgnoreCase))
                        {
                            stopPlaying = true;
                            break;
                        }
                    };
                };                

                if (stopPlaying)
                    break;
            }

            Console.WriteLine($"Player {playerId}, you have exit the game, press any key to close the Client.");
            Console.WriteLine($"Thank you.");

            Console.ReadKey();
            Environment.Exit(0);
        }
        catch(Exception ex)
        {

            Console.WriteLine($"{ex.Message}"); 
        }
       
    }

}