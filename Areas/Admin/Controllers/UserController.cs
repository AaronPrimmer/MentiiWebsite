using MentiiWebsite.Areas.Admin.Models.ViewModels;
using MentiiWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MentiiWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : Controller
    {
        public UserManager<ApplicationUser> userManager = userManager;
        public RoleManager<IdentityRole> roleManager = roleManager;

        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = [];

            var userList = userManager.Users.ToList();

            foreach (ApplicationUser user in userList)
            {
                user.RoleNames = await userManager.GetRolesAsync(user);
                users.Add(user);
            }

            UserViewModel userViewModel = new()
            {
                Users = users,
                Roles = roleManager.Roles
            };
            return View(userViewModel);
        }
    }
}
