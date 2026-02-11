using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveUnitsToSeparateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "UnitEn",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "UnitRu",
                table: "Attributes");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Attributes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AttributeUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    NameEn = table.Column<string>(type: "text", maxLength: 255, nullable: false),
                    NameRu = table.Column<string>(type: "text", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeUnits", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_UnitId",
                table: "Attributes",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_AttributeUnits_UnitId",
                table: "Attributes",
                column: "UnitId",
                principalTable: "AttributeUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_AttributeUnits_UnitId",
                table: "Attributes");

            migrationBuilder.DropTable(
                name: "AttributeUnits");

            migrationBuilder.DropIndex(
                name: "IX_Attributes_UnitId",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Attributes");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Attributes",
                type: "text",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitEn",
                table: "Attributes",
                type: "text",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitRu",
                table: "Attributes",
                type: "text",
                maxLength: 255,
                nullable: true);
        }
    }
}
