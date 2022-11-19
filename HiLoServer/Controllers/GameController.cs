using GameLogic.Interfaces;

namespace HiLoServer.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    private readonly ILogger<GameController> _logger;
    private readonly IEngine _engine;

    public GameController(ILogger<GameController> logger, IEngine engine)
    {
        _logger = logger;
        _engine = engine;
    }

    [HttpGet(Name = "Guess")]
    public string Get([FromBody]GuessDto guessDto)
    {
        try {
            return _engine.CheckGuess(guessDto.GameId, guessDto.PlayerId, guessDto.Guess);
        }
        catch {
            return "Something went wrong while processing your request";
        }        
    }
}