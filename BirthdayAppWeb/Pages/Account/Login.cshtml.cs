using BirthdayApp.Core.Constants;
using BirthdayApp.Core.Entities;
using BirthdayApp.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BirthdayApp.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginCredentials Credentials { get; set; }

        [BindProperty]
        public bool RememberMe { get; set; }

        public readonly IUserService userService;

        public LoginModel(IUserService userService)
        {
            this.userService = userService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            ClaimsPrincipal principal = await userService.LoginAsync(Credentials);

            if (principal == null)
            {
                ModelState.AddModelError("Credentials.Password", "Incorrect username or password");
                return Page();
            }

            var authPropoties = new AuthenticationProperties
            {
                IsPersistent = RememberMe
            };

            await HttpContext.SignInAsync(CookieConstants.CookieName, principal, authPropoties);

            return RedirectToPage("/Index");
        }
    }
}