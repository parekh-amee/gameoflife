using System.Text.Json;
using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Models;

namespace GameOfLife.ConsoleApp.Services;

// Responsible only for reading and writing saved game data.
public class GameStateStorage
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public GameStateStorage(string? filePath = null)
    {
        _filePath = filePath ?? Path.Combine(
            AppContext.BaseDirectory,
            GameTexts.SaveFileName);

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    public bool Save(GameBoard board, int generation)
    {
        GameSaveData data = new()
        {
            Rows = board.Rows,
            Columns = board.Columns,
            Generation = generation,
            Cells = ConvertToJaggedArray(board.Cells)
        };

        try
        {
            string json = JsonSerializer.Serialize(data, _jsonOptions);
            File.WriteAllText(_filePath, json);
            return true;
        }
        catch (IOException)
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

        GameSaveData? data =
            JsonSerializer.Deserialize<GameSaveData>(json, _jsonOptions);

        Validate(data);

        return data;
    }

    private static bool[][] ConvertToJaggedArray(bool[,] board)
    {
        int rows = board.GetLength(0);
        int columns = board.GetLength(1);

        bool[][] result = new bool[rows][];

        for (int i = 0; i < rows; i++)
        {
            result[i] = new bool[columns];

            for (int j = 0; j < columns; j++)
            {
                result[i][j] = board[i, j];
            }
        }

        return result;
    }

    private static void Validate(GameSaveData? data)
    {
        if (data is null ||
            data.Rows <= 0 ||
            data.Columns <= 0 ||
            data.Generation < 0 ||
            data.Cells is null ||
            data.Cells.Length != data.Rows)
        {
            throw new InvalidDataException(GameTexts.InvalidSavedGame);
        }

        foreach (bool[]? row in data.Cells)
        {
            if (row is null || row.Length != data.Columns)
            {
                throw new InvalidDataException(
                    GameTexts.InvalidSavedGameCells);
            }
        }
    }
}
