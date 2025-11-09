using QuizeMC.Domain.Enums;

namespace QuizeMC.Application.Models.Quiz
{
    public record QuizCreateModel(string Title, string Description, Guid CategoryId) : CreateModel;
}