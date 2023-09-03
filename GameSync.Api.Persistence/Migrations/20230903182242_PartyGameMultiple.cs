using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GameSync.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PartyGameMultiple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vote_PartiesGames_PartyGameGameId_PartyGamePartyId",
                table: "Vote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vote",
                table: "Vote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartiesGames",
                table: "PartiesGames");

            migrationBuilder.DropColumn(
                name: "PartyGameGameId",
                table: "Vote");

            migrationBuilder.RenameColumn(
                name: "PartyGamePartyId",
                table: "Vote",
                newName: "PartyGameId");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "PartiesGames",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PartiesGames",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "BoardGameGeekId",
                table: "PartiesGames",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PartiesGames",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vote",
                table: "Vote",
                columns: new[] { "PartyGameId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartiesGames",
                table: "PartiesGames",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PartiesGames_BoardGameGeekId",
                table: "PartiesGames",
                column: "BoardGameGeekId");

            migrationBuilder.CreateIndex(
                name: "IX_PartiesGames_GameId",
                table: "PartiesGames",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartiesGames_BoardGameGeekGames_BoardGameGeekId",
                table: "PartiesGames",
                column: "BoardGameGeekId",
                principalTable: "BoardGameGeekGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_PartiesGames_PartyGameId",
                table: "Vote",
                column: "PartyGameId",
                principalTable: "PartiesGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartiesGames_BoardGameGeekGames_BoardGameGeekId",
                table: "PartiesGames");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_PartiesGames_PartyGameId",
                table: "Vote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vote",
                table: "Vote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartiesGames",
                table: "PartiesGames");

            migrationBuilder.DropIndex(
                name: "IX_PartiesGames_BoardGameGeekId",
                table: "PartiesGames");

            migrationBuilder.DropIndex(
                name: "IX_PartiesGames_GameId",
                table: "PartiesGames");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PartiesGames");

            migrationBuilder.DropColumn(
                name: "BoardGameGeekId",
                table: "PartiesGames");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PartiesGames");

            migrationBuilder.RenameColumn(
                name: "PartyGameId",
                table: "Vote",
                newName: "PartyGamePartyId");


            migrationBuilder.AddColumn<int>(
                name: "PartyGameGameId",
                table: "Vote",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "PartiesGames",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vote",
                table: "Vote",
                columns: new[] { "PartyGameGameId", "PartyGamePartyId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartiesGames",
                table: "PartiesGames",
                columns: new[] { "GameId", "PartyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_PartiesGames_PartyGameGameId_PartyGamePartyId",
                table: "Vote",
                columns: new[] { "PartyGameGameId", "PartyGamePartyId" },
                principalTable: "PartiesGames",
                principalColumns: new[] { "GameId", "PartyId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
