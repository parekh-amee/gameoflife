namespace GameOfLife.ConsoleApp.Models;

public sealed class GameSaveData
{
    public int Rows { get; set; }

    public int Columns { get; set; }

    public int Generation { get; set; }

    public bool[][] Cells { get; set; } = [];
}
