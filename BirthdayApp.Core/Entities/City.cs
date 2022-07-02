using System.ComponentModel.DataAnnotations;

namespace BirthdayApp.Core.Entities
{
    public class City
    {
        [Required(ErrorMessage = "City field cannot be empty.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Country field cannot be empty.")]
        public string Country { get; set; }
    }
}