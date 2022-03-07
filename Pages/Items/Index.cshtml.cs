using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheLendingCircle.Models;
using TheLendingCircle.Data;

namespace TheLendingCircle.Pages.Items
{
    public class Index : PageModel
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<Index> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Item? CurrentItem { get; set; }
        public ApplicationUser? Owner {get;set;} = new ApplicationUser();

        [Required]
        [BindProperty, Display(Name = "Request Message")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage ="Message must be between 1 and 1000 characters")]
        public string? RequestMessage {get;set;}

        public Index(ILogger<Index> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var item = _context.Items.Where(i => i.Id == Id).Include("Owner").First();
            if (item == null)
            {
                return NotFound($"Unable to item user with ID '{Id}'.");
            }
            CurrentItem = item;
            // _logger.LogInformation($"Owner: {CurrentItem.Owner}");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                CurrentItem = _context.Items.Where(i => i.Id == Id).Include("Owner").First();
                return Page();
            }
            _context.Request.Add(new Request() {
                Message = RequestMessage, 
                CreationTime = DateTime.Now, 
                HasBeenViewed = false,
                Borrower = await _userManager.GetUserAsync(User)
                });
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
