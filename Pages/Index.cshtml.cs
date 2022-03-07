using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheLendingCircle.Models;
using TheLendingCircle.Data;

namespace TheLendingCircle.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public string? SearchString {get;set;}
    public List<Item> ItemsList {get;set;} = new List<Item>();
    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public void OnGet()
    {
        ItemsList = _context.Items.Take(6).ToList();
    }

    public IActionResult OnPost() {
        if (string.IsNullOrEmpty(SearchString)) return RedirectToPage("./Items/Search/");
        return Redirect("/Items/Search?query=" + SearchString);
    }
}
