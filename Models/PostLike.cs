using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentiiWebsite.Models
{
    [Table("mentii_post_likes_tbl")]
    public class PostLike
    {
        [Key]
        [Required]
        [Column("like_id")]
        public Guid LikeId { get; set; }

        [Required]
        [Column("post_id")]
        [ForeignKey("PostId")]
        public Guid PostId { get; set; }

        public Post? Post { get; set; }

        [Required]
        [Column("user_uuid")]
        public string UserUuid { get; set; } = string.Empty;

        public IdentityUser? User { get; set; }

        [Column("like_date")]
        public DateTime LikeDate { get; set; } = DateTime.UtcNow;
    }
}
