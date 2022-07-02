using BirthdayApp.Core.Constants;
using BirthdayApp.Core.Entities;
using BirthdayApp.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BirthdayApp.Web.Pages.Users
{
    [Authorize(Policy = IdentityConstants.PolicyUserOnly)]
    public class EditModel : PageModel
    {
        private readonly IUserService userService;
        private readonly IAuthorizeService authorizeService;

        [BindProperty]
        public User CurrentUser { get; set; }

        [BindProperty]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be between 3 and 30 characters long.")]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [BindProperty]
        public City CurrentCity { get; set; }

        [BindProperty]
        public IFormFile NewProfilePicture { get; set; }

        public EditModel(IUserService userService, IAuthorizeService authorizeService)
        {
            this.userService = userService;
            this.authorizeService = authorizeService;
        }

        public async Task OnGetAsync()
        {
            CurrentUser = await userService.GetUserByUsernameAsync(User.Identity.Name);
            CurrentCity = await userService.GetUserCityAsync(User.Identity.Name);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (NewProfilePicture != null)
            {
                string fileExtension = Path.GetExtension(NewProfilePicture.FileName);
                if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg")
                {
                    ModelState.AddModelError("", "Profile picture can be png, jpg or jpeg.");
                }
                int limit = 5 * 1024 * 1024;
                if (NewProfilePicture.Length > limit)
                {
                    ModelState.AddModelError("", "Profile picture can be maximum 5 MB.");
                }
            }

            if (ModelState.IsValid)
            {
                await userService.EditUserAsync(CurrentUser, CurrentCity, NewPassword, NewProfilePicture);
                await HttpContext.SignOutAsync(CookieConstants.CookieName);
                var principal = await authorizeService.AuthorizeAsync(CurrentUser);
                await HttpContext.SignInAsync(CookieConstants.CookieName, principal);
            }

            return Page();
        }
    }
}