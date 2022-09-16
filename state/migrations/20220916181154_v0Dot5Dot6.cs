using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class v0Dot5Dot6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesRelation_Series_TargetSeriesId",
                table: "SeriesRelation");

            migrationBuilder.AddColumn<string>(
                name: "FolderPath",
                table: "Series",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFolderScanned",
                table: "Series",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NormalizedLocalizedName",
                table: "Series",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesRelation_Series_TargetSeriesId",
                table: "SeriesRelation",
                column: "TargetSeriesId",
                principalTable: "Series",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesRelation_Series_TargetSeriesId",
                table: "SeriesRelation");

            migrationBuilder.DropColumn(
                name: "FolderPath",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "LastFolderScanned",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "NormalizedLocalizedName",
                table: "Series");

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesRelation_Series_TargetSeriesId",
                table: "SeriesRelation",
                column: "TargetSeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
