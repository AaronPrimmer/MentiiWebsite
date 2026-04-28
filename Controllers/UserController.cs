using MentiiWebsite.Data;
using MentiiWebsite.Models;
using MentiiWebsite.Models.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace MentiiWebsite.Controllers
{
    public class UserController(AppDbContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : Controller
    {
        private readonly AppDbContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<IdentityUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<IdentityUser> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        [Route("User/Profile/{username}")]
        public async Task<IActionResult> Profile(string username)
        {
            var user = await _userManager.GetUserAsync(User);

            Console.WriteLine($"Current user: {user?.UserName}, Requested profile: {username}");

            if (user != null && user.UserName != username || username == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var userInfo = new UserModel();

            try
            {
                userInfo = await _db.MentiiUsersTbl
                                .Include(s => s.Skills)
                                .FirstOrDefaultAsync(u => u.UserUsername == username);
                if (userInfo == null)
                {
                    return NotFound();
                }
            }
            catch (DbException dbEx)
            {
                return StatusCode(500, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(EditProfileViewModel userModel)
        {
            if (!ModelState.IsValid) 
            {
                if(userModel.UserName == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Profile", new { username = userModel.UserName });
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null) { return RedirectToAction("Index", "Home"); }

                List<string> userSkills = [.. userModel.UserSkills.Split(',').Select(s => s.Trim())];

                var existingSkills = await _db.MentiiSkillsTbl
                    .Where(s => s.UserUuid == Guid.Parse(user.Id)).ToListAsync();

                foreach (var skill in existingSkills)
                {
                    _db.MentiiSkillsTbl.Remove(skill);
                }

                await _db.SaveChangesAsync();

                foreach (var skill in userSkills)
                {
                    var newSkill = new Skill
                    {
                        SkillName = skill,
                        UserUuid = Guid.Parse(user.Id)
                    };

                    await _db.MentiiSkillsTbl.AddAsync(newSkill);
                }

                var userInfo = await _db.MentiiUsersTbl.FirstOrDefaultAsync(u => u.UserUuid == Guid.Parse(user.Id));

                if (userInfo != null)
                {
                    if (userInfo.UserUsername != userModel.UserName && !string.IsNullOrEmpty(userModel.UserName))
                    {
                        var newUserName = await _db.MentiiUsersTbl.FirstOrDefaultAsync(u => u.UserUsername == userModel.UserName && u.UserUuid != Guid.Parse(user.Id));
                        if (newUserName != null)
                        {
                            ModelState.AddModelError("UserName:", "Username is already in use.");
                            return RedirectToAction("Profile", new { username = userModel.UserName });
                        }
                        else
                        {
                            userInfo.UserUsername = userModel.UserName;
                        }
                    }

                    if (userInfo.UserEmail != userModel.UserEmail && !string.IsNullOrEmpty(userModel.UserEmail))
                    {
                        var newEmail = await _db.MentiiUsersTbl.FirstOrDefaultAsync(u => u.UserEmail == userModel.UserEmail && u.UserUuid != Guid.Parse(user.Id));
                        if (newEmail != null)
                        {
                            ModelState.AddModelError("UserEmail:", "Email is already in use.");
                            return RedirectToAction("Profile", new { username = userModel.UserName });
                        }
                        else
                        {
                            userInfo.UserEmail = userModel.UserEmail;
                        }
                    }

                    if (userInfo.UserBirthday != userModel.UserBirthday && userModel.UserBirthday.CompareTo(DateTime.MinValue) != 0)
                    {
                        var newBirthday = await _db.MentiiUsersTbl.FirstOrDefaultAsync(u => u.UserBirthday == userModel.UserBirthday && u.UserUuid != Guid.Parse(user.Id));
                        if (newBirthday != null)
                        {
                            ModelState.AddModelError("UserBirthday:", "Birthday is already in use.");
                            return RedirectToAction("Profile", new { username = userModel.UserName });
                        }
                        else
                        {
                            userInfo.UserBirthday = userModel.UserBirthday;
                        }
                    }

                    if (userInfo.UserFirstname != userModel.UserFirstName)
                    {
                        userInfo.UserFirstname = userModel.UserFirstName;
                    }

                    if (userInfo.UserLastname != userModel.UserLastName)
                    {
                        userInfo.UserLastname = userModel.UserLastName;
                    }

                    if (userInfo.UserTitle != userModel.UserTitle)
                    {
                        userInfo.UserTitle = userModel.UserTitle;
                    }
                }

                await _db.SaveChangesAsync();

                UserProfileViewModel userProfile = new()
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
                return View(userProfile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
