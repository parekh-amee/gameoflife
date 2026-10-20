using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Controllers;
using GameOfLife.ConsoleApp.Services;
using GameOfLife.ConsoleApp.Views;

GameSettings settings = new();

ConsoleView view = new();

GameBoardFactory boardFactory = new(settings);

GameEngine engine = new();

GameController controller = new(
    settings,
    view,
    boardFactory,
    engine);

controller.Run();