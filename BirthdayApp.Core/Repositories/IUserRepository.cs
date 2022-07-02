using BirthdayApp.Core.Entities;

namespace BirthdayApp.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByUsernameAsync(string username);

        Task<IEnumerable<User>> GetFollowedByUserAsync(string username);

        Task FollowUserAsync(string username, string followUsername);

        Task SetUserCity(User user, City city);

        ValueTask<IEnumerable<string>> GetUserRoles(User user);

        ValueTask<IEnumerable<UserCity>> GetAllUsersWithCityAsync();

        ValueTask<bool> CheckIfFollowsAsync(string username, string followUsername);

        Task UnfollowUserAsync(string username, string followUsername);
    }
}