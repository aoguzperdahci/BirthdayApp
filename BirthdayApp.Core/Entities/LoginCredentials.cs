using System.ComponentModel.DataAnnotations;

namespace BirthdayApp.Core.Entities
{
    public class LoginCredentials
    {
        [Required]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Username must be between 3 and 30 characters long.")]
        public string Username { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be between 3 and 30 characters long.")]
        public string Password { get; set; }
    }
}