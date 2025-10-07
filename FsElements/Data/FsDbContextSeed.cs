using Microsoft.AspNetCore.Identity;

namespace FsElements.Data
{
    public class FsDbContextSeed
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            SeedRoles(serviceProvider).Wait();
            SeedUsers(serviceProvider).Wait();
        }

        private static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var role = await roleManager.FindByNameAsync(Roles.Admin);
            if (role == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            var roleSeller = await roleManager.FindByNameAsync(Roles.Seller);
            if (roleSeller == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Seller));
            }
        }

        private static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var userManager = serviceProvider.GetRequiredService<UserManager<FsUser>>();
            var adminUser = configuration["AdminUser"];
            var adminPswd = configuration["AdminPassword"];

            var u = await userManager.FindByNameAsync(adminUser!);
            if (u == null)
            {
                var admin = new FsUser()
                {
                    UserName = adminUser,
                    Email = adminUser,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, adminPswd!);
                await userManager.AddToRoleAsync(admin, Roles.Admin);
            }
        }
    }
}
