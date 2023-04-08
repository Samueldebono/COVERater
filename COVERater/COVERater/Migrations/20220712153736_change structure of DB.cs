using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace COVERater.API.Migrations
{
    public partial class changestructureofDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthUsers",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    RoleType = table.Column<byte>(nullable: false),
                    ExperienceLevel = table.Column<byte>(nullable: false),
                    HashUser = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUsers", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CloudinaryId = table.Column<Guid>(nullable: false),
                    AddedUtc = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    TokenId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserGuid = table.Column<string>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ExpiresTime = table.Column<DateTime>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.TokenId);
                });

            migrationBuilder.CreateTable(
                name: "UserEmails",
                columns: table => new
                {
                    UserEmailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    PrizeSent = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEmails", x => x.UserEmailId);
                });

            migrationBuilder.CreateTable(
                name: "UserStats",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    FinishedPhaseUtc = table.Column<DateTime>(nullable: true),
                    TimePhase = table.Column<DateTime>(nullable: true),
                    PictureCycledPhase = table.Column<int>(nullable: true),
                    FinishingPercentPhase = table.Column<decimal>(nullable: true),
                    Phase = table.Column<byte>(nullable: false),
                    AuthUsersRoleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStats", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserStats_AuthUsers_AuthUsersRoleId",
                        column: x => x.AuthUsersRoleId,
                        principalTable: "AuthUsers",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubImage",
                columns: table => new
                {
                    SubImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CloudinaryId = table.Column<Guid>(nullable: false),
                    AddedUtc = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    CoverageRate = table.Column<decimal>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: true),
                    ImageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubImage", x => x.SubImageId);
                    table.ForeignKey(
                        name: "FK_SubImage_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersGuess",
                columns: table => new
                {
                    UsersGuessId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuessPercentage = table.Column<decimal>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    SubImageId = table.Column<int>(nullable: false),
                    Phase = table.Column<byte>(nullable: false),
                    GuessTimeUtc = table.Column<DateTime>(nullable: false),
                    UserStatsUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersGuess", x => x.UsersGuessId);
                    table.ForeignKey(
                        name: "FK_UsersGuess_SubImage_SubImageId",
                        column: x => x.SubImageId,
                        principalTable: "SubImage",
                        principalColumn: "SubImageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersGuess_UserStats_UserStatsUserId",
                        column: x => x.UserStatsUserId,
                        principalTable: "UserStats",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubImage_ImageId",
                table: "SubImage",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersGuess_SubImageId",
                table: "UsersGuess",
                column: "SubImageId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersGuess_UserStatsUserId",
                table: "UsersGuess",
                column: "UserStatsUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStats_AuthUsersRoleId",
                table: "UserStats",
                column: "AuthUsersRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "UserEmails");

            migrationBuilder.DropTable(
                name: "UsersGuess");

            migrationBuilder.DropTable(
                name: "SubImage");

            migrationBuilder.DropTable(
                name: "UserStats");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "AuthUsers");
        }
    }
}
