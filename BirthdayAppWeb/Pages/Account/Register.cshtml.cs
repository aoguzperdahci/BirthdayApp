using BirthdayApp.Core.Entities;
using BirthdayApp.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirthdayApp.Web.Pages.Account
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        private readonly IUserService userService;

        public User NewUser { get; set; }
        public LoginCredentials Credentials { get; set; }
        public City City { get; set; }

        public RegisterModel(IUserService userService)
        {
            this.userService = userService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            int res = DateTime.Compare(NewUser.Birthdate, DateTime.Now);
            if (res > 0)
            {
                ModelState.AddModelError("User.Birthdate", "Birthdate cannot be later than today.");
            }
            if (ModelState.IsValid)
            {
                if (await userService.CheckUserExistAsync(Credentials))
                {
                    ModelState.AddModelError("Credentials.Username", "Username " + Credentials.Username + " is already taken.");
                    return Page();
                }

                NewUser.ProfilePicture = "anonymous.png";
                await userService.CreateUserAsync(NewUser, Credentials, City);
                return RedirectToPage("Login");
            }

            return Page();
        }
    }
}