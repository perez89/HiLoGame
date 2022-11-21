namespace HiLoServer.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]")]
public class SessionController : ControllerBase
{
    private readonly ILogger<GameController> _logger;
    private readonly IConfiguration _configRoot;

    public SessionController(ILogger<GameController> logger, IConfiguration configRoot)
    {
        _logger = logger;
        _configRoot = configRoot;
    }

    [HttpGet(Name = "Health")]
    public string Health() {
        return "ok";
    }

    [HttpPost(Name = "Login")]
    public ActionResult<string> Login([FromBody] string playerId)
    {
        try
        {
            if (!string.IsNullOrEmpty(playerId) && playerId.Length > 2)
            {
                var token = GetToken(playerId);

                return Ok(token);
            }

            return Unauthorized($"Invalid nickname({playerId})");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong while getting JWT token");
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong while getting JWT token");
        }
    }

    private object GetToken(string playerId)
    {
        var issuer = _configRoot["Jwt:Issuer"];
        var audience = _configRoot["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
        (_configRoot["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, playerId),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMinutes(120),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }
}