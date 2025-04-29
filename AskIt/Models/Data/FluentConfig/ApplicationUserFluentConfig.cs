using AskIt.Models.Data.Entities;
using AskIt.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AskIt.Models.Data.FluentConfig;

public class ApplicationUserFluentConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.Status).HasDefaultValue(AccountStatus.Attivo);
        builder.Property(u => u.Status).HasConversion<string>();

        builder.Property(u => u.UserName).IsRequired();
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Status).IsRequired();
        builder.Property(u => u.IsBanned).IsRequired().HasDefaultValue(false);

        builder.HasIndex(u => u.Email).IsUnique();

        // Filtro globale
        builder.HasQueryFilter(u => u.Status != AccountStatus.Eliminato);
    }
}
