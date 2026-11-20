namespace GameOfLife.Core.Services;

// contains the conway rules for one cell only.
public class GameEngine
{
    public bool CalculateNextCellState(bool isAlive, int liveNeighbors)
    {
        // A cell is alive next generation if it has 3 neighbors,
        // or if it is already alive and has exactly 2 neighbors.
        return liveNeighbors == 3 || (isAlive && liveNeighbors == 2);
    }
}