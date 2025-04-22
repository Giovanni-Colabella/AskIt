namespace AskIt.Models.Customizations.Exceptions.Account;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException()
        :base("Nessun utente trovato")
    {   
    }
}
