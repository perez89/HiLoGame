namespace HiLoServer.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]")]
public class SessionController : ControllerBase
{
    private readonly ILogger<GameController> _logger;

    public SessionController(ILogger<GameController> logger)
    {
        _logger = logger;
    }


    [HttpPost(Name = "Login")]
    public ActionResult<string> Login([FromBody] string playerId)
    {
        try
        {
            var guid = Guid.NewGuid().ToString();

            return guid;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Something went wrong while processing the Login request");
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong while processing the Login request");      
        }
    }
}