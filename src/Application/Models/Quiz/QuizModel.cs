using QuizeMC.Domain.Enums;

namespace QuizeMC.Application.Models.Quiz
{
    public record QuizModel(
        Guid Id,
        string Title,
        string Description,
        QuizStatus Status,
        DateTime CreatedAt,
        DateTime? PublishedAt,
        DateTime? ArchivedAt,
        Guid CreatedByAdminId,
        string CreatedByAdminEmail,
        Guid CategoryId,
        string CategoryName,
        int QuestionsCount,
        int Complexity
    ) : Model(Id);
}