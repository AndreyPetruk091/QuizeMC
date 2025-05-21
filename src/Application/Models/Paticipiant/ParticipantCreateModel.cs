using QuizeMC.Application.Application.Models;
namespace QuizeMC.Application.Models.Paticipiant
{
    public record ParticipantCreateModel(
    Guid Id,
    string Username
) : CreateModel(Id, Username);
}
