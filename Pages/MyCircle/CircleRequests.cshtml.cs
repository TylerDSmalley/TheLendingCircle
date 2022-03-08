using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Models;
using System.ComponentModel.DataAnnotations;

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

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public int Id { get; set; }
        }
        private async Task LoadAsync(string id)
        {
            CircleRequests = await _context.Requests.Where(i => i.Owner.Id == CurrentUser.Id).Include(r => r.Owner).Include(r => r.Borrower).Include(r => r.ItemLoaned).ToListAsync();
            PendingRequests = await _context.Requests.Where(i => i.Borrower.Id == CurrentUser.Id).Include(r => r.Owner).Include(r => r.Borrower).Include(r => r.ItemLoaned).ToListAsync();
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
            foreach(var request in CircleRequests){
                request.HasBeenViewed = true;
            }
            await _context.SaveChangesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var deleteRequest = await _context.Requests.FindAsync(Input.Id);
            if (deleteRequest != null)
            {
                _context.Requests.Remove(deleteRequest);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./CircleRequests");
        }
    }
}
