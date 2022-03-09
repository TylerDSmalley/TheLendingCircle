using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Models;
using TheLendingCircle.Data;
using System.ComponentModel.DataAnnotations;

namespace TheLendingCircle.Pages.Items
{
    public class SearchModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public SearchModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [FromQuery(Name = "searchTerm")]
        public string? SearchTerm { get; set; }

        [BindProperty]
        public string? SearchString { get; set; }

        public List<Item> ItemsList { get; set; } = new List<Item>();

        public void OnGet()
        {
            GetItems();
        }
        private void GetItems(int itemCount = 0)
        {
            if (string.IsNullOrEmpty(SearchTerm))
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
                    .Where(item =>
                        item.Title
                            .ToLower()
                            .Contains(SearchTerm.ToLower())
                        )
                    .Skip(itemCount)
                    .Take(3)
                    .Include("Owner")
                    .ToList();
            }
        }


        public JsonResult OnGetLoadMore(int itemCount, string query)
        {
            SearchTerm = query;
            GetItems(itemCount);
            if (ItemsList.Count() < 1)
            {
                return new JsonResult(new { Success = false });
            }
            return new JsonResult(new { Success = true, Data = ItemsList });
        }
    }
}
