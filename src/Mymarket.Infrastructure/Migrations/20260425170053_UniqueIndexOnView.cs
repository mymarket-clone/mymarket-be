using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueIndexOnView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostViews_Posts_PostId",
                table: "PostViews");

            migrationBuilder.DropForeignKey(
                name: "FK_PostViews_Users_UserId",
                table: "PostViews");

            migrationBuilder.DropIndex(
                name: "IX_PostViews_PostId_SessionId_ViewDate",
                table: "PostViews");

            migrationBuilder.DropIndex(
                name: "IX_PostViews_PostId_UserId_ViewDate",
                table: "PostViews");

            migrationBuilder.DropIndex(
                name: "IX_PostViews_PostId_ViewDate",
                table: "PostViews");

            migrationBuilder.CreateIndex(
                name: "IX_PostViews_PostId_SessionId_ViewDate",
                table: "PostViews",
                columns: new[] { "PostId", "SessionId", "ViewDate" },
                unique: true,
                filter: "\"SessionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PostViews_PostId_UserId_ViewDate",
                table: "PostViews",
                columns: new[] { "PostId", "UserId", "ViewDate" },
                unique: true,
                filter: "\"UserId\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PostViews_Posts_PostId",
                table: "PostViews",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostViews_Users_UserId",
                table: "PostViews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostViews_Posts_PostId",
                table: "PostViews");

            migrationBuilder.DropForeignKey(
                name: "FK_PostViews_Users_UserId",
                table: "PostViews");

            migrationBuilder.DropIndex(
                name: "IX_PostViews_PostId_SessionId_ViewDate",
                table: "PostViews");

            migrationBuilder.DropIndex(
                name: "IX_PostViews_PostId_UserId_ViewDate",
                table: "PostViews");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PostViews_Posts_PostId",
                table: "PostViews",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostViews_Users_UserId",
                table: "PostViews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
