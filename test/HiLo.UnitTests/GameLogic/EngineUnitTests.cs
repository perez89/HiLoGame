namespace HiLo.UnitTests.GameLogic;

public class EngineUnitTests
{
    private readonly Mock<IEngineList> _engineListMock;
    private readonly Mock<IIterationsList> _iterationsListMock;
    private Engine _engine;

    public EngineUnitTests()
    {
        _engineListMock = new Mock<IEngineList>();
        _iterationsListMock = new Mock<IIterationsList>();

        _engine = new Engine(_engineListMock.Object, _iterationsListMock.Object);
    }

    [Fact]
    public void Should_Check_Guess_Greater()
    {
        var gameId = Guid.NewGuid().ToString();
        var playerId = "Zerep";
        var guess = 66;
        var mysteryNumber = 50;

        _engineListMock.Setup(s => s.GetValueFromPlayerGame(It.Is<string>(s => s.Equals(playerId)))).Returns(mysteryNumber);

        _iterationsListMock.Setup(s => s.Add(It.Is<string>(s=> s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId)), It.Is<int>(i => i == guess), It.Is<int>(i => i == mysteryNumber)));

        var (hasFinished, message) = _engine.CheckGuess(gameId, playerId, guess);

        hasFinished.Should().BeFalse();
        message.Should().Be($"The mystery number is less than your guess({guess}), player {playerId}.");


        _iterationsListMock.Verify(v => v.Add(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId)), It.Is<int>(i => i == guess), It.Is<int>(i => i == mysteryNumber)), Times.Once);
        _iterationsListMock.Verify(v => v.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);

        _engineListMock.Verify(v => v.GetValueFromPlayerGame(It.Is<string>(s => s.Equals(playerId))), Times.Once);
        _engineListMock.Verify(v => v.GetValueFromPlayerGame(It.IsAny<string>()), Times.Once);

        _iterationsListMock.Verify(v => v.Count(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId))), Times.Never);
        _iterationsListMock.Verify(v => v.Count(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        _engineListMock.Verify(v => v.ResetPlayerGame(It.Is<string>(s => s.Equals(playerId))), Times.Never);
        _engineListMock.Verify(v => v.ResetPlayerGame(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Should_Check_Guess_Less()
    {
        var gameId = Guid.NewGuid().ToString();
        var playerId = "Zerep";
        var guess = 15;
        var mysteryNumber = 30;

        _engineListMock.Setup(s => s.GetValueFromPlayerGame(It.Is<string>(s => s.Equals(playerId)))).Returns(mysteryNumber);

        _iterationsListMock.Setup(s => s.Add(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId)), It.Is<int>(i => i == guess), It.Is<int>(i => i == mysteryNumber)));

        var (hasFinished, message) = _engine.CheckGuess(gameId, playerId, guess);

        hasFinished.Should().BeFalse();
        message.Should().Be($"The mystery number is greater than your guess({guess}), player {playerId}.");


        _iterationsListMock.Verify(v => v.Add(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId)), It.Is<int>(i => i == guess), It.Is<int>(i => i == mysteryNumber)), Times.Once);
        _iterationsListMock.Verify(v => v.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);

        _engineListMock.Verify(v => v.GetValueFromPlayerGame(It.Is<string>(s => s.Equals(playerId))), Times.Once);
        _engineListMock.Verify(v => v.GetValueFromPlayerGame(It.IsAny<string>()), Times.Once);

        _iterationsListMock.Verify(v => v.Count(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId))), Times.Never);
        _iterationsListMock.Verify(v => v.Count(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        _engineListMock.Verify(v => v.ResetPlayerGame(It.Is<string>(s => s.Equals(playerId))), Times.Never);
        _engineListMock.Verify(v => v.ResetPlayerGame(It.IsAny<string>()), Times.Never);
    }


    [Fact]
    public void Should_Check_Guess_Equal()
    {
        var gameId = Guid.NewGuid().ToString();
        var playerId = "Zerep";
        var guess = 56;
        var mysteryNumber = 56;
        var numberIterations = 7;

        _engineListMock.Setup(s => s.GetValueFromPlayerGame(It.Is<string>(s => s.Equals(playerId)))).Returns(mysteryNumber);

        _iterationsListMock.Setup(s => s.Add(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId)), It.Is<int>(i => i == guess), It.Is<int>(i => i == mysteryNumber)));
        _iterationsListMock.Setup(v => v.Count(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId)))).Returns(numberIterations);

        var (hasFinished, message) = _engine.CheckGuess(gameId, playerId, guess);

        hasFinished.Should().BeTrue();
        message.Should().Be($"You have successfully found the mystery number({mysteryNumber}). You needed {numberIterations} iterations.");

        _iterationsListMock.Verify(v => v.Add(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId)), It.Is<int>(i => i == guess), It.Is<int>(i => i == mysteryNumber)), Times.Once);
        _iterationsListMock.Verify(v => v.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);

        _engineListMock.Verify(v => v.GetValueFromPlayerGame(It.Is<string>(s => s.Equals(playerId))), Times.Once);
        _engineListMock.Verify(v => v.GetValueFromPlayerGame(It.IsAny<string>()), Times.Once);

        _iterationsListMock.Verify(v => v.Count(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId))), Times.Once);
        _iterationsListMock.Verify(v => v.Count(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        _engineListMock.Verify(v => v.ResetPlayerGame(It.Is<string>(s => s.Equals(playerId))), Times.Once);
        _engineListMock.Verify(v => v.ResetPlayerGame(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Should_Fail_Check_Guess_Get_Value_Throw_Exception()
    {
        var gameId = Guid.NewGuid().ToString();
        var playerId = "Zerep";
        var guess = 56;
        var mysteryNumber = 56;
        var errorMessage = "random error message";

        _engineListMock.Setup(s => s.GetValueFromPlayerGame(It.Is<string>(s => s.Equals(playerId)))).Throws(new Exception(errorMessage));

        _iterationsListMock.Setup(s => s.Add(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId)), It.Is<int>(i => i == guess), It.Is<int>(i => i == mysteryNumber)));

        Action act = () => _engine.CheckGuess(gameId, playerId, guess);       

        act.Invoking(y => y.Invoke())
        .Should().Throw<Exception>()
        .WithMessage(errorMessage);

        _iterationsListMock.Verify(v => v.Add(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId)), It.Is<int>(i => i == guess), It.Is<int>(i => i == mysteryNumber)), Times.Never);
        _iterationsListMock.Verify(v => v.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);

        _engineListMock.Verify(v => v.GetValueFromPlayerGame(It.Is<string>(s => s.Equals(playerId))), Times.Once);
        _engineListMock.Verify(v => v.GetValueFromPlayerGame(It.IsAny<string>()), Times.Once);

        _iterationsListMock.Verify(v => v.Count(It.Is<string>(s => s.Equals(gameId)), It.Is<string>(s => s.Equals(playerId))), Times.Never);
        _iterationsListMock.Verify(v => v.Count(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        _engineListMock.Verify(v => v.ResetPlayerGame(It.Is<string>(s => s.Equals(playerId))), Times.Never);
        _engineListMock.Verify(v => v.ResetPlayerGame(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Should_Start_Game()
    {
        var playerId = "Zerep";     

        _engineListMock.Setup(s => s.StartPlayerGame(It.Is<string>(s => s.Equals(playerId))));

        var (gameId, message) = _engine.StartGame(playerId);

        gameId.Should().NotBeNullOrWhiteSpace();
        message.Should().Be($"You can start your guess's player {playerId}.");

        _engineListMock.Verify(v => v.StartPlayerGame(It.Is<string>(s => s.Equals(playerId))), Times.Once);
        _engineListMock.Verify(v => v.StartPlayerGame(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Should_Fail_Start_Game_Throw_Exception()
    {
        var playerId = "Zerep";
        var errorMessage = "random error message";

        _engineListMock.Setup(s => s.StartPlayerGame(It.Is<string>(s => s.Equals(playerId)))).Throws(new Exception(errorMessage));

        Action act = () => _engine.StartGame(playerId);

        act.Invoking(y => y.Invoke())
        .Should().Throw<Exception>()
        .WithMessage(errorMessage);

        _engineListMock.Verify(v => v.StartPlayerGame(It.Is<string>(s => s.Equals(playerId))), Times.Once);
        _engineListMock.Verify(v => v.StartPlayerGame(It.IsAny<string>()), Times.Once);
    }
}