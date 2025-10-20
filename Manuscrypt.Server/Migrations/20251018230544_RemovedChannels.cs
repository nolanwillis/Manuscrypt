using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Manuscrypt.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovedChannels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Channels_ChannelId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Channels_ChannelId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_ChannelId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "ChannelId",
                table: "Posts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_ChannelId",
                table: "Posts",
                newName: "IX_Posts_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Posts",
                newName: "ChannelId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                newName: "IX_Posts_ChannelId");

            migrationBuilder.AddColumn<int>(
                name: "ChannelId",
                table: "Subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channels_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ChannelId",
                table: "Subscriptions",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_UserId",
                table: "Channels",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Channels_ChannelId",
                table: "Posts",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Channels_ChannelId",
                table: "Subscriptions",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
