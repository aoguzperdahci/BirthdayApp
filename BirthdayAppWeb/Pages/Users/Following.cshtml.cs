using BirthdayApp.Core.Constants;
using BirthdayApp.Core.Entities;
using BirthdayApp.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirthdayApp.Web.Pages.Users
{
    [Authorize(Policy = IdentityConstants.PolicyUserOnly)]
    public class FollowingModel : PageModel
    {
        private readonly IUserService userService;
        public IEnumerable<User> Follows { get; set; }

        public FollowingModel(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task OnGetAsync()
        {
            var users = await userService.GetFollowedByUserAsync(User.Identity.Name);
            Follows = users.OrderBy(o => o.CalculateNextBirthday());
        }
    }
}