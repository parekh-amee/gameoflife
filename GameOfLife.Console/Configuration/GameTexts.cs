namespace GameOfLife.ConsoleApp.Configuration;

public static class GameTexts
{
    public const string Title = "-- Conway's Game of Life --";
    public const string EnterAlternateScreen = "\u001b[?1049h";
    public const string ExitAlternateScreen = "\u001b[?1049l";

    public const string SelectBoardSize = "Select board size:";
    public const string SmallBoardChoice = "1";
    public const string MediumBoardChoice = "2";
    public const string LargeBoardChoice = "3";
    public const string CustomBoardChoice = "4";

    public const string SmallBoardFormat = "{0}. Small ({1} x {1})";
    public const string MediumBoardFormat = "{0}. Medium ({1} x {1})";
    public const string LargeBoardFormat = "{0}. Large ({1} x {1})";
    public const string CustomBoardFormat = "{0}. Custom size";

    public const string EnterRows = "Enter number of rows:";
    public const string EnterColumns = "Enter number of columns:";

    public const string InvalidChoice =
        "Invalid choice. Please select a valid option.";

    public const string InvalidSize =
        "Invalid size. Enter a whole number in the allowed range.";

    public const string BoardSizeRangeFormat =
        "Allowed board dimensions: {0} to {1}.";

    public const string AliveCell = "■ ";
    public const string DeadCell = "· ";
    public const string AliveLegend = "■ = Alive   · = Dead";

    public const string GenerationPrefix = "Generation: ";
    public const string LiveCellsPrefix = "Live cells: ";

    public const string QuitPrompt = "Press Q to save and quit.";
    public const string SavePrompt = "Press S to save the current game.";
    public const string QuitMessage = "Simulation stopped.";

    public const string RestorePrompt =
        "A saved game was found. Restore it? (Y/N): ";

    public const string SaveSuccess = "Game state saved.";
    public const string SaveFailure = "Could not save the game state.";
    public const string RestoreFailure =
        "The saved game could not be restored. Starting a new game.";

    public const string InvalidBoardDimensions =
        "Board dimensions must be positive.";

    public const string InvalidAlivePercentage =
        "Initial alive percentage must be between 0 and 100.";

    public const string CellCoordinatesOutsideBoard =
        "Cell coordinates are outside the board.";
}
