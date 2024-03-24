using ShippingPricingStrategy.Domain.Models.Entities;

namespace ShippingPricingStrategy.Application.Services;

public class MultiPurchaseStrategy(Service service) : Strategy(service)
{
    public override async Task<decimal?> CalculateTotalPrice()
    {
        var originalQuantity = service.Quantity;
        var quantityPromotion = service.Price.QuantityPromotion;
        var pricePromotion = service.Price.MultiPurchasePrice;
        if (originalQuantity >= quantityPromotion)
        {
            var groupByPromo = originalQuantity / quantityPromotion;
            var mod = originalQuantity % quantityPromotion;
            var price = groupByPromo * pricePromotion;
            price += (mod * originalQuantity); 
            return (decimal)price;
        }

        return null;
    }
}