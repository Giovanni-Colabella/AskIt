using AskIt.Models.Data.Entities;
using AskIt.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AskIt.Models.Data.FluentConfig;

public class QuestionFluentConfig : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasOne(q => q.Author)
            .WithMany(u => u.Questions)
            .HasForeignKey(q => q.AuthorId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        

        builder.Property(q => q.Status).HasDefaultValue(QuestionStatus.Open);
        builder.Property(q => q.Status).HasConversion<string>();   
    }
}
