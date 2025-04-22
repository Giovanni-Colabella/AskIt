using AskIt.Models.Customizations.Exceptions.Account;
using AskIt.Models.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AskIt.ViewComponents.AccountSettings;

public class ChangePasswordViewComponent : ViewComponent
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ChangePasswordViewComponent(UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Recupera le informazioni dell'utente 
        if(_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.User == null)
            throw new AccountNotFoundException();
        ApplicationUser? user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

        return View();
    }
}
