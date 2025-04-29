using System.Text;

using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Exceptions.Account;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.Data.Entities;
using AskIt.Models.Enums;
using AskIt.Models.InputModels;
using AskIt.Models.InputModels.AccountModels;
using AskIt.Models.ViewModels;

using Microsoft.AspNetCore.Identity;

namespace AskIt.Models.Services.Application.Account
{
    public class EfCoreAuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EfCoreAuthService> _logger;
        public EfCoreAuthService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            ILogger<EfCoreAuthService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Result<LoginSuccessViewModel, LoginError>> LoginAsync(LoginInputModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Result<LoginSuccessViewModel, LoginError>.Failure(LoginError.InvalidCredentials);

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.RequiresTwoFactor)
                return Result<LoginSuccessViewModel, LoginError>.Failure(LoginError.TwoFactorRequired);

            if (!result.Succeeded)
                return Result<LoginSuccessViewModel, LoginError>.Failure(LoginError.InvalidCredentials);

            _logger.LogInformation($"Indirizzo {model.Email} ha effettuato l'accesso al suo account");

            // Verifica se l'utente ha un ruolo assegnato, se non ha un ruolo, gli assegna il ruolo di User
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0 || !roles.Contains(nameof(Roles.User)))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, nameof(Roles.User));
                if (!roleResult.Succeeded)
                {
                    _logger.LogError($"Errore assegnazione ruolo: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    return Result<LoginSuccessViewModel, LoginError>.Failure(LoginError.GenericError);
                }
            }

            var viewModel = new LoginSuccessViewModel
            {
                UserId = user.Id,
                UserName = user.UserName ?? ""
            };
            return Result<LoginSuccessViewModel, LoginError>.Success(viewModel);
        }

        public async Task<Result<RegistrationError>> RegisterAsync(RegisterInputModel model)
        {
            // Controllo email esistente
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                _logger.LogInformation($"Email già registrata: {model.Email}");
                return Result<RegistrationError>.Failure(RegistrationError.EmailAlreadyExisting);
            }

            // Creazione utente
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogError($"Errore registrazione: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return Result<RegistrationError>.Failure(RegistrationError.GenericError);
            }

            // Assegna il ruolo direttamente al momento della creazione
            var roleResult = await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            if (!roleResult.Succeeded)
            {
                _logger.LogError($"Errore assegnazione ruolo: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");

                // Se fallisce l'assegnazione del ruolo, rimuovi l'utente appena creato
                await _userManager.DeleteAsync(user);
                _logger.LogError($"Utente {model.Email} eliminato a causa di errore assegnazione ruolo");

                return Result<RegistrationError>.Failure(RegistrationError.GenericError);
            }

            _logger.LogInformation($"Utente registrato con successo: {model.Email}");
            return Result<RegistrationError>.Success();
        }



        public async Task LogoutAsync()
        {
            var user = _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);
            _logger.LogInformation($"Utente {user.Result.Email} ha eseguito il logout");
            await _signInManager.SignOutAsync();
        }

        public async Task<EnableAuthenticatorViewModel> Enable2FaAsync()
        {
            // Ottieni l'oggetto utente dall' httpContext corrente 
            var user = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);

            var is2FaEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            var key = await _userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(key))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                key = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            var model = new EnableAuthenticatorViewModel
            {
                SharedKey = FormatKey(key!),
                AuthenticatorUri = GenerateQrCodeUri(user.UserName!, key!),
                Is2FaEnabled = is2FaEnabled
            };
            return model;
        }

        public async Task<bool> SignInWith2FaAsync(VerifyAuthenticatorCodeInputModel model)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null) throw new AccountNotFoundException();

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, model.Code);

            if (isValid)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(model.Code, true, false);
                if (!result.Succeeded) return false;
                _logger.LogInformation($"User {user.Email} #{user.Id} ha effettuato il login usando 2FA");
                return true;
            }
            _logger.LogInformation($"Login con 2FA fallito per user: {user.Email} #{user.Id}");
            return false;
        }

        public async Task<bool> Verify2FaAsync(VerifyAuthenticatorCodeInputModel model)
        {
            var user = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);

            bool isValid = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, model.Code);
            if (!isValid)
            {
                _logger.LogWarning($"Abilitazione 2FA non riuscita per utente: {user.Email}");
                return false;
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            _logger.LogInformation("2FA abilitato per utente: " + user.Email);
            return true;
        }

        // Metodo per generare l'URI del QR code 2FA
        private string GenerateQrCodeUri(string userName, string key)
        {
            var uri = $"otpauth://totp/AskIt:{userName}?secret={key}&issuer=YourApp&algorithm=SHA256&digits=6";
            return uri;
        }

        // Metodo per formattare la chiave del 2FA
        private string FormatKey(string key)
        {
            var result = new StringBuilder();
            for (int i = 0; i < key.Length; i += 4)
            {
                if (i + 4 < key.Length)
                    result.Append(key.Substring(i, 4)).Append(" ");
                else
                    result.Append(key.Substring(i));
            }

            return result.ToString().ToLowerInvariant();
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordInputModel model)
        {
            var user = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);

            bool isOldPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.OldPassword);

            if (!isOldPasswordCorrect) return false;

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded) return false;

            _logger.LogInformation($"Utente {user.Email} #{user.Id} ha cambiato la sua password.");
            return true;
        }

        public async Task Disable2FaAsync()
        {
            var user = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            _logger.LogInformation($"Utente {user.Email} #{user.Id} ha disabilitato l'autenticazione a due fattori");
        }

    }
}
