using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Pages.TheGame
{
    public class LoadGameModel : PageModel
    {
        private readonly AppDbContext _context;

        // Inject the database context
        public LoadGameModel(AppDbContext context)
        {
            _context = context;
        }

        // Property to store the games to display
        public List<Game> Games { get; set; } = new List<Game>();

        // Handle GET request to load games
        public void OnGet()
        {
            // Fetch the list of available games from the database
            Games = _context.SavedGames.ToList();
        }

        // Handle POST request when a game is selected
        public IActionResult OnPost(string selectedGame)
        {
            if (string.IsNullOrEmpty(selectedGame))
            {
                ModelState.AddModelError("selectedGame", "Please select a game.");
                return Page();
            }
            
            return RedirectToPage("/TheGame/StartPlay", new { gameId = selectedGame });
        }
    }
}