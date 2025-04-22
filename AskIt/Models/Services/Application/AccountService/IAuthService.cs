using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.InputModels;
using AskIt.Models.ViewModels;

namespace AskIt.Models.Services.Application.Account
{
    public interface IAuthService
    {
        Task<Result<LoginSuccessViewModel, LoginError>> LoginAsync(LoginInputModel model);
        Task<Result<RegistrationError>> RegisterAsync(RegisterInputModel model);
        Task LogoutAsync();
        Task<EnableAuthenticatorViewModel> Enable2FaAsync();
        Task<bool> SignInWith2FaAsync(VerifyAuthenticatorCodeInputModel model);
        Task<bool> Verify2FaAsync(VerifyAuthenticatorCodeInputModel model);
        Task<bool> ChangePasswordAsync(ChangePasswordInputModel model);
        Task Disable2FaAsync();
    }
}
