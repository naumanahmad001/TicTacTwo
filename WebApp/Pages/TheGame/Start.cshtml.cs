using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.TheGame
{
    public class StartModel : PageModel
    {
        private readonly ILogger<StartModel> _logger;

        public StartModel(ILogger<StartModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnPost(string action)
        {
            if (action == "new-game")
            {
                // Redirect to the page where the user starts a new game
                return RedirectToPage("/TheGame/NewGame");
            }
            else if (action == "load-game")
            {
                // Redirect to the page where the user can load an existing game
                return RedirectToPage("/TheGame/LoadGame");
            }

            // Default case if something unexpected happens
            return Page();
        }
    }
}