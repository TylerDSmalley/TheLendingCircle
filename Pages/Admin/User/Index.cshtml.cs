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

namespace TheLendingCircle.Pages_Admin_User
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        public IndexModel(TheLendingCircle.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ApplicationUser> ApplicationUser { get;set; }

        public async Task OnGetAsync()
        {
            ApplicationUser = await _context.Users.ToListAsync();
        }
    }
}
