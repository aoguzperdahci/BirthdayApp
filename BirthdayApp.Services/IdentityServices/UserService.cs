using BirthdayApp.Core.Entities;
using BirthdayApp.Core.Repositories;
using BirthdayApp.Core.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BirthdayApp.Services.IdentityServices
{
    public class UserService : IUserService, IAuthenticateService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _hashService;
        private readonly IAuthorizeService _authorizeService;
        private readonly ICityRepository cityRepository;
        private readonly IFileUploadService fileUploadService;

        public UserService(IUserRepository userRepository, IPasswordHashService hashService, IAuthorizeService authorizeService, ICityRepository cityRepository, IFileUploadService fileUploadService)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            _authorizeService = authorizeService;
            this.cityRepository = cityRepository;
            this.fileUploadService = fileUploadService;
        }

        public bool Authenticate(User user, LoginCredentials loginCredentials)
        {
            string saltString = user.Salt;
            byte[] salt = Convert.FromBase64String(saltString);
            string hash = _hashService.HashPassword(loginCredentials.Password, salt);
            return hash == user.PasswordHash;
        }

        public async ValueTask<bool> CheckIfFollowsAsync(string username, string followUsername)
        {
            return await _userRepository.CheckIfFollowsAsync(username, followUsername);
        }

        public async ValueTask<bool> CheckUserExistAsync(LoginCredentials credentials)
        {
            return await _userRepository.GetUserByUsernameAsync(credentials.Username) != null;
        }

        public async Task CreateUserAsync(User user, LoginCredentials credentials, City city)
        {
            byte[] salt = _hashService.GenerateSalt();
            string hash = _hashService.HashPassword(credentials.Password, salt);
            string saltString = Convert.ToBase64String(salt);

            user.Username = credentials.Username;
            user.Salt = saltString;
            user.PasswordHash = hash;
            await _userRepository.AddAsync(user);
            await _userRepository.SetUserCity(user, city);
        }

        public async Task EditUserAsync(User user, City city, string? newPassword, IFormFile profilePicture)
        {
            User old = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (newPassword != null)
            {
                byte[] salt = _hashService.GenerateSalt();
                string hash = _hashService.HashPassword(newPassword, salt);
                string saltString = Convert.ToBase64String(salt);
                user.Salt = saltString;
                user.PasswordHash = hash;
            }
            else
            {
                user.Salt = old.Salt;
                user.PasswordHash = old.PasswordHash;
            }

            if (profilePicture != null)
            {
                string fileName = user.Username + Path.GetExtension(profilePicture.FileName);
                fileUploadService.UploadFile(profilePicture, fileName);
                user.ProfilePicture = fileName;
            }
            else
            {
                user.ProfilePicture = old.ProfilePicture;
            }

            user.Birthdate = old.Birthdate;
            user.Gender = old.Gender;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SetUserCity(user, city);
        }

        public async Task FollowUserAsync(string username, string followUsername)
        {
            await _userRepository.FollowUserAsync(username, followUsername);
        }

        public async ValueTask<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users;
        }

        public async ValueTask<IEnumerable<UserCity>> GetAllUsersWithCityAsync()
        {
            return await _userRepository.GetAllUsersWithCityAsync();
        }

        public async ValueTask<IEnumerable<User>> GetFollowedByUserAsync(string username)
        {
            var list = await _userRepository.GetFollowedByUserAsync(username);
            return list;
        }

        public async ValueTask<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        public async ValueTask<City> GetUserCityAsync(string username)
        {
            return await cityRepository.GetUserCityAsync(username);
        }

        public async ValueTask<ClaimsPrincipal?> LoginAsync(LoginCredentials credentials)
        {
            var user = await _userRepository.GetUserByUsernameAsync(credentials.Username);
            if (user == null)
            {
                return null;
            }

            if (!Authenticate(user, credentials))
            {
                return null;
            }

            return await _authorizeService.AuthorizeAsync(user);
        }

        public async Task UnfollowUserAsync(string username, string followUsername)
        {
            await _userRepository.UnfollowUserAsync(username, followUsername);
        }
    }
}