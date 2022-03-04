#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Models;
using System.ComponentModel.DataAnnotations;

namespace TheLendingCircle.Pages.User
{
    public class AddItemModel : PageModel
    {
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddItemModel(UserManager<ApplicationUser> userManager, TheLendingCircle.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

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

        [Required, StringLength(500, MinimumLength = 1, ErrorMessage ="Condition must be between 1 and 500 characters")]
        [Display(Name = "ItemPhotoPath")]
        public string ItemPhotoPath { get; set; }

        [Required]
        [Display(Name = "Available")]
        public bool Available { get; set; }
        }

        public IActionResult OnGet()
        {
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
                ItemPhotoPath = Input.ItemPhotoPath, 
                Available = Input.Available, 
                Owner = user
            };
            _context.Items.Add(newItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./MyCircle");
        }
    }
}
