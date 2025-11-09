using QuizeMC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Infrastructure.EntityFramework.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.Id);

            // Конфигурация Text Value Object
            builder.OwnsOne(q => q.Text, textBuilder =>
            {
                textBuilder.Property(t => t.Value)
                    .HasColumnName("Text")
                    .IsRequired()
                    .HasMaxLength(500);
            });

            // Конфигурация CorrectAnswerIndex Value Object
            builder.OwnsOne(q => q.CorrectAnswerIndex, indexBuilder =>
            {
                indexBuilder.Property(i => i.Value)
                    .HasColumnName("CorrectAnswerIndex")
                    .IsRequired();
            });

            // Настройка Answers как owned types (Value Objects)
            builder.OwnsMany(q => q.Answers, a =>
            {
                a.WithOwner().HasForeignKey("QuestionId");

                a.Property<int>("Id")
                    .ValueGeneratedOnAdd();
                a.HasKey("Id");

                // Конфигурация Text Value Object для Answer
                a.OwnsOne(ans => ans.Text, textBuilder =>
                {
                    textBuilder.Property(t => t.Value)
                        .HasColumnName("Text")
                        .IsRequired()
                        .HasMaxLength(200);
                });
            });

            // Связь с Quiz
            builder.HasOne<Quiz>()
                .WithMany(q => q.Questions)
                .HasForeignKey("QuizId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}