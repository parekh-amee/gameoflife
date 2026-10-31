using System.Text;
using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Models;

namespace GameOfLife.ConsoleApp.Views;

public sealed class ConsoleView
{
    public void DisplayMenu(GameSettings settings)
    {
        StringBuilder menu = new();

        menu.AppendLine(GameTexts.Title);
        menu.AppendLine();
        menu.AppendLine(GameTexts.SelectBoardSize);

        menu.AppendLine(string.Format(
            GameTexts.SmallBoardFormat,
            GameTexts.SmallBoardChoice,
            settings.SmallBoardSize));

        menu.AppendLine(string.Format(
            GameTexts.MediumBoardFormat,
            GameTexts.MediumBoardChoice,
            settings.MediumBoardSize));

        menu.AppendLine(string.Format(
            GameTexts.LargeBoardFormat,
            GameTexts.LargeBoardChoice,
            settings.LargeBoardSize));

        menu.AppendLine(string.Format(
            GameTexts.CustomBoardFormat,
            GameTexts.CustomBoardChoice));

        Console.Write(menu.ToString());
    }

    public string? ReadMenuChoice()
    {
        return Console.ReadLine();
    }

    public bool ConfirmRestoreSavedGame()
    {
        Console.Write(GameTexts.RestorePrompt);

        string? answer = Console.ReadLine();

        return string.Equals(
            answer?.Trim(),
            "Y",
            StringComparison.OrdinalIgnoreCase);
    }

    public int ReadDimension(
        string prompt,
        GameSettings settings)
    {
        while (true)
        {
            Console.WriteLine(prompt);

            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value) &&
                value >= settings.MinimumBoardSize &&
                value <= settings.MaximumBoardSize)
            {
                return value;
            }

            DisplayMessage(GameTexts.InvalidSize);

            DisplayMessage(string.Format(
                GameTexts.BoardSizeRangeFormat,
                settings.MinimumBoardSize,
                settings.MaximumBoardSize));
        }
    }

    public void DisplayBoard(
        GameBoard board,
        int generation)
    {
        StringBuilder frame = new();

        frame.AppendLine(GameTexts.Title);

        frame.AppendLine(
            $"{GameTexts.GenerationPrefix}{generation}");

        frame.AppendLine(
            $"{GameTexts.LiveCellsPrefix}{board.CountLiveCells()}");

        frame.AppendLine();

        for (int row = 0; row < board.Rows; row++)
        {
            for (int column = 0;
                 column < board.Columns;
                 column++)
            {
                frame.Append(
                    board.IsAlive(row, column)
                        ? GameTexts.AliveCell
                        : GameTexts.DeadCell);
            }

            frame.AppendLine();
        }

        frame.AppendLine(GameTexts.AliveLegend);
        frame.AppendLine(GameTexts.PausePrompt);
        frame.AppendLine(GameTexts.SavePrompt);
        frame.AppendLine(GameTexts.QuitPrompt);

        Console.Write(frame.ToString());
    }

    public void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }
}