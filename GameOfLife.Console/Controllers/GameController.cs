using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Models;
using GameOfLife.ConsoleApp.Services;
using GameOfLife.ConsoleApp.Views;

namespace GameOfLife.ConsoleApp.Controllers;

// mvc controller: coordinates user actions, application services and the view.
public class GameController
{
    private readonly GameSettings _settings;
    private readonly ConsoleView _view;
    private readonly GameBoardFactory _boardFactory;
    private readonly GameSimulationService _simulation;
    private readonly GameStateStorage _storage;

    public GameController(GameSettings settings,
        ConsoleView view,
        GameBoardFactory boardFactory,
        GameSimulationService simulation,
        GameStateStorage storage)
    {
        _settings = settings;
        _view = view;
        _boardFactory = boardFactory;
        _simulation = simulation;
        _storage = storage;
    }

    public void Run()
    {
        GameBoard? board = StartGame(out int generation);

        if (board is null)
        {
            return;
        }

        _view.SetCursorVisible(false);

        try
        {
            while (true)
            {
                _view.ClearScreen();
                _view.DisplayBoard(board, generation, _settings);

                Thread.Sleep(_settings.UpdateIntervalMilliseconds);

                if (ProcessAvailableInput(board, generation))
                {
                    break;
                }
                
                board = _simulation.CreateNextGeneration(board);
                generation++;
            }
        }
        finally
        {
            _view.SetCursorVisible(true);
        }

        _view.DisplayMessage(GameTexts.Stopped);
    }

    private GameBoard? StartGame(out int generation)
    {
        GameSaveData? savedGame = _storage.Load();

        if (savedGame is not null && _view.ConfirmRestore())
        {
            generation = savedGame.Generation;
            return _boardFactory.CreateBoard(savedGame.Cells);
        }

        (int rows, int columns) = SelectBoardDimensions();
        generation = 0;

        return rows > 0 && columns > 0 ? _boardFactory.CreateRandomBoard(rows, columns) : null;
    }

    private (int Rows, int Columns) SelectBoardDimensions()
    {
        while (true)
        {
            _view.DisplayMenu(_settings);
            string? choice = _view.ReadMenuChoice()?.Trim();

            switch (choice)
            {
                case GameTexts.SmallChoice:
                    return (_settings.SmallBoardSize, _settings.SmallBoardSize);

                case GameTexts.MediumChoice:
                    return (_settings.MediumBoardSize, _settings.MediumBoardSize);

                case GameTexts.LargeChoice:
                    return (_settings.LargeBoardSize, _settings.LargeBoardSize);

                case GameTexts.CustomChoice:
                    int rows = _view.ReadDimension(GameTexts.EnterRows, _settings);
                    int columns = _view.ReadDimension(GameTexts.EnterColumns, _settings);
                    return (rows, columns);

                default:
                    _view.DisplayMessage(GameTexts.InvalidChoice);
                    break;
            }
        }
    }

    private bool ProcessAvailableInput(GameBoard board, int generation)
    {
        while (_view.HasKeyAvailable())
        {
            ConsoleKey key = _view.ReadKey();

            if (key == _settings.QuitKey)
            {
                return true;
            }

            if (key == _settings.SaveKey)
            {
                SaveGame(board, generation);
            }
            else if (key == _settings.PauseKey && PauseGame(board, generation))
            {
                return true;
            }
        }

        return false;
    }

    private bool PauseGame(GameBoard board, int generation)
    {
        _view.DisplayPaused(_settings);

        while (true)
        {
            ConsoleKey key = _view.ReadKey();

            if (key == _settings.PauseKey)
            {
                _view.DisplayMessage(GameTexts.Resumed);
                return false;
            }

            if (key == _settings.SaveKey)
            {
                SaveGame(board, generation);
            }
            else if (key == _settings.QuitKey)
            {
                return true;
            }
        }
    }

   

    // The one save method is shared by normal play and paused play.
    private void SaveGame(GameBoard board, int generation)
    {
        bool saved = _storage.Save(board, generation);

        _view.DisplayMessage(saved ? GameTexts.SaveSuccess : GameTexts.SaveFailure);
    }
}