using AskIt.Models.InputModels;

namespace AskIt.Models.ViewModels;

public class Enable2FaPageViewModel
{
    public EnableAuthenticatorViewModel SetupInfo { get; set; } = new();
    public VerifyAuthenticatorCodeInputModel Input { get; set; } = new();
}
