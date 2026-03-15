using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Images_LogoId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Brands_LogoId",
                table: "Brands");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Posts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BrandId",
                table: "Posts",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_LogoId",
                table: "Brands",
                column: "LogoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Images_LogoId",
                table: "Brands",
                column: "LogoId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Brands_BrandId",
                table: "Posts",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Images_LogoId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Brands_BrandId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_BrandId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Brands_LogoId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Posts");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_LogoId",
                table: "Brands",
                column: "LogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Images_LogoId",
                table: "Brands",
                column: "LogoId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
