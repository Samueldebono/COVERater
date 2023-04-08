using Microsoft.EntityFrameworkCore.Migrations;

namespace COVERater.API.Migrations
{
    public partial class addlinkforguesses3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropIndex(
                name: "IX_UsersGuess_UserStatsUserId",
                table: "UsersGuess");

            migrationBuilder.DropColumn(
                name: "SubImUserStatsId",
                table: "UsersGuess");

            migrationBuilder.DropColumn(
                name: "UserStatsUserId",
                table: "UsersGuess");

            migrationBuilder.CreateIndex(
                name: "IX_UsersGuess_UserId",
                table: "UsersGuess",
                column: "UserId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_UsersGuess_UserStats_UserId",
            //    table: "UsersGuess",
            //    column: "UserId",
            //    principalTable: "UserStats",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_UsersGuess_UserStats_UserId",
            //    table: "UsersGuess");

            migrationBuilder.DropIndex(
                name: "IX_UsersGuess_UserId",
                table: "UsersGuess");

            migrationBuilder.AddColumn<int>(
                name: "SubImUserStatsId",
                table: "UsersGuess",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserStatsUserId",
                table: "UsersGuess",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersGuess_UserStatsUserId",
                table: "UsersGuess",
                column: "UserStatsUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGuess_UserStats_UserStatsUserId",
                table: "UsersGuess",
                column: "UserStatsUserId",
                principalTable: "UserStats",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
