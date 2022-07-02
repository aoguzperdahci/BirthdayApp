using BirthdayApp.Core.Repositories;
using BirthdayApp.Services.ApiEntities;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayApp.Services.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<List<UserEntity>> Get(string search)
        {
            var users = await userRepository.GetAllAsync();
            var result = users.Where(x => x.Username.Contains(search, StringComparison.InvariantCultureIgnoreCase)
                                    || x.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase)
                                    || x.Surname.Contains(search, StringComparison.InvariantCultureIgnoreCase)
                                ).Select(x => new UserEntity { Username = x.Username, Name = x.Name, Surname = x.Surname, ProfilePicture = x.ProfilePicture }).Take(5);

            return result.ToList();
        }
    }
}