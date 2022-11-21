namespace HiLo.UnitTests.HiLoServer;

public class GameControllerUnitTests
{
    private readonly Mock<ILogger<GameController>> _loggerMock;
    private readonly Mock<IEngine> _engineMock;

    GameController gameController;
    public GameControllerUnitTests()
    {
        _loggerMock = new Mock<ILogger<GameController>>();       
        _engineMock = new Mock<IEngine>();

        gameController = new GameController(_loggerMock.Object, _engineMock.Object); 
    }

    [Fact]
    public void Should_Return_Ok_With_Dto()
    {
        // Arrange
        var gameId = Guid.NewGuid().ToString();
        var playerId = "Zerep";
        var guess = 33;

        var (hasFinish, response) = (false, "The quick brown fox jumps over the lazy dog.");
        _engineMock.Setup(s => s.CheckGuess(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId)), It.Is<int>(i => i==guess))).Returns((hasFinish, response));

        //act
        var resp = gameController.Guess(gameId, playerId, guess);

        //assert
        resp.Should().NotBeNull();
        resp.Should().BeOfType<ActionResult<PlayDto>>();
        resp.Value.Should().NotBeNull();

        resp.Value.HasFinish.Should().Be(hasFinish);
        resp.Value.Response.Should().Be(response);

        _engineMock.Verify(s => s.CheckGuess(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId)), It.Is<int>(i => i == guess)), Times.Once);
        _engineMock.Verify(s => s.CheckGuess(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        
        _loggerMock.Verify(logger => logger.Log(
         It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
         It.IsAny<EventId>(),
         It.IsAny<It.IsAnyType>(),
         It.IsAny<Exception>(),
         It.IsAny<Func<It.IsAnyType, Exception, string>>()),
     Times.Never);
    }

    [Fact]
    public void Should_Fail_Internal_Error()
    {
        // Arrange
        _engineMock.Setup(s => s.CheckGuess(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());

        //act
        var resp = gameController.Guess(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>());

        //Assert
        resp.Should().NotBeNull();
        resp.Should().BeOfType<ActionResult<PlayDto>>();
        resp.Result.Should().BeOfType<ObjectResult>();      
        var result3 = (ObjectResult)resp.Result;

        result3.Should().NotBeNull();
        result3.StatusCode.Should().Be(500);
        result3.Value.Should().Be("Something went wrong while processing the Guess request");

        _engineMock.Verify(s => s.CheckGuess(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once());

        _loggerMock.Verify(logger => logger.Log(
         It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
         It.IsAny<EventId>(),
         It.IsAny<It.IsAnyType>(),
         It.IsAny<Exception>(),
         It.IsAny<Func<It.IsAnyType, Exception, string>>()),
     Times.Once);
    }
}