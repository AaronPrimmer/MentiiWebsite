using MentiiWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using MentiiWebsite.Data;

namespace MentiiWebsite.Controllers
{    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User User)
        {
            if(ModelState.IsValid)
            {
                var userlookup = _db.MentiiUsersTbl.FirstOrDefault(u => u.UserUsername == User.UserUsername);
                if (userlookup != null && BCrypt.Net.BCrypt.EnhancedVerify(User.UserPassword, userlookup.UserPassword))
                {
                    // Authentication successful, redirect to the desired page
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                }
            }
            return View(User);
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            string NewUserUsername = model.Username;
            var existingUsername = _db.MentiiUsersTbl.FirstOrDefault(u => u.UserUsername == NewUserUsername);
            if (existingUsername != null) 
            {
                ModelState.AddModelError("Username", "Username already exists. Please choose a different username.");
                return View(model);
            }

            var existingEmail = _db.MentiiUsersTbl.FirstOrDefault(u => u.UserEmail == model.Email);
            if (existingEmail != null) 
            { 
                ModelState.AddModelError("Email", "Email already exists. Please use a different email address.");
                return View(model);
            }

            User signupUser = new User
            {
                UserFirstname = model.FirstName,
                UserLastname = model.LastName,
                UserUsername = model.Username,
                UserEmail = model.Email,
                UserPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password),
                UserTitle = model.Title,
                UserBirthday = model.Birthday,
                UserEnabled = true
            };

            if (ModelState.IsValid)
            {
                var newUser = await _db.MentiiUsersTbl.AddAsync(signupUser);
                await _db.SaveChangesAsync();

                if (newUser == null)
                {
                    ModelState.AddModelError("SignupError", "An error occurred while creating the user. Please try again.");
                    return View(model);
                }
                List<string> skillsString = model.SkillsRaw.Split(',').Select(s => s.Trim().ToString()).ToList();
                foreach (var item in skillsString)
                {
                    Skill Skill = new Skill
                    {
                        UserUuid = newUser.Entity.UserUuid,
                        SkillName = item
                    };

                    _db.MentiiSkillsTbl.Add(Skill);
                }
                await _db.SaveChangesAsync();
                // Handle the signup logic here, e.g., save the user to the database
                return RedirectToAction("Login");
            }
            return View(model);
        }
    }
}
