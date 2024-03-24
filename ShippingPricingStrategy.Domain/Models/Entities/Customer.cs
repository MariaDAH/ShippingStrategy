using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingPricingStrategy.Domain.Models.Entities;

[Table("Customer")]
public record Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long CustomerId { get; init; }
    
    public string? NationalIdentifier { get; set; }
    
    public string? Name { get; set; }
    
    public string? AddressLine { get; set; }

    public static Customer Create(string nationalId,string name, string addressLine)
    {
        return new Customer
        {
            NationalIdentifier = nationalId,
            Name = name,
            AddressLine = addressLine
        };
    }

    public void Update(string nationalId, string name, string addressLine)
    {
        NationalIdentifier = nationalId;
        Name = name;
        AddressLine = addressLine;
    }
}