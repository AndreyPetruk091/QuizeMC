using Models.Paticipiant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstactions
{
    public interface IParticipantApplicationService
    {
        Task<ParticipantModel?> GetParticipantByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ParticipantModel?> CreateParticipantAsync(ParticipantCreateModel participantInfo, CancellationToken cancellationToken);
    }
}
