using Microsoft.AspNetCore.Authorization;

namespace AskIt.Models.Authorization;

public class EditCourseRequirement : IAuthorizationRequirement
{
    public EditCourseRequirement()
    {
    }
}
