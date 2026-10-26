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
            GameSaveData data = new()
            {
                Rows = board.Rows,
                Columns = board.Columns,
                Generation = generation,
                Cells = new bool[board.Rows][]
            };

            for (int row = 0; row < board.Rows; row++)
            {
                data.Cells[row] = new bool[board.Columns];

                for (int column = 0; column < board.Columns; column++)
                {
                    data.Cells[row][column] = board.IsAlive(row, column);
                }
            }

            string json = JsonSerializer.Serialize(
                data,
                new JsonSerializerOptions { WriteIndented = true });

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
            return null;

        try
        {
            string json = File.ReadAllText(_filePath);
            GameSaveData? data = JsonSerializer.Deserialize<GameSaveData>(json);

            if (data is null ||
                data.Rows <= 0 ||
                data.Columns <= 0 ||
                data.Generation < 0 ||
                data.Cells.Length != data.Rows)
            {
                return null;
            }

            foreach (bool[] row in data.Cells)
            {
                if (row is null || row.Length != data.Columns)
                    return null;
            }

            return data;
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
}
