using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Sync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostAttributes_AttributesOptions_OptionId",
                table: "PostAttributes");

            migrationBuilder.DropIndex(
                name: "IX_PostAttributes_OptionId",
                table: "PostAttributes");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "PostAttributes");

            migrationBuilder.RenameColumn(
                name: "ValueText",
                table: "PostAttributes",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "ValueNumber",
                table: "PostAttributes",
                newName: "AttributesOptionsEntityId");

            migrationBuilder.AddColumn<int>(
                name: "ValueType",
                table: "PostAttributes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostAttributes_AttributesOptions_AttributesOptionsEntityId",
                table: "PostAttributes");

            migrationBuilder.DropIndex(
                name: "IX_PostAttributes_AttributesOptionsEntityId",
                table: "PostAttributes");

            migrationBuilder.DropColumn(
                name: "ValueType",
                table: "PostAttributes");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "PostAttributes",
                newName: "ValueText");

            migrationBuilder.RenameColumn(
                name: "AttributesOptionsEntityId",
                table: "PostAttributes",
                newName: "ValueNumber");

            migrationBuilder.AddColumn<int>(
                name: "OptionId",
                table: "PostAttributes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostAttributes_OptionId",
                table: "PostAttributes",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostAttributes_AttributesOptions_OptionId",
                table: "PostAttributes",
                column: "OptionId",
                principalTable: "AttributesOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
