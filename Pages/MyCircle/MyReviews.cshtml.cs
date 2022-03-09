using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Models;

namespace TheLendingCircle.Pages.MyCircle.MyReviews
{
    [Authorize]
    public class MyReviewsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        public MyReviewsModel(UserManager<ApplicationUser> userManager,
            TheLendingCircle.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public ApplicationUser CurrentUser { get; set; }

        public List<Review> Reviews { get; set; }
        public List<Request> CircleRequests { get; set; }
        public int UnseenRequests { get; set; }
        public List<Loan> PendingLoans { get; set; }
        public int UnseenLoans { get; set; }

        private async Task LoadAsync(string id)
        {
            CircleRequests = await _context.Requests.Where(i => i.Owner.Id == CurrentUser.Id).ToListAsync();
            Reviews = await _context.Reviews.Where(r => r.Borrower.Id == id).Include(r => r.Owner).Include(r => r.Borrower).ToListAsync();
            PendingLoans = await _context.Loans.Where(i => i.Borrower.Id == CurrentUser.Id && i.HasBeenViewed == false).Include(r => r.Owner).Include(r => r.Borrower).Include(r => r.ItemLoaned).ToListAsync();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            CurrentUser = user;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await LoadAsync(user.Id);
            foreach(var request in CircleRequests) {
                if(request.HasBeenViewed == false) {
                    UnseenRequests++;
                }
            }
            if(PendingLoans != null) {
                UnseenLoans = PendingLoans.Count();
            } else {
                UnseenLoans = 0;
            }
            return Page();
        }
    }
}
