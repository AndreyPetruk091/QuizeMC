using QuizeMC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Infrastructure.EntityFramework.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            // Конфигурация Name Value Object
            builder.OwnsOne(c => c.Name, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // Конфигурация Description Value Object
            builder.OwnsOne(c => c.Description, descBuilder =>
            {
                descBuilder.Property(d => d.Value)
                    .HasColumnName("Description")
                    .IsRequired(false)
                    .HasMaxLength(500);
            });

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            // Индексы - исправляем без .Value
            builder.HasIndex("Name.Value")  // Используем строковое представление
                .IsUnique()
                .HasDatabaseName("IX_Categories_Name");

            builder.HasIndex(c => c.CreatedByAdminId)
                .HasDatabaseName("IX_Categories_CreatedByAdminId");

            // Связи
            builder.HasOne(c => c.CreatedByAdmin)
                .WithMany(a => a.CreatedCategories)
                .HasForeignKey(c => c.CreatedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Quizzes)
                .WithOne(q => q.Category)
                .HasForeignKey(q => q.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}