using Components;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WebApp.Pages.TheGame
{
    public class PlayModel : PageModel
    {
        private readonly AppDbContext _context;
        IFileSaveLoad gameLoader = FileSaveLoadFactory.GetFileSaveLoadImplementation();
        public PlayModel(AppDbContext context)
        {
            _context = context;
        }
        public Config GameConfig { get; set; } = default!;
        public string GameName { get; set; }
        public bool IsNewGame { get; set; }
        public string MyPiece { get; set; }
        public Dictionary<string, char> SavedPieces { get; set; } = new Dictionary<string, char>();

        public IActionResult OnGet(string? gameName, int? configId, string? gameId, string? myPiece)
        {
            if (!string.IsNullOrEmpty(gameId))
            {
                // Load game by gameId
                var game = gameLoader.LoadGame(gameId);
                //IsNewGame = false;

                if (game.pieces != null)
                {
                       SavedPieces = game.pieces.ToDictionary(
                       kvp => $"{kvp.Key.Item1},{kvp.Key.Item2}", // Convert tuple keys to "row,col" strings
                       kvp => kvp.Value
                   );
                }
                GameConfig = _context.Configurations.FirstOrDefault(c => c.ConfigName == game.config.ConfigName);
                GameName = gameId;
                MyPiece = myPiece;
            }
            //else if (!string.IsNullOrEmpty(gameName) && configId.HasValue)
            //{
            //    IsNewGame = true;
            //    // Load game configuration by configId
            //    GameConfig = _context.Configurations.FirstOrDefault(a => a.Id == configId.Value);
            //    if (GameConfig == null)
            //    {
            //        return NotFound("Configuration not found.");
            //    }

            //    GameName = gameName;
            //}
            else
            {
                return BadRequest("Invalid request parameters.");
            }

            return Page();
        }


        public class SaveTempGameRequest
        {
            public string GameSaveName { get; set; } = default!;
            public Dictionary<string, string> Positions { get; set; } = new Dictionary<string, string>();
            public GridDto Grid { get; set; } = default!;
        }

        public class GridDto
        {
            public string TopLeft { get; set; } = default!;
        }

       

    }
}
