using Components;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult OnGet(string? gameName, int? configId, string? gameId)
        {
            if (!string.IsNullOrEmpty(gameId))
            {
                // Load game by gameId
                var game = gameLoader.LoadGame(gameId);

                //var config = _context.Configurations
                //.FirstOrDefault(c => c.Id == game.config);
                //GameConfig = game.config; // Assuming the loaded game has a GameConfig
                //GameName = game.gam;     // Assuming the loaded game has a GameName
            }
            else if (!string.IsNullOrEmpty(gameName) && configId.HasValue)
            {
                // Load game configuration by configId
                GameConfig = _context.Configurations.FirstOrDefault(a => a.Id == configId.Value);
                if (GameConfig == null)
                {
                    return NotFound("Configuration not found.");
                }

                GameName = gameName;
            }
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
