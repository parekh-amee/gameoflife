using GameOfLife.ConsoleApp.Models;

namespace GameOfLife.ConsoleApp.Services;

public class GameBoardService
{
    public bool IsAlive(GameBoard board, int row, int column)
    {
        ValidateCoordinates(board, row, column);
        return board.Cells[row, column];
    }

    public void SetCell(
        GameBoard board, int row, int column, bool isAlive)
    {
        ValidateCoordinates(board, row, column);
        board.Cells[row, column] = isAlive;
    }

    public int CountLiveCells(GameBoard board)
    {
        int count = 0;

        for (int row = 0; row < board.Rows; row++)
        {
            for (int column = 0; column < board.Columns; column++)
            {
                if (IsAlive(board, row, column))
                {
                    count++;
                }
            }
        }

        return count;
    }

    public int CountLiveNeighbors(GameBoard board, int row, int column)
    {
        int liveNeighbors = 0;

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

                if (neighborRow < 0 || neighborRow >= board.Rows || neighborColumn < 0 || neighborColumn >= board.Columns)
                {
                    continue;
                }

                if (IsAlive(board, neighborRow, neighborColumn))
                {
                    liveNeighbors++;
                }
            }
        }

        return liveNeighbors;
    }

    private void ValidateCoordinates(
        GameBoard board, int row, int column)
    {
        if (row < 0 || row >= board.Rows)
        {
            throw new ArgumentOutOfRangeException(nameof(row));
        }

        if (column < 0 || column >= board.Columns)
        {
            throw new ArgumentOutOfRangeException(nameof(column));
        }
    }
}
