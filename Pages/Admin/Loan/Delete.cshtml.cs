#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Data;
using TheLendingCircle.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace TheLendingCircle.Pages_Admin_Loan
{
    [Authorize(Roles = "Admin")]

    public class DeleteModel : PageModel
    {
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        public DeleteModel(TheLendingCircle.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Loan Loan { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Loan = await _context.Loans.FirstOrDefaultAsync(m => m.Id == id);

            if (Loan == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Loan = await _context.Loans.FindAsync(id);

            if (Loan != null)
            {
                _context.Loans.Remove(Loan);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
