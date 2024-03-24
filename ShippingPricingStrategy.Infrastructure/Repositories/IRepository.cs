namespace ShippingPricingStrategy.Infrastructure.Repositories;

public interface IRepository<TEntity>: IDisposable
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> RemoveAsync(TEntity entity);
    Task<TEntity> Update(TEntity entity);
    Task<TEntity>? GetByIdAsync(object id);
    Task<IEnumerable<TEntity>> GetAllAsync();
}