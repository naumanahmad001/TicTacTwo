using Components;

namespace GameBrain
{
    using DAL;

    public class GameFlow
    {
        private static CustomConfig _customConfig;
        private static char playerOnePiece;
        private static char playerTwoPiece;
        private static string gameSaveName;


        public static void StartGame()
        {
            CleanupTempStates();
            InitialMenu();
        }
        

        private static void InitialMenu()
{
    while (true)
    {
        Menus.PrintFirstMenu();
        string action = Console.ReadLine()?.ToUpper();

        switch (action)
        {
            case "N":
                HandleNewGameMenu();
                break;
            case "L":
                LoadSavedGame();
                break;
            case "E":
                Console.WriteLine("Have a nice day :)");
                return;
            default:
                Console.WriteLine($"{action} is not an option from the menu. Try again.");
                break;
        }
    }
}

private static void HandleNewGameMenu()
{
    while (true)
    {
        CreateNewGame();
        Menus.PrintSecondMenu();
        
        string action2 = Console.ReadLine()?.ToUpper();

        switch (action2)
        {
            case "S":
                CreateAndSaveNewConfig();
                break;
            case "L":
                LoadCustomConfiguration();
                break;
            case "B":
                return;
            case "E":
                Console.WriteLine("Have a nice day :)");
                Environment.Exit(0); 
                break;
            default:
                Console.WriteLine($"{action2} is not an option from the menu. Try again.");
                break;
        }
    }
}

public static void CreateNewGame()
{
    Console.WriteLine("Name of the new game?");
    var name = Console.ReadLine();
    gameSaveName = name;

    IFileSaveLoad fileSaveLoad = FileSaveLoadFactory.GetFileSaveLoadImplementation();
    fileSaveLoad.SaveInitialGame(name,1, string.Empty, string.Empty);


}




private static void CreateAndSaveNewConfig()
{
    _customConfig = CustomConfig.CreateGameConfig();
    string? configName = _customConfig.ConfigName;

    if (!string.IsNullOrEmpty(configName))
    {
        IFileSaveLoad fileSaveLoad = FileSaveLoadFactory.GetFileSaveLoadImplementation();
        fileSaveLoad.SaveConfiguration(_customConfig, configName);
        InitializeGame();
    }
}

private static void LoadCustomConfiguration()
{
    IFileSaveLoad configLoader = FileSaveLoadFactory.GetFileSaveLoadImplementation();
    var configurations = configLoader.DisplayAllConfigurations();

    if (configurations.Count > 0)
    {
        Console.Clear();
        Console.WriteLine("Available configurations:\n");
        foreach (var config in configurations)
        {
            Console.WriteLine(config.ConfigName);
        }

        Console.WriteLine("\nEnter the name of the configuration to load:");
        string? nameToLoad = Console.ReadLine();

        if (!string.IsNullOrEmpty(nameToLoad))
        {
            _customConfig = configLoader.LoadConfiguration(nameToLoad);
            InitializeGame();
        }
    }
    else
    {
        Console.WriteLine("No configurations found.");
    }
}

private static void LoadSavedGame()
{
    IFileSaveLoad gameLoader = FileSaveLoadFactory.GetFileSaveLoadImplementation();
    var gameNames = gameLoader.DisplayAllGames();

    if (gameNames.Count > 0)
    {
        Console.Clear();
        Console.WriteLine("All saved games:\n");
        foreach (var name in gameNames)
        {
            Console.WriteLine(name);
        }

        Console.Write("\nChoose a game to load (type the name) or type R to return > ");
        var saveName = Console.ReadLine();

        if (string.Equals(saveName, "R", StringComparison.OrdinalIgnoreCase))
        {
            Console.Clear();
            return;
        }

        if (!gameNames.Contains(saveName))
        {
            Console.Clear();
            Console.WriteLine($"Could not find < {saveName} >. Are you sure it exists?");
        }
        else
        {
            (CustomConfig loadedConfig, Dictionary<(int, int), char> loadedPieces, Grid loadedGrid) =
                gameLoader.LoadGame(saveName);

            _customConfig = loadedConfig;
            gameSaveName = saveName;
            InitializeLoadedGame(loadedPieces, loadedGrid);
        }
    }
    else
    {
        Console.WriteLine("No saved games found.");
    }
}

private static void InitializeGame()
{
    var board = Board.InitBoard(_customConfig.BoardSize);
    var grid = Grid.InitGrid(_customConfig.BoardSize, _customConfig.GridSize);
    var piecePositions = new Dictionary<(int, int), char>();

    playerOnePiece = _customConfig.PlayerOnePiece[0];
    playerTwoPiece = _customConfig.PlayerTwoPiece[0];

    var currentPlayer = playerOnePiece;
    RunGameLoop(board, grid, piecePositions, currentPlayer);
}

private static void InitializeLoadedGame(Dictionary<(int, int), char> loadedPieces, Grid loadedGrid)
{
    var distinctPieces = loadedPieces.Values.Distinct().ToList();

    if (distinctPieces.Count >= 2)
    {
        playerOnePiece = distinctPieces[0];
        playerTwoPiece = distinctPieces[1];
    }

    var currentPlayer = loadedPieces.Count % 2 == 0 ? playerOnePiece : playerTwoPiece;

    Console.WriteLine("Game loaded successfully.");
    RunGameLoop(Board.InitBoard(_customConfig.BoardSize), loadedGrid, loadedPieces, currentPlayer);
}




        

        
        
        

        private static void RunGameLoop(char[,] board, Grid grid, Dictionary<(int, int), char> piecePositions, char currentPlayer)
        {
            while (true)
            {
                Board.PrintBoard(board, grid, piecePositions);

                var action = PlayTurn(currentPlayer, board, grid, piecePositions, _customConfig.NumPieces);

                if (action == GameAction.Return)
                {
                    Environment.Exit(0);
                }

                var winner = WinChecker.CheckWin(piecePositions, grid);
                if (winner != ' ')
                {
                    Board.PrintBoard(board, grid, piecePositions);
                    Console.WriteLine($"Player {winner} wins!");
                    InitialMenu();
                    return;
                }

                if (action == GameAction.SwitchPlayer)
                {
                    currentPlayer = (currentPlayer == playerOnePiece) ? playerTwoPiece : playerOnePiece;
                    IFileSaveLoad fileSaveLoad = FileSaveLoadFactory.GetFileSaveLoadImplementation();
                    fileSaveLoad.SaveTempGameState(piecePositions, grid, gameSaveName);
                    
                }
            }
        }

        private static GameAction PlayTurn(char player, char[,] board, Grid grid,
            Dictionary<(int, int), char> piecePositions,
            int numPieces)
        {
            Console.WriteLine($"Player < {player} > turn.");
            Console.Write(
                "Do you want to place, move a piece, move the grid, save or return to the main menu? \nplace/move/grid/save/quit >>> ");
            var choice = Console.ReadLine().Trim().ToLower();

            switch (choice)
            {
                case "place":
                    Placer.PlacePiece(player, board, grid, piecePositions, numPieces);
                    return GameAction.SwitchPlayer;

                case "move":
                    Mover.MovePiece(player, board, grid, piecePositions);
                    return GameAction.SwitchPlayer;

                case "grid":
                    Mover.MoveGridInput(grid, board);
                    return GameAction.SwitchPlayer;

                case "save":
                    if (piecePositions.Count >= 2)
                    {
                        Console.WriteLine("Are you sure you want to save the game current game state?");
                        
                        IFileSaveLoad fileSaveLoad = FileSaveLoadFactory.GetFileSaveLoadImplementation();
                        string saveResult = fileSaveLoad.SaveGameState(_customConfig, piecePositions, grid, gameSaveName, _customConfig.ConfigName);
                        Console.WriteLine(saveResult);
                    }
                    else
                    {
                        Console.WriteLine("Both players should place at least one piece before saving.");
                    }

                    return GameAction.Continue;

                case "quit":
                    return GameAction.Return;
                

                default:
                    Console.WriteLine($"\n{choice} is not a valid option. Try again.");
                    PlayTurn(player, board, grid, piecePositions, numPieces);
                    return GameAction.Continue;
                
            }
        }
        
        
        private static void CleanupTempStates()
        {
            IFileSaveLoad fileSaveLoad = FileSaveLoadFactory.GetFileSaveLoadImplementation();
            fileSaveLoad.DeleteAllTempGameStates("");
        }
    }
}