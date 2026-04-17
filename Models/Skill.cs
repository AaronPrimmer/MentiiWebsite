using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentiiWebsite.Models
{
    [Table("mentii_skills_tbl")]
    public class Skill
    {
        [Key]
        [Column("skill_id")]
        public Guid SkillId { get; set; }

        [Column("user_uuid")]
        public Guid UserUuid { get; set; }

        [Column("skill_name")]
        public string SkillName { get; set; } = string.Empty;
    }
}
