using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
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

        [BindProperty]
        [Required(ErrorMessage = "Game Name is required.")]
        public string GameName { get; set; }
        public void OnGet()
        {
            Configurations = _context.Configurations.ToList(); 
        }
        
        public IActionResult OnPost(int selectedConfig)
        {
            if (selectedConfig <= 0)
            {
                ModelState.AddModelError("selectedConfig", "Please select a configuration.");
                return Page();
            }
            IFileSaveLoad fileSaveLoad = FileSaveLoadFactory.GetFileSaveLoadImplementation();
            fileSaveLoad.SaveInitialGame(GameName, selectedConfig);
            return RedirectToPage("/TheGame/Play", new {gameName = GameName, configId = selectedConfig });
        }
    }
}
