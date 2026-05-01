using MentiiWebsite.Data;
using Microsoft.AspNetCore.Identity;

namespace MentiiWebsite.Models.Config
{

    public class IdentityConfig
    {

        public static async Task CreateAdminUserAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var db = serviceProvider.GetRequiredService<AppDbContext>();

            string username = "admin";
            string password = "Admin_123";
            string roleName = "Admin";

            // If role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            //if user doesn't exist, create it and assign role
            if(await userManager.FindByNameAsync(username) == null)
            {
                ApplicationUser user = new() { UserName = username, Email = "admin@mentii.com", EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);

                    UserModel newUser = new()
                    {
                        UserUuid = Guid.Parse(user.Id),
                        UserFirstname = "Admin",
                        UserLastname = "User",
                        UserUsername = username,
                        UserEmail = user.Email,
                        UserTitle = "Administrator",
                        UserBirthday = new DateTime(1970, 1, 1),
                        UserEnabled = true,
                        UserDateCreated = DateTime.Now
                    };

                    await db.MentiiUsersTbl.AddAsync(newUser);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
