using FsElements.Data;
using FsElements.Models.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FsElements.Services
{
    public class UsersService : IUsersService
    {
        private readonly FsDbContext dbContext;
        private readonly UserManager<FsUser> userManager;

        public UsersService(FsDbContext dbContext, UserManager<FsUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<List<UserWithRolesViewModel>> GetAllUsers()
        {
            var users = userManager.Users.ToList();
            var usersWithRoles = new List<UserWithRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                usersWithRoles.Add(new UserWithRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsActiveSeller = user.IsActiveSeller,
                    Roles = roles.ToList()
                });
            }

            return usersWithRoles;
        }

        public async Task SetIsActiveSeller(string userId, bool isActiveSeller)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsActiveSeller = isActiveSeller;
                await userManager.UpdateAsync(user);
            }
        }
    }
}
