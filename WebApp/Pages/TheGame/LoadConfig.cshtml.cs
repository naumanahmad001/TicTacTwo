using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace WebApp.Pages.TheGame
{
    public class LoadConfigModel : PageModel
    {
        private readonly AppDbContext _context; 

        public LoadConfigModel(AppDbContext context)
        {
            _context = context;
        }
        
        public List<Config> Configurations { get; set; }
        
        public void OnGet()
        {
            Configurations = _context.Configurations.ToList(); 
        }
        
        public IActionResult OnPost(string selectedConfig)
        {
            if (string.IsNullOrEmpty(selectedConfig))
            {
                ModelState.AddModelError("selectedConfig", "Please select a configuration.");
                return Page();
            }
            
            return RedirectToPage("/TheGame/Play", new { configId = selectedConfig });
        }
    }
}
