using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Models;

namespace TheLendingCircle.Pages
{
    
    public class ProfileModel : PageModel
    {
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        public ProfileModel(TheLendingCircle.Data.ApplicationDbContext context){
            _context = context;
        }

        public ApplicationUser? curUser {get; set;}
        public List<Review> userReviews {get; set;}
       public async Task<IActionResult> OnGetAsync(string? id)
        {
             curUser = await _context.Users.FirstOrDefaultAsync(e => e.Email == id);
             userReviews = await _context.Review.Where(i => i.Borrower.Id == curUser.Id).ToListAsync();
            
            if (curUser == null)
            {
                return NotFound($"Unable to load user");
            }
            return Page();
        }
    }
}
