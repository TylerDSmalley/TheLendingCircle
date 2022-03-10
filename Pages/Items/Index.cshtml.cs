using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheLendingCircle.Models;
using TheLendingCircle.Data;

namespace TheLendingCircle.Pages.Items
{
    public class Index : PageModel
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<Index> _logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public bool Requested { get; set; } = false;

        [TempData]
        public string? StatusMessage { get; set; }
        public Item? CurrentItem { get; set; }
        public ApplicationUser? CurrentOwner { get; set; } = new ApplicationUser();

        [Required]
        [BindProperty, Display(Name = "Request Message")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Message must be between 1 and 1000 characters")]
        public string? RequestMessage { get; set; }

        public Index(ILogger<Index> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Item item = await _context.Items.Where(i => i.Id == Id).Include("Owner").FirstAsync();
            if (item == null)
            {
                return NotFound($"Unable to item user with ID '{Id}'.");
            }
            CurrentItem = item;

            var LoggedInUser = await _userManager.GetUserAsync(User);

            if (LoggedInUser != null)
            {
                // user cant request twice
                var RequestExists = await _context.Requests.Where(req =>
                    req.Borrower.Id == LoggedInUser.Id
                    && req.ItemLoaned.Id == CurrentItem.Id
                ).FirstOrDefaultAsync();

                if (RequestExists != null)
                {
                    Requested = true;
                }
            }

            return Page();
        }

        [Authorize]
        public async Task<IActionResult> OnPostAsync()
        {
            var Borrower = await _userManager.GetUserAsync(User);
            var ThisItem = await _context.Items.Where(i => i.Id == Id).Include("Owner").FirstAsync();

            if (!ModelState.IsValid)
            {
                CurrentItem = ThisItem;
                StatusMessage = "An unexpected error occured. Please try again.";
                return Page();
            }

            // user cant request twice
            var RequestExists = await _context.Requests.Where(req =>
                req.Borrower.Id == Borrower.Id
                && req.ItemLoaned.Id == ThisItem.Id
            ).FirstOrDefaultAsync();

            if (RequestExists != null)
            {
                StatusMessage = "Error: You have already requested this item!";
                CurrentItem = ThisItem;
                return Page();
            }

            _context.Request.Add(new Request()
            {
                Message = RequestMessage,
                CreationTime = DateTime.Now,
                HasBeenViewed = false,
                Owner = ThisItem.Owner,
                Borrower = Borrower,
                ItemLoaned = ThisItem
            });
            await _context.SaveChangesAsync();

            StatusMessage = "Request sent successfully!";
            Requested = true;
            CurrentItem = ThisItem;

            string to = ThisItem.Owner.Email;
            string from = "thelendingcircle@gmail.com";

            MailMessage message = new MailMessage(from, to);

            string mailbody = "Please <a href='http://thelendingcircle.us-east-1.elasticbeanstalk.com/Identity/Account/Login'>login</a> to respond to the following request:<br/><br/>" + RequestMessage;  
            message.Subject = "You have a new Lending Circle request!";  
            message.Body = mailbody;  
            message.IsBodyHtml = true;  
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new  System.Net.NetworkCredential("thelendingcircle@gmail.com", "gawcPO4TPwaoy5vT");  
            client.EnableSsl = true;  
            client.UseDefaultCredentials = false;  
            client.Credentials = basicCredential1;  
            try   
            {  
            client.Send(message);  
            }   
  
            catch (Exception ex)   
            {  
            throw ex;  
            }  

            return Page();
        }
    }
}
