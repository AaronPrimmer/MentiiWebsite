using System.ComponentModel.DataAnnotations;

namespace MentiiWebsite.Models
{
    public class SignupViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Birthdate")]
        public DateTime Birthday { get; set; }

        public bool IsEnabled { get; set; } = true;

        [Display(Name = "Skills")]
        public string SkillsRaw { get; set; } = string.Empty;
    }
}
