using ShippingPricingStrategy.Infrastructure.Daos;
using ShippingPricingStrategy.Infrastructure.Repositories;

namespace ShippingPricingStrategy.Infraestructure.UnitOfWork;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = new();

    public void Commit()
    {
        context.SaveChanges();
    }

    public void Rollback()
    {
        // Rollback changes if needed
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        var repository = new Repository<TEntity>(context);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }

    public void Dispose()
    {
        context.Dispose();
    }
}