using System.Linq;
using Components;
using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DAL
{
    public class FileSaveLoadDb : IFileSaveLoad
    {
        private readonly AppDbContext _dbContext;

        public FileSaveLoadDb()
        {
            var factory = new AppDbContextFactory();
            _dbContext = factory.CreateDbContext(new string[0]);
        }
        





        public void SaveConfiguration(CustomConfig customConfig, string configName)
        {
            var existingConfig = _dbContext.Configurations.FirstOrDefault(c => c.ConfigName == configName);

            if (existingConfig != null)
            {
                Console.WriteLine(
                    $"A configuration with the name '{configName}' already exists. Do you want to overwrite it? (y/n)");
                var overwrite = Console.ReadLine()?.Trim().ToLower();

                if (overwrite != "y" && overwrite != "yes")
                {
                    Console.WriteLine("Configuration save cancelled.");
                    return;
                }

                existingConfig.BoardSize = customConfig.BoardSize;
                existingConfig.GridSize = customConfig.GridSize;
                existingConfig.PiecesAmount = customConfig.NumPieces;
                existingConfig.PlayerOnePiece = customConfig.PlayerOnePiece;
                existingConfig.PlayerTwoPiece = customConfig.PlayerTwoPiece;

                Console.WriteLine("Configuration overwritten successfully.");
            }
            else
            {
                var newConfig = new Config
                {
                    ConfigName = configName,
                    BoardSize = customConfig.BoardSize,
                    GridSize = customConfig.GridSize,
                    PiecesAmount = customConfig.NumPieces,
                    PlayerOnePiece = customConfig.PlayerOnePiece,
                    PlayerTwoPiece = customConfig.PlayerTwoPiece
                };

                _dbContext.Configurations.Add(newConfig);
                Console.WriteLine("Configuration saved successfully.");
            }

            _dbContext.SaveChanges();
        }












        public CustomConfig LoadConfiguration(string configName)
        {
            var config = _dbContext.Configurations
                .FirstOrDefault(c => c.ConfigName == configName);

            if (config == null)
            {
                Console.WriteLine($"No configuration found with the name '{configName}'");
                return null;
            }

            return new CustomConfig
            {
                BoardSize = config.BoardSize,
                GridSize = config.GridSize,
                NumPieces = config.PiecesAmount,
                PlayerOnePiece = config.PlayerOnePiece,
                PlayerTwoPiece = config.PlayerTwoPiece,
                ConfigName = config.ConfigName
            };
        }












        public List<CustomConfig> DisplayAllConfigurations()
        {
            var configs = _dbContext.Configurations
                .Select(c => new CustomConfig
                {
                    ConfigName = c.ConfigName,
                    BoardSize = c.BoardSize,
                    GridSize = c.GridSize,
                    NumPieces = c.PiecesAmount,
                    PlayerOnePiece = c.PlayerOnePiece,
                    PlayerTwoPiece = c.PlayerTwoPiece
                })
                .ToList();

            if (configs.Count == 0)
            {
                Console.WriteLine("No configurations found.");
                return new List<CustomConfig>();
            }

            return configs;
        }
        
        
        
        
        
        
        


        public string SaveGameState(CustomConfig customConfig, Dictionary<(int, int), char> pieces, Grid grid,
            int gameId, string configName)
        {

            var existingGame = _dbContext.SavedGames.FirstOrDefault(g => g.Id == gameId);
            var config = _dbContext.Configurations.FirstOrDefault(c => c.ConfigName == configName);

            try
            {
                string? gridTopLeft = $"{grid.TopLeft.Item1},{grid.TopLeft.Item2}";
                string firstPlayerPassword = GeneratePassword();
                string secondPlayerPassword = GeneratePassword();

                if (existingGame != null)
                {
                    //existingGame.ConfigId = config.Id;
                    existingGame.PositionsJson = JsonConvert.SerializeObject(pieces); // Updating PositionsJson
                    existingGame.GridTopLeft = gridTopLeft; // Updating GridTopLeft
                    //existingGame.FirstPlayerPassword = firstPlayerPassword; // Updating password for the first player
                    //existingGame.SecondPlayerPassword = secondPlayerPassword; // Updating password for the second player
                }


                // Save the changes to the database.
                _dbContext.SaveChanges();

                return "Game saved successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the game: {ex.Message}");
                return "Game save failed";
            }
        }







        
        
        
        
        private string GeneratePassword()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }









        public List<string> DisplayAllGames()
        {
            var gameNames = _dbContext.SavedGames
                .Select(g => g.GameName)
                .ToList();

            if (gameNames.Count == 0)
            {
                Console.WriteLine("No games found.");
                return new List<string>();
            }

            return gameNames;
        }












        public (CustomConfig config, Dictionary<(int, int), char> pieces, Grid grid) LoadGame(int saveId)
        {
            var game = _dbContext.SavedGames
                .FirstOrDefault(g => g.Id == saveId);

            if (game == null)
            {
                Console.WriteLine($"Game with the id '{saveId}' not found.");
                return (null, null, null);
            }

            var config = _dbContext.Configurations
                .FirstOrDefault(c => c.Id == game.ConfigId);

            if (config == null)
            {
                Console.WriteLine($"Configuration for id '{saveId}' not found.");
                return (null, null, null);
            }

            if (game.PositionsJson != null && game.GridTopLeft != null)
            {
                var pieces = JsonConvert.DeserializeObject<Dictionary<string, char>>(game.PositionsJson)
                    .ToDictionary(entry =>
                        {
                            var cleanedKey =
                                entry.Key.Replace("(", "").Replace(")", "").Trim(); // Remove parentheses and trim spaces
                            var coords = cleanedKey.Split(',');
                            return (int.Parse(coords[0]), int.Parse(coords[1]));
                        },
                        entry => entry.Value);

                var topLeftCoords = game.GridTopLeft.Split(',');
                var gridTopLeft = (int.Parse(topLeftCoords[0]), int.Parse(topLeftCoords[1]));
                var grid = new Grid
                {
                    TopLeft = gridTopLeft,
                    Size = config.GridSize
                };

                return (new CustomConfig
                {
                    ConfigName = config.ConfigName,
                    BoardSize = config.BoardSize,
                    GridSize = config.GridSize,
                    NumPieces = config.PiecesAmount,
                    PlayerOnePiece = config.PlayerOnePiece,
                    PlayerTwoPiece = config.PlayerTwoPiece
                }, pieces, grid);
            }
            else {
                return (new CustomConfig
                {
                    ConfigName = config.ConfigName,
                    BoardSize = config.BoardSize,
                    GridSize = config.GridSize,
                    NumPieces = config.PiecesAmount,
                    PlayerOnePiece = config.PlayerOnePiece,
                    PlayerTwoPiece = config.PlayerTwoPiece
                }, null, null);
            }
        }

        
        
        
        
        
        
        
        
        

        public int SaveInitialGame(string saveName, int configId, string firstPlayerPassword, string secondPlayerPassword)
        {

            var newGame = new Game
            {
                GameName = saveName,
                ConfigId = configId,
                PositionsJson = null,
                GridTopLeft = null,
                FirstPlayerPassword = firstPlayerPassword,
                SecondPlayerPassword = secondPlayerPassword
            };

            _dbContext.SavedGames.Add(newGame);
            _dbContext.SaveChanges();
            return newGame.Id;
        }
        
        
        
        
        
        
        
        
        

        public void SaveTempGameState(Dictionary<(int, int), char> positions, Grid grid, int gameId)
        {
            var positionsStringKeys = positions.ToDictionary(
                kvp => $"{kvp.Key.Item1},{kvp.Key.Item2}",
                kvp => kvp.Value
            );
            var game = _dbContext.SavedGames.FirstOrDefault(g => g.Id == gameId);
            

            var highestMoveNumber = _dbContext.TempStates
                .Max(ts => (int?)ts.MoveNumber) ?? 0;

            var moveNumber = highestMoveNumber + 1;

            var tempGameState = new TempGameState
            {
                GridTopLeft = $"{grid.TopLeft.Item1},{grid.TopLeft.Item2}",
                Positions = positionsStringKeys,
                MoveNumber = moveNumber,
                GameId = gameId.ToString()
            };


            _dbContext.TempStates.Add(tempGameState);
            _dbContext.SaveChanges();
        }
        
        
        
        public void DeleteAllTempGameStates(int GameId)
        {
            var tempStates = _dbContext.TempStates.Where(a=>a.GameId == GameId.ToString()).ToList();
            _dbContext.TempStates.RemoveRange(tempStates);
            _dbContext.SaveChanges();
        }


    }
    
}
