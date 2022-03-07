using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TheLendingCircle.Models;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;
using System.Threading.Tasks;

namespace TheLendingCircle.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class UserPhotoUploadModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TheLendingCircle.Data.ApplicationDbContext _context;

        private const string bucketName = "lendingcircle";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static IAmazonS3 s3Client;

        public UserPhotoUploadModel(
            UserManager<ApplicationUser> userManager,
            TheLendingCircle.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            //Maybe filesize limit annotation
            public IFormFile userPhoto { get; set; }
        }

        public ApplicationUser CurrentUser { get; set; }


        public async Task<IActionResult> OnGetAsync(string? id)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            
            if (CurrentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (ModelState.IsValid)
            {


                s3Client = new AmazonS3Client(bucketRegion);
                string imagePath = "";

                if (Input.userPhoto != null)
                {
                    string fileExtension = Path.GetExtension(Input.userPhoto.FileName).ToLower();
                    string filePath = Path.GetFullPath(Input.userPhoto.FileName);
                    
                    Console.WriteLine(filePath);
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError(string.Empty, "Only image files (jpeg, jpg, png) are allowed");
                        return Page();
                    }

                    var invalids = System.IO.Path.GetInvalidFileNameChars();
                    var newName = String.Join("_", Input.userPhoto.FileName.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
            
                    UploadFileToS3(Input.userPhoto);

                    imagePath = Path.Combine("https://lendingcircle.s3.amazonaws.com/", newName);
                }

                ApplicationUser? user1 = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user1 == null)
                {
                    return NotFound();
                }
                user1.UserPhotoPath = imagePath;

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            return Page();
        }


        public async Task UploadFileToS3(IFormFile file)
        {
        using (var client = new AmazonS3Client("AKIAXJR27NJ66LXTY6EN", "fmnPlcqx20CYFbPGQXt2IfWtFektKnHFvj5brUB6", RegionEndpoint.USEast1))
        {
        using (var newMemoryStream = new MemoryStream())
        {
            file.CopyTo(newMemoryStream);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = file.FileName,
                BucketName = bucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            var fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }
    }
}
    
}
}
