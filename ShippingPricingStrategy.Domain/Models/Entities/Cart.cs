using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingPricingStrategy.Domain.Models.Entities;

[Table("Cart")]
public class Cart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long CartId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalAmount { get; set; }
    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalDiscount { get; set; }

    public Customer Customer { get; set; } = new();

    public List<Service> Services { get; init; } = new();

    public static Cart Create(decimal totalAmount, decimal totalDiscount)
    {
        return new Cart()
        {
            TotalDiscount = 0,
            TotalAmount = 0
        };
    }

    public void Update(decimal? totalAmount, decimal? totalDiscount)
    {
        TotalDiscount = totalDiscount;
        TotalAmount = totalAmount;
    }
}

