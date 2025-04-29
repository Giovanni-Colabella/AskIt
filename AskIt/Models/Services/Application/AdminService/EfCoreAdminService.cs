
using AskIt.Models.Authorization;
using AskIt.Models.Customizations.Exceptions.Account;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.Data;
using AskIt.Models.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AskIt.Models.Services.Application.AdminService
{
    public class EfCoreAdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbContext;
        public EfCoreAdminService(UserManager<ApplicationUser> userManager,
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }
        public async Task<bool> BanUser(string userId)
        {
            var adminUser = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);
            var banUser = await _userManager.FindByIdAsync(userId);
            if (banUser is null) return false;
            var authResult = await _authorizationService.AuthorizeAsync(
                _httpContextAccessor?.HttpContext?.User ?? throw new AccountNotFoundException(),
                banUser,
                new AdminRequirement()
            );

            if(!authResult.Succeeded) throw new UnauthorizedException();

            banUser.ChangeAccountBanStatus(true);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
