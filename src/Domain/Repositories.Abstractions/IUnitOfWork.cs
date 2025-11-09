namespace QuizeMC.Domain.Repositories.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IAdminRepository AdminRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IQuizRepository QuizRepository { get; }
        IQuestionRepository QuestionRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}