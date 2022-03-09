#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheLendingCircle.Data;
using TheLendingCircle.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace TheLendingCircle.Pages_Admin_User
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        public CreateModel(TheLendingCircle.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Users.Add(ApplicationUser);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
