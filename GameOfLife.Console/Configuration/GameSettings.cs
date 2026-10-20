namespace GameOfLife.ConsoleApp.Configuration;

public sealed class GameSettings
{
    public int InitialAlivePercentage { get; init; } = 30;

    public int UpdateIntervalMilliseconds { get; init; } = 1000;

    public int MinimumBoardSize { get; init; } = 1;
    public int MaximumBoardSize { get; init; } = 100;

    public int SmallBoardSize { get; init; } = 10;
    public int MediumBoardSize { get; init; } = 20;
    public int LargeBoardSize { get; init; } = 30;
}