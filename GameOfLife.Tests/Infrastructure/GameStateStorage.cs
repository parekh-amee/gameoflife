using System.Text.Json;
using GameOfLife.Core.Models;
using GameOfLife.Infrastructure;

namespace GameOfLife.Tests.Infrastructure;

[TestFixture]
public class GameStateStorageTests
{
    private string _testDirectory = null!;
    private string _filePath = null!;
    private GameStateStorage _storage = null!;

    [SetUp]
    public void SetUp()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(),Guid.NewGuid().ToString());

        Directory.CreateDirectory(_testDirectory);

        _filePath = Path.Combine(_testDirectory, "game-save.json");

        _storage = new GameStateStorage(_filePath);
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, recursive: true);
        }
    }

    [Test]
    public void Save_WhenGivenValidBoard_ShouldReturnTrue()
    {
        // Arrange
        GameBoard board = CreateBoard();

        // Act
        bool result = _storage.Save(board, 5);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(File.Exists(_filePath), Is.True);
    }

    [Test]
    public void Load_WhenSaveFileExists_ShouldReturnSavedGameData()
    {
        // Arrange
        GameBoard board = CreateBoard();

        _storage.Save(board, 5);

        // Act
        GameSaveData? result = _storage.Load();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Rows, Is.EqualTo(2));
        Assert.That(result.Columns, Is.EqualTo(2));
        Assert.That(result.Generation, Is.EqualTo(5));
    }

    [Test]
    public void SaveAndLoad_ShouldPreserveBoardCells()
    {
        // Arrange
        GameBoard board = CreateBoard();

        // Act
        _storage.Save(board, 5);

        GameSaveData? result = _storage.Load();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Cells[0][0], Is.True);
        Assert.That(result.Cells[0][1], Is.False);
        Assert.That(result.Cells[1][0], Is.False);
        Assert.That(result.Cells[1][1], Is.True);
    }

    [Test]
    public void Load_WhenFileDoesNotExist_ShouldReturnNull()
    {
        // Act
        GameSaveData? result = _storage.Load();

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Save_WhenDirectoryDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        string invalidPath = Path.Combine(_testDirectory,"missing","game-save.json");

        GameStateStorage storage = new(invalidPath);

        GameBoard board = CreateBoard();

        // Act
        bool result = storage.Save(board, 5);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Load_WhenJsonIsInvalid_ShouldThrowJsonException()
    {
        // Arrange
        File.WriteAllText(_filePath, "{ invalid json");

        // Act & Assert
        Assert.Throws<JsonException>(() => _storage.Load());
    }

    [Test]
    public void Load_WhenSavedDataIsNull_ShouldThrowInvalidDataException()
    {
        // Arrange
        File.WriteAllText(_filePath, "null");

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _storage.Load());
    }

    [TestCase(0, 2)]
    [TestCase(2, 0)]
    [TestCase(-1, 2)]
    [TestCase(2, -1)]
    public void Load_WhenDimensionsAreInvalid_ShouldThrowInvalidDataException(int rows,int columns)
    {
        // Arrange
        string json = $$"""
        {
            "Rows": {{rows}},
            "Columns": {{columns}},
            "Generation": 5,
            "Cells": [[true, false], [false, true]]
        }
        """;

        File.WriteAllText(_filePath, json);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _storage.Load());
    }

    [Test]
    public void Load_WhenGenerationIsNegative_ShouldThrowInvalidDataException()
    {
        // Arrange
        string json = """
        {
            "Rows": 2,
            "Columns": 2,
            "Generation": -1,
            "Cells": [[true, false], [false, true]]
        }
        """;

        File.WriteAllText(_filePath, json);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _storage.Load());
    }

    [Test]
    public void Load_WhenCellsAreNull_ShouldThrowInvalidDataException()
    {
        // Arrange
        string json = """
        {
            "Rows": 2,
            "Columns": 2,
            "Generation": 5,
            "Cells": null
        }
        """;

        File.WriteAllText(_filePath, json);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _storage.Load());
    }

    [Test]
    public void Load_WhenNumberOfRowsDoesNotMatch_ShouldThrowInvalidDataException()
    {
        // Arrange
        string json = """
        {
            "Rows": 2,
            "Columns": 2,
            "Generation": 5,
            "Cells": [[true, false]]
        }
        """;

        File.WriteAllText(_filePath, json);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _storage.Load());
    }

    [Test]
    public void Load_WhenRowIsNull_ShouldThrowInvalidDataException()
    {
        // Arrange
        string json = """
        {
            "Rows": 2,
            "Columns": 2,
            "Generation": 5,
            "Cells": [null, [false, true]]
        }
        """;

        File.WriteAllText(_filePath, json);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _storage.Load());
    }

    [Test]
    public void Load_WhenRowLengthDoesNotMatch_ShouldThrowInvalidDataException()
    {
        // Arrange
        string json = """
        {
            "Rows": 2,
            "Columns": 2,
            "Generation": 5,
            "Cells": [[true], [false, true]]
        }
        """;

        File.WriteAllText(_filePath, json);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => _storage.Load());
    }

    private static GameBoard CreateBoard()
    {
        return new GameBoard
        {
            Rows = 2,
            Columns = 2,
            Cells = new bool[,]
            {
                { true, false },
                { false, true }
            }
        };
    }
}
