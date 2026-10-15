using GameOfLife.ConsoleApp.Game;

namespace GameOfLife.ConsoleApp;

public class ConsoleRenderer
{
    private readonly TextWriter _writer;

    public ConsoleRenderer(TextWriter writer)
    {
        _writer = writer;
    }

    public void Display(GameBoard board)
    {
        for (int row = 0; row < board.Rows; row++)
        {
            for (int column = 0; column < board.Columns; column++)
                _writer.Write(board.IsAlive(row, column) ? "■ " : "· ");

            _writer.WriteLine();
        }

        _writer.WriteLine("■ = Alive · = Dead");
    }
}