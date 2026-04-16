using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentiiWebsite.Models
{
    [Table("mentii_users_tbl")]
    public class User
    {
        [Key]
        [Column("user_uuid")]
        public Guid UserUuid { get; set; }

        [Required]
        [Column("user_firstname")]
        public string? UserFirstname { get; set; }

        [Required]
        [Column("user_lastname")]
        public string UserLastname { get; set; } = string.Empty;

        [Required]
        [Column("user_username")]
        public string UserUsername { get; set; } = string.Empty;

        [Required]
        [Column("user_email")]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        [Column("user_password")]
        public string UserPassword { get; set; } = string.Empty;

        [Column("user_posts")]
        public string UserPosts { get; set; } = string.Empty;

        [Column("user_following")]
        public string UserFollowing { get; set; } = string.Empty;

        [Column("user_title")]
        public string UserTitle { get; set; } = string.Empty;

        [Column("user_skills")]
        public string UserSkills { get; set; } = string.Empty;



    }
}
