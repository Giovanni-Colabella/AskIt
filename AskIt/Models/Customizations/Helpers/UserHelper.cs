using AskIt.Models.Customizations.Exceptions.Account;
using AskIt.Models.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace AskIt.Models.Customizations.Helpers
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> GetUserFromHttpContextAsync(
            this UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor)
        {
            var httpContext = contextAccessor.HttpContext;

            if (httpContext == null || httpContext.User == null)
                throw new AccountNotFoundException();

            var user = await userManager.GetUserAsync(httpContext.User);
            if (user == null)
                throw new AccountNotFoundException();

            return user;
        }
    }
}
