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

            if (dimensions.Rows == 0)
            {
                return;
            }

            board = _boardFactory.CreateRandomBoard(
                dimensions.Rows,
                dimensions.Columns);

            generation = 0;
        }

        _storage.Save(board, generation);

        while (true)
        {
            Console.Clear();

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

                if (key == ConsoleKey.Q)
                {
                    _storage.Save(board, generation);
                    return true;
                }

                if (key == ConsoleKey.P)
                {
                    PauseGame(board, generation);
                }
                else if (key == ConsoleKey.S)
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

    private void PauseGame(
        GameBoard board,
        int generation)
    {
        _view.DisplayMessage(GameTexts.PausedMessage);

        while (true)
        {
            ConsoleKey key =
                Console.ReadKey(intercept: true).Key;

            if (key == ConsoleKey.P)
            {
                _view.DisplayMessage(
                    GameTexts.ResumedMessage);

                return;
            }

            if (key == ConsoleKey.S)
            {
                bool saved =
                    _storage.Save(board, generation);

                _view.DisplayMessage(
                    saved
                        ? GameTexts.SaveSuccess
                        : GameTexts.SaveFailure);
            }

            if (key == ConsoleKey.Q)
            {
                _storage.Save(board, generation);
                Environment.Exit(0);
            }
        }
    }

    private bool WaitForNextGeneration(
        GameBoard board,
        int generation)
    {
        const int inputCheckInterval = 50;

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
                board.SetCell(
                    row,
                    column,
                    data.Cells[row][column]);
            }
        }

        return board;
    }

    private (int Rows, int Columns)
        SelectBoardDimensions()
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