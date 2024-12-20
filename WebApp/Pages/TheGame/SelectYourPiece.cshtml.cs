using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Pages.TheGame
{
    public class SelectYourPieceModel : PageModel
    {
        private readonly AppDbContext _context;

        public SelectYourPieceModel(AppDbContext context)
        {
            _context = context;
        }
        IFileSaveLoad gameLoader = FileSaveLoadFactory.GetFileSaveLoadImplementation();
        [BindProperty]
        [Required(ErrorMessage = "Please select a piece.")]
        public string SelectedPiece { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter a code for your piece.")]
        [DataType(DataType.Password)]
        public string PiecePassword { get; set; }

        [BindProperty]
        public string GameName { get; set; }

        public List<string> ConfigPieces { get; set; }

        public void OnGet(string gameId)
        {
            GameName = gameId; // Set GameName from query string
            var game = gameLoader.LoadGame(gameId);
            Config GameConfig = _context.Configurations.FirstOrDefault(c => c.ConfigName == game.config.ConfigName);
            if (GameConfig != null)
            {
                ConfigPieces = new List<string>();
                ConfigPieces.Add(GameConfig.PlayerOnePiece);
                ConfigPieces.Add(GameConfig.PlayerTwoPiece);
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // If validation fails, return to the page with the current model state
                return Page();
            }

           
            
            return RedirectToPage("/TheGame/Play", new { gameId = GameName, myPiece = SelectedPiece });
        }
    }
}
