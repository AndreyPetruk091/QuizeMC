using QuizeMC.Application.Models.Answer;

namespace QuizeMC.Application.Models.Question
{
    public record QuestionCreateModel(
        string Text,
        int CorrectAnswerIndex,
        List<AnswerCreateModel> Answers
    ) : CreateModel;
}