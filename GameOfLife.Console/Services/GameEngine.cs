namespace GameOfLife.ConsoleApp.Services;

public class GameEngine
{
    // applies conway's Game of Life to one cell
    public bool CalculateNextCellState(bool isAlive, int liveNeighbors)
    {
        return liveNeighbors == 3 || (isAlive && liveNeighbors == 2);
    }
}