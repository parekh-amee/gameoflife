using GameOfLife.ConsoleApp.Configuration;

namespace GameOfLife.ConsoleApp.Models;

public sealed class GameBoard
{
    private readonly bool[,] _cells;

    public int Rows { get; }
    public int Columns { get; }

    public GameBoard(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(rows),
                GameTexts.InvalidBoardDimensions);
        }

        Rows = rows;
        Columns = columns;

        _cells = new bool[rows, columns];
    }

    public bool IsAlive(int row, int column)
    {
        ValidateCoordinates(row, column);

        return _cells[row, column];
    }

    public void SetCell(int row, int column, bool isAlive)
    {
        ValidateCoordinates(row, column);

        _cells[row, column] = isAlive;
    }

    public int CountLiveNeighbors(int row, int column)
    {
        ValidateCoordinates(row, column);

        int liveNeighbors = 0;

        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int columnOffset = -1; columnOffset <= 1; columnOffset++)
            {
                if (rowOffset == 0 && columnOffset == 0)
                    continue;

                int neighborRow = row + rowOffset;
                int neighborColumn = column + columnOffset;

                if (!IsInsideBoard(neighborRow, neighborColumn))
                    continue;

                if (IsAlive(neighborRow, neighborColumn))
                    liveNeighbors++;
            }
        }

        return liveNeighbors;
    }

    private bool IsInsideBoard(int row, int column) =>
        row >= 0 &&
        row < Rows &&
        column >= 0 &&
        column < Columns;

    private void ValidateCoordinates(int row, int column)
    {
        if (IsInsideBoard(row, column))
            return;

        throw new ArgumentOutOfRangeException(
            nameof(row),
            GameTexts.InvalidCellCoordinates);
    }
}
