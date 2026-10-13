namespace GameOfLife.ConsoleApp.Game;

public class GameBoard
{
    private readonly bool[,] _cells;

    public int Rows { get; }
    public int Columns { get; }

    public GameBoard(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Board size must be positive.");

        Rows = rows;
        Columns = columns;
        _cells = new bool[rows, columns];
    }

    public bool IsAlive(int row, int column) => _cells[row, column];

    public void SetCell(int row, int column, bool isAlive) =>
    _cells[row, column] = isAlive;

    public void Randomize(Random random)
    {
        for (int row = 0; row < Rows; row++)
            for (int column = 0; column < Columns; column++)
                _cells[row, column] = random.Next(100) < 30;
    }

    public int CountLiveNeighbors(int row, int column)
    {
        int count = 0;

        for (int r = row - 1; r <= row + 1; r++)
        {
            for (int c = column - 1; c <= column + 1; c++)
            {
                if (r == row && c == column) continue;

                if (r >= 0 && r < Rows && c >= 0 && c < Columns &&
                IsAlive(r, c))
                    count++;
            }
        }

        return count;
    }
}
