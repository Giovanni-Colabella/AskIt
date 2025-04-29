namespace AskIt.Models.Services.Application.AdminService
{
    public interface IAdminService
    {
        Task<bool> BanUser(string userId);
    }
}
