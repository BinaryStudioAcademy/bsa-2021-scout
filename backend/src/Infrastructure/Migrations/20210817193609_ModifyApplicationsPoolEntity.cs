using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ModifyApplicationsPoolEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Pools");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Pools",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Pools",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Pools",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Pools",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Pools");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Pools");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Pools");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Pools");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Pools",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
