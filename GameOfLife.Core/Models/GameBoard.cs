namespace GameOfLife.Core.Models;

// mvc model: data only. No constructors, methods, validation or game logic.
public class GameBoard
{
    public int Rows { get; init; }
    public int Columns { get; init; }
    public bool[,] Cells { get; init; } = new bool[0, 0];
}