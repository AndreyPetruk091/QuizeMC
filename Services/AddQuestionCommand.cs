

namespace Models.Question
{
    /// <summary>
    /// Команда для добавления вопроса к викторине
    /// </summary>
    public record AddQuestionCommand(
        Guid QuizId,
        QuestionCreateModel Question
    );
}