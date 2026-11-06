namespace GameOfLife.ConsoleApp.Models;

public sealed class GameBoard
{
    private readonly bool[,] _cells;

    public int Rows { get; }
    public int Columns { get; }

    public GameBoard(int rows, int columns)
    {
        if (rows <= 0)
            throw new ArgumentOutOfRangeException(nameof(rows));

        if (columns <= 0)
            throw new ArgumentOutOfRangeException(nameof(columns));

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

    private void ValidateCoordinates(int row, int column)
    {
        if (row < 0 || row >= Rows)
            throw new ArgumentOutOfRangeException(nameof(row));

        if (column < 0 || column >= Columns)
            throw new ArgumentOutOfRangeException(nameof(column));
    }
}