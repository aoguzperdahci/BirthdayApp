using BirthdayApp.Core.Entities;

namespace BirthdayApp.Core.Services
{
    public interface IAuthenticateService
    {
        bool Authenticate(User user, LoginCredentials loginCredentials);
    }
}