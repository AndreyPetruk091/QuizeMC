using QuizeMC.Application.Models.Answer;

namespace QuizeMC.Application.Models.Question
{
    public record QuestionWithAnswersModel(
        Guid Id,
        string Text,
        int CorrectAnswerIndex,
        List<AnswerCreateModel> Answers
    ) : Model(Id);
}