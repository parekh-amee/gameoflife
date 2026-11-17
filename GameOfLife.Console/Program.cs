using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Controllers;
using GameOfLife.ConsoleApp.Views;
using GameOfLife.Core.Services;
using GameOfLife.Infrastructure;

// Composition root: creates and connects application objects.
GameSettings settings = new();

GameBoardService boardService = new();

GameBoardFactory boardFactory = new(settings.InitialAlivePercentage);

GameEngine engine = new();

GameSimulationService simulation = new(engine,boardService,boardFactory);

ConsoleView view = new(boardService);

GameStateStorage storage = new();

GameController controller = new(settings,view,boardFactory,simulation,storage);

controller.Run();
