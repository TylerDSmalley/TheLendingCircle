using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using TheLendingCircle.Models;
using System;


namespace TheLendingCircle.Models
{
    public class Review
    {
        public int Id { get; set; }

        [StringLength(5000, MinimumLength = 1, ErrorMessage ="Review Body must be between 1 and 5000 characters")]
        [Display(Name = "Review Body")]
        public string ReviewBody { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation Date")]
        public DateTime CreationTime { get; set; }

        [Required]
        [Display(Name = "Rating")]
        public double Rating { get; set; }

        //Add FK connections
        
        public ApplicationUser Owner { get; set; }

        [Required]
        public ApplicationUser Borrower { get; set; }
    }
}