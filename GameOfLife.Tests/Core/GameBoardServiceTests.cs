using GameOfLife.Core.Models;
using GameOfLife.Core.Services;

namespace GameOfLife.Tests.Core;

[TestFixture]
public class GameBoardServiceTests
{
    private GameBoardService _boardService = null!;

    [SetUp]
    public void SetUp()
    {
        _boardService = new GameBoardService();
    }

    [Test]
    public void CountLiveCells_WhenBoardIsEmpty_ShouldReturnZero()
    {
        // Arrange
        GameBoard board = new()
        {
            Rows = 0,
            Columns = 0,
            Cells = new bool[0, 0]
        };

        // Act
        int result = _boardService.CountLiveCells(board);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void CountLiveCells_WhenAllCellsAreDead_ShouldReturnZero()
    {
        // Arrange
        GameBoard board = new()
        {
            Rows = 2,
            Columns = 2,
            Cells = new bool[2, 2]
        };

        // Act
        int result = _boardService.CountLiveCells(board);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void CountLiveCells_WhenAllCellsAreAlive_ShouldReturnTotalCellCount()
    {
        // Arrange
        GameBoard board = new()
        {
            Rows = 2,
            Columns = 3,
            Cells = new bool[,]
            {
                { true, true, true },
                { true, true, true }
            }
        };

        // Act
        int result = _boardService.CountLiveCells(board);

        // Assert
        Assert.That(result, Is.EqualTo(6));
    }

    [Test]
    public void CountLiveCells_WhenBoardHasMixedCells_ShouldReturnLiveCellCount()
    {
        // Arrange
        GameBoard board = new()
        {
            Rows = 2,
            Columns = 3,
            Cells = new bool[,]
            {
                { true, false, true },
                { false, true, false }
            }
        };

        // Act
        int result = _boardService.CountLiveCells(board);

        // Assert
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void CountLiveNeighbors_WhenAllSurroundingCellsAreAlive_ShouldReturnEight()
    {
        // Arrange
        GameBoard board = new()
        {
            Rows = 3,
            Columns = 3,
            Cells = new bool[,]
            {
                { true, true, true },
                { true, false, true },
                { true, true, true }
            }
        };

        // Act
        int result = _boardService.CountLiveNeighbors(board, 1, 1);

        // Assert
        Assert.That(result, Is.EqualTo(8));
    }

    [Test]
    public void CountLiveNeighbors_WhenCellIsAtEdge_ShouldCountOnlyValidNeighbors()
    {
        // Arrange
        GameBoard board = new()
        {
            Rows = 3,
            Columns = 3,
            Cells = new bool[,]
            {
                { true, true, true },
                { true, false, true },
                { false, false, false }
            }
        };

        // Act
        int result = _boardService.CountLiveNeighbors(board, 0, 1);

        // Assert
        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void CountLiveNeighbors_WhenOnlyCurrentCellIsAlive_ShouldReturnZero()
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
        int result = _boardService.CountLiveNeighbors(board, 1, 1);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void CountLiveNeighbors_WhenCellIsAtCornerAndAllValidNeighborsAreAlive_ShouldReturnThree()
    {
        // Arrange
        GameBoard board = new()
        {
            Rows = 2,
            Columns = 2,
            Cells = new bool[,]
            {
                { false, true },
                { true, true }
            }
        };

        // Act
        int result = _boardService.CountLiveNeighbors(board, 0, 0);

        // Assert
        Assert.That(result, Is.EqualTo(3));
    }
}
