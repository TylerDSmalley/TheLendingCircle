using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheLendingCircle.Data;
using TheLendingCircle.Models;

namespace TheLendingCircle.Pages.MyCircle
{
    public class CloseCircleModel : PageModel
    {private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        //NEEDS LOGIC TO CLOSE CIRCLE AND CHANGE LOAN STATUS TO closed;
        //THE ID PASSED IS LOAN ID

        public CloseCircleModel(TheLendingCircle.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Review Review { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Broken Query
            // var LoanedItems = await _context.Loans.Where(l => l.Id == Id );

            _context.Review.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ClosedCircles");
        }
    }
}

