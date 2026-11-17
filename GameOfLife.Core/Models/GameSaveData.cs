namespace GameOfLife.Core.Models;

// mvc model used by persistence. It contains properties only.
public class GameSaveData
{
    public int Rows { get; set; }
    public int Columns { get; set; }
    public int Generation { get; set; }
    public bool[][] Cells { get; set; } = [];
}