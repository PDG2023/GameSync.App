using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameSync.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PartyEntity_LocationColumnd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeOfParty",
                table: "Parties",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Parties",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Parties");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeOfParty",
                table: "Parties",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
