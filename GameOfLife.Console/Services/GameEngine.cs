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
            for (int column = 0; column < currentBoard.Columns; column++)
            {
                int neighbors = CountLiveNeighbors(
                    currentBoard, row, column);

                bool isAlive = currentBoard.IsAlive(row, column);

                bool nextCellState =
                    neighbors == 3 || (isAlive && neighbors == 2);

                nextBoard.SetCell(row, column, nextCellState);
            }
        }

        return nextBoard;
    }

    private int CountLiveNeighbors(
        GameBoard board, int row, int column)
    {
        int liveNeighbors = 0;

        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int columnOffset = -1; columnOffset <= 1; columnOffset++)
            {
                if (rowOffset == 0 && columnOffset == 0)
                    continue;

                int neighborRow = row + rowOffset;
                int neighborColumn = column + columnOffset;

                if (neighborRow < 0 || neighborRow >= board.Rows)
                    continue;

                if (neighborColumn < 0 ||
                    neighborColumn >= board.Columns)
                    continue;

                if (board.IsAlive(neighborRow, neighborColumn))
                    liveNeighbors++;
            }
        }

        return liveNeighbors;
    }
}
