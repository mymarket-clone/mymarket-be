using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PostViewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CategoryBrands_CategoryId",
                table: "CategoryBrands");

            migrationBuilder.CreateTable(
                name: "PostViews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: true),
                    ViewDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ViewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostViews_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostViews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBrands_CategoryId_BrandId",
                table: "CategoryBrands",
                columns: new[] { "CategoryId", "BrandId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostViews_PostId_SessionId_ViewDate",
                table: "PostViews",
                columns: new[] { "PostId", "SessionId", "ViewDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PostViews_PostId_UserId_ViewDate",
                table: "PostViews",
                columns: new[] { "PostId", "UserId", "ViewDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PostViews_PostId_ViewDate",
                table: "PostViews",
                columns: new[] { "PostId", "ViewDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PostViews_UserId",
                table: "PostViews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostViews_ViewedAt",
                table: "PostViews",
                column: "ViewedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostViews");

            migrationBuilder.DropIndex(
                name: "IX_CategoryBrands_CategoryId_BrandId",
                table: "CategoryBrands");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBrands_CategoryId",
                table: "CategoryBrands",
                column: "CategoryId");
        }
    }
}
