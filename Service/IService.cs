using Domain.Entities.Base;

namespace Service
{
    public interface IService<TEntity> where TEntity : EntityBase
    {
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct);
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct);
        Task<bool> UpdateAsync(TEntity entity, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    }
}