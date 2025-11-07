using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manuscrypt.UserService.Migrations
{
    /// <inheritdoc />
    public partial class ChangedSeedId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeedId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeedId",
                table: "AspNetUsers");
        }
    }
}
