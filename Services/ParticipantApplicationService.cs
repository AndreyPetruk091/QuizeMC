using AutoMapper;
using  Domain.Entities;
using Models.Paticipiant;
using Repositories.Abstractions;
using Services.Abstactions;


namespace Services
{
    public class ParticipantApplicationService(
        IParticipantRepository participantRepository,
        IMapper mapper) : IParticipantApplicationService
    {
        public async Task<ParticipantModel?> CreateParticipantAsync(ParticipantCreateModel participantInfo, CancellationToken cancellationToken)
        {
            var participant = new Paticipiant(participantInfo.Username);
            var created = await participantRepository.AddAsync(participant, cancellationToken);
            return created == null ? null : mapper.Map<ParticipantModel>(created);
        }

        public async Task<ParticipantModel?> GetParticipantByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var participant = await participantRepository.GetByIdAsync(id, cancellationToken);
            return participant == null ? null : mapper.Map<ParticipantModel>(participant);
        }
    }
}