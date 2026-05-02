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

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            IdentityResult result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {   //if failed
                string errorMessage = "";
                foreach (IdentityError error in result.Errors)
                {
                    errorMessage += error.Description + " | ";
                }
                TempData["message"] = errorMessage;
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(string userId, string roleName)
        {
            if (userId == null || roleName == null)
            {
                return RedirectToAction("Index");
            }
            IdentityRole role = await roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return RedirectToAction("Index");
            }

            ApplicationUser user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }

            return RedirectToAction("Index");

        }
    }
}
