using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Models;

namespace GameOfLife.ConsoleApp.Services;

public class GameBoardFactory
{
    private readonly GameSettings _settings;
    private readonly Random _random;
    private readonly GameBoardService _boardService;

    public GameBoardFactory(GameSettings settings,GameBoardService boardService,Random? random = null)
    {
        _settings = settings;
        _random = random ?? Random.Shared;
        _boardService = boardService;

        if (_settings.InitialAlivePercentage is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(settings), GameTexts.InvalidAlivePercentage);
        }
    }

    public GameBoard CreateEmptyBoard(int rows, int columns)
    {
        return new GameBoard(rows, columns);
    }

    public GameBoard CreateBoard(bool[,] cells)
    {
        int rows = cells.GetLength(0);
        int columns = cells.GetLength(1);
        GameBoard board = CreateEmptyBoard(rows, columns);

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                _boardService.SetCell(board, row, column, cells[row, column]);
            }
        }

        return board;
    }

    public GameBoard CreateRandomBoard(int rows, int columns)
    {
        GameBoard board = CreateEmptyBoard(rows, columns);

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                bool isAlive =_random.Next(100) < _settings.InitialAlivePercentage;
                _boardService.SetCell(board, row, column, isAlive);
            }
        }

        return board;
    }
}