using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingPricingStrategy.Domain.Models.Entities;

[Table("Service")]
public record Service
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ServiceId { get; set; }
    public string? ServiceName { get; set; }
    public Price Price { get; set; } = new();
    public int Quantity { get; set; } = 0;

    public decimal TotalAmount { get; set; } = 0;

    public decimal TotalDiscount { get; set; } = 0;
    
    public static Service Create(string serviceName, int quantity)
    {
        return new Service()
        {
            ServiceName = serviceName,
            Quantity = quantity,
        };
    }

    public void Update(string serviceName, int quantity, Price price, decimal totalAmount, decimal totalDiscount)
    {
        ServiceName = serviceName;
        Quantity = quantity;
        Price = price;
        TotalDiscount = totalDiscount;
        TotalAmount = totalAmount;
    }
}