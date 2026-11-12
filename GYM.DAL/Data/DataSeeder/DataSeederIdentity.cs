using GYM.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace GYM.DAL.Data.DataSeeder
{
    public static class DataSeederIdentity
    {
        public const string AdminRole = "Admin";
        public const string ReceptionistRole = "Receptionist";

        // Default admin account for first login. Change the password after the first run.
        private const string AdminEmail = "admin@gym.com";
        private const string AdminPassword = "Admin@123";

        public static async Task SeedRolesAndAdminAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in new[] { AdminRole, ReceptionistRole })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (await userManager.FindByEmailAsync(AdminEmail) is not null) return;

            var admin = new AppUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                FullName = "System Admin",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, AdminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, AdminRole);
            }
        }
    }
}
