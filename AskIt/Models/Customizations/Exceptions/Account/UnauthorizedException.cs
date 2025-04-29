namespace AskIt.Models.Customizations.Exceptions.Account
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
            :base("Non sei autorizzato ad accedere a questa risorsa.")
        {
            
        }
    }
}
