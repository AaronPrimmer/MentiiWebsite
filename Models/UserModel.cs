using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MentiiWebsite.Models
{
    [Table("mentii_users_tbl")]
    public class UserModel
    {
        [Key]
        [Column("user_uuid")]
        public Guid UserUuid { get; set; }

        [Column("user_firstname")]
        [StringLength(50, ErrorMessage = "The first name cannot exceed 50 characters.")]
        public string UserFirstname { get; set; } = string.Empty;

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

        [Column("user_title")]
        [StringLength(100, ErrorMessage = "The title cannot exceed 100 characters.")]
        public string UserTitle { get; set; } = string.Empty;

        [Required]
        [Column("user_bday")]
        [DataType(DataType.Date)]
        public DateTime UserBirthday { get; set; }

        [Column("user_enabled")]
        public bool UserEnabled { get; set; } = true;

        [Column("user_date_created")]
        [DataType(DataType.Date)]
        public DateTime UserDateCreated { get; set; } = DateTime.UtcNow;

        public ICollection<Skill> Skills { get; set; } = [];
    }
}
