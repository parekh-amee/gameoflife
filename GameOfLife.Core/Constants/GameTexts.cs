namespace GameOfLife.Core.Constants;

// Contains all application text constants.
public static class GameTexts
{
    // General
    public const string Title = "-- Conway's Game of Life --";
    public const string SaveFileName = "game-state.json";

    // Menu
    public const string SelectBoardSize = "Select board size:";
    public const string SmallChoice = "1";
    public const string MediumChoice = "2";
    public const string LargeChoice = "3";
    public const string CustomChoice = "4";

    public const string SmallBoardFormat = "{0}. Small ({1} x {1})";
    public const string MediumBoardFormat = "{0}. Medium ({1} x {1})";
    public const string LargeBoardFormat = "{0}. Large ({1} x {1})";
    public const string CustomBoardFormat = "{0}. Custom size";

    // Input
    public const string EnterRows = "Enter number of rows:";
    public const string EnterColumns = "Enter number of columns:";
    public const string InvalidChoice = "Invalid choice. Please select 1, 2, 3 or 4.";

    public const string InvalidSize ="Enter a whole number within the allowed range.";

    public const string SizeRangeFormat = "Allowed size: {0} to {1}.";

    // Board display
    public const string AliveCell = "■ ";
    public const string DeadCell = "· ";
    public const string Legend = "■ = Alive · = Dead";

    public const string GenerationFormat = "Generation: {0}";
    public const string LiveCellsFormat = "Live cells: {0}";

    // Keyboard instructions
    public const string PausePromptFormat = "Press {0} to pause/resume.";
    public const string SavePromptFormat = "Press {0} to save.";
    public const string QuitPromptFormat = "Press {0} to quit.";

    public const string PausedFormat = "Paused. Press {0} to resume, {1} to save, or {2} to quit.";

    public const string Resumed = "Simulation resumed.";
    public const string Stopped = "Simulation stopped.";

    // Save and restore
    public const string RestorePrompt ="A saved game was found. Restore it? (Y/N): ";

    public const string Yes = "Y";
    public const string SaveSuccess = "Game state saved.";
    public const string SaveFailure = "Could not save the game state.";

    public const string InvalidBoardDimensions ="Board dimensions must be positive.";

    public const string InvalidAlivePercentage = "Initial alive percentage must be between 0 and 100.";

    public const string InvalidSavedGame = "The saved game data is invalid.";

    public const string InvalidSavedGameCells = "The saved game contains invalid cell data.";
}
