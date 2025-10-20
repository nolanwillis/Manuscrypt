using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Manuscrypt.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovedEdits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Edits");

            migrationBuilder.DropTable(
                name: "PostTags");

            migrationBuilder.AddColumn<int[]>(
                name: "PostTags",
                table: "Posts",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostTags",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "Edits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OriginalText = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SubscriberId = table.Column<int>(type: "integer", nullable: false),
                    SuggestedText = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Edits_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Edits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    Tag = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostTags_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Edits_PostId",
                table: "Edits",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Edits_UserId",
                table: "Edits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_PostId",
                table: "PostTags",
                column: "PostId");
        }
    }
}
