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
    private readonly GameBoardService _boardService;

    public GameController(
        GameSettings settings,
        ConsoleView view,
        GameBoardFactory boardFactory,
        GameEngine engine,
        GameStateStorage storage,
        GameBoardService boardService)
    {
        _settings = settings;
        _view = view;
        _boardFactory = boardFactory;
        _engine = engine;
        _storage = storage;
        _boardService = boardService;
    }

    public void Run()
    {
        GameBoard board;
        int generation;

        GameSaveData? savedGame = _storage.Load();

        if (savedGame is not null &&
            _view.ConfirmRestoreSavedGame())
        {
            board = RestoreBoard(savedGame);
            generation = savedGame.Generation;
        }
        else
        {
            (int Rows, int Columns) dimensions =
                SelectBoardDimensions();

            if (dimensions.Rows <= 0 ||
                dimensions.Columns <= 0)
            {
                return;
            }

            board = _boardFactory.CreateRandomBoard(
                dimensions.Rows,
                dimensions.Columns);

            generation = 0;
        }

        _storage.Save(board, generation);

        Console.CursorVisible = false;

        try
        {
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);

                _view.DisplayBoard(board, generation);

                if (HandleInput(board, generation))
                {
                    break;
                }

                if (WaitForNextGeneration(board, generation))
                {
                    break;
                }

                board = _engine.NextGeneration(board);
                generation++;

                _storage.Save(board, generation);
            }
        }
        finally
        {
            Console.CursorVisible = true;
        }

        _view.DisplayMessage(GameTexts.QuitMessage);
    }

    private bool HandleInput(
        GameBoard board,
        int generation)
    {
        try
        {
            while (Console.KeyAvailable)
            {
                ConsoleKey key =
                    Console.ReadKey(intercept: true).Key;

                if (key == _settings.QuitKey)
                {
                    _storage.Save(board, generation);
                    return true;
                }

                if (key == _settings.PauseKey)
                {
                    if (PauseGame(board, generation))
                    {
                        return true;
                    }
                }
                else if (key == _settings.SaveKey)
                {
                    bool saved =
                        _storage.Save(board, generation);

                    _view.DisplayMessage(
                        saved
                            ? GameTexts.SaveSuccess
                            : GameTexts.SaveFailure);
                }
            }
        }
        catch (InvalidOperationException)
        {
            // Console input is unavailable.
        }

        return false;
    }

    private bool PauseGame(
        GameBoard board,
        int generation)
    {
        _view.DisplayMessage(GameTexts.PausedMessage);

        while (true)
        {
            ConsoleKey key =
                Console.ReadKey(intercept: true).Key;

            if (key == _settings.PauseKey)
            {
                _view.DisplayMessage(
                    GameTexts.ResumedMessage);

                return false;
            }

            if (key == _settings.SaveKey)
            {
                bool saved =
                    _storage.Save(board, generation);

                _view.DisplayMessage(
                    saved
                        ? GameTexts.SaveSuccess
                        : GameTexts.SaveFailure);
            }

            if (key == _settings.QuitKey)
            {
                _storage.Save(board, generation);
                return true;
            }
        }
    }

    private bool WaitForNextGeneration(
        GameBoard board,
        int generation)
    {
        int inputCheckInterval =
            _settings.inputCheckIntervalMilliseconds;

        int elapsedMilliseconds = 0;

        while (elapsedMilliseconds <
               _settings.UpdateIntervalMilliseconds)
        {
            Thread.Sleep(inputCheckInterval);

            elapsedMilliseconds += inputCheckInterval;

            if (HandleInput(board, generation))
            {
                return true;
            }
        }

        return false;
    }

    private GameBoard RestoreBoard(GameSaveData data)
    {
        GameBoard board = new(
            data.Rows,
            data.Columns);

        for (int row = 0; row < data.Rows; row++)
        {
            for (int column = 0;
                 column < data.Columns;
                 column++)
            {
                _boardService.SetCell(
                    board,
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

        string? choice =
            _view.ReadMenuChoice()?.Trim();

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
                _view.DisplayMessage(
                    GameTexts.InvalidChoice);

                return (0, 0);
        }
    }
}
