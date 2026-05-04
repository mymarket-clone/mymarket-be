using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mymarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChatColsRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_BuyerId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_SellerId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_BuyerId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_PostId_BuyerId",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Chats",
                newName: "User2Id");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Chats",
                newName: "User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_SellerId",
                table: "Chats",
                newName: "IX_Chats_User2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_PostId",
                table: "Chats",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_User1Id_User2Id_PostId",
                table: "Chats",
                columns: new[] { "User1Id", "User2Id", "PostId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_User1Id",
                table: "Chats",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_User2Id",
                table: "Chats",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_User1Id",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_User2Id",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_PostId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_User1Id_User2Id_PostId",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "User2Id",
                table: "Chats",
                newName: "SellerId");

            migrationBuilder.RenameColumn(
                name: "User1Id",
                table: "Chats",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_User2Id",
                table: "Chats",
                newName: "IX_Chats_SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_BuyerId",
                table: "Chats",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_PostId_BuyerId",
                table: "Chats",
                columns: new[] { "PostId", "BuyerId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_BuyerId",
                table: "Chats",
                column: "BuyerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_SellerId",
                table: "Chats",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
