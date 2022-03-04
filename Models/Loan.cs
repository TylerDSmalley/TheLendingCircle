using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using TheLendingCircle.Models;
using System;

namespace TheLendingCircle.Models
{
    public class Loan
    {
        public int Id { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation Date")]
        public DateTime CreationTime { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Required]
        [Display(Name = "Viewed")]
        public bool HasBeenViewed { get; set; }
        
        [Required, StringLength(30, MinimumLength = 1, ErrorMessage ="Circle Status must be between 1 and 30 characters")]
        [Display(Name = "Circle Status")]
        public string Status { get; set; }

        //Add FK connections
        
        public ApplicationUser Owner { get; set; }

        [Required]
        public ApplicationUser Borrower { get; set; }

        public Item? ItemLoaned { get; set; }
    }
}