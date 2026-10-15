namespace GameOfLife.ConsoleApp.Game;

public class GameEngine
{
    public GameBoard NextGeneration(GameBoard currentBoard)
    {
        GameBoard nextBoard = new(currentBoard.Rows, currentBoard.Columns);

        for (int row = 0; row < currentBoard.Rows; row++)
        {
            for (int column = 0; column < currentBoard.Columns; column++)
            {
                int neighbors = currentBoard.CountLiveNeighbors(row, column);
                bool alive = currentBoard.IsAlive(row, column);

                bool shouldLive = neighbors == 3 || (alive && neighbors == 2);
                nextBoard.SetCell(row, column, shouldLive);
            }
        }

        return nextBoard;
    }
}
