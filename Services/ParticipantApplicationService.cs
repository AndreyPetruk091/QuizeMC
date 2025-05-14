using Domain.Entities;
using Models.Paticipiant;
using Repositories.Abstractions;
using Services.Abstactions;
using AutoMapper;
using ValueObjects;

namespace Services.Implementations
{
    public class ParticipantApplicationService : IParticipantApplicationService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;

        public ParticipantApplicationService(
            IParticipantRepository participantRepository,
            IMapper mapper)
        {
            _participantRepository = participantRepository;
            _mapper = mapper;
        }

        public async Task<ParticipantModel?> CreateParticipantAsync(ParticipantCreateModel participantInfo, CancellationToken cancellationToken)
        {
            // Валидация и преобразование string -> Username Value Object
            if (string.IsNullOrWhiteSpace(participantInfo.Username))
                return null;

            var username = new Username(participantInfo.Username);
            var participant = new Paticipiant(username);

            var created = await _participantRepository.AddAsync(participant, cancellationToken);
            return created == null ? null : _mapper.Map<ParticipantModel>(created);
        }

        public async Task<ParticipantModel?> GetParticipantByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var participant = await _participantRepository.GetByIdAsync(id, cancellationToken);
            return participant == null ? null : _mapper.Map<ParticipantModel>(participant);
        }

        public async Task<ParticipantModel?> GetParticipantByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var participant = await _participantRepository.GetParticipantByUsernameAsync(username, cancellationToken);
            return participant == null ? null : _mapper.Map<ParticipantModel>(participant);
        }
    }
}