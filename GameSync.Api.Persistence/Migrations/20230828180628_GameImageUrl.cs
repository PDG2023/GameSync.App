using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameSync.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GameImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Vote",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Vote",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Games",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PartiesGames_Games_GameId",
                table: "PartiesGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_AspNetUsers_UserId",
                table: "Vote",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartiesGames_Games_GameId",
                table: "PartiesGames");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_AspNetUsers_UserId",
                table: "Vote");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Vote",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Vote",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
