using AskIt.Models.Data.Entities;
using AskIt.Models.Enums;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AskIt.Models.Data.FluentConfig;

public class CourseFluentConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property(c => c.CourseStatus)
            .HasDefaultValue(CourseStatus.Privato)
            .HasConversion<string>();

        builder.Property(c => c.Price)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(c => c.AuthorId)
            .IsRequired();

        builder.HasOne(c => c.Author)
            .WithMany(u => u.Courses)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}
