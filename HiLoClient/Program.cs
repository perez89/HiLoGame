# nullable disable

var host = CreateHostBuilder(args).Build();
StartUp app = host.Services.GetRequiredService<StartUp>();

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            (_, services) => services
                .AddHttpClient()
                .AddSingleton<StartUp, StartUp>()
                .AddScoped<IGameSession, GameSession>()
                .AddScoped<IGameInitialization, GameInitialization>()
                .AddScoped<IGamePlay, GamePlay>()
                .AddTransient<IConsolePlayerName, ConsolePlayerName>()
                .AddTransient<IConsoleKeepPlaying, ConsoleKeepPlaying>()
                .AddTransient<IConsoleGuess, ConsoleGuess>());
}

await host.RunAsync();

class StartUp
{
    readonly IGameSession _gameSession;
    public StartUp(ILogger<StartUp> logger, IGameSession gameSession)
    {
        _gameSession = gameSession;
        logger.Log(LogLevel.Information, "Application constructed");

        Initialize();
    }

    public async void Initialize()
    {
        try
        {
            await _gameSession.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }
}