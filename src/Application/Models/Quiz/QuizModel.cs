using QuizeMC.Common.Enums;
using QuizeMC.Application.Models.Question;

namespace QuizeMC.Application.Models.Quiz
{
    public record QuizModel(
        Guid Id,
        string Title,
        IEnumerable<QuestionModel> Questions,
        QuizStatus Status
    );
}