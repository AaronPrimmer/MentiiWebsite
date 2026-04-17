using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentiiWebsite.Models
{
    [Table("mentii_posts_tbl")]
    public class Post
    {
        [Key]
        [Column("post_uuid")]
        public Guid PostUuid { get; set; }

        [Required]
        [Column("user_uuid")]
        [ForeignKey("UserUuid")]
        public Guid UserUuid { get; set; }

        [Required]
        [Column("post_title")]
        [StringLength(100, ErrorMessage = "The title cannot exceed 100 characters.")]
        public string PostTitle { get; set; } = string.Empty;

        [Required]
        [Column("post_body")]
        [StringLength(1000, ErrorMessage = "The body cannot exceed 1000 characters.")]
        public string PostBody { get; set; } = string.Empty;

        [Column("post_date")]
        public DateTime PostDate { get; set; }
    }
}
