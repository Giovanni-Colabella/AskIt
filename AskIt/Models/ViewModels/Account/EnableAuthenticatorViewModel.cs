namespace AskIt.Models.ViewModels;

public class EnableAuthenticatorViewModel
{
    public string SharedKey { get; set; } = string.Empty;
    public string AuthenticatorUri { get; set; } = string.Empty;
    public bool Is2FaEnabled { get; set; } = false;
}
