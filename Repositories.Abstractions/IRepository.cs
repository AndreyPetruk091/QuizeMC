using Domain.Entities.Base;


namespace Repositories.Abstractions
{
    public interface IRepository<TEntity>
        where TEntity : EntityBase 
    {

        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken, bool asNoTracking = false);
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TEntity?> AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);

    }
}
