using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameEmailVerificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailVerificationEntities_Users_UserId",
                table: "EmailVerificationEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailVerificationEntities",
                table: "EmailVerificationEntities");

            migrationBuilder.RenameTable(
                name: "EmailVerificationEntities",
                newName: "EmailVerification");

            migrationBuilder.RenameIndex(
                name: "IX_EmailVerificationEntities_UserId",
                table: "EmailVerification",
                newName: "IX_EmailVerification_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailVerification",
                table: "EmailVerification",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailVerification_Users_UserId",
                table: "EmailVerification",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailVerification_Users_UserId",
                table: "EmailVerification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailVerification",
                table: "EmailVerification");

            migrationBuilder.RenameTable(
                name: "EmailVerification",
                newName: "EmailVerificationEntities");

            migrationBuilder.RenameIndex(
                name: "IX_EmailVerification_UserId",
                table: "EmailVerificationEntities",
                newName: "IX_EmailVerificationEntities_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailVerificationEntities",
                table: "EmailVerificationEntities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailVerificationEntities_Users_UserId",
                table: "EmailVerificationEntities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
