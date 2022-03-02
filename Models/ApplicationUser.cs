using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using TheLendingCircle.Models;
using System;

namespace TheLendingCircle.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(100, MinimumLength = 1, ErrorMessage ="First Name must be between 1 and 100 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, StringLength(100, MinimumLength = 1, ErrorMessage ="Last Name must be between 1 and 100 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, StringLength(100, MinimumLength = 1, ErrorMessage ="Address must be between 1 and 100 characters")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required, StringLength(10, MinimumLength = 1, ErrorMessage ="Postal Code must be between 1 and 100 characters")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required, StringLength(100, MinimumLength = 1, ErrorMessage ="City must be between 1 and 100 characters")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required, StringLength(100, MinimumLength = 1, ErrorMessage ="Province must be between 1 and 100 characters")]
        [Display(Name = "Province")]
        public string Province { get; set; }

        [Display(Name = "Average Rating")]
        public double AvgRating { get; set; }

        [Required, StringLength(500, MinimumLength = 1, ErrorMessage ="Condition must be between 1 and 500 characters")]
        [Display(Name = "ItemPhotoPath")]
        public string UserPhotoPath { get; set; }
    }
}