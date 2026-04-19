using MentiiWebsite.Data;
using MentiiWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace MentiiWebsite.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _db;

        public UserController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile(Guid userUuid)
        {
            var userInfo = _db.MentiiUsersTbl.FirstOrDefault(u => u.UserUuid == userUuid);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserModel userModel)
        {
            return View(userModel);
        }
    }
}
