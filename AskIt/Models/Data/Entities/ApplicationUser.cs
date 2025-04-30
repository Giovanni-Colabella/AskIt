using AskIt.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace AskIt.Models.Data.Entities;

public class ApplicationUser : IdentityUser
{
    
    public AccountStatus Status { get; set; } = AccountStatus.Attivo;
    public bool IsBanned { get; private set; } = false;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();

    public void ChangeAccountStatus(AccountStatus newStatus) {  Status = newStatus; }

    /// <summary>
    /// Modifica in memoria lo stato di ban dell'account.
    /// E' necessario salvare i cambiamenti nel database per renderli permanenti.
    /// </summary>
    /// <param name="newBanStatus">True per bannare l'account, false per sbloccarlo.</param>

    public void ChangeAccountBanStatus(bool newBanStatus) { IsBanned = newBanStatus; }

}
