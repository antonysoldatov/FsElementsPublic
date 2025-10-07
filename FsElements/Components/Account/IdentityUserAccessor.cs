using FsElements.Data;
using Microsoft.AspNetCore.Identity;

namespace FsElements.Components.Account
{
    internal sealed class IdentityUserAccessor(UserManager<FsUser> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<FsUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }
    }
}
