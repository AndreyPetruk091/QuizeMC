using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Domain.Enums;

namespace QuizeMC.Infrastructure.EntityFramework.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasKey(q => q.Id);

            // Конфигурация Title Value Object
            builder.OwnsOne(q => q.Title, titleBuilder =>
            {
                titleBuilder.Property(t => t.Value)
                    .HasColumnName("Title")
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // Конфигурация Description Value Object
            builder.OwnsOne(q => q.Description, descBuilder =>
            {
                descBuilder.Property(d => d.Value)
                    .HasColumnName("Description")
                    .IsRequired(false)
                    .HasMaxLength(1000);
            });

            builder.Property(q => q.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(q => q.CreatedAt)
                .IsRequired();

            builder.Property(q => q.PublishedAt)
                .IsRequired(false);

            builder.Property(q => q.ArchivedAt)
                .IsRequired(false);

            // Индексы - исправляем без .Value
            builder.HasIndex("Title.Value")  // Используем строковое представление
                .HasDatabaseName("IX_Quizzes_Title");

            builder.HasIndex(q => q.Status)
                .HasDatabaseName("IX_Quizzes_Status");

            builder.HasIndex(q => q.CreatedByAdminId)
                .HasDatabaseName("IX_Quizzes_CreatedByAdminId");

            builder.HasIndex(q => q.CategoryId)
                .HasDatabaseName("IX_Quizzes_CategoryId");

            // Связи
            builder.HasOne(q => q.CreatedByAdmin)
                .WithMany(a => a.CreatedQuizzes)
                .HasForeignKey(q => q.CreatedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(q => q.Category)
                .WithMany(c => c.Quizzes)
                .HasForeignKey(q => q.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(q => q.Questions)
                .WithOne()
                .HasForeignKey("QuizId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}