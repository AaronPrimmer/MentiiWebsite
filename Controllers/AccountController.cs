using MentiiWebsite.Data;
using MentiiWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MentiiWebsite.Controllers
{    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(AppDbContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please correct the errors in the form.");
                return View(model);
            }

            // ✅ Check if username is already taken
            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("Username", "This username is already taken.");
                return View(model);
            }

            var user = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var newUser = await _db.MentiiUsersTbl.AddAsync(new UserModel
                    {
                        UserUuid = Guid.Parse(user.Id),
                        UserFirstname = "",
                        UserLastname = "",
                        UserUsername = model.Username,
                        UserEmail = model.Email,
                        UserTitle = "",
                        UserBirthday = model.Birthday,
                        UserEnabled = true,
                        UserDateCreated = DateTime.UtcNow
                });

                await _db.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please correct the errors in the form.");
                return View(User);
            }

            var result = _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.IsCompletedSuccessfully)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsFaulted || result.IsCanceled)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while trying to log in. Please try again.");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            //string NewUserUsername = model.Username;
            //var existingUsername = _db.MentiiUsersTbl.FirstOrDefault(u => u.UserUsername == NewUserUsername);
            //if (existingUsername != null) 
            //{
            //    ModelState.AddModelError("Username", "Username already exists. Please choose a different username.");
            //    return View(model);
            //}

            //var existingEmail = _db.MentiiUsersTbl.FirstOrDefault(u => u.UserEmail == model.Email);
            //if (existingEmail != null) 
            //{ 
            //    ModelState.AddModelError("Email", "Email already exists. Please use a different email address.");
            //    return View(model);
            //}

            //User signupUser = new User
            //{
            //    UserFirstname = model.FirstName,
            //    UserLastname = model.LastName,
            //    UserUsername = model.Username,
            //    UserEmail = model.Email,
            //    UserPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password),
            //    UserTitle = model.Title,
            //    UserBirthday = model.Birthday,
            //    UserEnabled = true
            //};

            //if (ModelState.IsValid)
            //{
            //    var newUser = await _db.MentiiUsersTbl.AddAsync(signupUser);
            //    await _db.SaveChangesAsync();

            //    if (newUser == null)
            //    {
            //        ModelState.AddModelError("SignupError", "An error occurred while creating the user. Please try again.");
            //        return View(model);
            //    }
            //    List<string> skillsString = model.SkillsRaw.Split(',').Select(s => s.Trim().ToString()).ToList();
            //    foreach (var item in skillsString)
            //    {
            //        Skill Skill = new Skill
            //        {
            //            UserUuid = newUser.Entity.UserUuid,
            //            SkillName = item
            //        };

            //        _db.MentiiSkillsTbl.Add(Skill);
            //    }
            //    await _db.SaveChangesAsync();
            //    // Handle the signup logic here, e.g., save the user to the database
            //    return RedirectToAction("Login");
            //}
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
