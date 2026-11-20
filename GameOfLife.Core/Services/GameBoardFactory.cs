using GameOfLife.Core.Constants;
using GameOfLife.Core.Models;

namespace GameOfLife.Core.Services;

// Creates empty, random or restored game boards.
public class GameBoardFactory
{
    private readonly int _initialAlivePercentage;
    private readonly Random _random;

    public GameBoardFactory(int initialAlivePercentage, Random? random = null)
    {
        if (initialAlivePercentage is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(initialAlivePercentage),initialAlivePercentage, GameTexts.InvalidAlivePercentage);
        }

        _initialAlivePercentage = initialAlivePercentage;
        _random = random ?? Random.Shared;
    }

    public GameBoard CreateEmptyBoard(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rows), rows, GameTexts.InvalidBoardDimensions);
        }

        return new GameBoard
        {
            Rows = rows,
            Columns = columns,
            Cells = new bool[rows, columns]
        };
    }

    public GameBoard CreateRandomBoard(int rows, int columns)
    {
        GameBoard board = CreateEmptyBoard(rows, columns);

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                board.Cells[row, column] =_random.Next(100) < _initialAlivePercentage;
            }
        }

        return board;
    }

    public GameBoard CreateBoard(bool[,] cells)
    {
        ArgumentNullException.ThrowIfNull(cells);

        int rows = cells.GetLength(0);
        int columns = cells.GetLength(1);

        if (rows == 0 || columns == 0)
        {
            throw new ArgumentException( GameTexts.InvalidBoardDimensions, nameof(cells));
        }

        return new GameBoard
        {
            Rows = rows,
            Columns = columns,
            Cells = (bool[,])cells.Clone()
        };
    }

    public GameBoard CreateBoard(bool[][] cells)
    {
        ArgumentNullException.ThrowIfNull(cells);

        int rows = cells.Length;

        if (rows == 0 || cells[0] is null || cells[0].Length == 0)
        {
            throw new ArgumentException(GameTexts.InvalidBoardDimensions, nameof(cells));
        }

        int columns = cells[0].Length;
        bool[,] board = new bool[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            if (cells[row] is null || cells[row].Length != columns)
            {
                throw new ArgumentException( GameTexts.InvalidSavedGameCells, nameof(cells));
            }

            for (int column = 0; column < columns; column++)
            {
                board[row, column] = cells[row][column];
            }
        }

        return CreateBoard(board);
    }
}
