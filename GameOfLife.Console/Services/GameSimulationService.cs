using GameOfLife.ConsoleApp.Models;

namespace GameOfLife.ConsoleApp.Services;

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
        GameBoard nextBoard = _boardFactory.CreateEmptyBoard(currentBoard.Rows,currentBoard.Columns);

        for (int row = 0; row < currentBoard.Rows; row++)
        {
            for (int column = 0; column < currentBoard.Columns; column++)
            {
                bool isAlive = _boardService.IsAlive(currentBoard, row, column);

                int liveNeighbors = _boardService.CountLiveNeighbors(currentBoard, row, column);

                bool nextCellState = _engine.CalculateNextCellState(isAlive, liveNeighbors);

                _boardService.SetCell(nextBoard, row, column, nextCellState);
            }
        }

        return nextBoard;
    }
}