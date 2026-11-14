namespace GameOfLife.ConsoleApp.Models;

public class GameSaveData
{
    public int Rows { get; set; }

    public int Columns { get; set; }

    public int Generation { get; set; }

    public bool[,] Cells { get; set; } = new bool[0, 0];
}