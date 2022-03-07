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
    public class CreateLoanModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;
        public CreateLoanModel(UserManager<ApplicationUser> userManager,
            TheLendingCircle.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public DateTime DueDate { get; set; }
        }
        public ApplicationUser CurrentUser { get; set; }
        public Request CurrentRequest { get; set; }
        public List<Request> CircleRequests { get; set; }
        public int UnseenRequests { get; set; }

        private async Task LoadAsync(string id)
        {
            CircleRequests = await _context.Requests.Where(i => i.Owner.Id == CurrentUser.Id).ToListAsync();
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            CurrentUser = user;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var Request = await _context.Requests.Where(r => r.Id == id).Include(r => r.Owner).Include(r => r.Borrower).Include(r => r.ItemLoaned).FirstAsync();
            if(Request == null) 
            {
                return NotFound("Unable to load Request with ID");
            }
            CurrentRequest = Request;
            await LoadAsync(user.Id);
            foreach(var request in CircleRequests) {
                if(request.HasBeenViewed == false) {
                    UnseenRequests++;
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (ModelState.IsValid)
            {
                var Request = await _context.Requests.Where(r => r.Id == id).Include(r => r.Owner).Include(r => r.Borrower).Include(r => r.ItemLoaned).FirstAsync();
                var item = await _context.Items.Where(i => i.Id == Request.ItemLoaned.Id).FirstAsync();
                var newLoan = new TheLendingCircle.Models.Loan { CreationTime =  DateTime.Now, DueDate = Input.DueDate, HasBeenViewed = false, Status = "open", Owner = Request.Owner, Borrower = Request.Borrower, ItemLoaned = Request.ItemLoaned};
                item.Available = false;
                _context.Loans.Add(newLoan);
                await _context.SaveChangesAsync();
                _context.Requests.Remove(Request);
                await _context.SaveChangesAsync();
            return RedirectToPage("./OpenCircles");
            }
            return Page();
        }
    }
}
