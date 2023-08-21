using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameSync.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedBoardGameGeekGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardGameGeekId",
                table: "Games",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Games",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Games_BoardGameGeekId",
                table: "Games",
                column: "BoardGameGeekId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_BoardGameGeekId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "BoardGameGeekId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Games");
        }
    }
}
