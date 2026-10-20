using GameOfLife.ConsoleApp.Models;

namespace GameOfLife.ConsoleApp.Services;

public sealed class GameEngine
{
    public GameBoard NextGeneration(GameBoard currentBoard)
    {
        GameBoard nextBoard = new(
            currentBoard.Rows,
            currentBoard.Columns);

        for (int row = 0; row < currentBoard.Rows; row++)
        {
            for (int column = 0;
                 column < currentBoard.Columns;
                 column++)
            {
                int neighbors =
                    currentBoard.CountLiveNeighbors(row, column);

                bool isAlive =
                    currentBoard.IsAlive(row, column);

                bool nextCellState =
                    neighbors == 3 ||
                    (isAlive && neighbors == 2);

                nextBoard.SetCell(
                    row,
                    column,
                    nextCellState);
            }
        }

        return nextBoard;
    }
}