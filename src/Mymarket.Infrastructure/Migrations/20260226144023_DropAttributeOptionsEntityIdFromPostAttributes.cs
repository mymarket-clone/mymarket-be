using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DropAttributeOptionsEntityIdFromPostAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostAttributes_AttributesOptions_AttributesOptionsEntityId",
                table: "PostAttributes");

            migrationBuilder.DropIndex(
                name: "IX_PostAttributes_AttributesOptionsEntityId",
                table: "PostAttributes");

            migrationBuilder.DropColumn(
                name: "AttributesOptionsEntityId",
                table: "PostAttributes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttributesOptionsEntityId",
                table: "PostAttributes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostAttributes_AttributesOptionsEntityId",
                table: "PostAttributes",
                column: "AttributesOptionsEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostAttributes_AttributesOptions_AttributesOptionsEntityId",
                table: "PostAttributes",
                column: "AttributesOptionsEntityId",
                principalTable: "AttributesOptions",
                principalColumn: "Id");
        }
    }
}
