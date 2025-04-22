using Microsoft.AspNetCore.Authorization;

namespace AskIt.Models.Authorization
{
    public class DeleteQuestionRequirement : IAuthorizationRequirement
    {
        public DeleteQuestionRequirement()
        {
        }
    }
}
