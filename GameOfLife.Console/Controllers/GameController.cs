using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Services;
using GameOfLife.ConsoleApp.Views;

namespace GameOfLife.ConsoleApp.Controllers;

public sealed class GameController
{
    private readonly GameSettings _settings;
    private readonly ConsoleView _view;
    private readonly GameBoardFactory _boardFactory;
    private readonly GameEngine _engine;

    public GameController(
        GameSettings settings,
        ConsoleView view,
        GameBoardFactory boardFactory,
        GameEngine engine)
    {
        _settings = settings;
        _view = view;
        _boardFactory = boardFactory;
        _engine = engine;
    }

    public void Run()
    {
        (int Rows, int Columns) dimensions = SelectBoardDimensions();

        if (dimensions.Rows == 0)
            return;

        var board = _boardFactory.CreateRandomBoard(
            dimensions.Rows,
            dimensions.Columns);

        int generation = 0;

        try
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);

                _view.DisplayBoard(board, generation);

                if (IsQuitRequested())
                    break;

                Thread.Sleep(_settings.UpdateIntervalMilliseconds);

                if (IsQuitRequested())
                    break;

                board = _engine.NextGeneration(board);
                generation++;

            }
        }
        finally
        {
            Console.CursorVisible = true;
            
        }

        _view.DisplayMessage(GameTexts.QuitMessage);
    }

    private bool IsQuitRequested()
    {
        try
        {
            while (Console.KeyAvailable)
            {
                if (Console.ReadKey(intercept: true).Key == ConsoleKey.Q)
                    return true;
            }
        }
        catch (InvalidOperationException)
        {
            return false;
        }

        return false;
    }

    private (int Rows, int Columns) SelectBoardDimensions()
    {
        _view.DisplayMenu(_settings);

        string? choice = _view.ReadMenuChoice()?.Trim().ToLowerInvariant();

        switch (choice)
        {
            case GameTexts.SmallBoardChoice:
                return (_settings.SmallBoardSize, _settings.SmallBoardSize);

            case GameTexts.MediumBoardChoice:
                return (_settings.MediumBoardSize, _settings.MediumBoardSize);

            case GameTexts.LargeBoardChoice:
                return (_settings.LargeBoardSize, _settings.LargeBoardSize);

            case GameTexts.CustomBoardChoice:
                int rows = _view.ReadDimension(GameTexts.EnterRows, _settings);
                int columns = _view.ReadDimension(GameTexts.EnterColumns, _settings);
                return (rows, columns);

            default:
                _view.DisplayMessage(GameTexts.InvalidChoice);
                return (0, 0);
        }
    }
}