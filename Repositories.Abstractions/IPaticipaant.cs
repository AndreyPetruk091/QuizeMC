using Domain.Entities;

namespace Repositories.Abstractions
{
    public interface IParticipantRepository : IRepository<Paticipiant>
    {

        Task<Paticipiant?> GetParticipantByUsernameAsync(string username, CancellationToken cancellationToken);
    }
}
