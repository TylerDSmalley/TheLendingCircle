#nullable disable
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Models;

namespace TheLendingCircle.Pages.MyCircle
{
    [Authorize]
    public class AddItemModel : PageModel
    {
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddItemModel(UserManager<ApplicationUser> userManager, TheLendingCircle.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public ApplicationUser CurrentUser { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
        [Required, StringLength(100, MinimumLength = 1, ErrorMessage ="Title must be between 1 and 100 characters")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required, StringLength(4000, MinimumLength = 1, ErrorMessage ="Description must be between 1 and 4000 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required, StringLength(100, MinimumLength = 1, ErrorMessage ="Condition must be between 1 and 100 characters")]
        [Display(Name = "Condition")]
        public string Condition { get; set; }
        }
        public int UnseenRequests { get; set; }
        public List<Request> CircleRequests { get; set; }
        public List<Loan> PendingLoans { get; set; }
        public int UnseenLoans { get; set; }


        private async Task LoadAsync(string id)
        {
            CircleRequests = await _context.Requests.Where(i => i.Owner.Id == CurrentUser.Id).ToListAsync();
            PendingLoans = await _context.Loans.Where(i => i.Borrower.Id == CurrentUser.Id && i.HasBeenViewed == false).Include(r => r.Owner).Include(r => r.Borrower).Include(r => r.ItemLoaned).ToListAsync();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await LoadAsync(CurrentUser.Id);
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

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {   
                return Page();
            }
            Item newItem = new TheLendingCircle.Models.Item { 
                Title = Input.Title, 
                Description = Input.Description, 
                Condition = Input.Condition, 
                ItemPhotoPath = "https://tonsmb.org/wp-content/uploads/2014/03/default-placeholder.png", 
                Available = true, 
                Owner = user
            };
            _context.Items.Add(newItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./MyItems");
        }
    }
}
