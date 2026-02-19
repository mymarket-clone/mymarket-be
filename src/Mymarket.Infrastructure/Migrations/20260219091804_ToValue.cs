using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ToValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValueText",
                table: "PostAttributes",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "ValueNumber",
                table: "PostAttributes",
                newName: "PostEntityId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostAttributes_Posts_PostEntityId",
                table: "PostAttributes");

            migrationBuilder.DropIndex(
                name: "IX_PostAttributes_PostEntityId",
                table: "PostAttributes");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "PostAttributes",
                newName: "ValueText");

            migrationBuilder.RenameColumn(
                name: "PostEntityId",
                table: "PostAttributes",
                newName: "ValueNumber");
        }
    }
}
