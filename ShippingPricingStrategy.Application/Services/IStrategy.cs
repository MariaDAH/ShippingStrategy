using ShippingPricingStrategy.Domain.Models.Entities;

namespace ShippingPricingStrategy.Application.Services;

public interface IStrategy
{
    Task<decimal?> CalculateTotalPrice();
}