using GameOfLife.ConsoleApp.Models;

namespace GameOfLife.ConsoleApp.Services;

// Performs calculations on board data. It does not create, display or save boards.
public class GameBoardService
{
    public int CountLiveCells(GameBoard board)
    {
        int count = 0;

        for (int row = 0; row < board.Rows; row++)
        {
            for (int column = 0; column < board.Columns; column++)
            {
                if (board.Cells[row, column])
                {
                    count++;
                }
            }
        }

        return count;
    }

    public int CountLiveNeighbors(GameBoard board, int row, int column)
    {
        int count = 0;

        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int columnOffset = -1; columnOffset <= 1; columnOffset++)
            {
                // The cell itself is not one of its neighbors.
                if (rowOffset == 0 && columnOffset == 0)
                {
                    continue;
                }

                int neighborRow = row + rowOffset;
                int neighborColumn = column + columnOffset;

                // Cells outside the board are ignored (no wrap-around).
                if (neighborRow >= 0 && neighborRow < board.Rows && neighborColumn >= 0 && neighborColumn < board.Columns &&
                    board.Cells[neighborRow, neighborColumn])
                {
                    count++;
                }
            }
        }

        return count;
    }
}