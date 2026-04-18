using System.ComponentModel.DataAnnotations;

namespace MentiiWebsite.Models
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8), DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
