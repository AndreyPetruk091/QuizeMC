using QuizeMC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Infrastructure.EntityFramework.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(a => a.Id);

            // Конфигурация Email Value Object
            builder.OwnsOne(a => a.Email, emailBuilder =>
            {
                emailBuilder.Property(e => e.Value)
                    .HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(255);
            });

            // Конфигурация PasswordHash Value Object
            builder.OwnsOne(a => a.PasswordHash, passwordBuilder =>
            {
                passwordBuilder.Property(p => p.Value)
                    .HasColumnName("PasswordHash")
                    .IsRequired()
                    .HasMaxLength(255);
            });

            builder.Property(a => a.IsActive)
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.LastLoginAt)
                .IsRequired(false);

            // Индексы - исправляем без .Value
            builder.HasIndex(a => a.Email.Value)
                .IsUnique()
                .HasDatabaseName("IX_Admins_Email");

            builder.HasIndex(a => a.IsActive)
                .HasDatabaseName("IX_Admins_IsActive");

            // Связи
            builder.HasMany(a => a.CreatedQuizzes)
                .WithOne(q => q.CreatedByAdmin)
                .HasForeignKey(q => q.CreatedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.CreatedCategories)
                .WithOne(c => c.CreatedByAdmin)
                .HasForeignKey(c => c.CreatedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}