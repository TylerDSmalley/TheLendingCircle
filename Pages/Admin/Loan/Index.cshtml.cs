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

namespace TheLendingCircle.Pages_Admin_Loan
{
    public class IndexModel : PageModel
    {
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        public IndexModel(TheLendingCircle.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Loan> Loan { get;set; }

        public async Task OnGetAsync()
        {
            Loan = await _context.Loans.ToListAsync();
        }
    }
}
