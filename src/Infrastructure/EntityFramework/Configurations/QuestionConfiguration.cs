using QuizeMC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizeMC.Domain.ValueObjects;

namespace EntityFramework.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.Text)
                .IsRequired()
                .HasConversion(text => text.Value, str => new QuestionText(str))
                .HasMaxLength(500);

            builder.Property(x => x.CorrectAnswerIndex)
                .IsRequired();

            builder.HasMany(x => x.Answers)
                .WithOne()
                .HasForeignKey("QuestionId");
        }
    }
}