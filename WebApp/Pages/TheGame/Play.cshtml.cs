using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.TheGame
{
    public class PlayModel : PageModel
    {
        private readonly AppDbContext _context;

        public PlayModel(AppDbContext context)
        {
            _context = context;
        }
        public Config GameConfig { get; set; } = default!;
        public IActionResult OnGet(int configId)
        {
            // Load the game configuration based on configId
            GameConfig = _context.Configurations.Where(a=>a.Id == configId).FirstOrDefault();

            if (GameConfig == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
