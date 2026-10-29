using System.Text.Json;
using GameOfLife.ConsoleApp.Models;

namespace GameOfLife.ConsoleApp.Services;

public sealed class GameStateStorage
{
    private readonly string _filePath;

    public GameStateStorage(string? filePath = null)
    {
        _filePath = filePath ??
            Path.Combine(AppContext.BaseDirectory, "game-state.json");
    }

    public bool Save(GameBoard board, int generation)
    {
        try
        {
            GameSaveData saveData = CreateSaveData(board, generation);

            string json = ConvertToJson(saveData);

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

        try
        {
            string json = File.ReadAllText(_filePath);

            GameSaveData? saveData =
                JsonSerializer.Deserialize<GameSaveData>(json);

            if (!IsValidSaveData(saveData))
            {
                return null;
            }

            return saveData;
        }
        catch (IOException)
        {
            return null;
        }
        catch (UnauthorizedAccessException)
        {
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
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

    private bool[][] CopyBoardCells(GameBoard board)
    {
        bool[][] cells = new bool[board.Rows][];

        for (int row = 0; row < board.Rows; row++)
        {
            cells[row] = new bool[board.Columns];

            for (int column = 0; column < board.Columns; column++)
            {
                cells[row][column] = board.IsAlive(row, column);
            }
        }

        return cells;
    }

    private string ConvertToJson(GameSaveData saveData)
    {
        return JsonSerializer.Serialize(
            saveData,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });
    }

    private bool IsValidSaveData(GameSaveData? data)
    {
        if (data is null ||
            data.Rows <= 0 ||
            data.Columns <= 0 ||
            data.Generation < 0 ||
            data.Cells is null ||
            data.Cells.Length != data.Rows)
        {
            return false;
        }

        foreach (bool[]? row in data.Cells)
        {
            if (row is null || row.Length != data.Columns)
            {
                return false;
            }
        }

        return true;
    }
}