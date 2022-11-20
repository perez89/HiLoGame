# nullable disable

using HiLoClient;

var host = CreateHostBuilder(args).Build();
Game app = host.Services.GetRequiredService<Game>();
static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            (_, services) => services
                .AddHttpClient()
                .AddSingleton<Game, Game>()
                .AddSingleton<IAppConfig, AppConfig>()
                .AddScoped<IGameSession, GameSession>()
                .AddScoped<IGameInitialization, GameInitialization>()
                .AddScoped<IGamePlay, GamePlay>());
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
    readonly IGameSession _gameSession;
    public Game(IAppConfig config, ILogger<Game> logger, IGameSession gameSession)
    {
        this.config = config;
        _gameSession = gameSession;
        logger.Log(LogLevel.Information, "Application constructed");


        Initialize();
    }

    public async void Initialize()
    {
        try {
            await _gameSession.Start();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"{ex.Message}"); 
        }       
    }

}