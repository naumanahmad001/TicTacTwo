using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppTest.Data;

namespace WebAppTest.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly ApplicationDbContext _ctx;

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    public void OnGet()
    {
    }
}