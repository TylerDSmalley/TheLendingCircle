using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Models;
using TheLendingCircle.Data;

namespace TheLendingCircle.Pages.Items
{
    public class SearchModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public SearchModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [FromQuery(Name = "query")]
        public string? Query { get; set; }

        [BindProperty]
        public string? SearchString { get; set; }

        public List<Item> ItemsList { get; set; } = new List<Item>();

        public void OnGet()
        {
            GetItems();
        }

        public IActionResult OnPost()
        {
            return Redirect("./Search?query=" + SearchString);
        }

        private void GetItems(int itemCount = 0)
        {
            if (string.IsNullOrEmpty(Query))
            {
                ItemsList = _context.Items
                                .Skip(itemCount)
                                .Take(3)
                                .Include("Owner")
                                .ToList();
            }
            else
            {
                ItemsList = _context.Items
                    .Skip(itemCount)
                    .Take(3)
                    .Where(item =>
                        item.Title
                            .ToLower()
                            .Contains(Query.ToLower())
                        )
                    .Include("Owner")
                    .ToList();
            }
        }


        public JsonResult OnGetLoadMore(int itemCount)
        {
            GetItems(itemCount);
            return new JsonResult(ItemsList);
        }
    }
}
