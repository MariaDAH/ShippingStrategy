using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingPricingStrategy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] {"NationalIdentifier", "Name", "AddressLine"},
                values: new object[,]
                {
                    { "111111", "Acme1","King Street" },
                    { "222222", "Acme2", "Royal Street" },
                    { "333333", "Acme3", "Queen Street" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Customer]");
        }
    }
}
