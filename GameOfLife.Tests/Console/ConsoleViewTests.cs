
using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Views;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Models;
using GameOfLife.Core.Services;

namespace GameOfLife.Tests.ConsoleTests;

[TestFixture]
public class ConsoleViewTests
{
    private StringReader _input = null!;
    private StringWriter _output = null!;

    [SetUp]
    public void SetUp()
    {
        _input = new StringReader(string.Empty);
        _output = new StringWriter();
    }

    [TearDown]
    public void TearDown()
    {
        _input.Dispose();
        _output.Dispose();
    }

    private ConsoleView CreateView(string input = "")
    {
        _input.Dispose();
        _input = new StringReader(input);

        return new ConsoleView(new GameBoardService(), _input, _output);
    }

    [Test]
    public void DisplayMenu_ShouldDisplayAllBoardOptions()
    {
        // Arrange
        ConsoleView view = CreateView();
        GameSettings settings = new();

        // Act
        view.DisplayMenu(settings);

        // Assert
        string result = _output.ToString();

        Assert.That(result, Does.Contain(GameTexts.Title));
        Assert.That(result, Does.Contain(GameTexts.SelectBoardSize));

        Assert.That(result, Does.Contain(string.Format(GameTexts.SmallBoardFormat,GameTexts.SmallChoice,settings.SmallBoardSize)));

        Assert.That(result, Does.Contain(string.Format(GameTexts.MediumBoardFormat,GameTexts.MediumChoice,settings.MediumBoardSize)));

        Assert.That(result, Does.Contain(string.Format(GameTexts.LargeBoardFormat,GameTexts.LargeChoice,settings.LargeBoardSize)));

        Assert.That(result, Does.Contain(string.Format(GameTexts.CustomBoardFormat,GameTexts.CustomChoice)));
    }

    [Test]
    public void ReadMenuChoice_ShouldReturnEnteredChoice()
    {
        // Arrange
        ConsoleView view = CreateView("2");

        // Act
        string? result = view.ReadMenuChoice();

        // Assert
        Assert.That(result, Is.EqualTo("2"));
    }

    [Test]
    public void ReadMenuChoice_WhenInputIsEmpty_ShouldReturnNull()
    {
        // Arrange
        ConsoleView view = CreateView();

        // Act
        string? result = view.ReadMenuChoice();

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void ConfirmRestore_WhenUserEntersYes_ShouldReturnTrue()
    {
        // Arrange
        ConsoleView view = CreateView("Y");

        // Act
        bool result = view.ConfirmRestore();

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_output.ToString(), Is.EqualTo(GameTexts.RestorePrompt));
    }

    [Test]
    public void ConfirmRestore_WhenUserEntersLowercaseYes_ShouldReturnTrue()
    {
        // Arrange
        ConsoleView view = CreateView("y");

        // Act
        bool result = view.ConfirmRestore();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void ConfirmRestore_WhenUserEntersNo_ShouldReturnFalse()
    {
        // Arrange
        ConsoleView view = CreateView("N");

        // Act
        bool result = view.ConfirmRestore();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void ConfirmRestore_WhenUserEntersYesWithWhitespace_ShouldReturnTrue()
    {
        // Arrange
        ConsoleView view = CreateView("  Y  ");

        // Act
        bool result = view.ConfirmRestore();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void ReadDimension_WhenValidInput_ShouldReturnDimension()
    {
        // Arrange
        ConsoleView view = CreateView("15");
        GameSettings settings = new();

        // Act
        int result = view.ReadDimension(GameTexts.EnterRows,settings);

        // Assert
        Assert.That(result, Is.EqualTo(15));
        Assert.That(_output.ToString(),Does.Contain(GameTexts.EnterRows));
    }

    [Test]
    public void ReadDimension_WhenInputIsNotANumber_ShouldAskAgain()
    {
        // Arrange
        ConsoleView view = CreateView("abc\n15");
        GameSettings settings = new();

        // Act
        int result = view.ReadDimension(GameTexts.EnterRows,settings);

        // Assert
        Assert.That(result, Is.EqualTo(15));
        Assert.That(_output.ToString(),Does.Contain(GameTexts.InvalidSize));
    }

    [Test]
    public void ReadDimension_WhenInputIsBelowMinimum_ShouldAskAgain()
    {
        // Arrange
        ConsoleView view = CreateView("0\n10");
        GameSettings settings = new();

        // Act
        int result = view.ReadDimension(GameTexts.EnterRows,settings);

        // Assert
        Assert.That(result, Is.EqualTo(10));
        Assert.That(_output.ToString(),Does.Contain(GameTexts.InvalidSize));
    }

    [Test]
    public void ReadDimension_WhenInputIsAboveMaximum_ShouldAskAgain()
    {
        // Arrange
        ConsoleView view = CreateView("101\n10");
        GameSettings settings = new();

        // Act
        int result = view.ReadDimension(GameTexts.EnterRows,settings);

        // Assert
        Assert.That(result, Is.EqualTo(10));
        Assert.That(_output.ToString(),Does.Contain(GameTexts.InvalidSize));
    }

    [Test]
    public void ReadDimension_WhenInputIsAtMinimum_ShouldReturnDimension()
    {
        // Arrange
        ConsoleView view = CreateView("1");
        GameSettings settings = new();

        // Act
        int result = view.ReadDimension(GameTexts.EnterRows,settings);

        // Assert
        Assert.That(result, Is.EqualTo(settings.MinimumBoardSize));
    }

    [Test]
    public void ReadDimension_WhenInputIsAtMaximum_ShouldReturnDimension()
    {
        // Arrange
        ConsoleView view = CreateView("100");
        GameSettings settings = new();

        // Act
        int result = view.ReadDimension(GameTexts.EnterRows,
settings);

        // Assert
        Assert.That(result, Is.EqualTo(settings.MaximumBoardSize));
    }

    [Test]
    public void DisplayBoard_ShouldDisplayGenerationAndLiveCellCount()
    {
        // Arrange
        ConsoleView view = CreateView();

        GameBoard board = new()
        {
            Rows = 2,
            Columns = 2,
            Cells = new bool[,]
            {
                { true, false },
                { true, true }
            }
        };

        // Act
        view.DisplayBoard(board, 5, new GameSettings());

        // Assert
        string result = _output.ToString();

        Assert.That(result,Does.Contain(string.Format(GameTexts.GenerationFormat, 5)));

        Assert.That(result,Does.Contain(string.Format(GameTexts.LiveCellsFormat, 3)));

        Assert.That(result, Does.Contain(GameTexts.AliveCell));
        Assert.That(result, Does.Contain(GameTexts.DeadCell));
        Assert.That(result, Does.Contain(GameTexts.Legend));
    }

    [Test]
    public void DisplayBoard_ShouldDisplayKeyboardInstructions()
    {
        // Arrange
        ConsoleView view = CreateView();

        GameBoard board = new()
        {
            Rows = 1,
            Columns = 1,
            Cells = new bool[,] { { true } }
        };

        GameSettings settings = new();

        // Act
        view.DisplayBoard(board, 0, settings);

        // Assert
        string result = _output.ToString();

        Assert.That(result,Does.Contain(string.Format(GameTexts.PausePromptFormat,settings.PauseKey)));

        Assert.That(result,Does.Contain(string.Format(GameTexts.SavePromptFormat,settings.SaveKey)));

        Assert.That(result,Does.Contain(string.Format(GameTexts.QuitPromptFormat, settings.QuitKey)));
    }

    [Test]
    public void DisplayPaused_ShouldDisplayPauseInstructions()
    {
        // Arrange
        ConsoleView view = CreateView();
        GameSettings settings = new();

        // Act
        view.DisplayPaused(settings);

        // Assert
        string expected = string.Format(GameTexts.PausedFormat,settings.PauseKey,settings.SaveKey,settings.QuitKey);

        Assert.That( _output.ToString(),Is.EqualTo(expected + Environment.NewLine));
    }

    [Test]
    public void DisplayMessage_ShouldWriteMessage()
    {
        // Arrange
        ConsoleView view = CreateView();
        string message = "Test message";

        // Act
        view.DisplayMessage(message);

        // Assert
        Assert.That( _output.ToString(),Is.EqualTo(message + Environment.NewLine));
    }
}
