using QuizeMC.Domain.Entities;


namespace QuizeMC.Domain.Repositories.Abstractions
{
    public interface IQuiz : IRepository<Quiz>
    {
        Task<IEnumerable<Quiz>> GetActiveAsync(int skip, int take, CancellationToken ct);
        Task<bool> ExistsByTitleAsync(string title, CancellationToken ct);
        Task<Quiz?> GetByQuestionIdAsync(Guid questionId, CancellationToken ct);
    }
}