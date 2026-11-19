using GameOfLife.Core.Services;

namespace GameOfLife.Tests.Core;

[TestFixture]
public class GameEngineTests
{
    private GameEngine _gameEngine = null!;

    [SetUp]
    public void SetUp()
    {
        _gameEngine = new GameEngine();
    }
    //same test logic with different inputs
    [TestCase(true, 0, false)]
    [TestCase(true, 1, false)]
    [TestCase(true, 2, true)]
    [TestCase(true, 3, true)]
    [TestCase(true, 4, false)]
    [TestCase(true, 5, false)]
    [TestCase(true, 6, false)]
    [TestCase(true, 7, false)]
    [TestCase(true, 8, false)]
    [TestCase(false, 0, false)]
    [TestCase(false, 1, false)]
    [TestCase(false, 2, false)]
    [TestCase(false, 3, true)]
    [TestCase(false, 4, false)]
    [TestCase(false, 5, false)]
    [TestCase(false, 6, false)]
    [TestCase(false, 7, false)]
    [TestCase(false, 8, false)]
    public void CalculateNextCellState_ShouldFollowConwaysRules(bool isAlive,int liveNeighbors,bool expected)
    {
        // Arrange
        // the current cell state and its live neighbour count are provided as parameters to the test case

        // Act
        bool result = _gameEngine.CalculateNextCellState(isAlive,liveNeighbors);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}
