using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Models;
using GameOfLife.ConsoleApp.Services;
using GameOfLife.ConsoleApp.Views;

namespace GameOfLife.ConsoleApp.Controllers;

public sealed class GameController
{
    private readonly GameSettings _settings;
    private readonly ConsoleView _view;
    private readonly GameBoardFactory _boardFactory;
    private readonly GameEngine _engine;
    private readonly GameStateStorage _storage;

    public GameController(
        GameSettings settings,
        ConsoleView view,
        GameBoardFactory boardFactory,
        GameEngine engine,
        GameStateStorage storage)
    {
        _settings = settings;
        _view = view;
        _boardFactory = boardFactory;
        _engine = engine;
        _storage = storage;
    }

    public void Run()
    {
        GameBoard board;
        int generation;

        GameSaveData? savedGame = _storage.Load();

        if (savedGame is not null && _view.ConfirmRestoreSavedGame())
        {
            board = RestoreBoard(savedGame);
            generation = savedGame.Generation;
        }
        else
        {
            (int Rows, int Columns) dimensions = SelectBoardDimensions();

            if (dimensions.Rows == 0)
                return;

            board = _boardFactory.CreateRandomBoard(
                dimensions.Rows,
                dimensions.Columns);

            generation = 0;
        }

        bool alternateScreenEnabled = false;

        try
        {
            Console.Write(GameTexts.EnterAlternateScreen);
            alternateScreenEnabled = true;
            Console.CursorVisible = false;

            _storage.Save(board, generation);

            while (true)
            {
                Console.Clear();
                _view.DisplayBoard(board, generation);

                if (HandleInput(board, generation))
                    break;

                Thread.Sleep(_settings.UpdateIntervalMilliseconds);

                if (HandleInput(board, generation))
                    break;

                board = _engine.NextGeneration(board);
                generation++;

                // Keep the most recent generation available for restoration.
                _storage.Save(board, generation);
            }
        }
        finally
        {
            Console.CursorVisible = true;

            if (alternateScreenEnabled)
                Console.Write(GameTexts.ExitAlternateScreen);
        }

        _view.DisplayMessage(GameTexts.QuitMessage);
    }

    private bool HandleInput(GameBoard board, int generation)
    {
        try
        {
            while (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(intercept: true).Key;

                if (key == ConsoleKey.Q)
                {
                    _storage.Save(board, generation);
                    return true;
                }

                if (key == ConsoleKey.S)
                {
                    bool saved = _storage.Save(board, generation);

                    _view.DisplayMessage(
                        saved
                            ? GameTexts.SaveSuccess
                            : GameTexts.SaveFailure);
                }
            }
        }
        catch (InvalidOperationException)
        {
            return false;
        }

        return false;
    }

    private GameBoard RestoreBoard(GameSaveData data)
    {
        GameBoard board = new(data.Rows, data.Columns);

        for (int row = 0; row < data.Rows; row++)
        {
            for (int column = 0; column < data.Columns; column++)
            {
                board.SetCell(
                    row,
                    column,
                    data.Cells[row][column]);
            }
        }

        return board;
    }

    private (int Rows, int Columns) SelectBoardDimensions()
    {
        _view.DisplayMenu(_settings);

        string? choice = _view.ReadMenuChoice()?.Trim();

        switch (choice)
        {
            case GameTexts.SmallBoardChoice:
                return (
                    _settings.SmallBoardSize,
                    _settings.SmallBoardSize);

            case GameTexts.MediumBoardChoice:
                return (
                    _settings.MediumBoardSize,
                    _settings.MediumBoardSize);

            case GameTexts.LargeBoardChoice:
                return (
                    _settings.LargeBoardSize,
                    _settings.LargeBoardSize);

            case GameTexts.CustomBoardChoice:
                int rows = _view.ReadDimension(
                    GameTexts.EnterRows,
                    _settings);

                int columns = _view.ReadDimension(
                    GameTexts.EnterColumns,
                    _settings);

                return (rows, columns);

            default:
                _view.DisplayMessage(GameTexts.InvalidChoice);
                return (0, 0);
        }
    }
}
