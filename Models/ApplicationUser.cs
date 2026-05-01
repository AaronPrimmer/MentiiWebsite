using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentiiWebsite.Models
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public IList<string>? RoleNames { get; set; } = null;
    }
}