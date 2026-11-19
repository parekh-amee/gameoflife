using GameOfLife.ConsoleApp.Configuration;
using GameOfLife.ConsoleApp.Controllers;
using GameOfLife.ConsoleApp.Services;
using GameOfLife.ConsoleApp.Views;

// Composition root: create and connect the application objects here.
GameSettings settings = new();
GameBoardService boardService = new();
GameBoardFactory boardFactory = new(settings);
GameEngine engine = new();

GameSimulationService simulation = new(engine, boardService, boardFactory);

ConsoleView view = new(boardService);
GameStateStorage storage = new();

GameController controller = new(settings, view, boardFactory, simulation, storage);

controller.Run();