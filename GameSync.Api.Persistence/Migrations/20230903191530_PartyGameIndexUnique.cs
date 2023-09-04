using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameSync.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PartyGameIndexUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PartiesGames_PartyId_BoardGameGeekId",
                table: "PartiesGames");

            migrationBuilder.DropIndex(
                name: "IX_PartiesGames_PartyId_GameId",
                table: "PartiesGames");

            migrationBuilder.CreateIndex(
                name: "IX_PartiesGames_PartyId_BoardGameGeekId",
                table: "PartiesGames",
                columns: new[] { "PartyId", "BoardGameGeekId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartiesGames_PartyId_GameId",
                table: "PartiesGames",
                columns: new[] { "PartyId", "GameId" },
                unique: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PartiesGames_PartyId_BoardGameGeekId",
                table: "PartiesGames");

            migrationBuilder.DropIndex(
                name: "IX_PartiesGames_PartyId_GameId",
                table: "PartiesGames");


            migrationBuilder.CreateIndex(
                name: "IX_PartiesGames_PartyId_BoardGameGeekId",
                table: "PartiesGames",
                columns: new[] { "PartyId", "BoardGameGeekId" });

            migrationBuilder.CreateIndex(
                name: "IX_PartiesGames_PartyId_GameId",
                table: "PartiesGames",
                columns: new[] { "PartyId", "GameId" });
        }
    }
}
