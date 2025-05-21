using QuizeMC.Application.Models.Question;

namespace QuizeMC.Application.Services
{
    /// <summary>
    /// Команда для добавления вопроса к викторине
    /// </summary>
    public record AddQuestionCommand(
        Guid QuizId,
        QuestionCreateModel Question
    );
}