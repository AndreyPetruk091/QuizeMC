using QuizeMC.Application.Models.Paticipiant;


namespace QuizeMC.Application.Services.Abstactions
{
    public interface IParticipantApplicationService
    {
        Task<ParticipantModel?> GetParticipantByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ParticipantModel?> CreateParticipantAsync(ParticipantCreateModel participantInfo, CancellationToken cancellationToken);
    }
}
