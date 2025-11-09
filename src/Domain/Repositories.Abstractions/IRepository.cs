using QuizeMC.Domain.Entities.Base;

namespace QuizeMC.Domain.Repositories.Abstractions
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> ExistsAsync(Guid id);
        Task<int> SaveChangesAsync();
    }
}