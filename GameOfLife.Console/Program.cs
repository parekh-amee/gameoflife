using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Controllers;
using GameOfLife.ConsoleApp.Services;
using GameOfLife.ConsoleApp.Views;

GameSettings settings = new();

GameBoardService boardService = new();

ConsoleView view = new(boardService);

GameBoardFactory boardFactory = new(
    settings,
    boardService);

GameEngine engine = new(boardService);

GameController controller = new(
    settings,
    view,
    boardFactory,
    engine);

controller.Run();