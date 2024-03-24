using Microsoft.EntityFrameworkCore;

namespace ShippingPricingStrategy.Infrastructure.Repositories;

public class Repository<TEntity>(DbContext context) : IRepository<TEntity>, IAsyncDisposable where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var result = await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return await Task.FromResult(result.Entity);
    }


    public async Task<TEntity> RemoveAsync(TEntity entity)
    { 
        var result =  _dbSet.Remove(entity);
        await context.SaveChangesAsync();
        return await Task.FromResult(result.Entity);
    }

    public Task<TEntity> Update(TEntity entity)
    {
        var result = _dbSet.Update(entity);
        context.SaveChanges();
        return Task.FromResult(result.Entity);
    }
    
    public async Task<TEntity>? GetByIdAsync(object id)
    {
        return (await _dbSet.FindAsync(id))!;
    }
    
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    
    public void Dispose()
    {
        context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }
}