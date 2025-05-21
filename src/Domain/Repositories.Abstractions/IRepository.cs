using QuizeMC.Domain.Entities.Base;

namespace QuizeMC.Domain.Repositories.Abstractions
{
    /// <summary>
    /// Базовый интерфейс репозитория для сущностей
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности, наследуемый от EntityBase</typeparam>
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct);
        Task<IEnumerable<TEntity>> GetAllNoTrackingAsync(CancellationToken ct);
        Task<bool> AddAsync(TEntity entity, CancellationToken ct);
        Task<bool> UpdateAsync(TEntity entity, CancellationToken ct);
        Task<bool> DeleteAsync(TEntity entity, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
        Task<int> CountAsync(CancellationToken ct);
    }
}