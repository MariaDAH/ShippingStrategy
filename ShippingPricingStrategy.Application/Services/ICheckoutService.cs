using ShippingPricingStrategy.Domain.Models.Dtos;

namespace ShippingPricingStrategy.Application.Services;

public interface ICheckoutService
{
    Task Scan(Purchase purchase);
    Task<decimal> GetTotalPrice();
}