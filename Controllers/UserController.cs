using MentiiWebsite.Data;
using MentiiWebsite.Models;
using MentiiWebsite.Models.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MentiiWebsite.Controllers
{
    public class UserController(AppDbContext db) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile(string username)
        {
            if (username == null) 
            {
                return RedirectToAction("Index", "Home");
            }

            var userInfo = db.MentiiUsersTbl
                .Include(s => s.Skills)
                .FirstOrDefault(u => u.UserUsername == username);
            if(userInfo == null)
            {
                return NotFound();
            }

            var viewModel = new UserProfileViewModel
            {
                UserId = userInfo.UserUuid.ToString(),
                UserName = userInfo.UserUsername,
                UserFirstName = userInfo.UserFirstname,
                UserLastName = userInfo.UserLastname,
                UserEmail = userInfo.UserEmail,
                UserTitle = userInfo.UserTitle,
                UserSkills = [.. userInfo.Skills.Select(s => s.SkillName)],
                UserBirthday = userInfo.UserBirthday
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserModel userModel)
        {
            return View(userModel);
        }
    }
}
