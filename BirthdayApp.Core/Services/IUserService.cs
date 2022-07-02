using BirthdayApp.Core.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BirthdayApp.Core.Services
{
    public interface IUserService
    {
        ValueTask<bool> CheckUserExistAsync(LoginCredentials credentials);

        Task CreateUserAsync(User user, LoginCredentials credentials, City city);

        ValueTask<ClaimsPrincipal?> LoginAsync(LoginCredentials credentials);

        Task FollowUserAsync(string username, string followUsername);

        ValueTask<IEnumerable<User>> GetFollowedByUserAsync(string username);

        ValueTask<IEnumerable<User>> GetAllUsersAsync();

        ValueTask<User> GetUserByUsernameAsync(string username);

        ValueTask<City> GetUserCityAsync(string username);

        Task EditUserAsync(User user, City city, string? newPassword, IFormFile profilePicture);

        ValueTask<IEnumerable<UserCity>> GetAllUsersWithCityAsync();

        ValueTask<bool> CheckIfFollowsAsync(string username, string followUsername);

        Task UnfollowUserAsync(string username, string followUsername);
    }
}