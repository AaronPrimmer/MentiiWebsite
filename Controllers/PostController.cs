using MentiiWebsite.Data;
using MentiiWebsite.Models;
using MentiiWebsite.Models.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MentiiWebsite.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public PostController(AppDbContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Create()
        {
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> Create(PostModelView post)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid && user != null)
            {
                var userPost = await _db.MentiiPostTbl.AddAsync(new Post
                {
                    PostUuid = Guid.NewGuid(),
                    UserUuid = Guid.Parse(user.Id),
                    PostTitle = post.Title,
                    PostBody = post.Content,
                    PostDate = DateTime.UtcNow
                });

                Console.WriteLine($"User {user.UserName}:{user.Id} is creating a post with title: {post.Title}");

                await _db.SaveChangesAsync();
                return Json(new { success = true, postId = userPost.Entity.PostUuid });
            }

            return Json(new { success = false, postId = 0 });
        }
    }
}
