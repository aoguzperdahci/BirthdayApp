using BirthdayApp.Core.Services;
using Microsoft.AspNetCore.Http;

namespace BirthdayApp.Services.FileServices
{
    public class LocalFileUploadService : IFileUploadService
    {
        public async Task UploadFile(IFormFile file, string fileName)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot\\images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}