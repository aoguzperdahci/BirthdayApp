using Microsoft.AspNetCore.Http;

namespace BirthdayApp.Core.Services
{
    public interface IFileUploadService
    {
        Task UploadFile(IFormFile file, string fileName);
    }
}