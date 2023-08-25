using System.Collections.Generic;
using GameSync.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GameSync.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PartyGames_And_Votes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Games",
                table: "Parties");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Parties",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "PartyGame",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    PartyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyGame", x => new { x.GameId, x.PartyId });
                    table.ForeignKey(
                        name: "FK_PartyGame_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    PartyGameGameId = table.Column<int>(type: "integer", nullable: false),
                    PartyGamePartyId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VoteYes = table.Column<bool>(type: "boolean", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => new { x.PartyGameGameId, x.PartyGamePartyId, x.Id });
                    table.ForeignKey(
                        name: "FK_Vote_PartyGame_PartyGameGameId_PartyGamePartyId",
                        columns: x => new { x.PartyGameGameId, x.PartyGamePartyId },
                        principalTable: "PartyGame",
                        principalColumns: new[] { "GameId", "PartyId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartyGame_PartyId",
                table: "PartyGame",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_UserId",
                table: "Vote",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vote");

            migrationBuilder.DropTable(
                name: "PartyGame");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Parties",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<IEnumerable<PartyGame>>(
                name: "Games",
                table: "Parties",
                type: "jsonb",
                nullable: true);
        }
    }
}
