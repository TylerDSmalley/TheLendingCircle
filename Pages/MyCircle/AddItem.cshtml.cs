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

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
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
                ItemPhotoPath = "http://bootdey.com/img/Content/avatar/avatar1.png", 
                Available = true, 
                Owner = user
            };
            _context.Items.Add(newItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./MyItems");
        }
    }
}
