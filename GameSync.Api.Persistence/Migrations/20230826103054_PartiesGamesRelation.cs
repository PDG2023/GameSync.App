using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameSync.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PartiesGamesRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyGame_Parties_PartyId",
                table: "PartyGame");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_PartyGame_PartyGameGameId_PartyGamePartyId",
                table: "Vote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartyGame",
                table: "PartyGame");

            migrationBuilder.RenameTable(
                name: "PartyGame",
                newName: "PartiesGames");

            migrationBuilder.RenameIndex(
                name: "IX_PartyGame_PartyId",
                table: "PartiesGames",
                newName: "IX_PartiesGames_PartyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartiesGames",
                table: "PartiesGames",
                columns: new[] { "GameId", "PartyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PartiesGames_Parties_PartyId",
                table: "PartiesGames",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_PartiesGames_PartyGameGameId_PartyGamePartyId",
                table: "Vote",
                columns: new[] { "PartyGameGameId", "PartyGamePartyId" },
                principalTable: "PartiesGames",
                principalColumns: new[] { "GameId", "PartyId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartiesGames_Parties_PartyId",
                table: "PartiesGames");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_PartiesGames_PartyGameGameId_PartyGamePartyId",
                table: "Vote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartiesGames",
                table: "PartiesGames");

            migrationBuilder.RenameTable(
                name: "PartiesGames",
                newName: "PartyGame");

            migrationBuilder.RenameIndex(
                name: "IX_PartiesGames_PartyId",
                table: "PartyGame",
                newName: "IX_PartyGame_PartyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartyGame",
                table: "PartyGame",
                columns: new[] { "GameId", "PartyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PartyGame_Parties_PartyId",
                table: "PartyGame",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_PartyGame_PartyGameGameId_PartyGamePartyId",
                table: "Vote",
                columns: new[] { "PartyGameGameId", "PartyGamePartyId" },
                principalTable: "PartyGame",
                principalColumns: new[] { "GameId", "PartyId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
