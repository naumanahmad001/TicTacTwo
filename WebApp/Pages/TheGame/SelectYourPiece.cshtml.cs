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
        
        public string ErrorMessage { get; set; }

      
        public string FirstPlayerPassword { get; set; }
       
        public string SecondPlayerPassword { get; set; }

       
        public List<string> ConfigPieces { get; set; }
       
        public Config GameConfig { get; set; }
        public void OnGet(string gameId)
        {
            LoadGameData(gameId);
        }

        public IActionResult OnPost()
        {
            LoadGameData(GameName);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (SelectedPiece == GameConfig.PlayerOnePiece && PiecePassword == FirstPlayerPassword)
            {
                return RedirectToPage("/TheGame/Play", new { gameId = GameName, myPiece = SelectedPiece });  
            }
            else if (SelectedPiece == GameConfig.PlayerTwoPiece && PiecePassword == SecondPlayerPassword) 
            {
                return RedirectToPage("/TheGame/Play", new { gameId = GameName, myPiece = SelectedPiece });
            }
            else {
                ErrorMessage = "Invalid piece or password. Please try again.";
                return Page();
            }
            
           
        }
        private void LoadGameData(string gameId)
        {
            GameName = gameId; // Set GameName from query string
            var game = gameLoader.LoadGame(gameId);
            var savedGame = _context.SavedGames.FirstOrDefault(g => g.GameName == gameId);
            GameConfig = _context.Configurations.FirstOrDefault(c => c.ConfigName == game.config.ConfigName);
            if (GameConfig != null)
            {
                ConfigPieces = new List<string>
        {
            GameConfig.PlayerOnePiece,
            GameConfig.PlayerTwoPiece
        };
                FirstPlayerPassword = savedGame.FirstPlayerPassword;
                SecondPlayerPassword = savedGame.SecondPlayerPassword;
            }
        }

    }
}
