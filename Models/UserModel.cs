using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentiiWebsite.Models
{
    [Table("mentii_users_tbl")]
    public class UserModel
    {
        [Key]
        [Column("user_uuid")]
        public Guid UserUuid { get; set; }

        [Required]
        [Column("user_firstname")]
        [StringLength(50, ErrorMessage = "The first name cannot exceed 50 characters.")]
        public string UserFirstname { get; set; } = string.Empty;

        [Required]
        [Column("user_lastname")]
        [StringLength(50, ErrorMessage = "The last name cannot exceed 50 characters.")]
        public string UserLastname { get; set; } = string.Empty;

        [Required]
        [Column("user_username")]
        [StringLength(20, ErrorMessage = "The username cannot exceed 20 characters.")]
        [Display(Name = "Username")]
        public string UserUsername { get; set; } = string.Empty;

        [Required]
        [Column("user_email")]
        [StringLength(100, ErrorMessage = "The email cannot exceed 100 characters.")]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        [Column("user_password")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "The password must be between 8 and 30 characters.")]
        [Display(Name = "Password")]
        public string UserPassword { get; set; } = string.Empty;

        [Column("user_title")]
        [StringLength(100, ErrorMessage = "The title cannot exceed 100 characters.")]
        public string UserTitle { get; set; } = string.Empty;

        [Required]
        [Column("user_bday")]
        [DataType(DataType.Date)]
        public DateTime UserBirthday { get; set; }

        [Column("user_enabled")]
        public bool UserEnabled { get; set; } = true;
    }
}
