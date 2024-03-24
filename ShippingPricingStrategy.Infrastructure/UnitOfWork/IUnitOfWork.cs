using ShippingPricingStrategy.Infrastructure.Repositories;

namespace ShippingPricingStrategy.Infraestructure.UnitOfWork;

public interface IUnitOfWork
{
    void Commit();
    void Rollback();
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
}