using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPricingAndUserBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Users",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ListingServicePrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceType = table.Column<int>(type: "integer", nullable: false),
                    FromDay = table.Column<int>(type: "integer", nullable: false),
                    ToDay = table.Column<int>(type: "integer", nullable: false),
                    PricePerDay = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OriginalPricePerDay = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingServicePrices", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ListingServicePrices",
                columns: new[] { "Id", "CreatedAt", "FromDay", "IsActive", "OriginalPricePerDay", "PricePerDay", "ServiceType", "ToDay", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 2.5m, 2.5m, 1, 30, null },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 4m, 4m, 2, 4, null },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, true, 4m, 3.5m, 2, 8, null },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, true, 4m, 3.15m, 2, 16, null },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, true, 4m, 3m, 2, 30, null },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 9m, 9m, 3, 4, null },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, true, 9m, 8m, 3, 8, null },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, true, 9m, 7.5m, 3, 16, null },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, true, 9m, 7m, 3, 30, null },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 0.3m, 0.3m, 4, 8, null },
                    { 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, true, 0.3m, 0.27m, 4, 16, null },
                    { 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, true, 0.3m, 0.25m, 4, 30, null },
                    { 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 0.25m, 0.25m, 5, 30, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingServicePrices_ServiceType_FromDay_ToDay",
                table: "ListingServicePrices",
                columns: new[] { "ServiceType", "FromDay", "ToDay" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingServicePrices");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Users");
        }
    }
}
