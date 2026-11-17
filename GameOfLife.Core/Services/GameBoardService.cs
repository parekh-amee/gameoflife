using GameOfLife.Core.Models;

namespace GameOfLife.Core.Services;

// Performs calculations on game board data.
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
                if (rowOffset == 0 && columnOffset == 0)
                {
                    continue;
                }

                int neighborRow = row + rowOffset;
                int neighborColumn = column + columnOffset;

                if (neighborRow >= 0 && neighborRow < board.Rows && neighborColumn >= 0 &&  neighborColumn < board.Columns && board.Cells[neighborRow, neighborColumn])
                {
                    count++;
                }
            }
        }

        return count;
    }
}
