using BirthdayApp.Core.Constants;
using BirthdayApp.Core.Entities;
using BirthdayApp.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirthdayApp.Web.Pages.Users
{
    [Authorize(Policy = IdentityConstants.PolicyUserOnly)]
    public class ProfileModel : PageModel
    {
        private readonly IUserService userService;

        public User Profile { get; set; }
        public City City { get; set; }

        [BindProperty]
        public string Username { get; set; }

        public ProfileModel(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task OnGetAsync(string username)
        {
            if (username == null)
            {
                Profile = await userService.GetUserByUsernameAsync(User.Identity.Name);
                City = await userService.GetUserCityAsync(User.Identity.Name);
            }
            else
            {
                Profile = await userService.GetUserByUsernameAsync(username);
                City = await userService.GetUserCityAsync(username);
            }
        }

        public async Task<IActionResult> OnPostAsync(string follow)
        {
            if (!String.IsNullOrEmpty(Username))
            {
                if (follow == "true")
                {
                    await userService.UnfollowUserAsync(User.Identity.Name, Username);
                }
                else
                {
                    await userService.FollowUserAsync(User.Identity.Name, Username);
                }
            }
            return RedirectToPage("/Users/Following");
        }

        public bool CheckFollows()
        {
            bool follows = userService.CheckIfFollowsAsync(User.Identity.Name, Profile.Username).Result;
            return follows;
        }
    }
}