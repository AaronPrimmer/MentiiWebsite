using MentiiWebsite.Models;
using Microsoft.AspNetCore.Identity;

namespace MentiiWebsite.Areas.Admin.Models.ViewModels
{
    public class UserViewModel
    {
        public IEnumerable<ApplicationUser>? Users { get; set; } = null;
        public IEnumerable<IdentityRole>? Roles { get; set; } = null;
    }
}
