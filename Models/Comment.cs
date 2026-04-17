using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentiiWebsite.Models
{
    [Table("mentii_comments_tbl")]
    public class Comment
    {
        [Key]
        [Column("comment_id")]
        public Guid CommentId { get; set; }

        [Column("post_id")]
        public Guid PostId { get; set; }

        [Column("user_uuid")]
        public Guid UserUuid { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The comment cannot exceed 300 characters.")]
        [Column("comment_body")]
        public string CommentBody { get; set; } = string.Empty;
    }
}
