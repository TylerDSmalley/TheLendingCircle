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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            CurrentUser = user;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var Request = await _context.Requests.FindAsync(id);
            if(Request == null) 
            {
                return NotFound("Unable to load Request with ID");
            }
            CurrentRequest = Request;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var newLoan = new TheLendingCircle.Models.Loan { CreationTime =  DateTime.Now, DueDate = Input.DueDate, HasBeenViewed = false, Status = "open", Owner = CurrentRequest.Owner, Borrower = CurrentRequest.Borrower, ItemLoaned = CurrentRequest.ItemLoaned};
                _context.Loans.Add(newLoan);
                await _context.SaveChangesAsync();
                _context.Requests.Remove(CurrentRequest);
                await _context.SaveChangesAsync();
            return RedirectToPage("./OpenCircles");
            }
            return Page();
        }
    }
}
