using QuizeMC.Application.Models.Answer;

namespace QuizeMC.Application.Models.Question
{
    public record QuestionModel(
        Guid Id,
        string Text,
        int CorrectAnswerIndex,
        Guid QuizId,
        List<AnswerModel> Answers
    ) : Model(Id);
}