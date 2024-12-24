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
        public int GameId { get; set; }
        public bool IsNewGame { get; set; }
        public string MyPiece { get; set; }
        public int MatrixStartRow { get; set; }
        public int MatrixStartColumn { get; set; }
        public int PiecesAmount { get; set; }
        public Dictionary<string, char> SavedPieces { get; set; } = new Dictionary<string, char>();

        public IActionResult OnGet(int? configId, int? gameId, string? myPiece, string? reload)
        {
            if (gameId != null)
            {
                GameId = gameId.Value;
                // Load game by gameId
                //Game savedGame = _context.SavedGames.Where(a=>a.Id == gameId).FirstOrDefault(); 
                var game = gameLoader.LoadGame(gameId.Value);
                string GridTopLeft = string.Empty;
                //IsNewGame = false;
                if (reload != null)
                {
                    var tempGame = _context.TempStates.Where(a => a.GameId == gameId.Value.ToString()).FirstOrDefault();
                    if (tempGame.Positions != null)
                    {
                        SavedPieces = tempGame.Positions;
                    }
                    GridTopLeft = tempGame.GridTopLeft;
                }
                else
                {
                    if (game.pieces != null)
                    {
                        SavedPieces = game.pieces.ToDictionary(
                        kvp => $"{kvp.Key.Item1},{kvp.Key.Item2}", // Convert tuple keys to "row,col" strings
                        kvp => kvp.Value
                    );
                    }
                }
                GameConfig = _context.Configurations.FirstOrDefault(c => c.ConfigName == game.config.ConfigName);
                //GameName = savedGame.GameName;
                MyPiece = myPiece;
                PiecesAmount = GameConfig.PiecesAmount;

                if (reload == null)
                {

                    if (game.grid == null)
                    {
                        MatrixStartRow = 0;
                        MatrixStartColumn = 0;
                    }
                    else
                    {
                        MatrixStartRow = game.grid.TopLeft.Item1;
                        MatrixStartColumn = game.grid.TopLeft.Item2;
                    }
                }
                else {
                    string[] GridTopLeftPoints = GridTopLeft.Split(",");
                    MatrixStartRow = Convert.ToInt16(GridTopLeftPoints[0].Trim());
                    MatrixStartColumn = Convert.ToInt16(GridTopLeftPoints[1].Trim());
                }
               
                
            }
            else
            {
                return BadRequest("Invalid request parameters.");
            }

            return Page();
        }


        public class SaveTempGameRequest
        {
            public int GameId { get; set; }
            public Dictionary<string, string> Positions { get; set; } = new Dictionary<string, string>();
            public GridDto Grid { get; set; } = default!;
            public bool? SkipDeleteTempStates { get; set; }
        }

        public class GridDto
        {
            public string TopLeft { get; set; } = default!;
        }

       

    }
}
