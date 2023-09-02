using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameSync.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InvitationTokenInParties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvitationToken",
                table: "Parties",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationToken",
                table: "Parties");
        }
    }
}
