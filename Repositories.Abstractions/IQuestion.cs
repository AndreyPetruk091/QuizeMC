using Domain.Entities;


namespace Repositories.Abstractions
{
    public interface IQuestionRepository : IRepository<Question> 
    {
        Task<IEnumerable<Question>> GetQuestionsByQuizIdAsync(Guid quizId, CancellationToken cancellationToken);
        Task<Question?> GetQuestionByIdAsync(Guid questionId, CancellationToken cancellationToken);
    }
}
