using ShippingPricingStrategy.Domain.Models.Entities;

namespace ShippingPricingStrategy.Application.Services;

public class StrategyIndividual(Service service): Strategy(service)
{
    public async override Task<decimal?> CalculateTotalPrice()
    {
        var quantity = service.Quantity;
        var price = service.Price.IndividualPrice;
        return quantity * price;
    }
}