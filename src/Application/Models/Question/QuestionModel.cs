using QuizeMC.Application.Application.Models;
using QuizeMC.Application.Models.Answer;

namespace QuizeMC.Application.Models.Question
{
    public record QuestionModel(
     Guid Id,
     string Text,
     IEnumerable<AnswerModel> Answers,
     int CorrectAnswerIndex,
     Guid QuizId
 ) : IQuestionModel<Guid>, IModel<Guid>;
}
