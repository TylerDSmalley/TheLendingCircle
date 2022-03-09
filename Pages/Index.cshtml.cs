using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheLendingCircle.Models;
using TheLendingCircle.Data;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace TheLendingCircle.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public string? SearchString { get; set; }
    public List<Item> ItemsList { get; set; } = new List<Item>();
    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public void OnGet()
    {
        ItemsList = _context.Items.Take(5).ToList();
    }
}
