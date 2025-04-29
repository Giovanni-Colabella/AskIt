using AskIt.Models.Customizations.Errors;
using AskIt.Models.InputModels;
using AskIt.Models.InputModels.AccountModels;
using AskIt.Models.Services.Application.Account;
using AskIt.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AskIt.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model) 
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(model);

            IActionResult response;

            if (result.IsSuccess)
            {
                response = RedirectToAction("Index", "Home");
            }
            else
            {
                switch (result.Error)
                {
                    case LoginError.InvalidCredentials:
                        ModelState.AddModelError(string.Empty, "Email o password non validi");
                        response = View(model);
                        break;

                    case LoginError.TwoFactorRequired:
                        response = RedirectToAction(nameof(SignInWith2FA));
                        break;

                    default:
                        ModelState.AddModelError(string.Empty, "Errore generico durante il login");
                        response = View(model);
                        break;
                }
            }

            return response;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var registrationResult = await _authService.RegisterAsync(model);

            if (registrationResult.IsSuccess)
            {
                return RedirectToAction("Login", "Account");
            }

            switch (registrationResult.Error)
            {
                case RegistrationError.EmailAlreadyExisting:
                    ModelState.AddModelError(string.Empty, "L'indirizzo scelto è già in uso");
                    break;

                default:
                    ModelState.AddModelError(string.Empty, "Errore generico durante la registrazione");
                    break;
            }

            return View(model);
        }



        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PerformLogout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Enable2FA()
        {
            var model = await _authService.Enable2FaAsync();

            var viewModel = new Enable2FaPageViewModel{
                SetupInfo = model,
                Input = new()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignInWith2FA(VerifyAuthenticatorCodeInputModel model)
        {
            if(!ModelState.IsValid)
                return RedirectToAction(nameof(SignInWith2FA));

            bool result = await _authService.SignInWith2FaAsync(model);

            if(!result){
                ModelState.AddModelError(nameof(VerifyAuthenticatorCodeInputModel.Code), "Codice di verifica non valido");
                return View(model);
            }
                
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SignInWith2FA()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Disable2FA()
        {
            await _authService.Disable2FaAsync();
            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verify2FA(VerifyAuthenticatorCodeInputModel model)
        {
            if(!ModelState.IsValid)
                return RedirectToAction(nameof(Enable2FA));
            bool result = await _authService.Verify2FaAsync(model);

            if(result)
            {
                TempData["Success"] = "Autenticazione a due fattori abilitata con successo";
            } else {
                TempData["Error"] = "Codice non valido. Riprova.";
            }

            return RedirectToAction(nameof(Enable2FA));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordInputModel model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Profile));

            bool result = await _authService.ChangePasswordAsync(model);
            
            if(!result)
            {
                ModelState.AddModelError(string.Empty, "Ops! Qualcosa non ha funzionato, controlla che la password corrente sia corretta");
                return View(nameof(Profile));
            }

            return View(nameof(Profile));
        }


    }
}
