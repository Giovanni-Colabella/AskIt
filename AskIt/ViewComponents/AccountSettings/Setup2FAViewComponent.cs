using AskIt.Models.Customizations.Exceptions.Account;
using AskIt.Models.Data.Entities;
using AskIt.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AskIt.ViewComponents.AccountSettings
{
    public class Setup2FAViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Setup2FAViewComponent(UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.User == null)
                throw new AccountNotFoundException();

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
                throw new AccountNotFoundException();

            var model = new EnableAuthenticatorViewModel
            {
                Is2FaEnabled = await _userManager.GetTwoFactorEnabledAsync(user)
            };

            return View(model);
        }
    }
}
