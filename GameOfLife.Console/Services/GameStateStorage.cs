using System.Text.Json;
using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Models;

namespace GameOfLife.ConsoleApp.Services;

public class GameStateStorage
{
    private readonly string _filePath;
    private readonly GameBoardService _boardService;

    public GameStateStorage(GameBoardService boardService,string? filePath = null)
    {
        _boardService = boardService;
        _filePath = filePath ?? Path.Combine(AppContext.BaseDirectory,GameTexts.DefaultSaveFileName);
    }

    public bool Save(GameBoard board, int generation)
    {
        try
        {
            GameSaveData saveData = CreateSaveData(board, generation);
            string json = JsonSerializer.Serialize(saveData, CreateJsonOptions());
            File.WriteAllText(_filePath, json);
            return true;
        }
        catch (IOException)
        {
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }

    public GameSaveData? Load()
    {
        if (!File.Exists(_filePath))
        {
            return null;
        }

        string json = File.ReadAllText(_filePath);
        GameSaveData? saveData = JsonSerializer.Deserialize<GameSaveData>(json, CreateJsonOptions());

        ValidateSaveData(saveData);
        return saveData;
    }

    private GameSaveData CreateSaveData(GameBoard board, int generation)
    {
        return new GameSaveData
        {
            Rows = board.Rows,
            Columns = board.Columns,
            Generation = generation,
            Cells = CopyBoardCells(board)
        };
    }

    private bool[,] CopyBoardCells(GameBoard board)
    {
        bool[,] cells = new bool[board.Rows, board.Columns];

        for (int row = 0; row < board.Rows; row++)
        {
            for (int column = 0; column < board.Columns; column++)
            {
                cells[row, column] = _boardService.IsAlive(board, row, column);
            }
        }

        return cells;
    }

    private static JsonSerializerOptions CreateJsonOptions()
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        options.Converters.Add(new JsonConverter());
        return options;
    }

    private void ValidateSaveData(GameSaveData? data)
    {
        if (data is null)
        {
            throw new InvalidDataException(GameTexts.InvalidSavedGameData);
        }

        if (data.Rows <= 0 || data.Columns <= 0 || data.Generation < 0)
        {
            throw new InvalidDataException(GameTexts.InvalidSavedGameDimensions);
        }

        if (data.Cells is null || data.Cells.GetLength(0) != data.Rows || data.Cells.GetLength(1) != data.Columns)
        {
            throw new InvalidDataException(GameTexts.InvalidSavedGameCells);
        }
    }
}