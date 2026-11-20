using GameOfLife.Core.Models;

namespace GameOfLife.Core.Services;

// Builds a complete next generation using board calculations and Conway's rules.
public class GameSimulationService
{
    private readonly GameEngine _engine;
    private readonly GameBoardService _boardService;
    private readonly GameBoardFactory _boardFactory;

    public GameSimulationService(GameEngine engine,GameBoardService boardService,GameBoardFactory boardFactory)
    {
        _engine = engine;
        _boardService = boardService;
        _boardFactory = boardFactory;
    }

    public GameBoard CreateNextGeneration(GameBoard currentBoard)
    {
        GameBoard nextBoard = _boardFactory.CreateEmptyBoard(currentBoard.Rows, currentBoard.Columns);

        for (int row = 0; row < currentBoard.Rows; row++)
        {
            for (int column = 0; column < currentBoard.Columns; column++)
            {
                bool isAlive = currentBoard.Cells[row, column];
                int neighbors = _boardService.CountLiveNeighbors(currentBoard, row, column);

                nextBoard.Cells[row, column] = _engine.CalculateNextCellState(isAlive, neighbors);
            }
        }

        // Return a new board so current-generation cells are not overwritten
        // while the next generation is still being calculated.
        return nextBoard;
    }
}