using BirthdayApp.Core.Constants;
using BirthdayApp.Core.Entities;
using BirthdayApp.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BirthdayApp.Web.Pages.Admin
{
    [Authorize(Policy = IdentityConstants.PolicyAdminOnly)]
    public class UserTableModel : PageModel
    {
        private readonly IUserService userService;

        [BindProperty]
        public IEnumerable<UserCity> UsersWithCity { get; set; }

        [BindProperty]
        [Display(Name = "Sort by")]
        public string OrderBy { get; set; }

        [BindProperty]
        public string Search { get; set; }

        public UserTableModel(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task OnGetAsync()
        {
            UsersWithCity = await userService.GetAllUsersWithCityAsync();
        }

        public async Task OnPostAsync()
        {
            UsersWithCity = await userService.GetAllUsersWithCityAsync();

            if (!String.IsNullOrEmpty(Search))
            {
                UsersWithCity = UsersWithCity.Where(x => x.User.Username.Contains(Search, StringComparison.InvariantCultureIgnoreCase)
                                                        || x.User.Name.Contains(Search, StringComparison.InvariantCultureIgnoreCase)
                                                        || x.User.Surname.Contains(Search, StringComparison.InvariantCultureIgnoreCase)
                                                        || x.City.Name.Contains(Search, StringComparison.InvariantCultureIgnoreCase)
                                                    );
            }

            switch (OrderBy)
            {
                case "username_desc":
                    UsersWithCity = UsersWithCity.OrderByDescending(x => x.User.Username);
                    break;

                case "username":
                    UsersWithCity = UsersWithCity.OrderBy(x => x.User.Username);
                    break;

                case "name_desc":
                    UsersWithCity = UsersWithCity.OrderByDescending(x => x.User.Name);
                    break;

                case "name":
                    UsersWithCity = UsersWithCity.OrderBy(x => x.User.Name);
                    break;

                case "surname_desc":
                    UsersWithCity = UsersWithCity.OrderByDescending(x => x.User.Surname);
                    break;

                case "surname":
                    UsersWithCity = UsersWithCity.OrderBy(x => x.User.Surname);
                    break;

                case "age_desc":
                    UsersWithCity = UsersWithCity.OrderByDescending(x => x.User.CalculateAge());
                    break;

                case "age":
                    UsersWithCity = UsersWithCity.OrderBy(x => x.User.CalculateAge());
                    break;

                case "next_birthday_desc":
                    UsersWithCity = UsersWithCity.OrderByDescending(x => x.User.CalculateNextBirthday());
                    break;

                case "next_birthday":
                    UsersWithCity = UsersWithCity.OrderBy(x => x.User.CalculateNextBirthday());
                    break;

                case "gender_desc":
                    UsersWithCity = UsersWithCity.OrderByDescending(x => x.User.Gender);
                    break;

                case "gender":
                    UsersWithCity = UsersWithCity.OrderBy(x => x.User.Gender);
                    break;

                case "city_desc":
                    UsersWithCity = UsersWithCity.OrderByDescending(x => x.City.Name);
                    break;

                case "city":
                    UsersWithCity = UsersWithCity.OrderBy(x => x.City.Name);
                    break;
            }
        }
    }
}