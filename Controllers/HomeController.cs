using MentiiWebsite.Data;
using MentiiWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MentiiWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(AppDbContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var posts = await _db.MentiiPostTbl
                .Include(p => p.Author)
                .OrderByDescending(p => p.PostDate)
                .Take(20)
                .ToListAsync();

            Console.WriteLine($"Retrieved {posts.Count} posts.");

            return View(posts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
