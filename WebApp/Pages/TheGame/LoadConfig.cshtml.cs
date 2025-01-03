﻿using DAL;
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
        [BindProperty]
        [Required(ErrorMessage = "First player password is required.")]
        public string FirstPlayerPassword { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Second player password is required.")]
        public string SecondPlayerPassword { get; set; }
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
            int gameId = fileSaveLoad.SaveInitialGame(GameName, selectedConfig, FirstPlayerPassword, SecondPlayerPassword);
            return RedirectToPage("/TheGame/SelectYourPiece", new { gameId = gameId });
        }
    }
}
