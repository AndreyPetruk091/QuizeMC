using QuizeMC.Domain.Entities;


namespace QuizeMC.Domain.Repositories.Abstractions
{
    public interface IQuestion : IRepository<Question>
    {
        Task<IEnumerable<Question>> GetByQuizIdAsync(Guid quizId, CancellationToken ct);
        Task<Question?> GetWithAnswersAsync(Guid questionId, CancellationToken ct);
    }
}