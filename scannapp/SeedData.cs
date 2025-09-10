using DAL.Models;
using Microsoft.AspNetCore.Identity;


namespace scannapp
{
    public static class SeedData
    {
        public static async Task SeedRolesAndAdmin(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            // Roles
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }

            // Admin User 
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                var user = new User
                {
                    UserName = "admin",
                    Firstname = "System",
                    Lastname = "Admin"
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
