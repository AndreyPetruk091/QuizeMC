using QuizeMC.Application.Application.Models;
using QuizeMC.Application.Models.Answer;

namespace QuizeMC.Application.Models.Question
{
    public record QuestionCreateModel(
      string Text,
      List<AnswerCreateModel> Answers,
      int CorrectAnswerIndex,
      Guid QuizId
  ) : ICreateModel;
}
