using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Pages.TheGame
{
    public class SelectYourPieceModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Please select a piece.")]
        public string SelectedPiece { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter a code for your piece.")]
        [DataType(DataType.Password)]
        public string PiecePassword { get; set; }

        [BindProperty]
        public string GameName { get; set; }

        public void OnGet(string gameId)
        {
            GameName = gameId; // Set GameName from query string
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // If validation fails, return to the page with the current model state
                return Page();
            }

            // Example logic
            // IFileSaveLoad fileSaveLoad = FileSaveLoadFactory.GetFileSaveLoadImplementation();
            // fileSaveLoad.SaveInitialGame(GameName, selectedConfig);

            return RedirectToPage("/TheGame/Play", new { gameId = GameName, myPiece = SelectedPiece });
        }
    }
}
