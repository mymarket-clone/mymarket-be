using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLogoToCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LogoId",
                table: "Categories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_LogoId",
                table: "Categories",
                column: "LogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Images_LogoId",
                table: "Categories",
                column: "LogoId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Images_LogoId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_LogoId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LogoId",
                table: "Categories");
        }
    }
}
