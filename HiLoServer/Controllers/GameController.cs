namespace HiLoServer.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;
    private readonly IEngine _engine;

    public GameController(ILogger<GameController> logger, IEngine engine)
    {
        _logger = logger;
        _engine = engine;
    }

    [HttpGet(Name = "Guess")]
    public ActionResult<PlayDto> Guess(string gameId, string playerId, int guess)
    {
        try
        {
            var (hasFinish, response) = _engine.CheckGuess(gameId, playerId, guess);

            return new PlayDto
            {
                HasFinish = hasFinish,
                Response = response
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong while processing the Guess request");
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong while processing the Guess request");
        }
    }


    [HttpGet(Name = "Start")]
    public ActionResult<StartDto> Start(string playerId)
    {
        try
        {
            var (gameId, response) = _engine.StartGame(playerId);

            return new StartDto
            {
                PlayerId = playerId,
                GameId = gameId,
                Response = response
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong while processing the Start request");
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong while processing the Start request");
        }
    }
}