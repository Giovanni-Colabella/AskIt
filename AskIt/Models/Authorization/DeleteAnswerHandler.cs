using AskIt.Models.Data.Entities;
using AskIt.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AskIt.Models.Authorization
{
    public class DeleteAnswerHandler : AuthorizationHandler<DeleteAnswerRequirement, Answer>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public DeleteAnswerHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            DeleteAnswerRequirement requirement, 
            Answer resource)
        {
            var user = await _userManager.GetUserAsync(context.User);
            if (user == null)
            {
                return; 
            }

            var userId = await _userManager.GetUserIdAsync(user);

            if(resource.AuthorId == userId || context.User.IsInRole(nameof(Roles.Admin)))
            {
                context.Succeed(requirement);
            }

        }
    }
}
