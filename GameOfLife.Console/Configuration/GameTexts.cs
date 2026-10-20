namespace GameOfLife.ConsoleApp.Configuration;

public static class GameTexts
{
    public const string Title = "-- Conway's Game of Life --";

    // Menu choices
    public const string SmallBoardChoice = "1";
    public const string MediumBoardChoice = "2";
    public const string LargeBoardChoice = "3";
    public const string CustomBoardChoice = "4";

    // Menu text
    public const string SelectBoardSize = "Select board size:";
    public const string SmallBoardFormat = "{0}. Small ({1} x {1})";
    public const string MediumBoardFormat = "{0}. Medium ({1} x {1})";
    public const string LargeBoardFormat = "{0}. Large ({1} x {1})";
    public const string CustomBoard = "{0}. Custom size";

    // Input prompts
    public const string EnterRows = "Enter number of rows:";
    public const string EnterColumns = "Enter number of columns:";

    // Validation messages
    public const string InvalidChoice =
        "Invalid choice. Please select a valid option.";

    public const string InvalidSize =
        "Invalid size. Enter a whole number in the allowed range.";

    public const string BoardSizeRangeFormat =
        "Allowed board dimensions: {0} to {1}.";

    public const string InvalidBoardDimensions =
        "Board dimensions must be positive.";

    public const string InvalidAlivePercentage =
        "Initial alive percentage must be between 0 and 100.";

    public const string InvalidCellCoordinates =
        "Cell coordinates are outside the board.";

    // Board display
    public const string AliveCell = "■ ";
    public const string DeadCell = "· ";
    public const string AliveLegend = "■ = Alive   · = Dead";
    public const string GenerationPrefix = "Generation: ";

    // Simulation controls
    public const string QuitPrompt = "Press Q to quit.";
    public const string QuitMessage = "Simulation stopped.";
}

