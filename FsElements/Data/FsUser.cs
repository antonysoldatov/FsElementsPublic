using Microsoft.AspNetCore.Identity;

namespace FsElements.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class FsUser : IdentityUser
    {
        public bool IsActiveSeller { get; set; }
    }
}
