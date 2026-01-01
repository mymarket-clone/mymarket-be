using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategoryTranslationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriesTranslations");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "Categories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameRu",
                table: "Categories",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "NameRu",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "CategoriesTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesTranslations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesTranslations_CategoryId_LanguageCode",
                table: "CategoriesTranslations",
                columns: new[] { "CategoryId", "LanguageCode" },
                unique: true);
        }
    }
}
