using FsElements.Models.Admin;

namespace FsElements.Services
{
    public interface IUsersService
    {
        Task<List<UserWithRolesViewModel>> GetAllUsers();
        Task SetIsActiveSeller(string userId, bool isActiveSeller);
    }
}