using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NameRu",
                table: "Attributes",
                type: "text",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "NameEn",
                table: "Attributes",
                type: "text",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "UnitEn",
                table: "Attributes",
                type: "text",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitRu",
                table: "Attributes",
                type: "text",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitEn",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "UnitRu",
                table: "Attributes");

            migrationBuilder.AlterColumn<string>(
                name: "NameRu",
                table: "Attributes",
                type: "text",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEn",
                table: "Attributes",
                type: "text",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
