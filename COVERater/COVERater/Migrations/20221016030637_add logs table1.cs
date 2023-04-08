using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace COVERater.API.Migrations
{
    public partial class addlogstable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "UserStats",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "UserStats",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "UserStats");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "UserStats");
        }
    }
}
