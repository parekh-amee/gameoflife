using GameOfLife.Core.Models;
using GameOfLife.Core.Services;

namespace GameOfLife.Tests.Core;

[TestFixture]
public class GameSimulationServiceTests
{
    private GameSimulationService _simulationService = null!;

    [SetUp]
    public void SetUp()
    {
        _simulationService = new GameSimulationService(new GameEngine(),new GameBoardService(),new GameBoardFactory(30));
    }

    [Test]
    public void CreateNextGeneration_WhenBoardIsEmpty_ShouldRemainEmpty()
    {
        // Arrange
        GameBoard board = new()
        {
            Rows = 3,
            Columns = 3,
            Cells = new bool[3, 3]
        };

        // Act
        GameBoard result = _simulationService.CreateNextGeneration(board);

        // Assert
        Assert.That(result.Rows, Is.EqualTo(3));
        Assert.That(result.Columns, Is.EqualTo(3));
        Assert.That(result.Cells.Cast<bool>(), Is.All.False);
    }

    [Test]
    public void CreateNextGeneration_WhenSingleCellIsAlive_ShouldDieFromUnderpopulation()
    {
        // Arrange
        GameBoard board = new()
        {
            Rows = 3,
            Columns = 3,
            Cells = new bool[,]
            {
                { false, false, false },
                { false, true, false },
                { false, false, false }
            }
        };

        // Act
        GameBoard result = _simulationService.CreateNextGeneration(board);

        // Assert
        Assert.That(result.Cells.Cast<bool>(), Is.All.False);
    }

    [Test]
    public void CreateNextGeneration_WhenHorizontalBlinker_ShouldBecomeVertical()
    {
        // Arrange
        GameBoard board = new()
        {
            Rows = 3,
            Columns = 3,
            Cells = new bool[,]
            {
                { false, false, false },
                { true, true, true },
                { false, false, false }
            }
        };

        // Act
        GameBoard result = _simulationService.CreateNextGeneration(board);

        // Assert
        bool[,] expected =
        {
            { false, true, false },
            { false, true, false },
            { false, true, false }
        };

        AssertBoardsEqual(expected, result.Cells);
    }

    [Test]
    public void CreateNextGeneration_WhenStableBlock_ShouldRemainUnchanged()
    {
        // Arrange
        GameBoard board = new()
        {
            Rows = 4,
            Columns = 4,
            Cells = new bool[,]
            {
                { false, false, false, false },
                { false, true, true, false },
                { false, true, true, false },
                { false, false, false, false }
            }
        };

        // Act
        GameBoard result = _simulationService.CreateNextGeneration(board);

        // Assert
        AssertBoardsEqual(board.Cells, result.Cells);
    }

    [Test]
    public void CreateNextGeneration_ShouldNotModifyOriginalBoard()
    {
        // Arrange
        bool[,] originalCells =
        {
            { false, false, false },
            { true, true, true },
            { false, false, false }
        };

        GameBoard board = new()
        {
            Rows = 3,
            Columns = 3,
            Cells = (bool[,])originalCells.Clone()
        };

        // Act
        _simulationService.CreateNextGeneration(board);

        // Assert
        AssertBoardsEqual(originalCells, board.Cells);
    }

    private static void AssertBoardsEqual(bool[,] expected,bool[,] actual)
    {
        Assert.That(actual.GetLength(0), Is.EqualTo(expected.GetLength(0)));
        Assert.That(actual.GetLength(1), Is.EqualTo(expected.GetLength(1)));

        for (int row = 0; row < expected.GetLength(0); row++)
        {
            for (int column = 0; column < expected.GetLength(1); column++)
            {
                Assert.That(actual[row, column],Is.EqualTo(expected[row, column]),$"Mismatch at row {row}, column {column}");
            }
        }
    }
}
