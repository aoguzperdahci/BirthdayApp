using BirthdayApp.Core.Constants;
using BirthdayApp.Core.Entities;
using BirthdayApp.Core.Repositories;
using BirthdayApp.Core.Services;
using System.Security.Claims;

namespace BirthdayApp.Services.IdentityServices
{
    public class DefaultAuthorizeService : IAuthorizeService
    {
        private readonly IUserRepository _userRepository;

        public DefaultAuthorizeService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ClaimsPrincipal> AuthorizeAsync(User user)
        {
            var roles = await _userRepository.GetUserRoles(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("NameSurname", user.Name + " " + user.Surname),
                new Claim(IdentityConstants.ProfilePicture, user.ProfilePicture)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(role, "true"));
            }

            var identity = new ClaimsIdentity(claims, CookieConstants.CookieName);

            ClaimsPrincipal principal = new(identity);

            return principal;
        }
    }
}