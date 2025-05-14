using Domain.Entities;

namespace Repositories.Abstractions
{
    public interface IQuestion : IRepository<Question>
    {
        Task<IEnumerable<Question>> GetByQuizIdAsync(Guid quizId, CancellationToken ct);
        Task<Question?> GetWithAnswersAsync(Guid questionId, CancellationToken ct);
    }
}