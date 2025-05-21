using QuizeMC.Domain.Entities;


namespace QuizeMC.Domain.Repositories.Abstractions
{
    public interface IParticipant : IRepository<Participant>
    {
        Task<Participant?> GetByUsernameAsync(string username, CancellationToken ct);
        Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct);
    }
}