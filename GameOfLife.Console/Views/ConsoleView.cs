using System.Text;
using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.Core.Constants;
using GameOfLife.Core.Models;
using GameOfLife.Core.Services;

namespace GameOfLife.ConsoleApp.Views;

// Responsible for console input and output.
public class ConsoleView
{
    private readonly GameBoardService _boardService;
    private readonly TextReader _input;
    private readonly TextWriter _output;

    public ConsoleView(GameBoardService boardService,TextReader? input = null,TextWriter? output = null)
    {
        _boardService = boardService;
        _input = input ?? Console.In;
        _output = output ?? Console.Out;
    }

    public void DisplayMenu(GameSettings settings)
    {
        StringBuilder text = new();

        text.AppendLine(GameTexts.Title);
        text.AppendLine(GameTexts.SelectBoardSize);

        text.AppendLine(string.Format(GameTexts.SmallBoardFormat,GameTexts.SmallChoice,settings.SmallBoardSize));

        text.AppendLine(string.Format(GameTexts.MediumBoardFormat,GameTexts.MediumChoice,settings.MediumBoardSize));

        text.AppendLine(string.Format(GameTexts.LargeBoardFormat,GameTexts.LargeChoice,settings.LargeBoardSize));

        text.AppendLine(string.Format(GameTexts.CustomBoardFormat,GameTexts.CustomChoice));

        _output.WriteLine(text.ToString());
    }

    public string? ReadMenuChoice() => _input.ReadLine();

    public bool ConfirmRestore()
    {
        _output.Write(GameTexts.RestorePrompt);

        return string.Equals(_input.ReadLine()?.Trim(),GameTexts.Yes,StringComparison.OrdinalIgnoreCase);
    }

    public int ReadDimension(string prompt, GameSettings settings)
    {
        while (true)
        {
            _output.WriteLine(prompt);

            if (int.TryParse(_input.ReadLine(), out int value) && value >= settings.MinimumBoardSize && value <= settings.MaximumBoardSize)
            {
                return value;
            }

            DisplayMessage(GameTexts.InvalidSize);

            DisplayMessage(string.Format(GameTexts.SizeRangeFormat, settings.MinimumBoardSize, settings.MaximumBoardSize));
        }
    }

    public void DisplayBoard(GameBoard board,int generation,GameSettings settings)
    {
        StringBuilder frame = new();

        frame.AppendLine(GameTexts.Title);

        frame.AppendLine(string.Format(GameTexts.GenerationFormat,generation));

        frame.AppendLine(string.Format(GameTexts.LiveCellsFormat, _boardService.CountLiveCells(board)));

        frame.AppendLine();

        for (int row = 0; row < board.Rows; row++)
        {
            for (int column = 0; column < board.Columns; column++)
            {
                frame.Append(
                    board.Cells[row, column] ? GameTexts.AliveCell : GameTexts.DeadCell);
            }

            frame.AppendLine();
        }

        frame.AppendLine(GameTexts.Legend);

        frame.AppendLine(string.Format(GameTexts.PausePromptFormat,settings.PauseKey));

        frame.AppendLine(string.Format(GameTexts.SavePromptFormat,settings.SaveKey));

        frame.AppendLine(string.Format(GameTexts.QuitPromptFormat,settings.QuitKey));

        _output.Write(frame.ToString());
    }

    public void DisplayPaused(GameSettings settings)
    {
        DisplayMessage(string.Format(GameTexts.PausedFormat,settings.PauseKey,settings.SaveKey,settings.QuitKey));
    }

    public void DisplayMessage(string message)
    {
        _output.WriteLine(message);
    }

    public bool HasKeyAvailable() => Console.KeyAvailable;

    public ConsoleKey ReadKey() => Console.ReadKey(intercept: true).Key;

    public void ClearScreen()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
    }

    public void SetCursorVisible(bool visible) => Console.CursorVisible = visible;
}
