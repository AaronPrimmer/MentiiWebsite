using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentiiWebsite.Models
{
    [Table("mentii_login_tbl")]
    public class Login
    {
        [Key]
        [Required]
        [Column("user_uuid")]
        public Guid UserUuid { get; set; }

        [Required]
        [Column("login_password")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; } = string.Empty;

        [Required]
        [Column("login_active")]
        public bool LoginActive { get; set; }
    }
}
