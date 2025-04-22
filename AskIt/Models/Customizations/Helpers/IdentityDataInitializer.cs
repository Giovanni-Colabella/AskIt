using AskIt.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace AskIt.Models.Customizations.Helpers
{
    public static class IdentityDataInitializer
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
