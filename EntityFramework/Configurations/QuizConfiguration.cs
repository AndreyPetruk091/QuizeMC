using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using ValueObjects;

namespace EntityFramework.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasConversion(title => title.Value, str => new QuizTitle(str))
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasMany(x => x.Questions)
                .WithOne()
                .HasForeignKey("QuizId");
        }
    }
}