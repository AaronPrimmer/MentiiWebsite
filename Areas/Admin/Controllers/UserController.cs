using MentiiWebsite.Areas.Admin.Models.ViewModels;
using MentiiWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MentiiWebsite.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string userId, string roleName)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await userManager.RemoveFromRoleAsync(user, roleName);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdminRole()
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            Console.WriteLine("Deleting role: " + roleId);
            IdentityRole role = await roleManager.FindByIdAsync(roleId);
            if (role != null) 
            {
                await roleManager.DeleteAsync(role);
                Console.WriteLine("Deleted role: " + role.Name);
            }
            return RedirectToAction("Index");
        }
    }
}
