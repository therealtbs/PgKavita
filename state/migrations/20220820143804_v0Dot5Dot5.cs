using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class v0Dot5Dot5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Series",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "MangaFile",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ConfirmationToken",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PromptForDownloadSize",
                table: "AppUserPreferences",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "AppUserBookmark",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "AppUserBookmark",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Series_AppUserId",
                table: "Series",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Series_AspNetUsers_AppUserId",
                table: "Series",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Series_AspNetUsers_AppUserId",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Series_AppUserId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "MangaFile");

            migrationBuilder.DropColumn(
                name: "ConfirmationToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PromptForDownloadSize",
                table: "AppUserPreferences");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "AppUserBookmark");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "AppUserBookmark");
        }
    }
}
