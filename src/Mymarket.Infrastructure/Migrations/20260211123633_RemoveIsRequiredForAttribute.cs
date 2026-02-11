using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsRequiredForAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "Attributes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "Attributes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
