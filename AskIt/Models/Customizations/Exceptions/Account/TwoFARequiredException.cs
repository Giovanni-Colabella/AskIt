namespace AskIt.Models.Customizations.Exceptions.Account;

public class TwoFARequiredException : Exception
{
    public string UserId { get; set; } = string.Empty;
    public TwoFARequiredException(string userId)
        :base("l'autenticazione a due fattori è richiesta")
    {
        UserId = userId;
    }
}
