namespace HiLo.UnitTests.HiLoClient;

public class GamePlayUnitTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<ILogger<GameSession>> _loggerMock;
    private readonly Mock<IConsoleGuess> _consoleGuessMock;
    private GamePlay _gamePlay;

    public GamePlayUnitTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _loggerMock= new Mock<ILogger<GameSession>>();
        _consoleGuessMock= new Mock<IConsoleGuess>();

        _gamePlay = new GamePlay(_loggerMock.Object, _httpClientFactoryMock.Object, _consoleGuessMock.Object);
    }

    [Fact]
    public async Task Should_Execute_All_Flow_Options_Guess_Greater_Less_Equal()
    {     
        var gameId = Guid.NewGuid().ToString();
        var playerId = "Zerep";
        var token = Guid.NewGuid().ToString(); 

        _consoleGuessMock.SetupSequence(s => s.GetResponse()).Returns(75).Returns(10).Returns(23);

        var url = $"https://localhost:7143/game/guess";

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var HttpResponseMessageList = GetHttpResponseMessages();

        handlerMock
            .Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(HttpResponseMessageList[0]).ReturnsAsync(HttpResponseMessageList[1]).ReturnsAsync(HttpResponseMessageList[2]);

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(url)
        };

        _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        Func<Task> act = () => _gamePlay.Start(gameId, playerId, token);

        await act.Should().NotThrowAsync<Exception>();

        _httpClientFactoryMock.Verify(_ => _.CreateClient(It.IsAny<string>()), Times.Exactly(3));

        VerifyLogs();
    }

    private void VerifyLogs()
    {
        _loggerMock.Verify(logger => logger.Log(
       It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
       It.IsAny<EventId>(),
       It.IsAny<It.IsAnyType>(),
       It.IsAny<Exception>(),
       It.IsAny<Func<It.IsAnyType, Exception, string>>()),
      Times.Never);

        _loggerMock.Verify(logger => logger.Log(
         It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
         It.IsAny<EventId>(),
         It.IsAny<It.IsAnyType>(),
         It.IsAny<Exception>(),
         It.IsAny<Func<It.IsAnyType, Exception, string>>()),
        Times.Never);

        _loggerMock.Verify(logger => logger.Log(
         It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
         It.IsAny<EventId>(),
         It.IsAny<It.IsAnyType>(),
         It.IsAny<Exception>(),
         It.IsAny<Func<It.IsAnyType, Exception, string>>()),
        Times.Never);
    }

    private List<HttpResponseMessage> GetHttpResponseMessages()
    {
        return new List<HttpResponseMessage>
        {
             new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new PlayDto { HasFinish = false, Response = "greater" })) //JsonConvert.SerializeObject(new PlayDto {HasFinish =false, Response = "greater" })
            },
            new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new PlayDto { HasFinish = false, Response = "less" })) //JsonConvert.SerializeObject(new PlayDto {HasFinish =false, Response = "greater" })
            },
            new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new PlayDto { HasFinish = true, Response = "equal" })) //JsonConvert.SerializeObject(new PlayDto {HasFinish =false, Response = "greater" })
            }
        };
    }
}