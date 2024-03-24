using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingPricingStrategy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Price",
                columns: new[] {"ServiceName", "IndividualPrice", "MultiPurchasePrice", "QuantityPromotion"},
                values: new object[,]
                {
                    { "A", 10.0, 25.0, 3 },
                    { "B", 12.0, 20.0, 2 },
                    { "C", 15.0, null, null },
                    { "D", 25.0, null, null },
                    { "F", 8.0, 15.0, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Price]");
        }
    }
}
