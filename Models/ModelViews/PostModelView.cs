using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MentiiWebsite.Models.ModelViews
{
    public class PostModelView
    {
        public Guid UserUuid { get; set; }

        [Required]
        [Display(Name = "Title")]
        [MaxLength(60)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Content")]
        [MaxLength(255)]
        public string Content { get; set; } = string.Empty;
    }
}
