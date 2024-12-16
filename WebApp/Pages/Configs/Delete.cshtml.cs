using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Configs
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Config Config { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var config = await _context.Configurations.FirstOrDefaultAsync(m => m.Id == id);

            if (config is not null)
            {
                Config = config;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var config = await _context.Configurations.FindAsync(id);
            if (config != null)
            {
                Config = config;
                _context.Configurations.Remove(Config);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
