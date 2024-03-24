using ShippingPricingStrategy.Domain.Models.Entities;

namespace ShippingPricingStrategy.Application.Services;

public abstract class Strategy(Service service): IStrategy
{
    public abstract Task<decimal?> CalculateTotalPrice();
}