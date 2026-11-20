using GameOfLife.Core.Services;

namespace GameOfLife.Tests.Core;

[TestFixture]
public class GameBoardFactoryTests
{
    [Test]
    public void CreateEmptyBoard_ShouldCreateBoardWithCorrectDimensions()
    {
        // Arrange
        GameBoardFactory factory = new(30);

        // Act
        var board = factory.CreateEmptyBoard(3, 4);

        // Assert
        Assert.That(board.Rows, Is.EqualTo(3));
        Assert.That(board.Columns, Is.EqualTo(4));
        Assert.That(board.Cells.GetLength(0), Is.EqualTo(3));
        Assert.That(board.Cells.GetLength(1), Is.EqualTo(4));
    }

    [Test]
    public void CreateEmptyBoard_ShouldCreateAllDeadCells()
    {
        // Arrange
        GameBoardFactory factory = new(30);

        // Act
        var board = factory.CreateEmptyBoard(2, 2);

        // Assert
        Assert.That(board.Cells.Cast<bool>(), Is.All.False);
    }

    [TestCase(0, 3)]
    [TestCase(3, 0)]
    [TestCase(-1, 3)]
    [TestCase(3, -1)]
    public void CreateEmptyBoard_WhenDimensionsAreInvalid_ShouldThrowException(int rows,int columns)
    {
        // Arrange
        GameBoardFactory factory = new(30);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => factory.CreateEmptyBoard(rows, columns));
    }

    [Test]
    public void CreateRandomBoard_WhenAlivePercentageIsZero_ShouldCreateAllDeadCells()
    {
        // Arrange
        GameBoardFactory factory = new(0);

        // Act
        var board = factory.CreateRandomBoard(3, 3);

        // Assert
        Assert.That(board.Cells.Cast<bool>(), Is.All.False);
    }

    [Test]
    public void CreateRandomBoard_WhenAlivePercentageIsOneHundred_ShouldCreateAllAliveCells()
    {
        // Arrange
        GameBoardFactory factory = new(100);

        // Act
        var board = factory.CreateRandomBoard(3, 3);

        // Assert
        Assert.That(board.Cells.Cast<bool>(), Is.All.True);
    }

    [TestCase(-1)]
    [TestCase(101)]
    public void Constructor_WhenAlivePercentageIsInvalid_ShouldThrowException(int percentage)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new GameBoardFactory(percentage));
    }

    [Test]
    public void CreateBoard_WhenGivenTwoDimensionalArray_ShouldCreateCorrectBoard()
    {
        // Arrange
        GameBoardFactory factory = new(30);

        bool[,] cells =
        {
            { true, false },
            { false, true }
        };

        // Act
        var board = factory.CreateBoard(cells);

        // Assert
        Assert.That(board.Rows, Is.EqualTo(2));
        Assert.That(board.Columns, Is.EqualTo(2));
        Assert.That(board.Cells[0, 0], Is.True);
        Assert.That(board.Cells[0, 1], Is.False);
        Assert.That(board.Cells[1, 0], Is.False);
        Assert.That(board.Cells[1, 1], Is.True);
    }

    [Test]
    public void CreateBoard_WhenGivenTwoDimensionalArray_ShouldCopyOriginalCells()
    {
        // Arrange
        GameBoardFactory factory = new(30);

        bool[,] cells =
        {
            { true, false },
            { false, true }
        };

        // Act
        var board = factory.CreateBoard(cells);
        cells[0, 0] = false;

        // Assert
        Assert.That(board.Cells[0, 0], Is.True);
    }

    [Test]
    public void CreateBoard_WhenGivenNullTwoDimensionalArray_ShouldThrowException()
    {
        // Arrange
        GameBoardFactory factory = new(30);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => factory.CreateBoard((bool[,])null!));
    }

    [Test]
    public void CreateBoard_WhenGivenJaggedArray_ShouldCreateCorrectBoard()
    {
        // Arrange
        GameBoardFactory factory = new(30);

        bool[][] cells =
        {
            new[] { true, false },
            new[] { false, true }
        };

        // Act
        var board = factory.CreateBoard(cells);

        // Assert
        Assert.That(board.Rows, Is.EqualTo(2));
        Assert.That(board.Columns, Is.EqualTo(2));
        Assert.That(board.Cells[0, 0], Is.True);
        Assert.That(board.Cells[0, 1], Is.False);
        Assert.That(board.Cells[1, 0], Is.False);
        Assert.That(board.Cells[1, 1], Is.True);
    }

    [Test]
    public void CreateBoard_WhenGivenNullJaggedArray_ShouldThrowException()
    {
        // Arrange
        GameBoardFactory factory = new(30);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => factory.CreateBoard((bool[][])null!));
    }

    [Test]
    public void CreateBoard_WhenJaggedArrayHasDifferentRowLengths_ShouldThrowException()
    {
        // Arrange
        GameBoardFactory factory = new(30);

        bool[][] cells =
        {
            new[] { true, false },
            new[] { true }
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => factory.CreateBoard(cells));
    }
}
