using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ValueObjects;

namespace EntityFramework.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
           
            // Настройка поля Value (хранится как обычный текст в БД)
            builder.Property(x => x.Value)
                .HasColumnName("Text")  // Опционально: переименовать столбец в БД
                .IsRequired()
                .HasMaxLength(200);    // Ограничение длины

            // Связь с Question (Answer принадлежит Question)
            builder.HasOne<Question>()
                .WithMany(q => q.Answers)
                .HasForeignKey("QuestionId")  // Теньное свойство для внешнего ключа
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);  // Удалять ответы при удалении вопроса
        }
    }
}