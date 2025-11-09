using QuizeMC.Domain.Entities;

namespace QuizeMC.Domain.Repositories.Abstractions
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetByNameAsync(string name);
        Task<bool> NameExistsAsync(string name);
        Task<Category> GetWithQuizzesAsync(Guid categoryId);
        Task<IEnumerable<Category>> GetAllWithQuizzesCountAsync();
        Task<IEnumerable<Category>> GetByAdminIdAsync(Guid adminId);
    }
}