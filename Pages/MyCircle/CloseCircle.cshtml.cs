using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheLendingCircle.Data;
using TheLendingCircle.Models;
using System.ComponentModel.DataAnnotations;

namespace TheLendingCircle.Pages.MyCircle
{
    [Authorize]
    public class CloseCircleModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        //NEEDS LOGIC TO CLOSE CIRCLE AND CHANGE LOAN STATUS TO closed;
        //THE ID PASSED IS LOAN ID

        public CloseCircleModel(UserManager<ApplicationUser> userManager, TheLendingCircle.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(5000, MinimumLength = 1, ErrorMessage = "Review Body must be between 1 and 5000 characters")]
            [Display(Name = "Review Body")]
            public string ReviewBody { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [Display(Name = "Creation Date")]
            public DateTime CreationTime { get; set; }

            [Required]
            [Display(Name = "Rating")]
            public double Rating { get; set; }

            [StringLength(100, MinimumLength = 1, ErrorMessage = "Item Condition must be between 1 and 100 characters")]
            [Display(Name = "Return Condition")]
            public string? ItemCondition { get; set; }
        }

        public ApplicationUser CurrentUser { get; set; }
        public Loan CurrentLoan { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            CurrentUser = user;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var Loan = await _context.Loans.Include(l => l.ItemLoaned).FirstOrDefaultAsync(m => m.Id == id);
            if (Loan == null)
            {
                return NotFound("Unable to load Request with ID");
            }
            CurrentLoan = Loan;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (ModelState.IsValid)
            {
                var newReview = new TheLendingCircle.Models.Review { ReviewBody = Input.ReviewBody, CreationTime = DateTime.Today, Rating = Input.Rating, Owner = CurrentLoan.Owner, Borrower = CurrentLoan.Borrower };
                _context.Reviews.Add(newReview);

                var closeLoan = await _context.Loans.FindAsync(id);
                if (closeLoan == null)
                {
                    return NotFound();
                }
                closeLoan.Status = "closed";

                var returnItem = await _context.Items.FindAsync(closeLoan.ItemLoaned.Id);
                if (returnItem == null)
                {
                    return NotFound();
                }
                returnItem.Available = true;
                if(returnItem.Condition != Input.ItemCondition) {
                    returnItem.Condition = Input.ItemCondition;
                }

                await _context.SaveChangesAsync();
                return RedirectToPage("./ClosedCircles");
            }
            return Page();
        }
    }
}

