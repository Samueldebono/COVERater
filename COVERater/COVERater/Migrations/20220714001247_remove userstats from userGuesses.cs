using Microsoft.EntityFrameworkCore.Migrations;

namespace COVERater.API.Migrations
{
    public partial class removeuserstatsfromuserGuesses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersGuess_UserStats_UserStatsUserId",
                table: "UsersGuess");

            migrationBuilder.AlterColumn<int>(
                name: "UserStatsUserId",
                table: "UsersGuess",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGuess_UserStats_UserStatsUserId",
                table: "UsersGuess",
                column: "UserStatsUserId",
                principalTable: "UserStats",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersGuess_UserStats_UserStatsUserId",
                table: "UsersGuess");

            migrationBuilder.AlterColumn<int>(
                name: "UserStatsUserId",
                table: "UsersGuess",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGuess_UserStats_UserStatsUserId",
                table: "UsersGuess",
                column: "UserStatsUserId",
                principalTable: "UserStats",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
