using AskIt.Models.Data.Entities;
using AskIt.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AskIt.Models.Authorization
{
    public class DeleteQuestionHandler : AuthorizationHandler<DeleteQuestionRequirement, Question>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            DeleteQuestionRequirement requirement, 
            Question resource)
        {

            // controlla se l'utente è admin
            if (context.User.IsInRole(nameof(Roles.Admin)))
                context.Succeed(requirement);

            // controlla se è proprietario della domanda 
            string? userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(resource.AuthorId == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
