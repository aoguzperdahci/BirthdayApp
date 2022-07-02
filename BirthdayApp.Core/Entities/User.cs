using BirthdayApp.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace BirthdayApp.Core.Entities
{
    public class User
    {
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters long.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Name must consist of only alphabetic characters.")]
        [Required(ErrorMessage = "Name field cannot be empty.")]
        public string Name { get; set; }

        [StringLength(30, MinimumLength = 3, ErrorMessage = "Surname must be between 3 and 30 characters long.")]
        [RegularExpression(@"[^0123456789/*-+.,:;!?%&½$#£><|(){}-]+$", ErrorMessage = "Surname must consist of only alphabetic characters.")]
        [Required(ErrorMessage = "Surname field cannot be empty.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Gender field cannot be empty.")]
        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Birthdate field cannot be empty.")]
        public DateTime Birthdate { get; set; }

        public string? ProfilePicture { get; set; }

        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? Salt { get; set; }

        public int CalculateAge()
        {
            DateTime today = DateTime.Today;
            int age = today.Year - Birthdate.Year;
            if (today < Birthdate.AddYears(age))
                age--;
            return age;
        }

        public int CalculateNextBirthday()
        {
            DateTime today = DateTime.Today;
            if (Birthdate.DayOfYear == today.DayOfYear)
            {
                return 0;
            }
            DateTime nextBirthday = Birthdate.AddYears(CalculateAge() + 1);

            TimeSpan difference = nextBirthday - DateTime.Today;

            return Convert.ToInt32(difference.TotalDays);
        }
    }
}