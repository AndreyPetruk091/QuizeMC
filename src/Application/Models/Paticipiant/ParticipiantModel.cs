using QuizeMC.Application.Application.Models;
using QuizeMC.Application.Models.Quiz;


namespace QuizeMC.Application.Models.Paticipiant
{
    public record ParticipantModel(
      Guid Id,
      string Username,
      IEnumerable<QuizModel> CompletedQuizzes
  ) : Model(Id, Username);

}
