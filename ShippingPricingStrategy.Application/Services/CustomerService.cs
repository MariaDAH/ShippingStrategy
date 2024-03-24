using ShippingPricingStrategy.Domain.Models.Entities;
using ShippingPricingStrategy.Infraestructure.UnitOfWork;

namespace ShippingPricingStrategy.Application.Services;

public class CustomerService(IUnitOfWork uow): ICustomerService
{
    public async Task<Customer>? GetCustomById(long id)
    {
        var repo = uow.GetRepository<Customer>();
        return await repo.GetByIdAsync(id)!;
    }
}