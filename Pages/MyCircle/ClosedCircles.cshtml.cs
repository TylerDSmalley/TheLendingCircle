using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Models;

namespace TheLendingCircle.Pages.MyCircle
{
    [Authorize]
    public class ClosedCirclesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        public ClosedCirclesModel(UserManager<ApplicationUser> userManager,
            TheLendingCircle.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public ApplicationUser CurrentUser { get; set; }
        public List<Loan> LoanedItems { get; set; }
        public List<Loan> BorrowedItems { get; set; }
        public List<Request> CircleRequests { get; set; }
        public int UnseenRequests { get; set; }
        private async Task LoadAsync(string id)
        {
            CircleRequests = await _context.Requests.Where(i => i.Owner.Id == CurrentUser.Id).ToListAsync();
            LoanedItems = await _context.Loans.Where(l => l.Owner.Id == id && l.Status == "closed").Include(l => l.ItemLoaned).Include(r => r.Owner).Include(r => r.Borrower).ToListAsync();
            BorrowedItems = await _context.Loans.Where(l => l.Borrower.Id == id && l.Status == "closed").Include(l => l.ItemLoaned).Include(r => r.Owner).Include(r => r.Borrower).ToListAsync();
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
            return Page();
        }
    }
}