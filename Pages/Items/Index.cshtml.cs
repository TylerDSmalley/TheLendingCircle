using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TheLendingCircle.Models;
using TheLendingCircle.Data;

namespace TheLendingCircle.Pages.Items
{
    public class Index : PageModel
    {
        private ApplicationDbContext _context;
        private readonly ILogger<Index> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Item? CurrentItem { get; set; }
        // public ApplicationUser? Owner {get;set;}

        public Index(ILogger<Index> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = _context.Users.First(i => i.Email == "jim@bob.com");
            var item = _context.Items.First(i => i.Id == Id);
            if (item == null)
            {
                return NotFound($"Unable to item user with ID '{Id}'.");
            }
            CurrentItem = item;
            _logger.LogInformation($"Owner: {CurrentItem.Owner}");

            // Owner = await _context.Users.FindAsync(CurrentItem.Owner.Id);
            return Page();
        }
    }
}
