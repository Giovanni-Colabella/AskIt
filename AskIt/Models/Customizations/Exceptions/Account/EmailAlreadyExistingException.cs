namespace AskIt.Models.Customizations.Exceptions.Account
{
    public class EmailAlreadyExistingException : Exception
    {
        public EmailAlreadyExistingException(string email)
            :base($"L'indirizzo mail {email} è già in uso")
        {
        }
    }
}
