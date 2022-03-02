using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using TheLendingCircle.Models;
using System;

namespace TheLendingCircle.Models
{
    public class Item
    {
        public int Id { get; set; }
        
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

        //Add FK connection
        [Required]
        public ApplicationUser Owner { get; set; }
    }
}