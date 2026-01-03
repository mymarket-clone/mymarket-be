using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostType = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    TitleEn = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DescriptionEn = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    TitleRu = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DescriptionRu = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    ForDisabledPerson = table.Column<bool>(type: "boolean", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    CurrencyType = table.Column<int>(type: "integer", nullable: false),
                    SalePercentage = table.Column<byte>(type: "smallint", nullable: false),
                    CanOfferPrice = table.Column<bool>(type: "boolean", nullable: false),
                    IsNegotiable = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(72)", maxLength: 72, nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PromoType = table.Column<int>(type: "integer", nullable: true),
                    IsColored = table.Column<bool>(type: "boolean", nullable: false),
                    AutoRenewal = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
