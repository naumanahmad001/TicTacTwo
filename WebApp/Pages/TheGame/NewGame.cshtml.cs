using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.TheGame
{
    public class NewGameModel : PageModel
    {
        private readonly ILogger<NewGameModel> _logger;

        public NewGameModel(ILogger<NewGameModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnPost(string action)
        {
            if (action == "new-config")
            {
                // Redirect to the page where a new config can be created
                return RedirectToPage("/TheGame/NewConfig");
            }
            else if (action == "set-config")
            {
                // Redirect to the page where an existing config can be selected
                return RedirectToPage("/TheGame/LoadConfig");
            }
            else if (action == "play")
            {
                // Redirect to the page where an existing config can be selected
                return RedirectToPage("/TheGame/Play");
            }

            // Default case
            return Page();
        }
    }
}