using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Models;

namespace GameOfLife.ConsoleApp.Services;

public sealed class GameBoardFactory
{
    private readonly GameSettings _settings;
    private readonly Random _random;

    public GameBoardFactory(
        GameSettings settings,
        Random? random = null)
    {
        _settings = settings;
        _random = random ?? Random.Shared;

        if (_settings.InitialAlivePercentage is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(settings),
                GameTexts.InvalidAlivePercentage);
        }
    }

    public GameBoard CreateRandomBoard(int rows, int columns)
    {
        GameBoard board = new(rows, columns);

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                bool isAlive =
                    _random.Next(100) <
                    _settings.InitialAlivePercentage;

                board.SetCell(row, column, isAlive);
            }
        }

        return board;
    }
}