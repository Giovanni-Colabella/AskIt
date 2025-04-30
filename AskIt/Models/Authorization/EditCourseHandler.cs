using AskIt.Models.Data.Entities;
using AskIt.Models.Enums;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AskIt.Models.Authorization;

public class EditCourseHandler : AuthorizationHandler<EditCourseRequirement, Course>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public EditCourseHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        EditCourseRequirement requirement,
        Course resource)
    {
        var user = await _userManager.GetUserAsync(context.User);
        if(user == null)
        {
            context.Fail();
            return;
        }

        if(context.User.IsInRole(nameof(Roles.Docente)) && resource.AuthorId == user.Id)
        {
            context.Succeed(requirement);
        } else
        {
            context.Fail();
        }
    }
}
