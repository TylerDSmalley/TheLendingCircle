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

namespace TheLendingCircle.Pages_Admin_Review
{
    public class DetailsModel : PageModel
    {
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        public DetailsModel(TheLendingCircle.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Review Review { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Review = await _context.Review.FirstOrDefaultAsync(m => m.Id == id);

            if (Review == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
