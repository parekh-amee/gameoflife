using GameOfLife.ConsoleApp;
using GameOfLife.ConsoleApp.Game;

Console.WriteLine("-- Conway's Game of Life  --");
Console.WriteLine("1. Small (10 x 10)");
Console.WriteLine("2. Medium (20 x 20)");
Console.WriteLine("3. Large (30 x 30)");
Console.Write("Select board size: ");

int size = Console.ReadLine() switch
{
    "1" => 10,
    "2" => 20,
    "3" => 30,
    _ => 0
};

if (size == 0) { Console.WriteLine("Invalid choice."); return; }

GameBoard board = new(size, size);
board.Randomize(new Random());

GameEngine engine = new();
ConsoleRenderer renderer = new(Console.Out);
int generation = 0;

while (true)
{
    Console.Clear();
    Console.WriteLine($"Generation: {generation}\n");
    renderer.Display(board);
    Console.WriteLine("\nPress Q to quit.");

    for (int i = 0; i < 10; i++)
    {
        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
        {
            Console.WriteLine("Simulation stopped.");
            return;
        }

        Thread.Sleep(100);
    }

    board = engine.NextGeneration(board);
    generation++;
}