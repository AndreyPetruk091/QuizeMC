using QuizeMC.Domain.Entities;
using QuizeMC.Domain.Enums;

namespace QuizeMC.Domain.Repositories.Abstractions
{
    public interface IQuizRepository : IRepository<Quiz>
    {
        Task<Quiz> GetWithQuestionsAndAnswersAsync(Guid quizId);
        Task<Quiz> GetWithCategoryAsync(Guid quizId);
        Task<Quiz> GetWithAdminAsync(Guid quizId);
        Task<IEnumerable<Quiz>> GetByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<Quiz>> GetByAdminIdAsync(Guid adminId);
        Task<IEnumerable<Quiz>> GetByStatusAsync(QuizStatus status);
        Task<IEnumerable<Quiz>> GetByAdminAndStatusAsync(Guid adminId, QuizStatus status);
        Task<bool> TitleExistsAsync(string title);
        Task<int> GetQuizzesCountByAdminAsync(Guid adminId);
        Task<int> GetQuizzesCountByCategoryAsync(Guid categoryId);
    }
}