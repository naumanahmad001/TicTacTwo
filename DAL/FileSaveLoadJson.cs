// namespace DAL;
//
// using System.Text.Json;
//
//
//
//     public static class FilePaths
//     {
//         public static string BasePath = Environment
//                                             .GetFolderPath(Environment.SpecialFolder.UserProfile)
//                                         + Path.DirectorySeparatorChar + "tic-tac-two" + Path.DirectorySeparatorChar;
//         
//         public static string GameExtension = ".json";
//     }
//
// public class FileSaveLoadJson : IFileSaveLoad
// {
//     private string SaveFolderPath = Path.Combine(FilePaths.BasePath, "SavedGames");
//
//     public FileSaveLoadJson()
//     {
//         if (!Directory.Exists(SaveFolderPath))
//         {
//             Directory.CreateDirectory(SaveFolderPath);
//         }
//     }
//     
//     public void SaveGame(CustomConfig customConfig, Dictionary<(int, int), char> pieces, Grid grid, string name)
//     {
//         string saveFilePath = Path.Combine(SaveFolderPath, $"{name}{FilePaths.GameExtension}");
//
//         var piecesToSave = new Dictionary<string, char>();
//         foreach (var piece in pieces)
//         {
//             piecesToSave[$"{piece.Key.Item1},{piece.Key.Item2}"] = piece.Value;
//         }
//
//         var gameState = new GameState
//         {
//             BoardSize = customConfig.BoardSize,
//             GridSize = grid.Size,
//             NumPieces = customConfig.NumPieces,
//             Pieces = piecesToSave,
//             GridTopLeft = $"{grid.TopLeft.Item1},{grid.TopLeft.Item2}"
//         };
//
//         var json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions { WriteIndented = true });
//         File.WriteAllText(saveFilePath, json);
//     }
//
//     
//     public (CustomConfig config, Dictionary<(int, int), char> pieces, Grid grid) LoadGame(string saveName)
//     {
//         if (!Directory.Exists(SaveFolderPath))
//             throw new DirectoryNotFoundException("Saved games directory not found.");
//         
//         string saveFilePath = Path.Combine(SaveFolderPath, $"{saveName}{FilePaths.GameExtension}");
//         
//         if (!File.Exists(saveFilePath))
//             throw new FileNotFoundException($"No saved game found with the name '{saveName}'.");
//         
//         var json = File.ReadAllText(saveFilePath);
//
//         if (string.IsNullOrWhiteSpace(json))
//         {
//             throw new InvalidOperationException("The saved game file is empty or invalid.");
//         }
//
//         var gameState = JsonSerializer.Deserialize<GameState>(json);
//
//         var config = new CustomConfig
//         {
//             BoardSize = gameState.BoardSize,
//             NumPieces = gameState.NumPieces
//         };
//
//         var pieces = new Dictionary<(int, int), char>();
//
//         foreach (var kvp in gameState.Pieces)
//         {
//             var coords = kvp.Key.Split(',');
//             var key = (int.Parse(coords[0]), int.Parse(coords[1]));
//             pieces[key] = kvp.Value;
//         }
//
//         var gridCoords = gameState.GridTopLeft.Split(',');
//         var gridTopLeft = (int.Parse(gridCoords[0]), int.Parse(gridCoords[1]));
//
//         var grid = new Grid
//         {
//             Size = gameState.GridSize,
//             TopLeft = gridTopLeft
//         };
//
//         return (config, pieces, grid);
//     }
//     
//     public List<string> GetAllGameNames()
//     {
//         var saveFiles = Directory.GetFiles(SaveFolderPath, $"*{FilePaths.GameExtension}");
//         
//         var gameNames = saveFiles.Select(Path.GetFileNameWithoutExtension).ToList();
//         
//         return gameNames;
//     }
//     
//     public class GameState
//     {
//         public int BoardSize { get; set; }
//         public int GridSize { get; set; }
//         public int NumPieces { get; set; }
//         public Dictionary<string, char> Pieces { get; set; }
//         public string GridTopLeft { get; set; }
//     }
// }
//
