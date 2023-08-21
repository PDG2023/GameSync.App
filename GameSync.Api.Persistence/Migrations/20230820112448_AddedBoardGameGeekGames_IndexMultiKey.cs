using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameSync.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedBoardGameGeekGames_IndexMultiKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_BoardGameGeekId",
                table: "Games");

            migrationBuilder.CreateIndex(
                name: "IX_Games_BoardGameGeekId_UserId",
                table: "Games",
                columns: new[] { "BoardGameGeekId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_BoardGameGeekId_UserId",
                table: "Games");

            migrationBuilder.CreateIndex(
                name: "IX_Games_BoardGameGeekId",
                table: "Games",
                column: "BoardGameGeekId",
                unique: true);
        }
    }
}
