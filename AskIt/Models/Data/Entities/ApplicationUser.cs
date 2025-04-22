using AskIt.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace AskIt.Models.Data.Entities;

public class ApplicationUser : IdentityUser
{
    public AccountStatus Status { get; set; } = AccountStatus.Attivo;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public void ChangeAccountStatus(AccountStatus newStatus)
    {
        Status = newStatus;
    }
}
