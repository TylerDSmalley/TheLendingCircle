using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using TheLendingCircle.Models;
using System;

namespace TheLendingCircle.Models
{
    public class Request
    {
        public int Id { get; set; }

        [StringLength(1000, MinimumLength = 1, ErrorMessage ="Message must be between 1 and 1000 characters")]
        [Display(Name = "Message")]
        public string Message { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation Date")]
        public DateTime CreationTime { get; set; }

        [Required]
        [Display(Name = "Viewed")]
        public bool HasBeenViewed { get; set; }

        //Add FK connections
        
        public ApplicationUser Owner { get; set; }

        [Required]
        public ApplicationUser Borrower { get; set; }

        public Item? ItemLoaned { get; set; }
    }
}