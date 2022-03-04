// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Models;
namespace TheLendingCircle.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TheLendingCircle.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required, StringLength(100, MinimumLength = 1, ErrorMessage = "First Name must be between 1 and 100 characters")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required, StringLength(100, MinimumLength = 1, ErrorMessage = "Last Name must be between 1 and 100 characters")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required, StringLength(100, MinimumLength = 1, ErrorMessage = "Address must be between 1 and 100 characters")]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Required, StringLength(10, MinimumLength = 1, ErrorMessage = "Postal Code must be between 1 and 100 characters")]
            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

            [Required, StringLength(100, MinimumLength = 1, ErrorMessage = "City must be between 1 and 100 characters")]
            [Display(Name = "City")]
            public string City { get; set; }

            [Required, StringLength(100, MinimumLength = 1, ErrorMessage = "Province must be between 1 and 100 characters")]
            [Display(Name = "Province")]
            public string Province { get; set; }

            [Required, StringLength(500, MinimumLength = 1, ErrorMessage = "ItemPhotoPath must be between 1 and 500 characters")]
            [Display(Name = "ItemPhotoPath")]
            public string UserPhotoPath { get; set; }
        }

        public ApplicationUser CurrentUser { get; set; }
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            CurrentUser = user;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            _context.Attach(user).State = EntityState.Modified;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }

            if (Input.Email != user.Email)
            {
                user.Email = Input.Email;
                user.UserName = Input.Email;
            }

            if (Input.Address != user.Address)
            {
                user.Address = Input.Address;
            }

            if (Input.City != user.City)
            {
                user.City = Input.City;
            }

            if (Input.Province != user.Province)
            {
                user.Province = Input.Province;
            }

            if (Input.PostalCode != user.PostalCode)
            {
                user.PostalCode = Input.PostalCode;
            }

            await _context.SaveChangesAsync();
            
            StatusMessage = "Your profile has been updated";
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
    }
}
