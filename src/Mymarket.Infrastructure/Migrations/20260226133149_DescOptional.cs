using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DescOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostAttributes_AttributesOptions_OptionId",
                table: "PostAttributes");

            migrationBuilder.RenameColumn(
                name: "OptionId",
                table: "PostAttributes",
                newName: "AttributesOptionsEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_PostAttributes_OptionId",
                table: "PostAttributes",
                newName: "IX_PostAttributes_AttributesOptionsEntityId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Posts",
                type: "text",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 4000);

            migrationBuilder.AddColumn<int>(
                name: "ValueType",
                table: "PostAttributes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.DropColumn(
                name: "ValueType",
                table: "PostAttributes");

            migrationBuilder.RenameColumn(
                name: "AttributesOptionsEntityId",
                table: "PostAttributes",
                newName: "OptionId");

            migrationBuilder.RenameIndex(
                name: "IX_PostAttributes_AttributesOptionsEntityId",
                table: "PostAttributes",
                newName: "IX_PostAttributes_OptionId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Posts",
                type: "text",
                maxLength: 4000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 4000,
                oldNullable: true);

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
