using AskIt.Models.Customizations.Exceptions.Account;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.Data.Entities;
using AskIt.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AskIt.Models.Authorization
{
    public class AdminHanlder : AuthorizationHandler<AdminRequirement, ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminHanlder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            AdminRequirement requirement, 
            ApplicationUser resource)
        {
            var user = await _userManager.GetUserAsync(context.User);
            if (user == null) throw new AccountNotFoundException();

            bool isAdmin = await _userManager.IsInRoleAsync(user, nameof(Roles.Admin));
            bool isResourceAdmin = await _userManager.IsInRoleAsync(resource, nameof(Roles.Admin));

            if(isAdmin && !isResourceAdmin) context.Succeed(requirement);
            
        }
    }
}
