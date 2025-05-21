
using QuizeMC.Application.Services.Abstactions;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Application.Models.Paticipiant;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Domain.Entities;
using AutoMapper;

namespace Services.Implementations
{
    public class ParticipantApplicationService : IParticipantApplicationService
    {
        private readonly IParticipant _participantRepository;
        private readonly IMapper _mapper;

        public ParticipantApplicationService(
            IParticipant participantRepository,
            IMapper mapper)
        {
            _participantRepository = participantRepository;
            _mapper = mapper;
        }

        public async Task<ParticipantModel?> CreateParticipantAsync(ParticipantCreateModel participantInfo, CancellationToken cancellationToken)
        {
          
            if (string.IsNullOrWhiteSpace(participantInfo.Username))
                return null;

            var username = new Username(participantInfo.Username);
            var participant = new Participant(username);

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
            var participant = await _participantRepository.GetByUsernameAsync(username, cancellationToken);
            return participant == null ? null : _mapper.Map<ParticipantModel>(participant);
        }
    }
}