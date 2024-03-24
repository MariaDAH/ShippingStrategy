using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingPricingStrategy.Domain.Models.Entities;

[Table("Price")]
public record Price
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long PriceCode { get; init; } = 0;
    public string ServiceName { get; set; }
    [Column(TypeName = "decimal(18, 2)")] public decimal IndividualPrice { get; set; }
    [Column(TypeName = "decimal(18, 2)")] public decimal? MultiPurchasePrice { get; set; }
    public int? QuantityPromotion { get; set; }
    
    public static Price Create(string serviceName, decimal price, int countPromo, decimal pricePromo)
    {
        return new Price()
        {
            ServiceName = serviceName,
            IndividualPrice = price,
            MultiPurchasePrice = pricePromo,
            QuantityPromotion = countPromo,
        };
    }

    public void Update(string serviceName, decimal price, int countPromo, decimal pricePromo)
    {
        ServiceName = serviceName;
        IndividualPrice = price;
        MultiPurchasePrice = pricePromo;
        QuantityPromotion = countPromo;
    }
}