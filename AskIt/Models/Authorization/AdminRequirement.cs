using Microsoft.AspNetCore.Authorization;

namespace AskIt.Models.Authorization
{
    public class AdminRequirement : IAuthorizationRequirement
    {
        public AdminRequirement()
        {
        }
    }
}
