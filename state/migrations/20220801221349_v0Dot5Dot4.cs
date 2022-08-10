using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class v0Dot5Dot4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PageLayoutMode",
                table: "AppUserPreferences",
                newName: "BookReaderLayoutMode");

            migrationBuilder.AddColumn<int>(
                name: "AvgHoursToRead",
                table: "Volume",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxHoursToRead",
                table: "Volume",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinHoursToRead",
                table: "Volume",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "WordCount",
                table: "Volume",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "AvgHoursToRead",
                table: "Series",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxHoursToRead",
                table: "Series",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinHoursToRead",
                table: "Series",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "WordCount",
                table: "Series",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFileAnalysis",
                table: "MangaFile",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AvgHoursToRead",
                table: "Chapter",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxHoursToRead",
                table: "Chapter",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinHoursToRead",
                table: "Chapter",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "WordCount",
                table: "Chapter",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "BlurUnreadSummaries",
                table: "AppUserPreferences",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GlobalPageLayoutMode",
                table: "AppUserPreferences",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvgHoursToRead",
                table: "Volume");

            migrationBuilder.DropColumn(
                name: "MaxHoursToRead",
                table: "Volume");

            migrationBuilder.DropColumn(
                name: "MinHoursToRead",
                table: "Volume");

            migrationBuilder.DropColumn(
                name: "WordCount",
                table: "Volume");

            migrationBuilder.DropColumn(
                name: "AvgHoursToRead",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "MaxHoursToRead",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "MinHoursToRead",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "WordCount",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "LastFileAnalysis",
                table: "MangaFile");

            migrationBuilder.DropColumn(
                name: "AvgHoursToRead",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "MaxHoursToRead",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "MinHoursToRead",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "WordCount",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "BlurUnreadSummaries",
                table: "AppUserPreferences");

            migrationBuilder.DropColumn(
                name: "GlobalPageLayoutMode",
                table: "AppUserPreferences");

            migrationBuilder.RenameColumn(
                name: "BookReaderLayoutMode",
                table: "AppUserPreferences",
                newName: "PageLayoutMode");
        }
    }
}
