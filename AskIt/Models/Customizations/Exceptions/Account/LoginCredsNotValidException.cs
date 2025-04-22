namespace AskIt.Models.Customizations.Exceptions.Account
{
    public class LoginCredsNotValidException : Exception
    {
        public LoginCredsNotValidException()
            :base("Le credenziali di accesso non sono valide")
        {
        }
    }
}
