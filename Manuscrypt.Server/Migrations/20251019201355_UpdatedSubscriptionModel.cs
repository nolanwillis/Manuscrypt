using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manuscrypt.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSubscriptionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Users_UserId",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Subscriptions",
                newName: "SubscriberId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_SubscriberId");

            migrationBuilder.AddColumn<int>(
                name: "SubscribedToId",
                table: "Subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscribedToId",
                table: "Subscriptions",
                column: "SubscribedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Users_SubscribedToId",
                table: "Subscriptions",
                column: "SubscribedToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Users_SubscriberId",
                table: "Subscriptions",
                column: "SubscriberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Users_SubscribedToId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Users_SubscriberId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SubscribedToId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SubscribedToId",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "SubscriberId",
                table: "Subscriptions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_SubscriberId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Users_UserId",
                table: "Subscriptions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
