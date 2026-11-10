using GameOfLife.ConsoleApp.Models;

namespace GameOfLife.ConsoleApp.Services;

public sealed class GameBoardService
{
    public bool IsAlive(GameBoard board, int row, int column)
    {
        ValidateCoordinates(board, row, column);
        return board.Cells[row, column];
    }

    public void SetCell(
        GameBoard board,
        int row,
        int column,
        bool isAlive)
    {
        ValidateCoordinates(board, row, column);
        board.Cells[row, column] = isAlive;
    }

    private void ValidateCoordinates(
        GameBoard board,
        int row,
        int column)
    {
        if (row < 0 || row >= board.Rows)
            throw new ArgumentOutOfRangeException(nameof(row));

        if (column < 0 || column >= board.Columns)
            throw new ArgumentOutOfRangeException(nameof(column));
    }
}