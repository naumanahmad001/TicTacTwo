using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.TheGame
{
    public class NewConfigModel : PageModel
    {
        private readonly AppDbContext _context;
        [BindProperty]
        public Config Config { get; set; } = new Config();
        public NewConfigModel(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Save the configuration to the database
            _context.Configurations.Add(Config);
            _context.SaveChanges();

            // Redirect to the list of configurations or home page
            return RedirectToPage("/Index");
        }
    }
}
