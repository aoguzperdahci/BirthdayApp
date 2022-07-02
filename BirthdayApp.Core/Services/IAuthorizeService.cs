using BirthdayApp.Core.Entities;
using System.Security.Claims;

namespace BirthdayApp.Core.Services
{
    public interface IAuthorizeService
    {
        Task<ClaimsPrincipal> AuthorizeAsync(User user);
    }
}