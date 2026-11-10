namespace GameOfLife.ConsoleApp.Models;

public sealed class GameBoard
{
    public int Rows { get; }
    public int Columns { get; }
    public bool[,] Cells { get; }

    public GameBoard(int rows, int columns)
    {
        if (rows <= 0)
            throw new ArgumentOutOfRangeException(nameof(rows));

        if (columns <= 0)
            throw new ArgumentOutOfRangeException(nameof(columns));

        Rows = rows;
        Columns = columns;
        Cells = new bool[rows, columns];
    }
}