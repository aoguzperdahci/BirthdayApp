using BirthdayApp.Core.Constants;
using BirthdayApp.Core.Entities;
using BirthdayApp.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirthdayApp.Web.Pages.Admin
{
    [Authorize(Policy = IdentityConstants.PolicyAdminOnly)]
    public class UserChartsModel : PageModel
    {
        private readonly IUserService userService;
        public IEnumerable<User> Users { get; set; }
        public int[] AgeChartData { get; set; }
        public int[] GenderChartData { get; set; }

        public UserChartsModel(IUserService userService)
        {
            this.userService = userService;
            AgeChartData = new int[4];
            GenderChartData = new int[2];
        }

        public async Task OnGetAsync()
        {
            Users = await userService.GetAllUsersAsync();

            foreach (User user in Users)
            {
                int age = user.CalculateAge();
                if (age < 15)
                {
                    AgeChartData[0]++;
                }
                else if (age < 30)
                {
                    AgeChartData[1]++;
                }
                else if (age < 45)
                {
                    AgeChartData[2]++;
                }
                else
                {
                    AgeChartData[3]++;
                }

                if (user.Gender == Gender.Male)
                {
                    GenderChartData[0]++;
                }
                else
                {
                    GenderChartData[1]++;
                }
            }
        }
    }
}