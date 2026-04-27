using MentiiWebsite.Data;
using MentiiWebsite.Models;
using MentiiWebsite.Models.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MentiiWebsite.Controllers
{
    public class PostController(AppDbContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : Controller
    {
        private readonly AppDbContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<IdentityUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<IdentityUser> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));

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

                await _db.SaveChangesAsync();
                return Json(new { success = true, postId = userPost.Entity.PostUuid });
            }

            return Json(new { success = false, postId = 0 });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> Like(Guid postId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var existingLike = await _db.MentiiPostLikeTbl.FirstOrDefaultAsync(p => p.PostId == postId && p.UserUuid == user.Id);
                if (existingLike == null)
                {
                    await _db.MentiiPostLikeTbl.AddAsync(new PostLike
                    {
                        LikeId = Guid.NewGuid(),
                        PostId = postId,
                        UserUuid = user.Id
                    });
                    await _db.SaveChangesAsync();
                    int likeCount = await _db.MentiiPostLikeTbl.CountAsync(p => p.PostId == postId);
                    return Json(new { success = true, liked = true, likeCount });
                }
                else
                {
                    _db.MentiiPostLikeTbl.Remove(existingLike);
                    await _db.SaveChangesAsync();
                    int likeCount = await _db.MentiiPostLikeTbl.CountAsync(p => p.PostId == postId);
                    return Json(new { success = true, liked = false, likeCount });
                }
            }
            return Json(new { success = false, liked = false });
        }
    }
}
