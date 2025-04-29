using Microsoft.AspNetCore.Authorization;

namespace AskIt.Models.Authorization
{
    public class DeleteAnswerRequirement : IAuthorizationRequirement
    {
        public DeleteAnswerRequirement()
        { 
        }
    }
}
