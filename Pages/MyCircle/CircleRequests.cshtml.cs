using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Models;

namespace TheLendingCircle.Pages.MyCircle
{
    [Authorize]
    public class CircleRequestsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        public CircleRequestsModel(UserManager<ApplicationUser> userManager,
            TheLendingCircle.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public ApplicationUser CurrentUser { get; set; }

        public List<Request> CircleRequests { get; set; }
        public List<Request> PendingRequests { get; set; }
        private async Task LoadAsync(string id)
        {
            CircleRequests = await _context.Requests.Where(i => i.Owner.Id == id).ToListAsync();
            PendingRequests = await _context.Requests.Where(i => i.Borrower.Id == id).ToListAsync();
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
            return Page();
        }
    }
}
