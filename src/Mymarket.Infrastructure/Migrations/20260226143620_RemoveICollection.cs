using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveICollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostAttributes_Posts_PostEntityId",
                table: "PostAttributes");

            migrationBuilder.DropIndex(
                name: "IX_PostAttributes_PostEntityId",
                table: "PostAttributes");

            migrationBuilder.DropColumn(
                name: "PostEntityId",
                table: "PostAttributes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostEntityId",
                table: "PostAttributes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostAttributes_PostEntityId",
                table: "PostAttributes",
                column: "PostEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostAttributes_Posts_PostEntityId",
                table: "PostAttributes",
                column: "PostEntityId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
