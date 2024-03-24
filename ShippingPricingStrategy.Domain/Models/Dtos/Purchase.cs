namespace ShippingPricingStrategy.Domain.Models.Dtos;

public record Purchase(long CustomerId, long CartId,string ServiceName, int Quantity);